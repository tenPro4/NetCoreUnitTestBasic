using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhotoSauce.MagicScaler;
using StationMonitorAPI.Configurations.Constants;
using StationMonitorAPI.Configurations.Settings;
using StationMonitorAPI.DTOs;
using StationMonitorAPI.Helpers;
using StationMonitorAPI.Mappers;
using StationMonitorAPI.Models;
using StationMonitorAPI.ViewModels;

namespace StationMonitorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        private readonly IWebHostEnvironment _env;

        private readonly IOptionsMonitor<FileSettings> fileSettingsMonitor;

        public HomeController(AppDbContext ctx, IWebHostEnvironment env
            , IOptionsMonitor<FileSettings> fileSettingsMonitor)
        {
            _ctx = ctx;
            _env = env;
            this.fileSettingsMonitor = fileSettingsMonitor;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var mapping = _ctx.MappingObject
                .Include(x => x.Unit)
                .ToList();

            return Ok(mapping);
        }

        [HttpPost]
        public async Task<ActionResult> AddObject(MappingObject mo)
        {
            if (!_ctx.MappingObject.Any(x => x.UnitID == mo.UnitID))
            {
                _ctx.MappingObject.Add(mo);
                await _ctx.SaveChangesAsync();

                var unit = _ctx.Unit.FirstOrDefault(x => x.ID == mo.UnitID);

                mo.Unit = unit;

                return Ok(mo);
            }

            return Ok(null);
        }

        [HttpPost("MapImage")]
        public IActionResult MapImage(IFormFile file)
        {
            var mi = _ctx.WebSettings.FirstOrDefault(x => x.Title == SettingsConstant.DashboardMapUrl);

            ProcessImageSettings settings = new ProcessImageSettings
            {
                ResizeMode = CropScaleMode.Crop,
                SaveFormat = FileFormat.Jpeg,
                JpegQuality = 100
            };

            var rn = CommonHelper.RandomImgName("atlas");

            var save_path = Path.Combine(_env.WebRootPath, rn);

            if (mi != null)
            {
                using (var fileStream = new FileStream(save_path, FileMode.Create))
                {
                    MagicImageProcessor.ProcessImage(file.OpenReadStream(), fileStream, settings);
                }

                mi.KeyValue = fileSettingsMonitor.CurrentValue.ImageUrl + rn;

                _ctx.WebSettings.Update(mi);
            }
            else
            {
                using (var fileStream = new FileStream(save_path, FileMode.Create))
                {
                    MagicImageProcessor.ProcessImage(file.OpenReadStream(), fileStream, settings);
                }

                _ctx.WebSettings.Add(new WebSettings
                {
                    Title = SettingsConstant.DashboardMapUrl,
                    KeyValue = fileSettingsMonitor.CurrentValue.ImageUrl + rn
                }); ;
            }

            _ctx.SaveChanges();

            return Ok();
        }

        [HttpGet("image/{fileName}")]
        public IActionResult GetImage([FromRoute] string fileName)
        {
            var savePath = Path.Combine(_env.WebRootPath, fileName);

            return new FileStreamResult(new FileStream(savePath, FileMode.Open, FileAccess.Read), "image/*");
        }

        [HttpGet("CheckMap")]
        public IActionResult CheckMap()
        {
            var mapUrl = _ctx.WebSettings.FirstOrDefault(x => x.Title == SettingsConstant.DashboardMapUrl);
            if (mapUrl != null) return Ok(mapUrl.KeyValue);

            return Ok(string.Empty);

        }

        [HttpPut]
        public async Task<ActionResult> UpdateObect([FromBody] MappingObject mo)
        {
            var check = _ctx.MappingObject.FirstOrDefault(x => x.UnitID == mo.UnitID);
            if (check != null)
            {
                check.PositionX = mo.PositionX; 
                check.PositionY = mo.PositionY;
                _ctx.MappingObject.Update(check);
                await _ctx.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteObject([FromRoute] long id)
        {
            var check = _ctx.MappingObject.FirstOrDefault(x => x.UnitID == id);

            if (check != null)
            {
                _ctx.MappingObject.Remove(check);
                _ctx.SaveChanges();
            }

            return NoContent();
        }

        [HttpPost("searchResult")]
        public IActionResult SearchResult(SearchResultModel model)
        {
            var resultIdentifier = _ctx.ResultIdentifier.Where(x => x.Identifier == model.SearchValue).FirstOrDefault();

            if (resultIdentifier == null) return NotFound();

            var allUnit = _ctx.Unit.ToList();

            var query = _ctx.vResultIdentifier
                .Where(x => x.Identifier == model.SearchValue)
                .Select(x => x.ResultID).ToList();

            if(query.Count <= 0) return NotFound();

            var query2 = _ctx.Result.Where(x => query.Contains(x.ID)).ToList();
            var resultList = new List<long>();
            foreach (var unit in allUnit)
            {
                if (query2.Any(x => x.UnitID == unit.ID))
                {
                    if (model.Latest)
                    {
                        var pickLatestResult = query2.Where(x => x.UnitID == unit.ID)
                        .OrderByDescending(x => x.ResultInsertDateTime)
                        .FirstOrDefault();
                        if (pickLatestResult != null)
                        {
                            resultList.Add(pickLatestResult.ID);
                        }
                    }
                    else
                    {
                        var allResults = query2.Where(x => x.UnitID == unit.ID)
                                                .OrderByDescending(x => x.ResultInsertDateTime)
                                                .Select(x => x.ID)
                                                .ToList();
                        if (allResults.Count > 0)
                        {
                            resultList.AddRange(allResults);
                        }
                    }
                    
                }
            }

            var resultTighteningBasic = _ctx.vResultTighteningBasic
               .Where(x => resultList.Contains(x.ResultID))
               .ToList();

            var vm = new List<SearchResultVM>();

            foreach (var result in resultTighteningBasic)
            {
                var viewModel = new SearchResultVM();
                viewModel.Identifier = resultIdentifier.Identifier;
                viewModel.ResultID = result.ResultID;
                viewModel.ResultDateTime = result.ResultDateTime;
                viewModel.OverallStatus = result.OverallStatus;
                viewModel.TorqueStatus = result.TorqueStatus;
                viewModel.AngleStatus = result.AngleStatus;
                viewModel.RundownAngleStatus = result.RundownAngleStatus;
                viewModel.FinalAngle = result.FinalAngle;
                viewModel.FinalTorque = result.FinalTorque;
                viewModel.RundownAngle = result.RundownAngle;
                viewModel.ResultSequenceNumber = result.ResultSequenceNumber;
                viewModel.Unit = new Unit {ID = result.UnitID,Name = result.UnitName };
                viewModel.Program = new Models.Program { ID = result.ProgramID,Name = result.ProgramName};
                vm.Add(viewModel);
            }

            return Ok(vm);
        }

        [HttpGet("history")]
        public async Task<ActionResult> GetHistory()
        {
            var vm = new List<SearchResultVM>();

            var tighteningRecord = await _ctx.ResultTightening
                .Include(x => x.Result)
                .ThenInclude(x => x.ResultStatusType)
                .Include(x => x.Result)
                .ThenInclude(x => x.Unit)
                .Include(x => x.Result)
                .ThenInclude(x => x.Program)
                .ToListAsync();

            foreach(var tr in tighteningRecord)
            {
                var viewModel = new SearchResultVM();
                var checkIdentifier = _ctx.ResultToResultIdentifier
                    .Include(x => x.ResultIdentifier)
                    .FirstOrDefault(x => x.ResultID == tr.ResultID);

                if(checkIdentifier != null)
                {
                    viewModel.Identifier = checkIdentifier.ResultIdentifier.Identifier;
                    viewModel.ResultID = tr.ResultID;
                    viewModel.ResultDateTime = tr.Result.ResultDateTime;
                    viewModel.OverallStatus = tr.Result.ResultStatusType.LanguageConstant;
                    viewModel.TorqueStatus = tr.FinalTorqueStatus.LanguageConstant;
                    viewModel.AngleStatus = tr.FinalAngleStatus.LanguageConstant;
                    viewModel.RundownAngleStatus = tr.FinalRunDownStatus.LanguageConstant;
                    viewModel.FinalAngle = tr.FinalAngle;
                    viewModel.FinalTorque = tr.FinalTorque;
                    viewModel.RundownAngle = tr.RundownAngle;
                    viewModel.ResultSequenceNumber = tr.Result.ResultSequenceNumber;
                    viewModel.Unit = tr.Result.Unit;
                    viewModel.UnitName = tr.Result.Unit.Name;
                    viewModel.Program = tr.Result.Program;
                    viewModel.ProgramName = tr.Result.Program.Name;
                    vm.Add(viewModel);
                }
            }

            return Ok(vm);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetail([FromRoute]long id)
        {
            var tr = _ctx.vResultTighteningBasic.FirstOrDefault(x => x.ResultID == id);

            var checkIdentifier = _ctx.ResultToResultIdentifier
                    .Include(x => x.ResultIdentifier)
                    .FirstOrDefault(x => x.ResultID == id);

            var viewModel = new SearchResultVM
            {
                Identifier = checkIdentifier.ResultIdentifier.Identifier,
                ResultID = tr.ResultID,
                ResultDateTime = tr.ResultDateTime,
                OverallStatus = tr.OverallStatus,
                TorqueStatus = tr.TorqueStatus,
                AngleStatus = tr.AngleStatus,
                RundownAngleStatus = tr.RundownAngleStatus,
                FinalAngle = tr.FinalAngle,
                FinalTorque = tr.FinalTorque,
                RundownAngle = tr.RundownAngle,
                ResultSequenceNumber = tr.ResultSequenceNumber,
                UnitName = tr.UnitName,
                ProgramName = tr.ProgramName
            };

            return Ok(viewModel);
        }
    }
}

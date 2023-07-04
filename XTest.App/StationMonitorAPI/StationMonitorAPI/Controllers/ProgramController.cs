using Microsoft.AspNetCore.Mvc;
using StationMonitorAPI.Models;

namespace StationMonitorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public ProgramController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_ctx.Program.Where(x => !string.IsNullOrEmpty(x.Name)).ToList());
        }
    }
}

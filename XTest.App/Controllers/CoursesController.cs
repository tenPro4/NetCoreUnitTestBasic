using AutoMapper;
using Blog.Turnmeup.API.Models.Courses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using XTesting.Services.Infrastructure.ErrorHandler;
using XTesting.Services.Models;
using XTesting.Services.Services;

namespace XTest.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController:ControllerBase
    {
        private readonly ICourseService _service;
        private readonly IErrorHandler _errorHandler;

        public CoursesController(ICourseService service, IMapper mapper, IErrorHandler errorHandler)
        {
            _service = service;
            _errorHandler = errorHandler;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<CourseResponseModel>> Get()
        {

            return await _service.GetAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<CourseResponseModel> Get([Required] int id)
        {
            return await _service.GetById(id);
        }

        [HttpGet("{fieldLabel}/{fieldValue}")]
        public List<CourseResponseModel> Where(string fieldLabel, string fieldValue)
        {

            return
                _service.Where(
                    entity => (string)entity.GetType().GetProperty(fieldLabel).GetValue(entity, null) == fieldValue).ToList();

        }

        [HttpGet("where/criterias/{criteriasString}")]
        public List<CourseResponseModel> Where(string criteriasString)
        {

            var criteriasModel = JsonConvert.DeserializeObject<WhereRequestModel>(criteriasString);
            var whereResult = _service.GetAsync().Result;
            whereResult = criteriasModel.Criterias.Aggregate(whereResult, (current, attribute) => current.Where(entity => (string)entity.GetType().GetProperty(attribute.Key).GetValue(entity, null) == attribute.Value).AsEnumerable());
            return whereResult.ToList();
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody] CourseResponseModel entity)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation), "", ModelState.Values.First().Errors.First().ErrorMessage));
            }

            _service.AddOrUpdate(entity);
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete([Required] int id)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation), "", ModelState.Values.First().Errors.First().ErrorMessage));
            }

            _service.Remove(id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using StationMonitorAPI.Models;

namespace StationMonitorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnitController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public UnitController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_ctx.Unit.ToList());
        }
    }
}

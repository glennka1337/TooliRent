using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TooliRent.Services.Services;

namespace TooliRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _service;
        public StatisticsController(IStatisticsService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stats = await _service.GetAsync();
            return Ok(stats);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services;

namespace TooliRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToolsController : ControllerBase
    {
        private readonly IToolService _service;
        private readonly IMapper _mapper;

        public ToolsController(IToolService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToolDto>>> Get([FromQuery] string? category, [FromQuery] bool? active)
        {
            var tools = await _service.GetAsync(category, active);
            return Ok(_mapper.Map<IEnumerable<ToolDto>>(tools));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ToolDto>> GetById(int id)
        {
            var tool = await _service.GetByIdAsync(id);
            if (tool is null) return NotFound();
            return Ok(_mapper.Map<ToolDto>(tool));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ToolDto>> Create([FromBody] CreateToolDto dto)
        {
            var entity = _mapper.Map<Tool>(dto);
            var id = await _service.CreateAsync(entity);
            var result = _mapper.Map<ToolDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id }, result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateToolDto dto)
        {
            var updated = _mapper.Map<Tool>(dto);
            await _service.UpdateAsync(id, updated);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
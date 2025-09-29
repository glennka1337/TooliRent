using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TooliRent.Core.Interfaces;

namespace TooliRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _users;
        public UsersController(IUserRepository users) => _users = users;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await Task.FromResult(await _users.GetAllAsync());
            return Ok(list.Select(u => new { u.Id, u.Username, u.Role, u.IsActive }));
        }

        [HttpPost("{id:int}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var user = await _users.GetByIdAsync(id);
            if (user is null) return NotFound();
            user.IsActive = true;
            await _users.UpdateAsync(user);
            return NoContent();
        }

        [HttpPost("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var user = await _users.GetByIdAsync(id);
            if (user is null) return NotFound();
            user.IsActive = false;
            await _users.UpdateAsync(user);
            return NoContent();
        }
    }
}
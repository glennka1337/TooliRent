using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services;

namespace TooliRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("register")]
        public async Task<ActionResult<AuthResultDto>> Register(RegisterDto dto)
        {
            var result = await _auth.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResultDto>> Login(LoginDto dto)
        {
            var result = await _auth.LoginAsync(dto);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResultDto>> Refresh(RefreshRequestDto dto)
        {
            var result = await _auth.RefreshAsync(dto.RefreshToken);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var uid = User.FindFirst("uid")?.Value;
            if (!int.TryParse(uid, out var id))
            {
                var nameId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(nameId, out id)) return Forbid();
            }

            await _auth.LogoutAsync(id);
            return NoContent();
        }
    }
}
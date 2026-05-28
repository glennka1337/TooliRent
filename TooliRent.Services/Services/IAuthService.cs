using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto dto);
        Task<AuthResultDto> LoginAsync(LoginDto dto);
        Task<AuthResultDto> RefreshAsync(string refreshToken);
        Task LogoutAsync(int userId);
    }
}
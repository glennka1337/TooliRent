namespace TooliRent.Services.DTOs
{
    public record AuthResultDto(string Token, DateTime ExpiresUtc, string Username, string Role);
}
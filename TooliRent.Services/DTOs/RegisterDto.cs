namespace TooliRent.Services.DTOs
{
    public record RegisterDto(string Username, string Password, string Role = "Member");
}
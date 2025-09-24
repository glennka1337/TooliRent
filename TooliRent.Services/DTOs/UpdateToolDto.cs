namespace TooliRent.Services.DTOs
{
    public record UpdateToolDto(string Name, string? Description, int CategoryId, bool IsActive);
}
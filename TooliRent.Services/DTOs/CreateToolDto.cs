namespace TooliRent.Services.DTOs
{
    public record CreateToolDto(string Name, string? Description, int CategoryId, bool IsActive = true);
}

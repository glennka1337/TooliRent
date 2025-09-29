namespace TooliRent.Services.DTOs
{
    public record CreateBookingRequestDto(DateTime StartDate, DateTime EndDate, IReadOnlyList<int> ToolIds);
}
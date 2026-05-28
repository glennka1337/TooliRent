namespace TooliRent.Services.DTOs
{
    public record ToolUsageDto(string ToolName, int UsageCount);

    public record StatisticsDto(int TotalBookings, int ActiveBookings, int OverdueBookings, IEnumerable<ToolUsageDto> TopTools);
}

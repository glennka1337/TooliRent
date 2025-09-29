using TooliRent.Core.Models.Enums;

namespace TooliRent.Services.DTOs
{
    public record BookingDto(
        int Id,
        int ToolId,
        string ToolName,
        DateTime StartDate,
        DateTime EndDate,
        BookingStatus Status
    );
}
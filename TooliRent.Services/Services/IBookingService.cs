using TooliRent.Core.Models.Enums;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services
{
    public interface IBookingService
    {
        Task<IReadOnlyList<BookingDto>> CreateForUserAsync(int userId, CreateBookingRequestDto dto);
        Task<IReadOnlyList<BookingDto>> GetMineAsync(int userId, BookingStatus? status);
        Task CancelAsync(int bookingId, int userId, bool isAdmin);
        Task ApproveAsync(int bookingId);
        Task PickUpAsync(int bookingId, int userId, bool isAdmin);
        Task ReturnAsync(int bookingId, int userId, bool isAdmin);
        Task<int> MarkOverdueAsync();
    }
}
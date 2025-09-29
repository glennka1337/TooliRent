using AutoMapper;
using AutoMapper.QueryableExtensions;
using TooliRent.Core.Interfaces;
using TooliRent.Core.Models;
using TooliRent.Core.Models.Enums;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IToolRepository _tools;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository repo, IToolRepository tools, IMapper mapper)
        {
            _repo = repo;
            _tools = tools;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BookingDto>> CreateForUserAsync(int userId, CreateBookingRequestDto dto)
        {
            // Basic domain checks
            if (dto.StartDate >= dto.EndDate) throw new ArgumentException("StartDate must be before EndDate.");

            var created = new List<BookingDto>(dto.ToolIds.Count);

            foreach (var toolId in dto.ToolIds.Distinct())
            {
                var tool = await _tools.GetByIdAsync(toolId);
                if (tool is null || !tool.IsActive)
                    throw new ArgumentException($"Tool {toolId} not found or inactive.");

                var blocks = await _repo.HasBlockingOverlapAsync(toolId, dto.StartDate, dto.EndDate);
                if (blocks)
                    throw new ArgumentException($"Tool {tool.Name} is not available in the requested period.");

                var entity = new Booking
                {
                    UserId = userId,
                    ToolId = toolId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Status = BookingStatus.Requested,
                    CreatedUtc = DateTime.UtcNow
                };

                await _repo.AddAsync(entity);

                created.Add(new BookingDto(entity.Id, entity.ToolId, tool.Name, entity.StartDate, entity.EndDate, entity.Status));
            }

            return created;
        }

        public async Task<IReadOnlyList<BookingDto>> GetMineAsync(int userId, BookingStatus? status)
        {
            var bookings = await _repo.GetByUserAsync(userId, status);
            return bookings.Select(b => new BookingDto(b.Id, b.ToolId, b.Tool.Name, b.StartDate, b.EndDate, b.Status)).ToList();
        }

        public async Task CancelAsync(int bookingId, int userId, bool isAdmin)
        {
            var b = await _repo.GetByIdAsync(bookingId) ?? throw new KeyNotFoundException("Booking not found.");

            if (!isAdmin && b.UserId != userId) throw new UnauthorizedAccessException("Not your booking.");

            if (b.Status == BookingStatus.Returned || b.Status == BookingStatus.Cancelled)
                throw new ArgumentException("Booking already completed or cancelled.");

            if (!isAdmin && (b.Status == BookingStatus.PickedUp || DateTime.UtcNow >= b.StartDate))
                throw new ArgumentException("Cannot cancel after pickup or after start.");

            b.Status = BookingStatus.Cancelled;
            await _repo.UpdateAsync(b);
        }

        public async Task ApproveAsync(int bookingId)
        {
            var b = await _repo.GetByIdAsync(bookingId) ?? throw new KeyNotFoundException("Booking not found.");
            if (b.Status != BookingStatus.Requested) throw new ArgumentException("Only requested bookings can be approved.");

            var blocks = await _repo.HasBlockingOverlapAsync(b.ToolId, b.StartDate, b.EndDate, excludeBookingId: b.Id);
            if (blocks) throw new ArgumentException("Booking period is no longer available.");

            b.Status = BookingStatus.Approved;
            await _repo.UpdateAsync(b);
        }

        public async Task PickUpAsync(int bookingId, int userId, bool isAdmin)
        {
            var b = await _repo.GetByIdAsync(bookingId) ?? throw new KeyNotFoundException("Booking not found.");
            if (!isAdmin && b.UserId != userId) throw new UnauthorizedAccessException("Not your booking.");
            if (b.Status != BookingStatus.Approved) throw new ArgumentException("Only approved bookings can be picked up.");
            if (DateTime.UtcNow < b.StartDate) throw new ArgumentException("Cannot pick up before start date.");

            b.Status = BookingStatus.PickedUp;
            await _repo.UpdateAsync(b);
        }

        public async Task ReturnAsync(int bookingId, int userId, bool isAdmin)
        {
            var b = await _repo.GetByIdAsync(bookingId) ?? throw new KeyNotFoundException("Booking not found.");
            if (!isAdmin && b.UserId != userId) throw new UnauthorizedAccessException("Not your booking.");
            if (b.Status != BookingStatus.PickedUp && b.Status != BookingStatus.Overdue)
                throw new ArgumentException("Only picked up or overdue bookings can be returned.");

            b.Status = BookingStatus.Returned;
            await _repo.UpdateAsync(b);
        }

        public Task<int> MarkOverdueAsync()
        {
            return _repo.MarkOverdueAsync(DateTime.UtcNow);
        }
    }
}
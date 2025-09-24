using System;
using TooliRent.Core.Models.Enums;

namespace TooliRent.Core.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ToolId { get; set; }
        public Tool Tool { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Requested;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TooliRent.Core.Models.Enums;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services;

namespace TooliRent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingsController(IBookingService service)
        {
            _service = service;
        }

        private int GetUserId()
        {
            var uid = User.FindFirstValue("uid");
            if (int.TryParse(uid, out var id)) return id;

            var nameId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(nameId, out id)) return id;

            throw new FormatException("Authenticated user id is missing or invalid.");
        }

        private bool IsAdmin() => User.IsInRole("Admin");

        // Create one booking per toolId
        [HttpPost]
        public async Task<ActionResult<IReadOnlyList<BookingDto>>> Create([FromBody] CreateBookingRequestDto dto)
        {
            var result = await _service.CreateForUserAsync(GetUserId(), dto);
            return Created(string.Empty, result);
        }

        // Current user's bookings
        [HttpGet("mine")]
        public async Task<ActionResult<IReadOnlyList<BookingDto>>> Mine([FromQuery] BookingStatus? status)
        {
            var result = await _service.GetMineAsync(GetUserId(), status);
            return Ok(result);
        }

        // Cancel booking (users can cancel their own, admins can cancel all)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _service.CancelAsync(id, GetUserId(), IsAdmin());
            return NoContent();
        }

        // Admins can approve requested bookings
        [HttpPost("{id:int}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            await _service.ApproveAsync(id);
            return NoContent();
        }

        // Pickup (member or admin)
        [HttpPost("{id:int}/pickup")]
        public async Task<IActionResult> Pickup(int id)
        {
            await _service.PickUpAsync(id, GetUserId(), IsAdmin());
            return NoContent();
        }

        // Return (member or admin)
        [HttpPost("{id:int}/return")]
        public async Task<IActionResult> Return(int id)
        {
            await _service.ReturnAsync(id, GetUserId(), IsAdmin());
            return NoContent();
        }

        // Admin: sweep overdue bookings
        [HttpPost("overdue/sweep")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> SweepOverdue()
        {
            var count = await _service.MarkOverdueAsync();
            return Ok(count);
        }
    }
}
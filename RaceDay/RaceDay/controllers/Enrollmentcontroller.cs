using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.DTOs;
using RaceDay.Models;


namespace RaceDay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Participant enrolls in an event under a category.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> Enroll(EnrollmentDto request)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null || category.EventId != request.EventId)
                return BadRequest("Invalid category or event.");

            var alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.ParticipantId == userId && e.EventId == request.EventId);
            if (alreadyEnrolled)
                return BadRequest("You are already enrolled in this event.");

            var enrollment = new Enrollment
            {
                ParticipantId = userId.Value,
                EventId = request.EventId,
                CategoryId = request.CategoryId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return Ok("Enrolled successfully.");
        }

        /// <summary>
        /// Organiser views all enrollments for their event.
        /// </summary>
        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetForEvent(int eventId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var ownsEvent = await _context.Events
                .AnyAsync(e => e.EventId == eventId && e.OrganiserId == userId);
            if (!ownsEvent) return Forbid();

            var enrollments = await _context.Enrollments
                .Include(e => e.Participant)
                .Include(e => e.Category)
                .Where(e => e.EventId == eventId)
                .Select(e => new
                {
                    e.EnrollmentId,
                    Participant = e.Participant!.FullName,
                    Category = e.Category!.Name,
                    e.FinishTime,
                    e.Position
                })
                .ToListAsync();

            return Ok(enrollments);
        }

        /// <summary>
        /// Participant views their own enrollments.
        /// </summary>
        [HttpGet("mine")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMine()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var enrollments = await _context.Enrollments
                .Include(e => e.Event)
                .Include(e => e.Category)
                .Where(e => e.ParticipantId == userId)
                .Select(e => new
                {
                    e.EnrollmentId,
                    Event = e.Event!.Name,
                    Category = e.Category!.Name,
                    EventDate = e.Event.Date,
                    e.FinishTime,
                    e.Position
                })
                .ToListAsync();

            return Ok(enrollments);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;


    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RaceDay.DTOS;
    

    namespace RaceDay.controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ResultsController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public ResultsController(ApplicationDbContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Organiser records the result for an enrollment.
            /// </summary>
            [HttpPut("capture")]
            [Authorize(Roles = "Organiser")]
            public async Task<IActionResult> Capture(ResultDto request)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var enrollment = await _context.Enrollments
                    .Include(e => e.Event)
                    .FirstOrDefaultAsync(e => e.EnrollmentId == request.EnrollmentId);

                if (enrollment == null) return NotFound("Enrollment not found.");
                if (enrollment.Event!.OrganiserId != userId) return Forbid();

                enrollment.FinishTime = request.FinishTime;
                enrollment.Position = request.Position;

                await _context.SaveChangesAsync();
                return Ok("Result captured.");
            }

            /// <summary>
            /// Participant views their own results.
            /// </summary>
            [HttpGet("mine")]
            [Authorize(Roles = "Participant")]
            public async Task<IActionResult> GetMine()
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var results = await _context.Enrollments
                    .Include(e => e.Event)
                    .Where(e => e.ParticipantId == userId && e.FinishTime != null)
                    .Select(e => new
                    {
                        Event = e.Event!.Name,
                        e.FinishTime,
                        e.Position
                    })
                    .ToListAsync();

                return Ok(results);
            }
        }
    }


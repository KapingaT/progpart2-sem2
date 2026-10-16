using System;
using System.Collections.Generic;
using System.Text;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RaceDay.DTOS;
    using RaceDay.Models;
    using RaceDay.Data;
  
    namespace RaceDay.controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class EventsController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public EventsController(ApplicationDbContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Gets all events. Any logged-in user can view.
            /// </summary>
            [HttpGet]
            [Authorize]
            public async Task<IActionResult> GetAll()
            {
                var events = await _context.Events
                    .Include(e => e.Categories)
                    .Select(e => new
                    {
                        e.EventId,
                        e.Name,
                        e.Description,
                        e.Date,
                        e.Location,
                        e.Distance,
                        e.EventType,
                        Organiser = e.Organiser!.FullName,
                        Categories = e.Categories.Select(c => c.Name)
                    })
                    .ToListAsync();

                return Ok(events);
            }

            /// <summary>
            /// Gets a single event by ID.
            /// </summary>
            [HttpGet("{id}")]
            [Authorize]
            public async Task<IActionResult> GetById(int id)
            {
                var ev = await _context.Events
                    .Include(e => e.Categories)
                    .FirstOrDefaultAsync(e => e.EventId == id);

                if (ev == null) return NotFound("Event not found.");
                return Ok(ev);
            }

            /// <summary>
            /// Creates a new event. Only Organisers.
            /// </summary>
            [HttpPost]
            [Authorize(Roles = "Organiser")]
            public async Task<IActionResult> Create(EventDto request)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null) return Unauthorized();

                var ev = new Event
                {
                    Name = request.Name,
                    Description = request.Description,
                    Date = request.Date,
                    Location = request.Location,
                    Distance = request.Distance,
                    EventType = request.EventType,
                    OrganiserId = userId.Value
                };

                _context.Events.Add(ev);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Event created.", EventId = ev.EventId });
            }

            /// <summary>
            /// Updates an event. Only the owning Organiser.
            /// </summary>
            [HttpPut("{id}")]
            [Authorize(Roles = "Organiser")]
            public async Task<IActionResult> Update(int id, EventDto request)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var ev = await _context.Events.FindAsync(id);

                if (ev == null) return NotFound("Event not found.");
                if (ev.OrganiserId != userId) return Forbid();

                ev.Name = request.Name;
                ev.Description = request.Description;
                ev.Date = request.Date;
                ev.Location = request.Location;
                ev.Distance = request.Distance;
                ev.EventType = request.EventType;

                await _context.SaveChangesAsync();
                return Ok("Event updated.");
            }

            /// <summary>
            /// Deletes an event. Only the owning Organiser.
            /// </summary>
            [HttpDelete("{id}")]
            [Authorize(Roles = "Organiser")]
            public async Task<IActionResult> Delete(int id)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var ev = await _context.Events.FindAsync(id);

                if (ev == null) return NotFound("Event not found.");
                if (ev.OrganiserId != userId) return Forbid();

                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
                return Ok("Event deleted.");
            }
        }
    }
}

 using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RaceDay.DTOS;
    using RaceDay.Data;
    using RaceDay.Models;
    
    namespace RaceDay.controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class CategoriesController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public CategoriesController(ApplicationDbContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Get all categories for a specific event. Any authenticated user.
            /// </summary>
            [HttpGet("event/{eventId}")]
            [Authorize]
            public async Task<IActionResult> GetForEvent(int eventId)
            {
                var categories = await _context.Categories
                    .Where(c => c.EventId == eventId)
                    .ToListAsync();
                return Ok(categories);
            }

            /// <summary>
            /// Create a category for an event. Organiser only.
            /// </summary>
            [HttpPost]
            [Authorize(Roles = "Organiser")]
            public async Task<IActionResult> Create(CategoryDto request)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var ev = await _context.Events
                    .FirstOrDefaultAsync(e => e.EventId == request.EventId && e.OrganiserId == userId);

                if (ev == null) return NotFound("Event not found or not yours.");

                var category = new Category
                {
                    Name = request.Name,
                    Description = request.Description,
                    EventId = request.EventId
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return Ok(category);
            }
        }
    }


using System;
using System.Collections.Generic;
using System.Text;

namespace RaceDay.Models
{
    using System.ComponentModel.DataAnnotations;

    namespace RaceDayAPI.Models
    {
        public class Event
        {
            [Key]
            public int EventId { get; set; }

            [Required]
            public string Name { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            [Required]
            public DateTime Date { get; set; }

            [Required]
            public string Location { get; set; } = string.Empty;

            public double Distance { get; set; }

            [Required]
            public string EventType { get; set; } = string.Empty; // Run, Walk, Cycle

            // Foreign Key
            public int OrganiserId { get; set; }
            public User? Organiser { get; set; }

            // Navigation
            public ICollection<Category> Categories { get; set; } = new List<Category>();
        }
    }
}

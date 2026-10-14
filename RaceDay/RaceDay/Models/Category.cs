using System;
using System.Collections.Generic;
using System.Text;

namespace RaceDay.Models
{
    using System.ComponentModel.DataAnnotations;

    namespace RaceDayAPI.Models
    {
        public class Category
        {
            [Key]
            public int CategoryId { get; set; }

            [Required]
            public string Name { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            // Foreign Key
            public int EventId { get; set; }
            public Event? Event { get; set; }

            // Navigation
            public ICollection<Enrollment> Enrollment { get; set; } = new List<Enrollment>();
        }
    }
}

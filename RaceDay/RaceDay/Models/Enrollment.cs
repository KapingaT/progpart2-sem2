using System;
using System.Collections.Generic;
using System.Text;

 using System.ComponentModel.DataAnnotations;

    namespace RaceDay.Models
    {
        public class Enrollment
        {
            [Key]
            public int EnrollmentId { get; set; }

            // Foreign Keys
            public int ParticipantId { get; set; }
            public User? Participant { get; set; }

            public int EventId { get; set; }
            public Event? Event { get; set; }

            public int CategoryId { get; set; }
            public Category? Category { get; set; }

            // Results (captured by Organiser after the event)
            public string? FinishTime { get; set; }
            public int? Position { get; set; }
        }
    }


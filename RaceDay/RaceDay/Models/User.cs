using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace RaceDay.Models
{
        public class User
        {
            [Key]// used to indicate primary keys 
            public int UserId { get; set; }

            [Required]// tells them its mandiory cant be null
            public string FullName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            public string PasswordHash { get; set; } = string.Empty;

            [Required]
            public string Role { get; set; } = "Participant"; // "Organiser" or "Participant"

            // Navigation properties
            public ICollection<Event> OrganisedEvents { get; set; } = new List<Event>();
            public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
            // ICollection is a collection of things  of enrollment
            // organised evnts is the name of the property 
            //event is the thing being stored 
        }
    } 


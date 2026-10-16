using Microsoft.EntityFrameworkCore;
using RaceDay.Models;
using RaceDay.Test.Helpers.RaceDay.Tests.Helpers;
using RaceDay.Tests.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RaceDay.Tests.Tests
{
    public class EnrollmentTests
    {
        [Fact]
        public async Task Enrollment_RecordsParticipantAndCategory()
        {
            var context = TestDbContextFactory.Create();

            var participant = new User { FullName = "P", Email = "p@t.com", PasswordHash = "x", Role = "Participant" };
            var organiser = new User { FullName = "O", Email = "o@t.com", PasswordHash = "x", Role = "Organiser" };
            context.Users.AddRange(participant, organiser);
            await context.SaveChangesAsync();

            var ev = new Event
            {
                Name = "E",
                Description = "Desc",
                Date = DateTime.Now,
                Location = "Loc",
                Distance = 5,
                EventType = "Run",
                OrganiserId = organiser.UserId
            };
            context.Events.Add(ev);
            await context.SaveChangesAsync();

            var cat = new Category
            {
                Name = "Senior",
                Description = "Senior category",
                EventId = ev.EventId
            };
            context.Categories.Add(cat);
            await context.SaveChangesAsync();

            context.Enrollments.Add(new Enrollment
            {
                ParticipantId = participant.UserId,
                EventId = ev.EventId,
                CategoryId = cat.CategoryId
            });
            await context.SaveChangesAsync();

            var enrollment = await context.Enrollments
                .Include(e => e.Participant)
                .Include(e => e.Category)
                .FirstAsync();

            Assert.Equal("P", enrollment.Participant!.FullName);
            Assert.Equal("Senior", enrollment.Category!.Name);
        }
    }
}
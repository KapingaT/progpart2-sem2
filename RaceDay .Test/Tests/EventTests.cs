
    using global::RaceDay.Test.Helpers.RaceDay.Tests.Helpers;
    using Microsoft.EntityFrameworkCore;
    using RaceDay.Models;
    using RaceDay.Tests.Helpers;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    namespace RaceDay.Tests.Tests
    {
        public class EventTests
        {
            [Fact]
            public async Task CreateEvent_SavesToDatabase()
            {
                var context = TestDbContextFactory.Create();

                var organiser = new User
                {
                    FullName = "O",
                    Email = "o@t.com",
                    PasswordHash = "x",
                    Role = "Organiser"
                };
                context.Users.Add(organiser);
                await context.SaveChangesAsync();

                context.Events.Add(new Event
                {
                    Name = "Comrades",
                    Description = "Ultra marathon",
                    Date = DateTime.Now.AddDays(30),
                    Location = "Durban",
                    Distance = 89,
                    EventType = "Run",
                    OrganiserId = organiser.UserId
                });
                await context.SaveChangesAsync();

                var saved = await context.Events.FirstAsync();
                Assert.Equal("Comrades", saved.Name);
            }

            [Fact]
            public async Task GetEvents_ReturnsAll()
            {
                var context = TestDbContextFactory.Create();

                var organiser = new User
                {
                    FullName = "O",
                    Email = "o@t.com",
                    PasswordHash = "x",
                    Role = "Organiser"
                };
                context.Users.Add(organiser);
                await context.SaveChangesAsync();

                context.Events.Add(new Event
                {
                    Name = "E1",
                    Description = "D1",
                    Date = DateTime.Now,
                    Location = "L1",
                    Distance = 5,
                    EventType = "Run",
                    OrganiserId = organiser.UserId
                });
                context.Events.Add(new Event
                {
                    Name = "E2",
                    Description = "D2",
                    Date = DateTime.Now,
                    Location = "L2",
                    Distance = 10,
                    EventType = "Walk",
                    OrganiserId = organiser.UserId
                });
                await context.SaveChangesAsync();

                var events = await context.Events.ToListAsync();

                Assert.Equal(2, events.Count);
            }
        }
    }
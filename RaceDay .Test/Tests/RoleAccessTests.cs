using Microsoft.EntityFrameworkCore;
using RaceDay.Models;
using RaceDay.Test.Helpers.RaceDay.Tests.Helpers;
using RaceDay.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RaceDay.Tests.Tests
{
    public class RoleAccessTests
    {
        [Fact]
        public async Task Participant_CannotBeOrganiser()
        {
            var context = TestDbContextFactory.Create();
            context.Users.Add(new User
            {
                FullName = "Jane",
                Email = "jane@test.com",
                PasswordHash = "x",
                Role = "Participant"
            });
            await context.SaveChangesAsync();

            var user = await context.Users.FirstAsync();

            Assert.Equal("Participant", user.Role);
            Assert.NotEqual("Organiser", user.Role);
        }
    }
}

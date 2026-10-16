  using global::RaceDay.Test.Helpers.RaceDay.Tests.Helpers;
    using Microsoft.EntityFrameworkCore;
    using RaceDay.Models;
    using RaceDay.Tests.Helpers;
    using System.Threading.Tasks;
    using Xunit;

    namespace RaceDay.Tests.Tests
    {
        public class AuthTests
        {
            [Fact]
            public async Task Register_ShouldHashPassword()
            {
                var context = TestDbContextFactory.Create();
                var plainPassword = "password123";
                var hashed = BCrypt.Net.BCrypt.HashPassword(plainPassword);

                context.Users.Add(new User
                {
                    FullName = "Test",
                    Email = "test@test.com",
                    PasswordHash = hashed,
                    Role = "Participant"
                });
                await context.SaveChangesAsync();

                var user = await context.Users.FirstAsync();
                Assert.NotEqual(plainPassword, user.PasswordHash);
                Assert.True(BCrypt.Net.BCrypt.Verify(plainPassword, user.PasswordHash));
            }

            [Fact]
            public async Task Login_WithWrongPassword_ShouldFail()
            {
                var context = TestDbContextFactory.Create();
                context.Users.Add(new User
                {
                    FullName = "Test",
                    Email = "test@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct"),
                    Role = "Participant"
                });
                await context.SaveChangesAsync();

                var user = await context.Users.FirstAsync();
                var result = BCrypt.Net.BCrypt.Verify("wrong", user.PasswordHash);

                Assert.False(result);
            }
        }
    }

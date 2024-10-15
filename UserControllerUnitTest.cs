using DailyPlanManager;
using DailyPlanManager.Data;
using DailyPlanManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DPMTest.Tests
{
    public class UserControllerUnitTest
    {
        private UserController _controller;
        private ApplicationDbContext _context;

        public UserControllerUnitTest()
        {
            // Setup in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                          .UseInMemoryDatabase(databaseName: "TestDatabase")
                          .Options;

            _context = new ApplicationDbContext(options);

            // Add test users to the in-memory database
            _context.User.AddRange(new List<User>
            {
                new User { Username = "Ronaldo" },
                new User { Username = "Messi" }
            });
            _context.SaveChanges();

            _controller = new UserController(_context);
        }

        [Fact]
        public void Index_Returns_View_With_Users_When_Context_Has_Users()
        {
            // Act
            var result = _controller.Index() as ViewResult;
            var model = result?.Model as List<User>;

            // Assert
            Assert.NotNull(result); // Check if the result is not null
            Assert.NotNull(model);  // Ensure the model is not null
            Assert.Equal(2, model.Count); // Check if the model contains exactly 2 users
            Assert.Equal("Select Users:", result.ViewData["Message"]);
        }

        [Fact]
        public void Index_Returns_Message_When_Context_Has_No_Users()
        {
            // Arrange - Clear existing users from the context
            _context.User.RemoveRange(_context.User);
            _context.SaveChanges();

            // Act
            var result = _controller.Index() as ViewResult;
            var model = result?.Model as List<User>;

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Empty(model);    // Check that the model (list of users) is empty
            Assert.Equal("No user created", result.ViewData["Message"]);
        }

    }
}

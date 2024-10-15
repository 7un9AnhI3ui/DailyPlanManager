using System;
using System.Runtime.Intrinsics.Arm;
using DailyPlanManager;
using DailyPlanManager.Data;
using DailyPlanManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

public class DailyPlanControllerUnitTest
{
    private DailyPlanController _dpcontroller;
    private ApplicationDbContext _context;
    public DailyPlanControllerUnitTest()
    {

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database per test
                      .Options;

        _context = new ApplicationDbContext(options);

        // Add sample data
        _context.User.AddRange(new List<User>
        {
            new User { Id = 1, Username = "Ronaldo" },
            new User { Id = 2, Username = "Messi" }
        });
        _context.SaveChanges();

        _context.DailyPlan.AddRange(new List<DailyPlan>
        {
            new DailyPlan { User_Id = 1, Title = "Sleep", Description = "go to bed", Date = new DateOnly(2024, 10, 6) },
            new DailyPlan { User_Id = 1, Title = "Sleep", Description = "go to bed", Date = new DateOnly(2024, 10, 6) },
            new DailyPlan { User_Id = 2, Title = "Train", Description = "football training", Date = new DateOnly(2024, 10, 6) },
            new DailyPlan { User_Id = 2, Title = "Train", Description = "football training", Date = new DateOnly(2024, 10, 7) }
        });
        _context.SaveChanges();

        // Mock HttpContext and Session
        var mockHttpContext = new Mock<HttpContext>();
        var mockSession = new Mock<ISession>();

        // Mock Set method to handle storing session values
        mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>())).Verifiable();
        //set up a session by setting the value of any string to thesession by set value of any string to value of any byte[] they can find

        // Mock TryGetValue to simulate retrieval from session
        byte[] outValue = BitConverter.GetBytes(1); // Simulate stored user ID = 1
        //convert 

        mockSession.Setup(s => s.TryGetValue("user", out outValue))
                   .Returns(true); // Simulate successful retrieval from session
        //set up session by try to get any value of the string in the session if it true = outValue

        // Setup HttpContext to return the mocked session
        mockHttpContext.Setup(ctx => ctx.Session).Returns(mockSession.Object);

        // Set HttpContext for the controller
        _dpcontroller = new DailyPlanController(_context)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            }
        };
    }

    [Fact]
    public void Index_Returns_View_With_Daily_Plan()
    {
        var result = _dpcontroller.Index(1) as ViewResult;
        var model = result?.Model as List<DailyPlan>;
        var result2 = _dpcontroller.Index(2) as ViewResult;
        var model2 = result2?.Model as List<DailyPlan>;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(model);
        Assert.NotNull(result2);
        Assert.NotNull(model2);
        Assert.Equal(2, model.Count);
        Assert.Equal(2, model2.Count);
    }

    [Fact]
    public void Create_Returns_View_With_Create_Daily_Plan()
    {

        var createresult = _dpcontroller.Create() as ViewResult;
        var newplan = new DailyPlan { Title = "An", Description = "an com", Date = new DateOnly(2024, 10, 14) };
        _dpcontroller.Create(newplan);

        var indexresult = _dpcontroller.Index(1) as ViewResult;
        var model = indexresult?.Model as List<DailyPlan>;

        // Assert
        Assert.NotNull(createresult);
        Assert.Equal(3, model?.Count);
    }

}
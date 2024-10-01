using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DailyPlanManager.Models;
using DailyPlanManager.Data;

namespace DailyPlanManager;

public class UserController : Controller
{
    //private readonly ILogger<UserController> _logger;
    private readonly ApplicationDbContext _context;

    /*public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }*/

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        if (_context.User != null)
        {
            List<User> users = _context.User.ToList();
            return View(users);
        } 
        var messages = "No user created";
        return View(messages);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


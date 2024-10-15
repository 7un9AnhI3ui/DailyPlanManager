using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DailyPlanManager.Models;
using DailyPlanManager.Data;

namespace DailyPlanManager;

public class UserController : Controller
{

    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        List<User> users = new List<User>(); // Initialize an empty list by default

        if (_context.User != null)
        {
            users = _context.User.ToList();
        }

        if (users.Any()) // Check if the list has any users
        {
            ViewBag.Message = "Select Users:";
        }
        else
        {
            ViewBag.Message = "No user created";
        }

        return View(users); // Always return a list (either empty or populated)
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


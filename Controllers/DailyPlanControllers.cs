using Microsoft.AspNetCore.Mvc;
using DailyPlanManager.Models;
using DailyPlanManager.Data;

namespace DailyPlanManager;

public class DailyPlanController : Controller
{

    private readonly ApplicationDbContext _context;

    
    public DailyPlanController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index(int id)
    {
        HttpContext.Session.SetInt32("user", id);
        var userplan = _context.DailyPlan.Where(plan => plan.User_Id == id).ToList();
        if (userplan == null)
        {
            return NotFound();
        }
        return View(userplan);
    }
    public IActionResult Create()
    {
        var user = HttpContext.Session.GetInt32("user");
        if (user == null)
        {
            return Redirect("https://localhost:7289");
        }
        return View();
    }
    [HttpPost]
    public IActionResult Create(DailyPlan dailyPlan)
    {
        var user = HttpContext.Session.GetInt32("user");
        dailyPlan.User_Id = user;
        if (ModelState.IsValid)
        {
            _context.DailyPlan.Add(dailyPlan);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { id = user.Value });
        }
        return View(dailyPlan);
    }
}


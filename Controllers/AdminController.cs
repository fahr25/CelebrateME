using Microsoft.AspNetCore.Mvc;

namespace CelebrateME.Controllers;

public class AdminController : Controller
{
    public IActionResult Home()
    {
        return View();
    }
}
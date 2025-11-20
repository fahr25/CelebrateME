using Microsoft.AspNetCore.Mvc;

namespace CelebrateME.Controllers;

public class MarketController : Controller
{
    public IActionResult Start()
    {
        return View();
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CelebrateME.Models;

namespace CelebrateME.Controllers;

public class ShopController : Controller
{
    private readonly CelebrateMEDbContext _db;
    public ShopController(CelebrateMEDbContext db) => _db = db;

    public IActionResult LoginCode() => View();

    [HttpPost]
    public async Task<IActionResult> LoginCode(string code)
    {
        if (string.IsNullOrEmpty(code)) { ModelState.AddModelError("", "Enter code"); return View(); }

        var ok = await _db.AccessCodes.AnyAsync(c => c.Code == code && c.IsActive);
        if (!ok) { ModelState.AddModelError("", "Invalid code"); return View(); }

        // Save code in session
        HttpContext.Session.SetString("ShopCode", code);
        return RedirectToAction(nameof(Browse));
    }

    public async Task<IActionResult> Browse()
    {
        var products = await _db.Set<Product>().Include(p => p.Category).ToListAsync();
        return View(products);
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, int qty = 1)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<(int id, int qty)>>("Cart") ?? new();
        var item = cart.FirstOrDefault(x => x.id == productId);
        if (item != default)
        {
            cart.Remove(item);
            cart.Add((productId, item.qty + qty));
        }
        else cart.Add((productId, qty));
        HttpContext.Session.SetObjectAsJson("Cart", cart);
        return RedirectToAction(nameof(Browse));
    }

    public async Task<IActionResult> Checkout()
    {
        var code = HttpContext.Session.GetString("ShopCode");
        if (string.IsNullOrEmpty(code)) return RedirectToAction(nameof(LoginCode));

        var cart = HttpContext.Session.GetObjectFromJson<List<(int id, int qty)>>("Cart") ?? new();
        if (!cart.Any()) return RedirectToAction(nameof(Browse));

        var products = await _db.Set<Product>().Where(p => cart.Select(c => c.id).Contains(p.Id)).ToListAsync();
        var order = new Order { CustomerCode = code };
        foreach (var c in cart)
        {
            var p = products.First(p2 => p2.Id == c.id);
            order.Items.Add(new OrderItem { ProductId = p.Id, Quantity = c.qty, PointsEach = p.Points });
            order.TotalPoints += p.Points * c.qty;
            // optionally reduce inventory:
            p.Inventory -= c.qty;
        }
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        HttpContext.Session.Remove("Cart");
        return RedirectToAction("Thanks");
    }

    public IActionResult Thanks() => View();
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CelebrateME.Models;

namespace CelebrateME.Controllers;

public class AdminController : Controller
{
    private readonly CelebrateMEDbContext _db;
    public AdminController(CelebrateMEDbContext db) => _db = db;

    
    // GET: /Admin or /Admin/Index
    public IActionResult Index()
    {
        return View();
    }

    // GET: /Admin/Home
    public async Task<IActionResult> Home()
    {
        var products = await _db.Set<Product>().Include(p => p.Category).ToListAsync();
        return View(products);
    }

    // GET: /Admin/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    // POST: /Admin/Create
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name", product?.CategoryId);
            return View(product);
        }
        _db.Add(product);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: /Admin/Details/{id}
    public async Task<IActionResult> Details(int id)
    {
        var p = await _db.Set<Product>().Include(p2 => p2.Category).FirstOrDefaultAsync(p2 => p2.Id == id);
        if (p == null) return NotFound();
        return View(p);
    }

    // GET: /Admin/Edit/{id}
    public async Task<IActionResult> Edit(int id)
    {
        var p = await _db.Set<Product>().FindAsync(id);
        if (p == null) return NotFound();
        ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name", p.CategoryId);
        return View(p);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    // POST: /Admin/Edit/{id}
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }
        _db.Update(product);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: /Admin/Delete/{id}
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Set<Product>().Include(p2 => p2.Category).FirstOrDefaultAsync(p2 => p2.Id == id);
        if (p == null) return NotFound();
        return View(p);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    // POST: /Admin/Delete/{id}
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var p = await _db.Set<Product>().FindAsync(id);
        if (p != null)
        {
            _db.Remove(p);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // Orders listing
    // GET: /Admin/Orders
    public async Task<IActionResult> Orders()
    {
        var orders = await _db.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToListAsync();
        return View(orders);
    }
}
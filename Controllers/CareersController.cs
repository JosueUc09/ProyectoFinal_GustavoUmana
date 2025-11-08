
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    [Authorize(Roles="Admin")]
    public class CareersController : Controller
    {
        private readonly AppDbContext _db;
        public CareersController(AppDbContext db) { _db = db; }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var list = await _db.Careers.AsNoTracking().ToListAsync();
            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Career c)
        {
            if (!ModelState.IsValid) return View(c);
            _db.Add(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Careers.FindAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Career c)
        {
            if (!ModelState.IsValid) return View(c);
            _db.Update(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Careers.FindAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _db.Careers.FindAsync(id);
            if (c != null) { _db.Remove(c); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    [Authorize(Roles="Admin")]
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;
        public CoursesController(AppDbContext db) { _db = db; }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var list = await _db.Courses.Include(c=>c.Career).AsNoTracking().ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.CareerId = new SelectList(_db.Careers.AsNoTracking().ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course c)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CareerId = new SelectList(_db.Careers.AsNoTracking().ToList(), "Id", "Name", c.CareerId);
                return View(c);
            }
            _db.Add(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Courses.FindAsync(id);
            if (c == null) return NotFound();
            ViewBag.CareerId = new SelectList(_db.Careers.AsNoTracking().ToList(), "Id", "Name", c.CareerId);
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Course c)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CareerId = new SelectList(_db.Careers.AsNoTracking().ToList(), "Id", "Name", c.CareerId);
                return View(c);
            }
            _db.Update(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Courses.Include(x=>x.Career).FirstOrDefaultAsync(x=>x.Id==id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _db.Courses.FindAsync(id);
            if (c != null) { _db.Remove(c); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}

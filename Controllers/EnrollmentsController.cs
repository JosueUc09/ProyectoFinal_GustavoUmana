
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    [Authorize]
    public class EnrollmentsController : Controller
    {
        private readonly AppDbContext _db;
        public EnrollmentsController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var list = await _db.Enrollments.Include(e=>e.Course).ThenInclude(c=>c.Career).AsNoTracking().ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_db.Courses.Include(c=>c.Career).AsNoTracking()
                .Select(c => new { c.Id, Label = c.Name + " (" + c.Career!.Name + ")" }).ToList(), "Id", "Label");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Enrollment e)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CourseId = new SelectList(_db.Courses.Include(c=>c.Career).AsNoTracking()
                    .Select(c => new { c.Id, Label = c.Name + " (" + c.Career!.Name + ")" }).ToList(), "Id", "Label", e.CourseId);
                return View(e);
            }
            _db.Add(e);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var e = await _db.Enrollments.FindAsync(id);
            if (e == null) return NotFound();
            ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking().ToList(), "Id", "Name", e.CourseId);
            return View(e);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Enrollment e)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CourseId = new SelectList(_db.Courses.AsNoTracking().ToList(), "Id", "Name", e.CourseId);
                return View(e);
            }
            _db.Update(e);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var e = await _db.Enrollments.Include(x=>x.Course).FirstOrDefaultAsync(x=>x.Id==id);
            if (e == null) return NotFound();
            return View(e);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var e = await _db.Enrollments.FindAsync(id);
            if (e != null) { _db.Remove(e); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}

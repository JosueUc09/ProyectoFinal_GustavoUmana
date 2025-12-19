using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;

        public CoursesController(AppDbContext db)
        {
            _db = db;
        }

        // =========================
        // LISTADO
        // =========================
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? credits, int page = 1)
        {
            int pageSize = 5;

            var query = _db.Courses
                .Include(c => c.Career)
                .Include(c => c.Teacher)
                .AsQueryable();

            // 🔹 FILTRO
            if (credits.HasValue)
                query = query.Where(c => c.Credits == credits);

            int totalItems = await query.CountAsync();

            var courses = await query
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.Credits = credits;

            return View(courses);
        }


        // =========================
        // CREATE (GET)
        // =========================
        public IActionResult Create()
        {
            LoadSelectLists();
            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course c)
        {
            if (!ModelState.IsValid)
            {
                LoadSelectLists(c);
                return View(c);
            }

            _db.Courses.Add(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT (GET)
        // =========================
        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Courses.FindAsync(id);
            if (c == null) return NotFound();

            LoadSelectLists(c);
            return View(c);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Course c)
        {
            if (!ModelState.IsValid)
            {
                LoadSelectLists(c);
                return View(c);
            }

            _db.Update(c);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Courses
                .Include(x => x.Career)
                .Include(x => x.Teacher)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _db.Courses.FindAsync(id);
            if (c != null)
            {
                _db.Courses.Remove(c);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Filter(int? credits, int page = 1)
        {
            int pageSize = 5;

            var query = _db.Courses
                .Include(c => c.Career)
                .Include(c => c.Teacher)
                .AsQueryable();

            if (credits.HasValue)
                query = query.Where(c => c.Credits == credits);

            int totalItems = await query.CountAsync();

            var list = await query
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.Credits = credits;

            return PartialView("_CourseTable", list);
        }
        private void LoadSelectLists(Course? c = null)
        {
            ViewBag.CareerId = new SelectList(
                _db.Careers.AsNoTracking(),
                "Id",
                "Name",
                c?.CareerId
            );

            ViewBag.TeacherId = new SelectList(
                _db.Teachers.AsNoTracking(),
                "Id",
                "Name",
                c?.TeacherId
            );
        }
    }
}

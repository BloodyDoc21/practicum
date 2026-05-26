using CleanLife.Web.Data;
using CleanLife.Web.Models;
using CleanLife.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CleanLife.Web.Controllers
{
    [Authorize]
    public class HabitController : Controller
    {
        private readonly IHabitService _habitService;

        private readonly ApplicationDbContext _context;

        public HabitController(
            IHabitService habitService,
            ApplicationDbContext context)
        {
            _habitService = habitService;
            _context = context;
        }

        // GET: /Habit
        public async Task<IActionResult> Index()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var habits =
                await _habitService
                .GetUserHabitsAsync(userId);

            return View(habits);
        }

        // GET: /Habit/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var habit =
                await _habitService
                .GetHabitByIdAsync(id, userId);

            if (habit == null)
                return NotFound();

            return View(habit);
        }

        // GET: /Habit/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Habit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Habit habit)
        {
            if (ModelState.IsValid)
            {
                var userId =
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier);

                await _habitService
                    .CreateHabitAsync(
                        habit,
                        userId,
                        Array.Empty<int>());

                return RedirectToAction(
                    nameof(Index));
            }

            return View(habit);
        }

        // GET: /Habit/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var habit =
                await _habitService
                .GetHabitByIdAsync(id, userId);

            if (habit == null)
                return NotFound();

            return View(habit);
        }

        // POST: /Habit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Habit habit)
        {
            if (id != habit.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var userId =
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier);

                var updated =
                    await _habitService
                    .UpdateHabitAsync(
                        habit,
                        userId,
                        Array.Empty<int>());

                if (updated == null)
                    return NotFound();

                return RedirectToAction(
                    nameof(Index));
            }

            return View(habit);
        }

        // GET: /Habit/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var habit =
                await _habitService
                .GetHabitByIdAsync(id, userId);

            if (habit == null)
                return NotFound();

            return View(habit);
        }

        // POST: /Habit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var success =
                await _habitService
                .DeleteHabitAsync(id, userId);

            if (!success)
                return NotFound();

            return RedirectToAction(
                nameof(Index));
        }
    }
}
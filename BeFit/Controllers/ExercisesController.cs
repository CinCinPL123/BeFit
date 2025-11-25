using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using System.Security.Claims;
using BeFit.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers
{
    [Authorize]
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var applicationDbContext = _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .Where(e => e.CreatedById == userId);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var exercise = await _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedById == userId);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // GET: Exercises/Create
        public IActionResult Create()
        {
            var userId = GetUserId();
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name");
            ViewData["SessionId"] = new SelectList(_context.Session.Where(s => s.CreatedById == userId), "Id", "StartTime");
            return View();
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SessionId,ExerciseTypeId,Weight,Series,Repetitions")] ExerciseDTO exerciseDTO)
        {
            if (ModelState.IsValid)
            {
                var session = await _context.Session.FindAsync(exerciseDTO.SessionId);
                var exerciseType = await _context.ExerciseType.FindAsync(exerciseDTO.ExerciseTypeId);

                var exercise = new Exercise
                {
                    SessionId = exerciseDTO.SessionId,
                    Session = session!,
                    ExerciseTypeId = exerciseDTO.ExerciseTypeId,
                    ExerciseType = exerciseType!,
                    Weight = exerciseDTO.Weight,
                    Series = exerciseDTO.Series,
                    Repetitions = exerciseDTO.Repetitions,
                    CreatedById = GetUserId()
                };
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var userId = GetUserId();
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name", exerciseDTO.ExerciseTypeId);
            ViewData["SessionId"] = new SelectList(_context.Session.Where(s => s.CreatedById == userId), "Id", "StartTime", exerciseDTO.SessionId);
            return View(exerciseDTO);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var exercise = await _context.Exercise.FindAsync(id);
            if (exercise == null || exercise.CreatedById != userId)
            {
                return NotFound();
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name", exercise.ExerciseTypeId);
            ViewData["SessionId"] = new SelectList(_context.Session.Where(s => s.CreatedById == userId), "Id", "StartTime", exercise.SessionId);
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionId,ExerciseTypeId,Weight,Series,Repetitions")] Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var existingExercise = await _context.Exercise.FindAsync(id);
            if (existingExercise == null || existingExercise.CreatedById != userId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingExercise.SessionId = exercise.SessionId;
                    existingExercise.ExerciseTypeId = exercise.ExerciseTypeId;
                    existingExercise.Weight = exercise.Weight;
                    existingExercise.Series = exercise.Series;
                    existingExercise.Repetitions = exercise.Repetitions;
                    _context.Update(existingExercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name", exercise.ExerciseTypeId);
            ViewData["SessionId"] = new SelectList(_context.Session.Where(s => s.CreatedById == userId), "Id", "StartTime", exercise.SessionId);
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var exercise = await _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedById == userId);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserId();
            var exercise = await _context.Exercise.FindAsync(id);
            if (exercise != null && exercise.CreatedById == userId)
            {
                _context.Exercise.Remove(exercise);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercise.Any(e => e.Id == id);
        }
    }
}

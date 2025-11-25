using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        // GET: Stats
        public async Task<IActionResult> Index()
        {
            var fourWeeksAgo = DateTime.Now.AddDays(-28);
            var userId = GetUserId();

            var statistics = await _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .Where(e => e.Session.StartTime >= fourWeeksAgo && e.CreatedById == userId)
                .GroupBy(e => e.ExerciseType.Name)
                .Select(g => new ExerciseStatistics
                {
                    ExerciseTypeName = g.Key,
                    ExecutionCount = g.Count(),
                    TotalRepetitions = g.Sum(e => e.Series * e.Repetitions),
                    AverageWeight = g.Where(e => e.Weight.HasValue).Average(e => e.Weight),
                    MaxWeight = g.Where(e => e.Weight.HasValue).Max(e => e.Weight)
                })
                .ToListAsync();

            return View(statistics);
        }
    }
}

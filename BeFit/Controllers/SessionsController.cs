using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using BeFit.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        // GET: Sessions
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            return View(await _context.Session.Where(s => s.CreatedById == userId).ToListAsync());
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var session = await _context.Session
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedById == userId);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // GET: Sessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime")] SessionDTO sessionDTO)
        {
            if (ModelState.IsValid)
            {
                var session = new Session
                {
                    StartTime = sessionDTO.StartTime,
                    EndTime = sessionDTO.EndTime,
                    CreatedById = GetUserId()
                };
                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sessionDTO);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var session = await _context.Session.FindAsync(id);
            if (session == null || session.CreatedById != userId)
            {
                return NotFound();
            }
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime")] Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var existingSession = await _context.Session.FindAsync(id);
            if (existingSession == null || existingSession.CreatedById != userId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingSession.StartTime = session.StartTime;
                    existingSession.EndTime = session.EndTime;
                    _context.Update(existingSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id))
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
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var session = await _context.Session
                .FirstOrDefaultAsync(m => m.Id == id && m.CreatedById == userId);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserId();
            var session = await _context.Session.FindAsync(id);
            if (session != null && session.CreatedById == userId)
            {
                _context.Session.Remove(session);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
            return _context.Session.Any(e => e.Id == id);
        }
    }
}

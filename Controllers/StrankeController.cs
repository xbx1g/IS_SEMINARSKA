using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoServis.Data;
using AutoServis.Models;
using Microsoft.AspNetCore.Authorization;

namespace AutoServis.Controllers
{
    [Authorize(Roles = "Administrator, Mehanik")]
    public class StrankeController : Controller
    {
        private readonly AutoServisContext _context;

        public StrankeController(AutoServisContext context)
        {
            _context = context;
        }

        // GET: Stranke
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stranke.ToListAsync());
        }

        // GET: Stranke/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stranka = await _context.Stranke
                .FirstOrDefaultAsync(m => m.ID == id);
            if (stranka == null)
            {
                return NotFound();
            }

            return View(stranka);
        }

        // GET: Stranke/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stranke/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Priimek,Ime,Telefon,Email,DatumRegistracije")] Stranka stranka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stranka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stranka);
        }

        // GET: Stranke/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stranka = await _context.Stranke.FindAsync(id);
            if (stranka == null)
            {
                return NotFound();
            }
            return View(stranka);
        }

        // POST: Stranke/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Priimek,Ime,Telefon,Email,DatumRegistracije")] Stranka stranka)
        {
            if (id != stranka.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stranka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StrankaExists(stranka.ID))
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
            return View(stranka);
        }

        // GET: Stranke/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stranka = await _context.Stranke
                .FirstOrDefaultAsync(m => m.ID == id);
            if (stranka == null)
            {
                return NotFound();
            }

            return View(stranka);
        }

        // POST: Stranke/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stranka = await _context.Stranke.FindAsync(id);
            if (stranka != null)
            {
                _context.Stranke.Remove(stranka);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StrankaExists(int id)
        {
            return _context.Stranke.Any(e => e.ID == id);
        }
    }
}
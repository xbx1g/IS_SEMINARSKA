using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoServis.Data;
using AutoServis.Models;
using Microsoft.AspNetCore.Authorization;

namespace AutoServis.Controllers
{
    [Authorize(Roles = "Administrator, Mehanik")]
    public class MehanikiController : Controller
    {
        private readonly AutoServisContext _context;

        public MehanikiController(AutoServisContext context)
        {
            _context = context;
        }

        // GET: Mehaniki
        public async Task<IActionResult> Index()
        {
            return View(await _context.Mehaniki.ToListAsync());
        }

        // GET: Mehaniki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mehanik = await _context.Mehaniki
                .FirstOrDefaultAsync(m => m.MehanikID == id);
            if (mehanik == null)
            {
                return NotFound();
            }

            return View(mehanik);
        }

        // GET: Mehaniki/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mehaniki/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MehanikID,Ime,Priimek,Specializacija,Telefon,Email")] Mehanik mehanik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mehanik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mehanik);
        }

        // GET: Mehaniki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mehanik = await _context.Mehaniki.FindAsync(id);
            if (mehanik == null)
            {
                return NotFound();
            }
            return View(mehanik);
        }

        // POST: Mehaniki/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MehanikID,Ime,Priimek,Specializacija,Telefon,Email,DatumZaposlitve")] Mehanik mehanik)
        {
            if (id != mehanik.MehanikID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mehanik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MehanikExists(mehanik.MehanikID))
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
            return View(mehanik);
        }

        // GET: Mehaniki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mehanik = await _context.Mehaniki
                .FirstOrDefaultAsync(m => m.MehanikID == id);
            if (mehanik == null)
            {
                return NotFound();
            }

            return View(mehanik);
        }

        // POST: Mehaniki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mehanik = await _context.Mehaniki.FindAsync(id);
            if (mehanik != null)
            {
                _context.Mehaniki.Remove(mehanik);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MehanikExists(int id)
        {
            return _context.Mehaniki.Any(e => e.MehanikID == id);
        }
    }
}
using MarineMexico.Data;
using MarineMexico.Models;
using MarineMexico.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarineMexico.Controllers
{
    public class InventariosController : Controller
    {
        private readonly SsamarineContext _context;

        public InventariosController(SsamarineContext context)
        {
            _context = context;
        }

        // GET: Inventarios
        public async Task<IActionResult> Index()
        {
            var ssamarineContext = _context.Inventarios.Include(i => i.Articulo).Include(i => i.Talla);
            return View(await ssamarineContext.ToListAsync());
        }

        // GET: Inventarios/Create
        public IActionResult Create()
        {
            ViewData["ArticuloId"] = new SelectList(_context.Articulos, "Id", "Nombre");
            ViewData["TallaId"] = new SelectList(_context.Tallas, "Id", "Descripcion");
            return View();
        }

        // POST: Inventarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArticuloId,TallaId,StockActual,StockMinimo")] InventarioViewModel inventario)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Inventarios.AnyAsync(i => i.ArticuloId == inventario.ArticuloId && i.TallaId == inventario.TallaId))
                {
                    ModelState.AddModelError("", "Ya existe un inventario para este artículo y talla.");
                    ViewData["ArticuloId"] = new SelectList(_context.Articulos, "Id", "Nombre", inventario.ArticuloId);
                    ViewData["TallaId"] = new SelectList(_context.Tallas, "Id", "Descripcion", inventario.TallaId);
                    return View(inventario);
                }

                var nuevoInventario = new Inventario
                {
                    ArticuloId = inventario.ArticuloId,
                    TallaId = inventario.TallaId,
                    StockActual = inventario.StockActual,
                    StockMinimo = inventario.StockMinimo
                };
                _context.Add(nuevoInventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticuloId"] = new SelectList(_context.Articulos, "Id", "Nombre", inventario.ArticuloId);
            ViewData["TallaId"] = new SelectList(_context.Tallas, "Id", "Descripcion", inventario.TallaId);
            return View(inventario);
        }

        // GET: Inventarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }
            ViewData["ArticuloId"] = new SelectList(_context.Articulos, "Id", "Nombre", inventario.ArticuloId);
            ViewData["TallaId"] = new SelectList(_context.Tallas, "Id", "Descripcion", inventario.TallaId);
            return View(inventario);
        }

        // POST: Inventarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArticuloId,TallaId,StockActual,StockMinimo")] Inventario inventario)
        {
            if (id != inventario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarioExists(inventario.Id))
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
            ViewData["ArticuloId"] = new SelectList(_context.Articulos, "Id", "Nombre", inventario.ArticuloId);
            ViewData["TallaId"] = new SelectList(_context.Tallas, "Id", "Descripcion", inventario.TallaId);
            return View(inventario);
        }

        // GET: Inventarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventarios
                .Include(i => i.Articulo)
                .Include(i => i.Talla)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // POST: Inventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario != null)
            {
                // Este DELETE activa el trigger INSTEAD OF DELETE tr_Inventario_PreventDelete
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Inventario WHERE Id = @p0", id);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int id)
        {
            return _context.Inventarios.Any(e => e.Id == id);
        }
    }
}

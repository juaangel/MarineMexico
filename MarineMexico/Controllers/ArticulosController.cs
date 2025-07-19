using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarineMexico.Data;
using MarineMexico.Models;

namespace MarineMexico.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly SsamarineContext _context;

        public ArticulosController(SsamarineContext context)
        {
            _context = context;
        }

        // GET: Articulos
        public async Task<IActionResult> Index()
        {
            var ssamarineContext = _context.Articulos.Include(a => a.TipoEmpleado);
            return View(await ssamarineContext.ToListAsync());
        }

        // GET: Articulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.TipoEmpleado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // GET: Articulos/Create
        public IActionResult Create()
        {
            ViewData["TipoEmpleadoId"] = new SelectList(_context.TiposEmpleados, "IdTipo", "Tipo");
            return View();
        }

        // POST: Articulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,TipoEmpleadoId")] Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoEmpleadoId"] = new SelectList(_context.TiposEmpleados, "IdTipo", "Tipo", articulo.TipoEmpleadoId);
            return View(articulo);
        }

        // GET: Articulos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }
            ViewData["TipoEmpleadoId"] = new SelectList(_context.TiposEmpleados, "IdTipo", "Tipo", articulo.TipoEmpleadoId);
            return View(articulo);
        }

        // POST: Articulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,TipoEmpleadoId")] Articulo articulo)
        {
            if (id != articulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // revisar si existen movimientos con empleados (debido al tipos)
                    if (await _context.MovimientosInventarios.AnyAsync(x =>
                            x.EmpleadoId.HasValue &&
                            x.Inventario.ArticuloId == id
                        ))
                    {
                        ModelState.AddModelError("", "Este artículo ya tiene movimientos de inventario vinculados a empleados, no se puede modificar.");
                        ViewData["TipoEmpleadoId"] = new SelectList(_context.TiposEmpleados, "IdTipo", "Tipo", articulo.TipoEmpleadoId);
                        return View(articulo);
                    }

                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticuloExists(articulo.Id))
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
            ViewData["TipoEmpleadoId"] = new SelectList(_context.TiposEmpleados, "IdTipo", "IdTipo", articulo.TipoEmpleadoId);
            return View(articulo);
        }

        // GET: Articulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .Include(a => a.TipoEmpleado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // POST: Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articulo = await _context.Articulos.Include(a => a.TipoEmpleado)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (articulo != null)
            {
                if (await _context.Inventarios.AnyAsync(x => x.ArticuloId == id))
                {
                    ModelState.AddModelError("", "No puede eliminar este artículo porque esta registrado en el inventario.");
                    return View(articulo);
                }

                _context.Articulos.Remove(articulo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticuloExists(int id)
        {
            return _context.Articulos.Any(e => e.Id == id);
        }
    }
}

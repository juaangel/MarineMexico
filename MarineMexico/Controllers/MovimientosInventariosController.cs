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
    public class MovimientosInventariosController : Controller
    {
        private readonly SsamarineContext _context;

        public MovimientosInventariosController(SsamarineContext context)
        {
            _context = context;
        }

        // GET: MovimientosInventarios
        public async Task<IActionResult> Index(int? inventarioId = null)
        {
            var query = _context.MovimientosInventarioViews.AsQueryable();

            if (inventarioId.HasValue)
            {
                query = query.Where(x => x.InventarioId == inventarioId);

                var inventario = await _context.Inventarios
                    .Include(x => x.Articulo)
                    .Include(x => x.Talla)
                    .FirstAsync(x => x.Id == inventarioId);

                ViewBag.Inventario = $"{inventario.Articulo.Nombre} | Talla: {inventario.Talla.Descripcion}";
            }

            var ssamarineContext = await query.ToListAsync();

            return View(ssamarineContext);
        }

        // GET: MovimientosInventarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimientosInventario = await _context.MovimientosInventarios
                .Include(m => m.Empleado)
                .Include(m => m.Inventario)
                .Include(m => m.TipoMovimiento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movimientosInventario == null)
            {
                return NotFound();
            }

            return View(movimientosInventario);
        }

        // GET: MovimientosInventarios/Create
        public IActionResult Create()
        {
            var inventario = _context.Inventarios
                .Include(x => x.Articulo)
                .Include(x => x.Articulo.TipoEmpleado)
                .Include(x => x.Talla)
                .Select(x => new
                {
                    x.Id,
                    Descripcion = $"{x.Articulo.Nombre} | {x.Talla.Talla1} ({x.Talla.Notacion}) | {(x.Articulo.TipoEmpleadoId.HasValue ? x.Articulo.TipoEmpleado.Tipo : "Sin Tipo")}",
                    x.Articulo.TipoEmpleadoId,
                    Tipo = x.Articulo.TipoEmpleado != null ? x.Articulo.TipoEmpleado.Tipo : "Sin Tipo"
                })
                .ToList();

            ViewData["Empleado"] = _context.Empleados
                //.Include(x => x.gr)
                .OrderBy(x => x.NombreEmpleado).ToList();
            ViewData["Inventario"] = inventario;
            ViewData["TipoMovimientoId"] = new SelectList(_context.TiposMovimientoInventarios, "Id", "Descripcion");
            ViewData["MotivoMovimiento"] = _context.MotivosMovimientoInventarios.ToList();

            var model = new MovimientosInventario();

            return View(model);
        }

        // POST: MovimientosInventarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoMovimientoId,MotivoMovimientoId,Cantidad,StockActualAntes,InventarioId,FechaMovimiento,EmpleadoId")] MovimientosInventario movimientosInventario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movimientosInventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", movimientosInventario.EmpleadoId);
            ViewData["InventarioId"] = new SelectList(_context.Inventarios, "Id", "Id", movimientosInventario.InventarioId);
            ViewData["TipoMovimientoId"] = new SelectList(_context.TiposMovimientoInventarios, "Id", "Id", movimientosInventario.TipoMovimientoId);
            return View(movimientosInventario);
        }

        // GET: MovimientosInventarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimientosInventario = await _context.MovimientosInventarios.FindAsync(id);
            if (movimientosInventario == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", movimientosInventario.EmpleadoId);
            ViewData["InventarioId"] = new SelectList(_context.Inventarios, "Id", "Id", movimientosInventario.InventarioId);
            ViewData["TipoMovimientoId"] = new SelectList(_context.TiposMovimientoInventarios, "Id", "Id", movimientosInventario.TipoMovimientoId);
            return View(movimientosInventario);
        }

        // POST: MovimientosInventarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TipoMovimientoId,MotivoMovimientoId,Cantidad,StockActualAntes,InventarioId,FechaMovimiento,EmpleadoId")] MovimientosInventario movimientosInventario)
        {
            if (id != movimientosInventario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimientosInventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimientosInventarioExists(movimientosInventario.Id))
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
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", movimientosInventario.EmpleadoId);
            ViewData["InventarioId"] = new SelectList(_context.Inventarios, "Id", "Id", movimientosInventario.InventarioId);
            ViewData["TipoMovimientoId"] = new SelectList(_context.TiposMovimientoInventarios, "Id", "Id", movimientosInventario.TipoMovimientoId);
            return View(movimientosInventario);
        }

        // GET: MovimientosInventarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimientosInventario = await _context.MovimientosInventarioViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movimientosInventario == null)
            {
                return NotFound();
            }

            return View(movimientosInventario);
        }

        // POST: MovimientosInventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movimientosInventario = await _context.MovimientosInventarios.FindAsync(id);
            if (movimientosInventario != null)
            {
                _context.MovimientosInventarios.Remove(movimientosInventario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovimientosInventarioExists(int id)
        {
            return _context.MovimientosInventarios.Any(e => e.Id == id);
        }
    }
}

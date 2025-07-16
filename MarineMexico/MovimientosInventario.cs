using System;
using System.Collections.Generic;

namespace MarineMexico;

public partial class MovimientosInventario
{
    public int Id { get; set; }

    public int TipoMovimientoId { get; set; }

    public int MotivoMovimientoId { get; set; }

    public int Cantidad { get; set; }

    public int StockActualAntes { get; set; }

    public int InventarioId { get; set; }

    public DateTime FechaMovimiento { get; set; }

    public int? EmpleadoId { get; set; }

    public virtual Empleado? Empleado { get; set; }

    public virtual Inventario Inventario { get; set; } = null!;

    public virtual TiposMovimientoInventario TipoMovimiento { get; set; } = null!;
}

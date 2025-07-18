using System;
using System.Collections.Generic;

namespace MarineMexico.Models;

public partial class MovimientosInventarioView
{
    public int Id { get; set; }
    public string Articulo { get; set; } = null!;

    public string Talla { get; set; } = null!;

    public string Movimiento { get; set; } = null!;

    public int Cantidad { get; set; }

    public string Motivo { get; set; } = null!;

    public string? NombreEmpleado { get; set; }
    public int InventarioId { get; set; }
    public DateTime FechaMovimiento { get; set; }
}

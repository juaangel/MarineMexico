using System;
using System.Collections.Generic;

namespace MarineMexico.Models;

public partial class MotivosMovimientoInventario
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public int TipoMovimientoInventarioId { get; set; }

    public virtual TiposMovimientoInventario TipoMovimientoInventario { get; set; } = null!;
    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();
}

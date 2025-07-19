using System;
using System.Collections.Generic;

namespace MarineMexico.Models;

public partial class TiposMovimientoInventario
{
    public const int
        ENTRADA = 1,
        SALIDA = 2;

    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<MotivosMovimientoInventario> MotivosMovimientoInventarios { get; set; } = new List<MotivosMovimientoInventario>();

    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();
}

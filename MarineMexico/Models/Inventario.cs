using System;
using System.Collections.Generic;

namespace MarineMexico.Models;

public partial class Inventario
{
    public int Id { get; set; }

    public int ArticuloId { get; set; }

    public int TallaId { get; set; }

    public int StockActual { get; set; }

    public int StockMinimo { get; set; }

    public virtual Articulo Articulo { get; set; } = null!;

    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();

    public virtual Talla Talla { get; set; } = null!;
}

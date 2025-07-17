using System;
using System.Collections.Generic;

namespace MarineMexico.Models;

public partial class Articulo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? TipoEmpleadoId { get; set; }

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    public virtual TiposEmpleado? TipoEmpleado { get; set; }
}

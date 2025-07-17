using System;
using System.Collections.Generic;

namespace MarineMexico.Models;

public partial class TiposEmpleado
{
    public int IdTipo { get; set; }

    public string Tipo { get; set; } = null!;

    public virtual ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();

    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}

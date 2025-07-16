using System;
using System.Collections.Generic;

namespace MarineMexico;

public partial class Talla
{
    public int Id { get; set; }

    public string Notacion { get; set; } = null!;

    public string Talla1 { get; set; } = null!;

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
}

using System;
using System.Collections.Generic;

namespace MarineMexico.Models;

public partial class Grupo
{
    public string IdGrupo { get; set; } = null!;

    public string Grupo1 { get; set; } = null!;

    public int IdTipo { get; set; }

    public virtual TiposEmpleado IdTipoNavigation { get; set; } = null!;
}

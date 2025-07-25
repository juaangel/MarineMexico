﻿using System;
using System.Collections.Generic;

namespace MarineMexico.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string NombreEmpleado { get; set; } = null!;

    public string IdGrupo { get; set; } = null!;

    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();
    public virtual Grupo IdGrupoNavigation { get; set; } = null!;

}

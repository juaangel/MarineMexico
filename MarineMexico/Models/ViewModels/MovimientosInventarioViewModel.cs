using System.ComponentModel.DataAnnotations;

namespace MarineMexico.Models.ViewModels
{
    public class MovimientosInventarioViewModel
    {
        [Required]
        public int TipoMovimientoId { get; set; }
        [Required]
        public int MotivoMovimientoId { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public int InventarioId { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public int? EmpleadoId { get; set; } = null;
    }
}
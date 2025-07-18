using System.ComponentModel.DataAnnotations;

namespace MarineMexico.Models.ViewModels
{
    public class InventarioViewModel
    {
        [Required]
        public int ArticuloId { get; set; }

        [Required]
        public int TallaId { get; set; }

        [Required]
        public int StockActual { get; set; } = 0;

        [Required]
        public int StockMinimo { get; set; } = 0;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.DTOs
{
    public class ImportStockDTO
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Số lượng phải > 0")]
        public int Quantity { get; set; }

        [Required]
        public string? Note { get; set; }
    }
}
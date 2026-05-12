using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.DTOs
{
    public class UpdateProductDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string? SKU { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int MinStock { get; set; }

        [Range(1, int.MaxValue)]
        public int SupplierId { get; set; }
    }
}
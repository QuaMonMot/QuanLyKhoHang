using System.ComponentModel.DataAnnotations;

namespace Warehouse.Web.Models
{
    public class UpdateProductViewModel
    {
        public int ProductId { get; set; }

        [Required]
        public string? SKU { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Range(0, 999999)]
        public int Quantity { get; set; }

        [Range(0, 999999)]
        public decimal Price { get; set; }

        [Range(0, 999999)]
        public int MinStock { get; set; }

        public int SupplierId { get; set; }
    }
}
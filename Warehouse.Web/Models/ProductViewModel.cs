namespace Warehouse.Web.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        public string? SKU { get; set; }

        public string? ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int MinStock { get; set; }

        public int SupplierId { get; set; }

        public string? SupplierName { get; set; }
    }
}
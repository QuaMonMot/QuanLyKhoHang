namespace Warehouse.Web.Models
{
    public class StockHistoryViewModel
    {
        public int LogId { get; set; }

        public string? SKU { get; set; }

        public string? ProductName { get; set; }

        public int Quantity { get; set; }

        public string? Type { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
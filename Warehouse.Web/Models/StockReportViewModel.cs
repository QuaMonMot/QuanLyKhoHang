namespace Warehouse.Web.Models
{
    public class StockReportViewModel
    {
        public string? ProductName { get; set; }

        public int TotalImport { get; set; }

        public int TotalExport { get; set; }
    }
}
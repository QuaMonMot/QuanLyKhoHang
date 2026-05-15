namespace Warehouse.Web.Models
{
    public class StockReportViewModel
    {
        public string? ProductName { get; set; }

        public string? SupplierName { get; set; }

        public long TotalImport { get; set; }

        public long TotalExport { get; set; }
    }
}
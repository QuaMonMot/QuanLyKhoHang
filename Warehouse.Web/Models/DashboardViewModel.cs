namespace Warehouse.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }

        public long TotalStock { get; set; }

        public int LowStockProducts { get; set; }

        public long TotalImport { get; set; }

        public long TotalExport { get; set; }

        public List<StockReportViewModel> ProductStats { get; set; } = new();
    }
}
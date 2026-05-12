namespace Warehouse.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }

        public int TotalStock { get; set; }

        public int LowStockProducts { get; set; }

        public int TotalImport { get; set; }

        public int TotalExport { get; set; }
    }
}
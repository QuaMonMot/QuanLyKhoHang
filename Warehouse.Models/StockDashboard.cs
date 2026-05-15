using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class StockDashboard
    {
        public int TotalProducts { get; set; }

        public long TotalStock { get; set; }

        public int LowStockProducts { get; set; }

        public long TotalImport { get; set; }

        public long TotalExport { get; set; }

        public List<ProductStatistic> ProductStats { get; set; } = new();
    }

    public class ProductStatistic
    {
        public string ProductName { get; set; }

        public long TotalImport { get; set; }

        public long TotalExport { get; set; }
    }
}

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

        public int TotalStock { get; set; }

        public int LowStockProducts { get; set; }

        public int TotalImport { get; set; }

        public int TotalExport { get; set; }
    }
}

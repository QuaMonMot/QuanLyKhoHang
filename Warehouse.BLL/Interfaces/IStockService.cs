using Warehouse.Models;

namespace Warehouse.BLL.Interfaces
{
    public interface IStockService
    {
        void ImportStock(
            int productId,
            int quantity,
            string note
        );

        void ExportStock(
            int productId,
            int quantity,
            string note
        );
        StockDashboard GetDashboard();

        List<Product> Search(string keyword);

        List<Product> Paging(
            int page,
            int pageSize
        );

        object GetReport();
        List<Product> GetInventory();

        List<Product> GetLowStock();

        List<StockLog> GetHistory();
    }
}
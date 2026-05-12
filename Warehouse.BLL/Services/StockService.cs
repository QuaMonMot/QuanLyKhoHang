using Warehouse.BLL.Interfaces;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.BLL.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository
            _stockRepository;

        public StockService(
            IStockRepository stockRepository
        )
        {
            _stockRepository = stockRepository;
        }

        public void ImportStock(
            int productId,
            int quantity,
            string note
        )
        {
            _stockRepository.ImportStock(
                productId,
                quantity,
                note
            );
        }

        public void ExportStock(
            int productId,
            int quantity,
            string note
        )
        {
            _stockRepository.ExportStock(
                productId,
                quantity,
                note
            );
        }

        public List<Product> GetInventory()
        {
            return _stockRepository.GetInventory();
        }

        public List<Product> GetLowStock()
        {
            return _stockRepository.GetLowStock();
        }

        public List<StockLog> GetHistory()
        {
            return _stockRepository.GetHistory();
        }
        public StockDashboard GetDashboard()
        {
            return _stockRepository.GetDashboard();
        }

        public List<Product> Search(string keyword)
        {
            return _stockRepository.Search(keyword);
        }

        public List<Product> Paging( int page,int pageSize)
        {
            return _stockRepository.Paging(
                page,
                pageSize
            );
        }

        public object GetReport()
        {
            return _stockRepository.GetReport();
        }
    }
}
using Warehouse.Models;

namespace Warehouse.DAL.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();

        Product GetById(int id);

        void Add(Product product);

        void Update(Product product);

        void Delete(int id);
    }
}
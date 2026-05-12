using Warehouse.Models;

namespace Warehouse.DAL.Interfaces
{
    public interface ISupplierRepository
    {
        List<Supplier> GetAll();

        Supplier GetById(int id);

        void Add(Supplier supplier);

        void Update(Supplier supplier);

        void Delete(int id);
    }
}
using Warehouse.Models;

namespace Warehouse.BLL.Interfaces
{
    public interface ISupplierService
    {
        List<Supplier> GetAll();

        Supplier GetById(int id);

        void Add(Supplier supplier);

        void Update(Supplier supplier);

        void Delete(int id);
    }
}
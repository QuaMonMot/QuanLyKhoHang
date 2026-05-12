using Warehouse.BLL.Interfaces;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.BLL.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(
            ISupplierRepository supplierRepository
        )
        {
            _supplierRepository = supplierRepository;
        }

        public List<Supplier> GetAll()
        {
            return _supplierRepository.GetAll();
        }

        public Supplier GetById(int id)
        {
            return _supplierRepository.GetById(id);
        }

        public void Add(Supplier supplier)
        {
            _supplierRepository.Add(supplier);
        }

        public void Update(Supplier supplier)
        {
            _supplierRepository.Update(supplier);
        }

        public void Delete(int id)
        {
            _supplierRepository.Delete(id);
        }
    }
}
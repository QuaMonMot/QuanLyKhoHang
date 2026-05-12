using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.DTOs
{
    public class CreateSupplierDTO
    {
        [Required]
        public string? SupplierCode { get; set; }

        [Required]
        public string? SupplierName { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [Required]
        public string? Address { get; set; }
    }
}
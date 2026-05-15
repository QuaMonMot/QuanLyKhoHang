using System.ComponentModel.DataAnnotations;

namespace Warehouse.Web.Models
{
    public class SupplierViewModel
    {
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Mã nhà cung cấp không được để trống")]
        [StringLength(50, ErrorMessage = "Mã nhà cung cấp không vượt quá 50 ký tự")]
        public string? SupplierCode { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
        [StringLength(200, ErrorMessage = "Tên nhà cung cấp không vượt quá 200 ký tự")]
        public string? SupplierName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15, ErrorMessage = "Số điện thoại không vượt quá 15 ký tự")]
        public string? Phone { get; set; }

        [StringLength(500, ErrorMessage = "Địa chỉ không vượt quá 500 ký tự")]
        public string? Address { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Web.Models
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "SKU không được để trống")]
        [StringLength(50, ErrorMessage = "SKU không vượt quá 50 ký tự")]
        public string? SKU { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không vượt quá 200 ký tự")]
        public string? ProductName { get; set; }

        [Range(0, 999999, ErrorMessage = "Số lượng phải từ 0 đến 999999")]
        public int Quantity { get; set; }

        [Range(0, 999999, ErrorMessage = "Giá phải từ 0 đến 999999")]
        public decimal Price { get; set; }

        [Range(0, 999999, ErrorMessage = "Tồn kho tối thiểu phải từ 0 đến 999999")]
        public int MinStock { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
        public int SupplierId { get; set; }
    }
}
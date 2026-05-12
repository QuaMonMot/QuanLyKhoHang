using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models.DTOs
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "SKU không được để trống")]
        public string? SKU { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string? ProductName { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "Số lượng phải >= 0")]
        public int Quantity { get; set; }

        [Range(0, 999999999,
            ErrorMessage = "Giá phải >= 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "MinStock phải >= 0")]
        public int MinStock { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Supplier không hợp lệ")]
        public int SupplierId { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc;
using Warehouse.BLL.Interfaces;
using Warehouse.Models.DTOs;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService
            _stockService;

        public StockController(
            IStockService stockService
        )
        {
            _stockService = stockService;
        }

        // =========================
        // IMPORT STOCK
        // THÊM SẢN PHẨM VÀO KHO
        // =========================
        [HttpPost("import")]
        [EndpointSummary("Thêm sản phẩm vào kho")]

        public IActionResult ImportStock(
            [FromBody] ImportStockDTO dto
        )
        {
            _stockService.ImportStock(
                dto.ProductId,
                dto.Quantity,
                dto.Note
            );

            return Ok("Import success");
        }

        // =========================
        // EXPORT STOCK
        // XUẤT SẢN PHẨM KHỎI KHO
        // =========================
        [HttpPost("export")]
        [EndpointSummary("Xuất sản phẩm khỏi kho")]

        public IActionResult ExportStock(
            [FromBody] ExportStockDTO dto
        )
        {
            _stockService.ExportStock(
                dto.ProductId,
                dto.Quantity,
                dto.Note
            );

            return Ok("Export success");
        }

        // =========================
        // INVENTORY
        // KIỂM TRA TỒN KHO
        // =========================
        [HttpGet("inventory")]
        [EndpointSummary("Kiểm tra tồn kho")]

        public IActionResult Inventory()
        {
            return Ok(
                _stockService.GetInventory()
            );
        }

        // =========================
        // LOW STOCK
        // KIỂM TRA TỒN KHO THẤP
        // =========================
        [HttpGet("low-stock")]
        [EndpointSummary("Kiểm tra tồn kho thấp")]

        public IActionResult LowStock()
        {
            return Ok(
                _stockService.GetLowStock()
            );
        }

        // =========================
        // HISTORY
        // KIỂM TRA LỊCH SỬ KHO
        // =========================
        [HttpGet("history")]
        [EndpointSummary("Kiểm tra lịch sử kho")]

        public IActionResult History()
        {
            return Ok(
                _stockService.GetHistory()
            );
        }

        // =========================
        // DASHBOARD
        // THỐNG KÊ KHO HÀNG
        // =========================
        [HttpGet("dashboard")]
        [EndpointSummary("Thống kê kho hàng")]

        public IActionResult Dashboard()
        {
            return Ok(
                _stockService.GetDashboard()
            );
        }

        // =========================
        // SEARCH
        // TÌM KIẾM SẢN PHẨM
        // =========================
        [HttpGet("search")]
        [EndpointSummary("Tìm kiếm sản phẩm")]

        public IActionResult Search(
            string keyword
        )
        {
            return Ok(
                _stockService.Search(keyword)
            );
        }

        // =========================
        // PAGING
        // PHÂN TRANG SẢN PHẨM
        // =========================
        [HttpGet("paging")]
        [EndpointSummary("Phân trang sản phẩm")]

        public IActionResult Paging(
            int page = 1,
            int pageSize = 5
        )
        {
            return Ok(
                _stockService.Paging(
                    page,
                    pageSize
                )
            );
        }

        // =========================
        // REPORT
        // BÁO CÁO NHẬP XUẤT KHO
        // =========================
        [HttpGet("report")]
        [EndpointSummary("Báo cáo nhập xuất kho")]

        public IActionResult Report()
        {
            return Ok(
                _stockService.GetReport()
            );
        }
    }
}
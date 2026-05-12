using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Warehouse.Web.Models;

namespace Warehouse.Web.Controllers
{
    public class StockController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public StockController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration
        )
        {
            _httpClientFactory = httpClientFactory;

            _configuration = configuration;
        }
        // KHO HÀNG
        public async Task<IActionResult> Index()
        {
            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Stock/inventory";

            var response =
                await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ProductViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            var options =
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

            var data =
                JsonSerializer.Deserialize<List<ProductViewModel>>
                (
                    json,
                    options
                );

            return View(data);
        }
        // IMPORT PAGE
        public IActionResult Import(
            int productId
        )
        {
            var model =
                new ImportStockDTO
                {
                    ProductId = productId
                };

            return View(model);
        }

        // IMPORT
        [HttpPost]
        public async Task<IActionResult> Import(
            ImportStockDTO dto
        )
        {
            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Stock/import";

            var json =
                JsonSerializer.Serialize(dto);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

            await client.PostAsync(
                apiUrl,
                content
            );

            return RedirectToAction(
                "Index",
                "Product"
            );
        }

        // EXPORT PAGE
        public IActionResult Export(
            int productId
        )
        {
            var model =
                new ExportStockDTO
                {
                    ProductId = productId
                };

            return View(model);
        }

        // EXPORT
        [HttpPost]
        public async Task<IActionResult> Export(
            ExportStockDTO dto
        )
        {
            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Stock/export";

            var json =
                JsonSerializer.Serialize(dto);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

            await client.PostAsync(
                apiUrl,
                content
            );

            return RedirectToAction(
                "Index",
                "Product"
            );
        }
        // =========================
        // LỊCH SỬ KHO
        // =========================
        public async Task<IActionResult> History()
        {
            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Stock/history";

            var response =
                await client.GetAsync(apiUrl);

            var json =
                await response.Content
                .ReadAsStringAsync();

            var data =
                JsonSerializer.Deserialize<
                    List<StockHistoryViewModel>
                >(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return View(data);
        }

        // =========================
        // TỒN KHO THẤP
        // =========================
        public async Task<IActionResult> LowStock()
        {
            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Stock/low-stock";

            var response =
                await client.GetAsync(apiUrl);

            var json =
                await response.Content
                .ReadAsStringAsync();

            var data =
                JsonSerializer.Deserialize<
                    List<ProductViewModel>
                >(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return View(data);
        }
    }
}
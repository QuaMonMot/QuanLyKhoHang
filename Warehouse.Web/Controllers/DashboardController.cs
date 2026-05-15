using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Warehouse.Web.Models;

namespace Warehouse.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DashboardController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration
        )
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var client =
                _httpClientFactory.CreateClient();

            string apiUrl =
                _configuration["ApiUrl"]
                + "Stock/dashboard";

            try
            {
                var response =
                    await client.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Không tải được dữ liệu tổng quan từ API.";

                    return View(new DashboardViewModel());
                }

                var json =
                    await response.Content.ReadAsStringAsync();

                var dashboard =
                    JsonSerializer.Deserialize<DashboardViewModel>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    ) ?? new DashboardViewModel();

                return View(dashboard);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Không kết nối được API dashboard.";

                return View(new DashboardViewModel());
            }
        }
    }
}

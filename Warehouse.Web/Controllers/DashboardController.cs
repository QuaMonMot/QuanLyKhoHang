using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
                + "Stock/report";

            var response =
                await client.GetAsync(apiUrl);

            var json =
                await response.Content.ReadAsStringAsync();

            ViewBag.ChartData = json;

            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Warehouse.Web.Models;

namespace Warehouse.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ReportController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration
        )
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // =========================
        // REPORT
        // =========================
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            string apiUrl =
                _configuration["ApiUrl"]
                + "Stock/report";

            var response =
                await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<StockReportViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            var options =
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

            var data =
                JsonSerializer.Deserialize<List<StockReportViewModel>>
                (
                    json,
                    options
                );

            return View(data);
        }
    }
}
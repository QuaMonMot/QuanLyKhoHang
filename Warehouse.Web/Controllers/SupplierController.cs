using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Warehouse.Web.Models;

namespace Warehouse.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public SupplierController(
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

            var apiUrl =
                _configuration["ApiUrl"]
                + "Supplier";

            var response =
                await client.GetAsync(apiUrl);

            var json =
                await response.Content
                .ReadAsStringAsync();

            var suppliers =
                JsonSerializer.Deserialize
                <List<SupplierViewModel>>
                (
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return View(suppliers);
        }
    }
}
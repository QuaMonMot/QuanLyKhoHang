using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Warehouse.Web.Models;

namespace Warehouse.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public ProductController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration
        )
        {
            _httpClientFactory = httpClientFactory;

            _configuration = configuration;
        }

        public async Task<IActionResult> Index(
            int page = 1,
            string? search = null
        )
        {
            int pageSize = 6;

            var client =
                _httpClientFactory.CreateClient();

            string apiUrl =
                _configuration["ApiUrl"]
                + $"Stock/paging?page={page}&pageSize={pageSize}";

            var response =
                await client.GetAsync(apiUrl);

            var json =
                await response.Content
                .ReadAsStringAsync();

            var products =
                JsonSerializer.Deserialize
                <List<ProductViewModel>>
                (
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            ViewBag.CurrentPage = page;

            ViewBag.TotalPage = 5;

            return View(products);
        }
        // =========================
        // CREATE PAGE
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("ROLE") != 1)
            {
                return RedirectToAction(
                    "Index"
                );
            }

            return View();
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create( CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Product";

            var json =
                JsonSerializer.Serialize(model);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

            var response =
                await client.PostAsync(
                    apiUrl,
                    content
                );

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "Index"
                );
            }

            ViewBag.Error =
                "Thêm sản phẩm thất bại";

            return View(model);
        }
        // =========================
        // EDIT PAGE
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("ROLE") != 1)
            {
                return RedirectToAction(
                    "Index"
                );
            }

            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Product/" + id;

            var response =
                await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "Index"
                );
            }

            var json =
                await response.Content
                .ReadAsStringAsync();

            var product =
                JsonSerializer.Deserialize
                <UpdateProductViewModel>
                (
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return View(product);
        }

        // =========================
        // EDIT
        // =========================
        [HttpPost]
        public async Task<IActionResult> Edit( UpdateProductViewModel model)
        {
            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Product/" + model.ProductId;

            var json =
                JsonSerializer.Serialize(model);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

            var response =
                await client.PutAsync(
                    apiUrl,
                    content
                );

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "Index"
                );
            }

            return View(model);
        }
        // =========================
        // DELETE
        // =========================
        public async Task<IActionResult> Delete(
            int id
        )
        {
            if (HttpContext.Session.GetInt32("ROLE") != 1)
            {
                return RedirectToAction(
                    "Index"
                );
            }

            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Product/" + id;

            await client.DeleteAsync(apiUrl);

            return RedirectToAction(
                "Index"
            );
        }

    }
}
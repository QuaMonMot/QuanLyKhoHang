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

        // =========================
        // CREATE PAGE
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("ROLE") != 1) return RedirectToAction("Index");
            return View();
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create(SupplierViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Supplier";
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Thêm nhà cung cấp thất bại";
            return View(model);
        }

        // =========================
        // EDIT PAGE
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("ROLE") != 1) return RedirectToAction("Index");

            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Supplier/" + id;
            var response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode) return RedirectToAction("Index");

            var json = await response.Content.ReadAsStringAsync();
            var supplier = JsonSerializer.Deserialize<SupplierViewModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(supplier);
        }

        // =========================
        // EDIT
        // =========================
        [HttpPost]
        public async Task<IActionResult> Edit(SupplierViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Supplier/" + model.SupplierId;
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Cập nhật nhà cung cấp thất bại";
            return View(model);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("ROLE") != 1) return RedirectToAction("Index");

            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Supplier/" + id;

            await client.DeleteAsync(apiUrl);

            return RedirectToAction("Index");
        }
    }
}
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
            var client = _httpClientFactory.CreateClient();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Get all products
            var productsResponse = await client.GetAsync(_configuration["ApiUrl"] + "Product");
            var productsJson = await productsResponse.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<ProductViewModel>>(productsJson, options) ?? new List<ProductViewModel>();

            // Get all suppliers to map names
            var suppliersResponse = await client.GetAsync(_configuration["ApiUrl"] + "Supplier");
            if (suppliersResponse.IsSuccessStatusCode)
            {
                var suppliersJson = await suppliersResponse.Content.ReadAsStringAsync();
                var suppliers = JsonSerializer.Deserialize<List<SupplierViewModel>>(suppliersJson, options) ?? new List<SupplierViewModel>();
                var supplierDict = suppliers.ToDictionary(s => s.SupplierId, s => s.SupplierName);
                
                foreach(var p in products)
                {
                    if (supplierDict.TryGetValue(p.SupplierId, out var sName))
                    {
                        p.SupplierName = sName;
                    }
                }
            }

            // Search filtering
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                products = products.Where(p => 
                    (p.ProductName != null && p.ProductName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (p.SKU != null && p.SKU.Contains(search, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // Pagination
            int totalItems = products.Count;
            int totalPage = (int)Math.Ceiling((double)totalItems / pageSize);
            
            if (page < 1) page = 1;
            if (page > totalPage && totalPage > 0) page = totalPage;

            var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.Search = search;

            return View(pagedProducts);
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
            if (!ModelState.IsValid)
            {
                // To display supplier dropdown correctly when validation fails
                var supplierClient = _httpClientFactory.CreateClient();
                var suppliersJson = await supplierClient.GetStringAsync(_configuration["ApiUrl"] + "Supplier");
                var suppliers = JsonSerializer.Deserialize<List<SupplierViewModel>>(
                    suppliersJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                ViewBag.Suppliers = suppliers;
                return View(model);
            }

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
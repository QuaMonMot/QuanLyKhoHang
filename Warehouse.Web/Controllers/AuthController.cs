using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Warehouse.Web.Models;

namespace Warehouse.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public AuthController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration
        )
        {
            _httpClientFactory = httpClientFactory;

            _configuration = configuration;
        }

        // =========================
        // LOGIN PAGE
        // =========================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost]
        public async Task<IActionResult> Login(
            LoginDTO dto
        )
        {
            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Auth/login";

            var json =
                JsonSerializer.Serialize(dto);

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

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error =
                    "Sai tài khoản hoặc mật khẩu";

                return View();
            }

            var result =
                await response.Content
                .ReadAsStringAsync();

            using JsonDocument doc =
                JsonDocument.Parse(result);

            int role =
                doc.RootElement
                .GetProperty("role")
                .GetInt32();

            HttpContext.Session.SetInt32(
                "ROLE",
                role
            );

            return RedirectToAction(
                "Index",
                "Product"
            );
        }

        // =========================
        // REGISTER PAGE
        // =========================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost]
        public async Task<IActionResult> Register(
            RegisterDTO dto
        )
        {
            var client =
                _httpClientFactory.CreateClient();

            var apiUrl =
                _configuration["ApiUrl"]
                + "Auth/register";

            var json =
                JsonSerializer.Serialize(dto);

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
                    "Login"
                );
            }

            ViewBag.Error =
                "Đăng ký thất bại";

            return View();
        }

        // =========================
        // LOGOUT
        // =========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login"
            );
        }
    }
}
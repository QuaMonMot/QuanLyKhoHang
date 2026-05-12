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

            string username = doc.RootElement.GetProperty("username").GetString() ?? "";
            int userId = doc.RootElement.GetProperty("userId").GetInt32();

            HttpContext.Session.SetInt32(
                "ROLE",
                role
            );
            HttpContext.Session.SetString("USERNAME", username);
            HttpContext.Session.SetInt32("USERID", userId);

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

        // =========================
        // PROFILE PAGE
        // =========================
        [HttpGet]
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetInt32("USERID") == null)
            {
                return RedirectToAction("Login");
            }

            var dto = new UpdateProfileDTO
            {
                Username = HttpContext.Session.GetString("USERNAME")
            };
            return View(dto);
        }

        // =========================
        // UPDATE PROFILE
        // =========================
        [HttpPost]
        public async Task<IActionResult> Profile(UpdateProfileDTO dto)
        {
            int? userId = HttpContext.Session.GetInt32("USERID");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Auth/profile/" + userId;

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("USERNAME", dto.Username ?? "");
                ViewBag.Success = "Cập nhật hồ sơ thành công!";
            }
            else
            {
                ViewBag.Error = "Cập nhật thất bại";
            }

            return View(dto);
        }
    }
}
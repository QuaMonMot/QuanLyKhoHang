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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Auth/login";

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            try
            {
                response = await client.PostAsync(apiUrl, content);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Không kết nối được API. Hãy chạy Warehouse.API trước rồi đăng nhập lại.";
                return View(dto);
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                ViewBag.Error = string.IsNullOrWhiteSpace(error)
                    ? "Sai tài khoản hoặc mật khẩu"
                    : error.Trim('"');

                return View(dto);
            }

            var result = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(result);

            int role = doc.RootElement.GetProperty("role").GetInt32();
            string username = doc.RootElement.GetProperty("username").GetString() ?? "";
            int userId = doc.RootElement.GetProperty("userId").GetInt32();

            string fullName = "";
            if (doc.RootElement.TryGetProperty("fullName", out JsonElement fullNameElement))
            {
                fullName = fullNameElement.GetString() ?? "";
            }

            HttpContext.Session.SetInt32("ROLE", role);
            HttpContext.Session.SetString("USERNAME", username);
            HttpContext.Session.SetInt32("USERID", userId);
            HttpContext.Session.SetString("FULLNAME", string.IsNullOrEmpty(fullName) ? username : fullName);

            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Auth/register";

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }

                ViewBag.Error = "Đăng ký thất bại.";
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Không kết nối được API. Hãy chạy Warehouse.API trước rồi thử lại.";
            }

            return View(dto);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            int? userId = HttpContext.Session.GetInt32("USERID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Auth/profile/" + userId;

            try
            {
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var userProfile = JsonSerializer.Deserialize<UpdateProfileDTO>(
                        json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    return View(userProfile);
                }
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Không thể kết nối đến API.";
            }

            return View(new UpdateProfileDTO { Username = HttpContext.Session.GetString("USERNAME") });
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UpdateProfileDTO dto)
        {
            int? userId = HttpContext.Session.GetInt32("USERID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Auth/profile/" + userId;

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("FULLNAME", dto.FullName ?? "");
                    HttpContext.Session.SetString("USERNAME", dto.Username ?? "");

                    ViewBag.Success = "Hồ sơ của bạn đã được cập nhật thành công!";
                }
                else
                {
                    ViewBag.Error = "Cập nhật thất bại. Vui lòng kiểm tra lại thông tin.";
                }
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Không thể kết nối đến API.";
            }

            return View(dto);
        }
    }
}

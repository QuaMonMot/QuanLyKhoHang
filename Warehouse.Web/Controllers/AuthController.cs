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
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Auth/login";

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }

            var result = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(result);

            // 1. Lấy dữ liệu từ JSON trả về (Đảm bảo API có trả về các trường này)
            int role = doc.RootElement.GetProperty("role").GetInt32();
            string username = doc.RootElement.GetProperty("username").GetString() ?? "";
            int userId = doc.RootElement.GetProperty("userId").GetInt32();

            // THÊM DÒNG NÀY: Lấy FullName từ API
            string fullName = "";
            if (doc.RootElement.TryGetProperty("fullName", out JsonElement fullNameElement))
            {
                fullName = fullNameElement.GetString() ?? "";
            }

            // 2. Lưu vào Session
            HttpContext.Session.SetInt32("ROLE", role);
            HttpContext.Session.SetString("USERNAME", username);
            HttpContext.Session.SetInt32("USERID", userId);

            // THÊM DÒNG NÀY: Lưu FullName vào Session
            // Nếu FullName trống thì dùng tạm Username làm dự phòng
            HttpContext.Session.SetString("FULLNAME", string.IsNullOrEmpty(fullName) ? username : fullName);

            return RedirectToAction("Index", "Product");
        }
        //[HttpPost]
        //public async Task<IActionResult> Login(
        //    LoginDTO dto
        //)
        //{
        //    var client =
        //        _httpClientFactory.CreateClient();

        //    var apiUrl =
        //        _configuration["ApiUrl"]
        //        + "Auth/login";

        //    var json =
        //        JsonSerializer.Serialize(dto);

        //    var content =
        //        new StringContent(
        //            json,
        //            Encoding.UTF8,
        //            "application/json"
        //        );

        //    var response =
        //        await client.PostAsync(
        //            apiUrl,
        //            content
        //        );

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        ViewBag.Error =
        //            "Sai tài khoản hoặc mật khẩu";

        //        return View();
        //    }

        //    var result =
        //        await response.Content
        //        .ReadAsStringAsync();

        //    using JsonDocument doc =
        //        JsonDocument.Parse(result);

        //    int role =
        //        doc.RootElement
        //        .GetProperty("role")
        //        .GetInt32();

        //    string username = doc.RootElement.GetProperty("username").GetString() ?? "";
        //    int userId = doc.RootElement.GetProperty("userId").GetInt32();

        //    HttpContext.Session.SetInt32(
        //        "ROLE",
        //        role
        //    );
        //    HttpContext.Session.SetString("USERNAME", username);
        //    HttpContext.Session.SetInt32("USERID", userId);

        //    return RedirectToAction(
        //        "Index",
        //        "Product"
        //    );
        //}

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
        // VIEW PROFILE (Lấy sạch thông tin)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // 1. Kiểm tra session đăng nhập
            int? userId = HttpContext.Session.GetInt32("USERID");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Auth/profile/" + userId;

            try
            {
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    // Deserialize toàn bộ các trường: FullName, Email, Phone, Address, DOB, Gender...
                    var userProfile = JsonSerializer.Deserialize<UpdateProfileDTO>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return View(userProfile);
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Không thể kết nối đến máy chủ dữ liệu.";
            }

            // Trường hợp lỗi: Trả về DTO rỗng để tránh lỗi null trên View
            return View(new UpdateProfileDTO { Username = HttpContext.Session.GetString("USERNAME") });
        }

        // =========================
        // UPDATE PROFILE (Cập nhật sạch thông tin)
        // =========================
        [HttpPost]
        public async Task<IActionResult> Profile(UpdateProfileDTO dto)
        {
            int? userId = HttpContext.Session.GetInt32("USERID");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiUrl"] + "Auth/profile/" + userId;

            // Gửi toàn bộ DTO (đã bỏ password) sang API
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                // Cập nhật lại Session để Layout hiển thị FullName mới ngay lập tức
                HttpContext.Session.SetString("FULLNAME", dto.FullName ?? "");
                HttpContext.Session.SetString("USERNAME", dto.Username ?? "");

                ViewBag.Success = "Hồ sơ của bạn đã được cập nhật thành công!";
            }
            else
            {
                ViewBag.Error = "Cập nhật thất bại. Vui lòng kiểm tra lại thông tin.";
            }

            return View(dto);
        }
    }
}
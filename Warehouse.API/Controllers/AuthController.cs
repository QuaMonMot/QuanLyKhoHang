using Microsoft.AspNetCore.Mvc;
using Warehouse.API.DTOs.Request;
using Warehouse.BLL.Interfaces;
using Warehouse.Models;
namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService
            _authService;

        public AuthController(
            IAuthService authService
        )
        {
            _authService = authService;
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost("login")]
        [EndpointSummary("Đăng nhập")]
        public IActionResult Login(
            [FromBody] LoginDTO dto
        )
        {
            var user =
                _authService.Login(
                    dto.Username,
                    dto.Password
                );

            if (user == null)
            {
                return BadRequest(
                    "Sai tài khoản hoặc mật khẩu"
                );
            }

            return Ok(user);
        }
        // =========================
        // REGISTER
        // =========================
        [HttpPost("register")]
        [EndpointSummary("Đăng ký tài khoản")]

        public IActionResult Register(
            [FromBody] RegisterDTO dto
        )
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ModelState
                    );
                }

                User user = new User
                {
                    Username = dto.Username,

                    Password = dto.Password,

                    // MẶC ĐỊNH USER
                    Role = 0
                };

                _authService.Register(user);

                return Ok(
                    "Đăng ký thành công"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message
                );
            }
        }

        [HttpGet("profile/{id}")]
        [EndpointSummary("Tìm người dùng theo ID")]

        public IActionResult GetProfile(int id)
        {
            var user = _authService.GetById(id);
            if (user == null) return NotFound("Không tìm thấy người dùng");

            // Lấy tất cả thông tin từ đối tượng user trả về từ DB
            return Ok(new UpdateProfileDTO
            {
                Username = user.Username,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Address = user.Address,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth
            });
        }
        [HttpPut("profile/{id}")]
        [EndpointSummary("Cập nhật hồ sơ")]
        public IActionResult UpdateProfile(int id, [FromBody] UpdateProfileDTO dto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("ID không hợp lệ");
                }

                // Truyền cả đối tượng dto vào để Service xử lý trọn gói các cột mới
                _authService.UpdateProfile(id, dto);

                return Ok("Cập nhật hồ sơ thành công");
            }
            catch (Exception ex)
            {
                // Trả về lỗi chi tiết nếu có vấn đề trong quá trình lưu DB
                return BadRequest("Lỗi khi cập nhật: " + ex.Message);
            }
        }
    }
}
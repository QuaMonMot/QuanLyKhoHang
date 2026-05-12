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

        // =========================
        // UPDATE PROFILE
        // =========================
        [HttpPut("profile/{id}")]
        [EndpointSummary("Cập nhật hồ sơ")]
        public IActionResult UpdateProfile(
            int id,
            [FromBody] UpdateProfileRequest dto
        )
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("ID không hợp lệ");
                }

                _authService.UpdateProfile(id, dto.Username, dto.Password);

                return Ok("Cập nhật hồ sơ thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
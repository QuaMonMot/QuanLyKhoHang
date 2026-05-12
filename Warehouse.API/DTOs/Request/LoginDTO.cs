using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.DTOs.Request
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username bắt buộc")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password bắt buộc")]
        public string? Password { get; set; }
    }
}
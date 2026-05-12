using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.DTOs.Request
{
    public class RegisterDTO
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
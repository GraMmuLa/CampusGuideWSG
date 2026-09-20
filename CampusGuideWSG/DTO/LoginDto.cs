using System.ComponentModel.DataAnnotations;

namespace CampusGuideWSG.DTO
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}

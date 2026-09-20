using System.ComponentModel.DataAnnotations;

namespace CampusGuideWSG.DTO
{
    public class RegisterDto
    {
        [MinLength(1), MaxLength(128)]
        [Required]
        public string Username { get; set; } = null!;

        [MinLength(1), MaxLength(128)]
        [Required]
        public string Name { get; set; } = null!;

        [MinLength(1), MaxLength(128)]
        [Required]
        public string Surname { get; set; } = null!;

        [MinLength(8), MaxLength(128)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; } = null!;
    }
}

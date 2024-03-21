using System.ComponentModel.DataAnnotations;

namespace ASP_API_MNGR.Models
{
    public class UserDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime BirthDate { get; set; }
    }
}

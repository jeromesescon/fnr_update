using System.ComponentModel.DataAnnotations;

namespace FnR.VetFront.ViewModels
{
    public class VMLogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
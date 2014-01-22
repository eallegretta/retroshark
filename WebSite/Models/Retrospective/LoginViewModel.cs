
using System.ComponentModel.DataAnnotations;

namespace RetroShark.Application.Models
{
    public class LoginViewModel
    {
        [Required]
        public string User { get; set; }
        
        [Required]
        public string ReturnUrl { get; set; }
    }
}
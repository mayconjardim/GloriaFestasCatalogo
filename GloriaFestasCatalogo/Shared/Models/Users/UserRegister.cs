using System.ComponentModel.DataAnnotations;

namespace GloriaFestasCatalogo.Shared.Models.Users
{
    public class UserRegister
    {

        [Required]
        public string Username { get; set; } = string.Empty;
        [Required, StringLength(100, MinimumLength = 3)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}

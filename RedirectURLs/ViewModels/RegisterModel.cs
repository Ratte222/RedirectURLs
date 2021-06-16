using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedirectURLs.ViewModels
{
    public class RegisterModel
    {
        [EmailAddress]
        [StringLength(50, ErrorMessage = "Email length can't be more than 50.")]
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [StringLength(32, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels.Identity
{
    public class RegisterUserViewModel
    {
        [Required]
        [MaxLength(256)]
        [Display(Name = "Имя пользователя", Prompt = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля", Prompt = "Подтверждение пароля")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels.Identity
{
    public class LoginUserViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя", Prompt = "Имя пользователя")]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Пароль", Prompt = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомни меня")]
        public bool RemeberMe { get; set; }

        //[HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Имя является обязательным")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Номер телефона не указан")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Адрес доставки не указан")]
        public string Address { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels
{
    public enum EmployeePosition
    {
        [Display(Name = "Стажёр")]
        Probation,
        [Display(Name = "Продавец")]
        Seller,
        [Display(Name = "Менеджер")]
        Manager,
        [Display(Name = "Администратор")]
        Adminitrator,
        [Display(Name = "Директор")]
        Director,
        [Display(Name = "Работник IT")]
        IT
    }

    public class EmployeePositionViewModel
    {
        public EmployeePosition Position { get; set; }
    }

    /// <summary>Модель-представления сотрудника</summary>
    public class EmployeeViewModel
    {
        //[HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя является обязательным полем", AllowEmptyStrings = false)]
        [StringLength(maximumLength: 200, MinimumLength = 2, ErrorMessage = "Длина имени должна быть в пределах от 2 до 200 символов")]
        [RegularExpression(@"(?:[А-ЯЁ][а-яё]+)|(?:[A-Z][a-z]+)", ErrorMessage = "Странное имя")]
        public string FirstName { get; set; }


        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия является обязательным полем", AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длина фамилии должна быть в пределах от 2 до 200 символов")]
        public string SecondName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Required(ErrorMessage = "Не указан возраст")]
        public int Age { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Принят на работу")]
        public DateTime StartWork { get; set; }

        [Display(Name = "Должность")]
        public EmployeePosition Position { get; set; }
    }
}

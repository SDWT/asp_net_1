using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
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

    public class EmployeeView
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Принят на работу")]
        public DateTime StartWork { get; set; }

        [Display(Name = "Должность")]
        public EmployeePosition Position { get; set; }
    }
}

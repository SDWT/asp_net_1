using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return GetAll();
        }

        private static readonly List<EmployeeView> __Employees = new List<EmployeeView>
        {
            new EmployeeView {Id = 1, FirstName = "Иван", SecondName = "Иванов", Patronymic = "Иванович", Age = 40,
                Birthday = new DateTime(2000, 9, 11), StartWork = new DateTime(2020, 9, 11), Position = EmployeePosition.Manager},
            new EmployeeView {Id = 2, FirstName = "Петр", SecondName = "Петров", Patronymic = "Петрович", Age = 30,
                Birthday = new DateTime(2010, 9, 11), StartWork = new DateTime(2030, 9, 11), Position = EmployeePosition.Seller},
            new EmployeeView {Id = 3, FirstName = "Сидор", SecondName = "Сидоров", Patronymic = "Сидорович", Age = 20,
                Birthday = new DateTime(2020, 9, 11), StartWork = new DateTime(2040, 9, 11), Position = EmployeePosition.Probation}
        };

        public IActionResult GetAll()
        {
            ViewBag.SomeData = "Hello World!";
            ViewData["Test"] = "TestData";

            return View(__Employees);
        }

        public IActionResult Details(int id)
        {
            EmployeeView result = new EmployeeView
            { Id = id };

            foreach (var employee in __Employees)
            {
                if (employee.Id == id)
                {
                    result = employee;
                    break;
                }
            }

            return View(result);
        }
    }
}
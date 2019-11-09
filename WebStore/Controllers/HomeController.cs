using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;

        public HomeController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReadConfig()
        {
            return Content(_Configuration["CustomData"]);
        }

        private static readonly List<EmployeeView> __Employees = new List<EmployeeView>
        {
            new EmployeeView {Id = 1, FirstName = "Иван", SecondName = "Иванов", Patronymic = "Иванович", Age = 40},
            new EmployeeView {Id = 2, FirstName = "Петр", SecondName = "Петров", Patronymic = "Петрович", Age = 30},
            new EmployeeView {Id = 3, FirstName = "Сидор", SecondName = "Сидоров", Patronymic = "Сидорович", Age = 20}
        };

        public IActionResult GetEmployees()
        {
            ViewBag.SomeData = "Hello World!";
            ViewData["Test"] = "TestData";

            return View(__Employees);
        }

        public IActionResult EmployeeDetails(int id)
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
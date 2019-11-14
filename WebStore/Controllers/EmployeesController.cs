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
            ViewBag.SomeData = "Hello World!";
            ViewData["Test"] = "TestData";

            return View(__Employees);
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
        
        public IActionResult Details(int? Id)
        {
            if (Id is null)
                return BadRequest();

            var employee = __Employees.FirstOrDefault(e => e.Id == Id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        public IActionResult DetailsName(string FirstName, string SecondName)
        {
            if (FirstName is null && SecondName is null)
                return BadRequest();

            IEnumerable<EmployeeView> employees = __Employees;

            if (!string.IsNullOrWhiteSpace(FirstName))
                employees = employees.Where(e => e.FirstName == FirstName);

            if (!string.IsNullOrWhiteSpace(SecondName))
                employees = employees.Where(e => e.SecondName == SecondName);
            
            var employee = employees.FirstOrDefault();
            
            if (employee is null)
                return NotFound();

            return View(nameof(Details), employee);
        }

        [HttpPost]
        public IActionResult Edit(int Id, [FromBody] EmployeeView Employee)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
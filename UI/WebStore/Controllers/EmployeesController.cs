using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;

namespace WebStore.Controllers
{
    //[Route("Users")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData EmployeesData) => _EmployeesData = EmployeesData;

        //[Route("{id}")]
        public IActionResult Index()
        {
            ViewBag.SomeData = "Hello World!";
            ViewData["Test"] = "TestData";

            return View(_EmployeesData.GetAll());
        }

        public IActionResult Details(int? Id)
        {
            if (Id is null)
                return BadRequest();

            var employee = _EmployeesData.GetById((int)Id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        public IActionResult DetailsName(string FirstName, string SecondName)
        {
            if (FirstName is null && SecondName is null)
                return BadRequest();

            var employees = _EmployeesData.GetAll();

            if (!string.IsNullOrWhiteSpace(FirstName))
                employees = employees.Where(e => e.FirstName == FirstName);

            if (!string.IsNullOrWhiteSpace(SecondName))
                employees = employees.Where(e => e.SecondName == SecondName);

            var employee = employees.FirstOrDefault();

            if (employee is null)
                return NotFound();

            return View(nameof(Details), employee);
        }


        [Authorize(Roles = Role.Administrator)]
        public IActionResult Create() => View(new EmployeeViewModel());

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Create(EmployeeViewModel NewEmployee)
        {
            if (!ModelState.IsValid)
                return View(NewEmployee);

            _EmployeesData.Add(NewEmployee);
            _EmployeesData.SaveChanges();

            return RedirectToAction("Details", new { NewEmployee.Id });
        }

        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(int Id)
        {
            if (Id < 0)
                return BadRequest();

            var employee = _EmployeesData.GetById((int)Id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(EmployeeViewModel Employee)
        {
            if (Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            if (Employee.Age < 18)
                ModelState.AddModelError(nameof(Employee.Age), "Возраст не может быть меньше 18 лет");

            if (Employee.FirstName == "123" && Employee.SecondName == "qwe")
                ModelState.AddModelError("", "Странное сочетание имени и фамилии");

            if (!ModelState.IsValid)
                return View(Employee);

            var id = Employee.Id;
            _EmployeesData.Edit(id, Employee);
            _EmployeesData.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrator)]
        public IActionResult Delete(int Id)
        {
            var employee = _EmployeesData.GetById(Id);
            if (employee is null)
                return NotFound();
            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult DeleteConfirmed(int Id)
        {
            _EmployeesData.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
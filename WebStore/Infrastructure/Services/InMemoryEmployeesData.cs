using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly List<EmployeeView> _Employees = new List<EmployeeView>
        {
            new EmployeeView {Id = 1, FirstName = "Иван", SecondName = "Иванов", Patronymic = "Иванович", Age = 40,
                Birthday = new DateTime(2000, 9, 11), StartWork = new DateTime(2020, 9, 11), Position = EmployeePosition.Manager},
            new EmployeeView {Id = 2, FirstName = "Петр", SecondName = "Петров", Patronymic = "Петрович", Age = 30,
                Birthday = new DateTime(2010, 9, 11), StartWork = new DateTime(2030, 9, 11), Position = EmployeePosition.Seller},
            new EmployeeView {Id = 3, FirstName = "Сидор", SecondName = "Сидоров", Patronymic = "Сидорович", Age = 20,
                Birthday = new DateTime(2020, 9, 11), StartWork = new DateTime(2040, 9, 11), Position = EmployeePosition.Probation}
        };

        public IEnumerable<EmployeeView> GetAll() => _Employees;

        public EmployeeView GetById(int Id) => _Employees.FirstOrDefault(e => e.Id == Id);

        public void Add(EmployeeView Employee)
        {
            if (Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            Employee.Id = _Employees.Count == 0 ? 1 : _Employees.Max(e => e.Id) + 1;
            _Employees.Add(Employee);
        }

        public void Edit(int Id, EmployeeView Employee)
        {
            if (Employee is null)
                throw new ArgumentNullException(nameof(Employee));

            var db_employee = GetById(Id);
            if (db_employee is null) return;

            db_employee.FirstName = Employee.FirstName;
            db_employee.SecondName = Employee.SecondName;
            db_employee.Patronymic = Employee.Patronymic;
            db_employee.Age = Employee.Age;
            db_employee.Birthday = Employee.Birthday;
            db_employee.Position = Employee.Position;
            db_employee.StartWork = Employee.StartWork;

        }

        public bool Delete(int Id)
        {
            var db_employee = GetById(Id);
            if (db_employee is null) return false;
            return _Employees.Remove(db_employee);
        }

        public void SaveChanges() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesApiController(IEmployeesData EmployeesData) => _EmployeesData = EmployeesData;

        [HttpGet, ActionName("Get")] // api/Employees or api/Employees/Get
        public IEnumerable<EmployeeViewModel> GetAll()
        {
            return _EmployeesData.GetAll();
        }

        [HttpGet("{Id}"), ActionName("Get")] // Для связки необходимо полное совпадение с точностью дло регистра
        public EmployeeViewModel GetById(int Id)
        {
            return _EmployeesData.GetById(Id);
        }

        [HttpPost, ActionName("Post")]
        public void Add(EmployeeViewModel Employee)
        {
            _EmployeesData.Add(Employee);
        }

        [HttpDelete("{Id}")]
        public bool Delete(int Id)
        {
            return _EmployeesData.Delete(Id);
        }

        [HttpPut("{Id}"), ActionName("Put")]
        public void Edit(int Id, EmployeeViewModel Employee)
        {
            _EmployeesData.Edit(Id, Employee);
        }

        [NonAction]
        public void SaveChanges()
        {
            _EmployeesData.SaveChanges();
        }
    }
}
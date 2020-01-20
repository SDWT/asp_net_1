using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration config) : base(config, "api/Employees") { }

        public IEnumerable<EmployeeViewModel> GetAll() => Get<List<EmployeeViewModel>>(_ServiceAddress);

        public EmployeeViewModel GetById(int Id) => Get<EmployeeViewModel>($"{_ServiceAddress}/{Id}");

        public void Add(EmployeeViewModel Employee) => Post(_ServiceAddress, Employee);

        public bool Delete(int id) => Delete($"{_ServiceAddress}/{id}").IsSuccessStatusCode;

        public EmployeeViewModel Edit(int Id, EmployeeViewModel Employee)
        {
            var response = Put($"{_ServiceAddress}/{Id}", Employee);
            return response.Content.ReadAsAsync<EmployeeViewModel>().Result;
        }

        public void SaveChanges() { }
    }
}

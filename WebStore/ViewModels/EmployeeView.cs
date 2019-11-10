using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public enum EmployeePosition
    {
        Probation, Seller, Manager, Adminitrator, Director, IT
    }

    public class EmployeeView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }

        public DateTime Birthday { get; set; }
        public DateTime StartWork { get; set; }
        public EmployeePosition Position { get; set; }
    }
}

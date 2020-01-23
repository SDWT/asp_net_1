using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>
    /// Контроллер API для работы с сотрудниками
    /// </summary>
    //[Route("api/[controller]")]
    [Route("api/Employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;

        /// <summary> 
        /// Конструктор контроллера для работы данными сотрудников
        /// из интерфейса IEmployeesData
        /// </summary>
        /// <param name="EmployeesData">Реализация интерфейса</param>
        public EmployeesApiController(IEmployeesData EmployeesData) => _EmployeesData = EmployeesData;

        /// <summary> Получение всех работников системы </summary>
        /// <returns>Перечисление работников, доступных в системе</returns>
        [HttpGet, ActionName("Get")] // api/Employees or api/Employees/Get
        public IEnumerable<EmployeeViewModel> GetAll()
        {
            return _EmployeesData.GetAll();
        }

        /// <summary>Получение работника из системы по идентификатору </summary>
        /// <param name="Id">Идентификатор</param>
        /// <returns>Работника с этим идентификатор</returns>
        [HttpGet("{Id}"), ActionName("Get")] // Для связки необходимо полное совпадение с точностью дло регистра
        public EmployeeViewModel GetById(int Id)
        {
            return _EmployeesData.GetById(Id);
        }

        /// <summary>Добавление работника в систему </summary>
        /// <param name="Employee">Модель работника</param>
        [HttpPost, ActionName("Post")]
        public void Add(EmployeeViewModel Employee)
        {
            _EmployeesData.Add(Employee);
        }

        /// <summary>Удаление работника из системы по идентификатору </summary>
        /// <param name="Id">Идентификатор</param>
        /// <returns>True, если работник был удалён</returns>
        [HttpDelete("{Id}")]
        public bool Delete(int Id)
        {
            return _EmployeesData.Delete(Id);
        }

        /// <summary>Изменение работника из системы по идентификатору </summary>
        /// <param name="Id">Идентификатор</param>
        /// <param name="Employee">Новые данные работника</param>
        /// <returns>Данные работника</returns>
        [HttpPut("{Id}"), ActionName("Put")]
        public EmployeeViewModel Edit(int Id, EmployeeViewModel Employee)
        {
            return _EmployeesData.Edit(Id, Employee);
        }

        /// <summary> Сохранение изменений </summary>
        [NonAction]
        public void SaveChanges()
        {
            _EmployeesData.SaveChanges();
        }
    }
}
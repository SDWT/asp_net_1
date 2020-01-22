using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary> Контроллер API для работы с ролями </summary>
    //[Route("api/[controller]")]
    [Route("api/roles")]
    [Produces("application/json")]
    [ApiController]
    public class RolesApiController : ControllerBase
    {
        private readonly RoleStore<Role, WebStoreContext> _RoleStore;

        /// <summary> Конструктор API для работы с ролями по контексту базы данных </summary>
        /// <param name="db">Контекст базы данных</param>
        public RolesApiController(WebStoreContext db) => _RoleStore = new RoleStore<Role, WebStoreContext>(db);

        /* ---------------------------------------------------------------- */

        /// <summary>Получение всех ролей системы</summary>
        /// <returns>Перечисление ролей, доступных в системе</returns>
        [HttpGet("AllRoles")]
        public async Task<IEnumerable<Role>> GetAllRoles() => await _RoleStore.Roles.ToArrayAsync();

        /* ---------------------------------------------------------------- */

        /// <summary> Создание роли </summary>
        /// <param name="role">Модель роли</param>
        /// <returns>True, если роль была успешно создана</returns>
        [HttpPost]
        public async Task<bool> CreateAsync(Role role) => (await _RoleStore.CreateAsync(role)).Succeeded;

        /// <summary> Редактирование роли по модели роли </summary>
        /// <param name="role">Модель роли</param>
        /// <returns>True, если роль была успешно изменена</returns>
        [HttpPut]
        public async Task<bool> UpdateAsync(Role role) => (await _RoleStore.UpdateAsync(role)).Succeeded;

        /// <summary> Удаление роли по модели роли </summary>
        /// <param name="role">Модель роли</param>
        /// <returns>True, если роль была успешно удалена</returns>
        [HttpPost("Delete")]
        public async Task<bool> DeleteAsync(Role role) => (await _RoleStore.DeleteAsync(role)).Succeeded;

        /// <summary> Получение идентификатора роли по роли</summary>
        /// <param name="role">Модель роли</param>
        /// <returns>Идентификатор роли</returns>
        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync(Role role) => await _RoleStore.GetRoleIdAsync(role);

        /// <summary> Получение имени роли по роли</summary>
        /// <param name="role">Модель роли</param>
        /// <returns>Имя роли</returns>
        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync(Role role) => await _RoleStore.GetRoleNameAsync(role);

        /// <summary> Установка имени роли по роли</summary>
        /// <param name="role">Роль для изменения имени</param>
        /// <param name="name">Новое имя роли</param>
        [HttpPost("SetRoleName/{name}")]
        public async Task SetRoleNameAsync(Role role, string name)
        {
            await _RoleStore.SetRoleNameAsync(role, name);
            await _RoleStore.UpdateAsync(role);
        }

        /// <summary>
        /// Получение нормализированного (приведённого к верхнему регистру)
        /// имени роли по роли
        /// </summary>
        /// <param name="role">Модель роли</param>
        /// <returns>Нормализированное имя роли</returns>
        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync(Role role) => await _RoleStore.GetNormalizedRoleNameAsync(role);

        /// <summary> Установка нормализированного имени роли по роли</summary>
        /// <param name="role">Роль для изменения нормализированного имени</param>
        /// <param name="name">Новое нормализированное имя роли</param>
        [HttpPost("SetNormalizedRoleName/{name}")]
        public async Task SetNormalizedRoleNameAsync(Role role, string name)
        {
            await _RoleStore.SetNormalizedRoleNameAsync(role, name);
            await _RoleStore.UpdateAsync(role);
        }

        /// <summary> Получение роли по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Роль</returns>
        [HttpGet("FindById/{id}")]
        public async Task<Role> FindByIdAsync(string id) => await _RoleStore.FindByIdAsync(id);

        /// <summary> Получение роли по имени </summary>
        /// <param name="name">Имя</param>
        /// <returns>Роль</returns>
        [HttpGet("FindByName/{name}")]
        public async Task<Role> FindByNameAsync(string name) => await _RoleStore.FindByNameAsync(name);
    }
}

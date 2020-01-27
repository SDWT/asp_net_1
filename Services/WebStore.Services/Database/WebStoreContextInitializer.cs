using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.DataBase
{
    public class WebStoreContextInitializer
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        private readonly ILogger<WebStoreContextInitializer> _Logger;

        public WebStoreContextInitializer(
            WebStoreContext db, 
            UserManager<User> UserManager, 
            RoleManager<Role> RoleManager,
            ILogger<WebStoreContextInitializer> Logger)
        {
             _db = db;
            _UserManager = UserManager;
            _RoleManager= RoleManager;
            _Logger = Logger;
        }

        public async Task InitializeAsync()
        {
            var db = _db.Database;

            //if (await db.EnsureDeletedAsync())
            //{
            //    // База данных существовала и была успешно удалена
            //}

            // Создаёт базу данных
            // await db.EnsureCreatedAsync();

            // Автомотическое создание бд и применение миграций до последней версии
            await db.MigrateAsync();

            await IdentityInitializeAsync();

            // Отказ от дальнейшей инициализации по признаку заполненности продуктов
            if (await _db.Products.AnyAsync()) return;

            #region Add Sections, Brands, Products to data base

            // Add Sections to data base
            using (var transaction = await db.BeginTransactionAsync())
            {
                await _db.Sections.AddRangeAsync(TestData.Sections);

                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] ON"); // Включение ручного управления первичными ключами
                await _db.SaveChangesAsync(); // Сохранение изменений
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF"); // Выключение ручного управления первичными ключами

                transaction.Commit(); // Отправка транзакции в бд
            }

            // Add Brands to data base
            using (var transaction = await db.BeginTransactionAsync())
            {
                await _db.Brands.AddRangeAsync(TestData.Brands);

                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");


                transaction.Commit();
            }
            
            // Add Products to data base
            using (var transaction = await db.BeginTransactionAsync())
            {
                await _db.Products.AddRangeAsync(TestData.Products);

                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");


                transaction.Commit();
            }
            #endregion

        }

        private async Task CheckRole(string RoleName)
        {
            if (!await _RoleManager.RoleExistsAsync(RoleName))
                await _RoleManager.CreateAsync(new Role { Name = RoleName });
        }

        private async Task IdentityInitializeAsync()
        {
            await CheckRole(Role.Administrator);
            await CheckRole(Role.User);

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User
                {
                    UserName = User.Administrator,
                    Email = "admin@server.com"
                };
                var create_result = await _UserManager.CreateAsync(admin, User.AdminPasswordDefault);

                if (create_result.Succeeded)
                    await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                else
                {
                    var errors = string.Join(", ", create_result.Errors.Select(e => e.Description));
                    _Logger.LogError("Ошибка при создании пользователя Администратора в БД {0}", errors);
                    throw new InvalidOperationException($"Ошибка при создании администратора в БД {errors}");
                }
            }

        }
    }
}

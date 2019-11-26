﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreContextInitializer
    {
        private readonly WebStoreContext _db;

        public WebStoreContextInitializer(WebStoreContext db) => _db = db;

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

            // Отказ от дальнейшей инициализации по признаку заполненности продуктов
            if (await _db.Products.AnyAsync()) return;

            using (var transaction = await db.BeginTransactionAsync())
            {
                await _db.Sections.AddRangeAsync(TestData.Sections);

                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] ON"); // Включение ручного управления первичными ключами
                await _db.SaveChangesAsync(); // Сохранение изменений
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF"); // Выключение ручного управления первичными ключами

                transaction.Commit(); // Отправка транзакции в бд
            }

            using (var transaction = await db.BeginTransactionAsync())
            {
                await _db.Brands.AddRangeAsync(TestData.Brands);

                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");


                transaction.Commit();
            }

            using (var transaction = await db.BeginTransactionAsync())
            {
                await _db.Products.AddRangeAsync(TestData.Products);

                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");


                transaction.Commit();
            }
        }
    }
}

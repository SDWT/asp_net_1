﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Services
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _UserManager;

        public SqlOrderService(WebStoreContext db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public Order GetOrderById(int Id) => _db.Orders
            .Include(order => order.OrderItems)
            .FirstOrDefault(order => order.Id == Id);

        public IEnumerable<Order> GetUserOrders(string UserName) => _db.Orders
            .Include(order => order.User)
            .Include(order => order.OrderItems)
            .Where(order => order.User.NormalizedUserName == UserName.ToUpper())
            .ToArray();

        public Order CreateOrder(OrderViewModel OrderModel, CartViewModel CartModel, string UserName)
        {
            var user = _UserManager.FindByNameAsync(UserName).Result;

            using (var transaction = _db.Database.BeginTransaction())
            {
                var order = new Order
                {
                    Name = OrderModel.Name,
                    Address = OrderModel.Address,
                    Phone = OrderModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };

                _db.Orders.Add(order);

                foreach (var (product_model, quantity) in CartModel.Items)
                {
                    var product = _db.Products.FirstOrDefault(p => p.Id == product_model.Id);
                    if (product is null)
                        throw new InvalidOperationException($"Товар с идентификатором id:{product_model.Id} отсутствует в базе данных");
                    var order_item = new OrderItem
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = quantity,
                        Product = product
                    };

                    _db.OrderItems.Add(order_item);
                }

                _db.SaveChanges();
                transaction.Commit();
                return order;
            }
        }
    }
}
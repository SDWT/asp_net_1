using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Domain.DTO.Orders;
using Microsoft.Extensions.Logging;
using WebStore.Services.Map;

namespace WebStore.Services.Product
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _UserManager;
        private readonly ILogger<SqlOrderService> _Logger;

        public SqlOrderService(WebStoreContext db, UserManager<User> UserManager, ILogger<SqlOrderService> Logger)
        {
            _db = db;
            _UserManager = UserManager;
            _Logger = Logger;
        }

        public OrderDTO GetOrderById(int Id)
        {
            var o = _db.Orders
            .Include(order => order.OrderItems)
            .FirstOrDefault(order => order.Id == Id);

            return o.ToDTO();
        }

        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => _db.Orders
            .Include(order => order.User)
            .Include(order => order.OrderItems)
            .Where(order => order.User.NormalizedUserName == UserName.ToUpper())
            .ToArray()
            .Select(OrderMapper.ToDTO);

        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName)
        {
            var user = _UserManager.FindByNameAsync(UserName).Result;
            using (_Logger.BeginScope("Создание заказа для пользователя с именем {0}", UserName))
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    var order = new Order
                    {
                        Name = OrderModel.OrderViewModel.Name,
                        Address = OrderModel.OrderViewModel.Address,
                        Phone = OrderModel.OrderViewModel.Phone,
                        User = user,
                        Date = DateTime.Now
                    };

                    _Logger.LogInformation("Добавление заказав от {0} в базу данных для пользователя {1}", order.Date, order.Name);
                    _db.Orders.Add(order);

                    _Logger.LogInformation("Добавление товаров заказа от {0} в базу данных", order.Date);
                    foreach (var item in OrderModel.OrderItems)
                    {
                        var product = _db.Products.FirstOrDefault(p => p.Id == item.Id);
                        if (product is null)
                        {
                            _Logger.LogError("Товар с идентификатором id:{0} из заказа {1} от {2} отсутствует в базе данных", 
                              item.Id, order.Name, order.Date);
                            throw new InvalidOperationException($"Товар с идентификатором id:{item.Id} отсутствует в базе данных");
                        }
                        var order_item = new OrderItem
                        {
                            Order = order,
                            Price = product.Price,
                            Quantity = item.Quantity,
                            Product = product
                        };

                        _db.OrderItems.Add(order_item);
                    }
                    _Logger.LogInformation("Товары заказа от {0} добавлены в базу данных", order.Date);

                    _db.SaveChanges();
                    transaction.Commit();
                    _Logger.LogInformation("Заказ сохранён в базе данных");
                    return order.ToDTO();
                }
            }
        }
    }
}

﻿using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;

namespace WebStore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Orders([FromServices] IOrderService OrderService) => View(OrderService
            .GetUserOrders(User.Identity.Name)
            .Select(order => new UserOrderViewModel
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                Phone = order.Phone,
                TotalSum = order.OrderItems.Sum(item => item.Price * item.Quantity)
            }));
    }
}

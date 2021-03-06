﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;
using WebStore.Domain.DTO.Orders;
using System.Linq;

namespace WebStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _CartService;
        public CartController(ICartService CartService) => _CartService = CartService;

        public IActionResult Details() => View(new CartDetailsViewModel
        {
            CartViewModel = _CartService.TransformFromCart(),
            OrderViewModel = new OrderViewModel()
        });

        public IActionResult AddToCart(int id)
        {
            _CartService.AddToCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult DecrementFromCart(int id)
        {
            _CartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveFromCart(int id)
        {
            _CartService.RemoveFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveAll()
        {
            _CartService.RemoveAll();
            return RedirectToAction("Details");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel Model, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new CartDetailsViewModel
                {
                    CartViewModel = _CartService.TransformFromCart(),
                    OrderViewModel = Model
                });

            var create_order_model = new CreateOrderModel
            {
                OrderViewModel = Model,
                OrderItems = _CartService.TransformFromCart().Items.Select(item => new OrderItemDTO
                {
                    Id = item.Key.Id,
                    Price = item.Key.Price,
                    Quantity = item.Value
                })
                .ToList()
            };

            var order = OrderService.CreateOrder(create_order_model, User.Identity.Name);

            _CartService.RemoveAll();

            return RedirectToAction("OrderConfirmed", new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        #region API

        public IActionResult AddToCartAPI(int id)
        {
            _CartService.AddToCart(id);
            return Json(new { id, message = $"Товар id:{id} успешно добавлен в корзину" });
        }

        public IActionResult DecrementFromCartAPI(int id)
        {
            _CartService.DecrementFromCart(id);
            return Json(new { id, message = $"Количество товара id:{id} в корзине уменьшено на 1" });
        }

        public IActionResult RemoveFromCartAPI(int id)
        {
            _CartService.RemoveFromCart(id);
            return Json(new { id, message = $"Товар id:{id} удалён из корзины" });
        }

        public IActionResult RemoveAllAPI()
        {
            _CartService.RemoveAll();
            return Json(new { message = "Корзина очищена" });
        }

        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult GetProductsCountFromCart() => Json(new { count = $"{_CartService.TransformFromCart().ItemsCount}"});

        #endregion
    }
}
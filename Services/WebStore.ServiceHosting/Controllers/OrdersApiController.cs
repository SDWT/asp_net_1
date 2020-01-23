using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>API контроллер заказов</summary>
    //[Route("api/[controller]")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _IOrderService;

        /// <summary>Конструктор контролера</summary>
        /// <param name="IOrderService">Интерфейс взаимодействия с сервисом хранения заказов</param>
        public OrdersApiController(IOrderService IOrderService) => _IOrderService = IOrderService;

        /// <summary>Создание заказа пользователя</summary>
        /// <param name="OrderModel">Модель создания заказа</param>
        /// <param name="UserName">Пользователь</param>
        /// <returns>Модель заказа</returns>
        [HttpPost("{UserName?}")]
        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName)
        {
            return _IOrderService.CreateOrder(OrderModel, UserName);
        }

        /// <summary>Получение заказа по идентификатору</summary>
        /// <param name="Id">Идентификатор</param>
        /// <returns></returns>
        [HttpGet("{Id}"), ActionName("Get")]
        public OrderDTO GetOrderById(int Id)
        {
            return _IOrderService.GetOrderById(Id);
        }

        /// <summary>Получение всех заказов пользователя</summary>
        /// <param name="UserName">Пользователь</param>
        /// <returns>Перечисление заказов</returns>
        [HttpGet("user/{UserName}")]
        public IEnumerable<OrderDTO> GetUserOrders(string UserName)
        {
            return _IOrderService.GetUserOrders(UserName);
        }
    }
}
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _IOrderService;

        public OrdersApiController(IOrderService IOrderService) => _IOrderService = IOrderService;

        [HttpPost("{UserName?}")]
        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName)
        {
            return _IOrderService.CreateOrder(OrderModel, UserName);
        }

        [HttpGet("{Id}"), ActionName("Get")]
        public OrderDTO GetOrderById(int Id)
        {
            return _IOrderService.GetOrderById(Id);
        }

        [HttpGet("user/{UserName}")]
        public IEnumerable<OrderDTO> GetUserOrders(string UserName)
        {
            return _IOrderService.GetUserOrders(UserName);
        }
    }
}
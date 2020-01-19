using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using WebStore.Clients.Base;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(IConfiguration Configuration) : base(Configuration, "api/orders") { }

        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName) =>
            Post($"{_ServiceAddress}/{UserName}", OrderModel)
            .Content
            .ReadAsAsync<OrderDTO>()
            .Result;


        public OrderDTO GetOrderById(int Id) => Get<OrderDTO>($"{_ServiceAddress}/{Id}");

        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => Get<List<OrderDTO>>($"{_ServiceAddress}/user/{UserName}");
    }
}

using System.Linq;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;

namespace ConvertDTO.Orders
{
    public static class OrderExtension
    {
        public static OrderDTO ConvertToDTO(this Order order) => new OrderDTO
        {
            Id = order.Id,
            Name = order.Name,
            Phone = order.Phone,
            Address = order.Address,
            Date = order.Date,
            OrderItems = order.OrderItems.Select(item => item.ConvertToDTO())
        };
    }
}

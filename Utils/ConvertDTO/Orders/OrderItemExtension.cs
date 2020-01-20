using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;

namespace ConvertDTO.Orders
{
    public static class OrderItemExtension
    {
        public static OrderItemDTO ConvertToDTO(this OrderItem orderItem) => new OrderItemDTO
        {
            Id = orderItem.Id,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity
        };
    }
}

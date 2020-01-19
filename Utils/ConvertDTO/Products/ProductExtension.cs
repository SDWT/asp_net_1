using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace ConvertDTO.Products
{
    public static class ProductExtension
    {
        public static ProductDTO ConvertToDTO(this Product product)
        {
            var dto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand is null ? null : product.Brand.ConvertToDTO(),
                Section = product.Section is null ? null : product.Section.ConvertToDTO()
            };

            return dto;
        }

    }
}

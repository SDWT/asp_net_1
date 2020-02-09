using WebStore.Domain.DTO.Products;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Map
{
    public static class ProductMapper
    {
        public static ProductDTO ToDTO(this Domain.Entities.Product product) => product is null ? null : new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Order = product.Order,
            Brand = product.Brand.ToDTO(),
            Section = product.Section.ToDTO()
        };

        public static Domain.Entities.Product FromDTO(this ProductDTO product) => product is null ? null : new Domain.Entities.Product
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Order = product.Order,
            BrandId = product.Brand?.Id,
            Brand = product.Brand.FromDTO(),
            Section = product.Section.FromDTO()
        };

        public static ProductViewModel ToViewModel(this ProductDTO product) => product is null ? null : new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Order = product.Order,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand?.Name,
            Section = product.Section?.Name
        };

        public static ProductDTO FromViewModel(this ProductViewModel product) => product is null ? null : new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Order = product.Order
        };
    }
}

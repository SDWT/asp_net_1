using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace ConvertDTO.Products
{
    public static class BrandExtension
    {
        public static BrandDTO ConvertToDTO(this Brand brand)
        {
            var dto = new BrandDTO
            {
                Id = brand.Id,
                Name = brand.Name,
            };

            return dto;
        }
    }
}

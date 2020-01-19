using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace ConvertDTO.Products
{
    public static class SectionExtension
    {
        public static SectionDTO ConvertToDTO(this Section section)
        {
            var dto = new SectionDTO
            {
                Id = section.Id,
                Name = section.Name,
                Order = section.Order
            };

            return dto;
        }
    }
}

using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.DTO.Products
{
    public class SectionDTO : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

    }
}

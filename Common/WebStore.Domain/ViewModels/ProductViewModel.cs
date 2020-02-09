using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.ViewModels
{
    public class ProductViewModel : INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }

        [Required, Display(Name = "Название")]
        public string Name { get; set; }

        [Required, Display(Name = "Порядок")]
        public int Order { get; set; }

        [Required, Display(Name = "Изображение")]
        public string ImageUrl { get; set; }

        [Required, Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Бренд")]
        public string Brand { get; set; }

        [Display(Name = "Категория")]
        public string Section { get; set; }
    }
}

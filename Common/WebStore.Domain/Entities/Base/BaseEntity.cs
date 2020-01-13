using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    /// <summary> Базовая сущность</summary>
    public abstract class BaseEntity : IBaseEntity
    {
        [Key] // Установка первичного ключа
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ограничение уникальности значений в столбце
        public int Id { get; set; }
    }
}

namespace WebStore.Domain.Entities.Base.Interfaces
{
    /// <summary>Интерфейс упорядочиваемой сущности</summary>
    public interface IOrderedEntity : IBaseEntity
    {
        /// <summary>Порядок</summary>
        int Order { get; set; }
    }
}

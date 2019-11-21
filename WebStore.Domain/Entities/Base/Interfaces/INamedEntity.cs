namespace WebStore.Domain.Entities.Base.Interfaces
{
    /// <summary>Интерфейс именованой сущности</summary>
    public interface INamedEntity : IBaseEntity
    {
        /// <summary>Имя</summary>
        string Name { get; set; }
    }
}

using WebStore.Common.Contracts;

namespace WebStore.Models.Entities
{
    public abstract class EntityBase<T> : IEntity<T>
    {
        public T Id { get; set; }
    }
}

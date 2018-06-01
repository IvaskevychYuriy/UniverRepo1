namespace WebStore.Models.Contracts
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}

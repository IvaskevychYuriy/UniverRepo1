namespace WebStore.Common.Contracts
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}

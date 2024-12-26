public interface IEntity<T> where T : EntityData
{
    public void SetUp(T data);
    public void SetFromStart();
}
using Data;

namespace Entities
{
    public interface IEntity<T> where T : EntityData
    {
        public void SetUp(T data);
        public void Reset();
    }
}
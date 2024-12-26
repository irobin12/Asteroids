using UnityEngine;

public abstract class EntitySpawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    protected GameObjectPool<T> Pool;

    public void SetUp(T prefab, int defaultSize = 5, int maxSize = 15)
    {
        Pool = new GameObjectPool<T>(prefab, defaultSize, maxSize);
    }

    public void ReleaseAll()
    {
        Pool.ReleaseAll();
    }
}
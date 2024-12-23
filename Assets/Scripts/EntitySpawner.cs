using UnityEngine;

public abstract class EntitySpawner<T> : MonoBehaviour where T: MonoBehaviour
{
    protected GameObjectPool<T> Pool;

    public void SetUp(T prefab, int defaultSize = 5, int maxSize = 15)
    {
        Pool = new(prefab, defaultSize, maxSize);
    }
}
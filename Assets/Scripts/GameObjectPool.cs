using System;
using Data;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public interface IPoolable
{
    void Release();
}

public class GameObjectPool<T> where T : MonoBehaviour, IPoolable
{
    public Action ReleaseAll;
    private readonly ObjectPool<T> pool;
    private readonly EntityData entityData;
    private readonly T objectToPool;
    private readonly Transform parent;
    
    public GameObjectPool(T prefab, int defaultSize = 5, int maxSize = 20)
    {
        objectToPool = prefab;
        parent = new GameObject($"{prefab.name}Pool").transform;
        
        pool = new ObjectPool<T>(
            CreatePooledObject,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            true,
            defaultSize,
            maxSize
            );
    }
    
    private T CreatePooledObject()
    {
        var pooledObject = Object.Instantiate(objectToPool, parent);
        // pooledObject.
        // pooledObject.SetUp(entityData);
        ReleaseAll += pooledObject.Release;
        return pooledObject;
    }
    
    public T GetObject(Vector3 position, Quaternion rotation)
    {
        var pooledObject = pool.Get();
        pooledObject.transform.position = position;
        pooledObject.transform.rotation = rotation;
        
        return pooledObject;
    }

    public void ReleaseGameObject(T pooledObject)
    {
        pool.Release(pooledObject);
    }

    private void OnGetFromPool(T pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(T pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(T pooledObject)
    {
        Object.Destroy(pooledObject);
    }

    public void ReleaseAllObjects()
    {
        ReleaseAll?.Invoke();
    }
}
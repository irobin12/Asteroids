using UnityEngine;
using UnityEngine.Pool;

public class MovingEntityPool
{
    private readonly float entitiesThrust;
    private readonly float entitiesTorque;
    
    private readonly Transform parent;
    private readonly MovingEntity prefab;
    private readonly ObjectPool<MovingEntity> pool;
    
    public MovingEntityPool(MovingEntity prefab, float entitiesThrust, float entitiesTorque = 0f, int defaultSize = 5, int maxSize = 20)
    {
        this.entitiesThrust = entitiesThrust;
        this.entitiesTorque = entitiesTorque;
        this.prefab = prefab;
        
        parent = new GameObject($"{prefab.name}Pool").transform;
        
        pool = new ObjectPool<MovingEntity>(
            CreatePooledObject,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            true,
            defaultSize,
            maxSize
            );
    }
    
    private MovingEntity CreatePooledObject()
    {
        var movingEntity = Object.Instantiate(prefab, parent);
        movingEntity.Initialise(entitiesThrust, entitiesTorque);
        return movingEntity;
    }
    
    public MovingEntity GetEntity(Transform spawnPoint)
    {
        MovingEntity movingEntity = pool.Get();
        movingEntity.transform.position = spawnPoint.position;
        movingEntity.transform.rotation = spawnPoint.rotation;
        
        if(movingEntity.TryGetComponent (out Rigidbody2D rigidbody))
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0f;
        }
        
        return movingEntity;
    }

    public void ReleaseEntity(MovingEntity movingEntity)
    {
        pool.Release(movingEntity);
    }

    private void OnGetFromPool(MovingEntity movingEntity)
    {
        movingEntity.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(MovingEntity movingEntity)
    {
        movingEntity.gameObject.SetActive(false);
        movingEntity.Reset();
    }

    private void OnDestroyPooledObject(MovingEntity movingEntity)
    {
        Object.Destroy(movingEntity);
    }
}
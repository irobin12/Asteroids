using Data;
using UnityEngine;
using UnityEngine.Pool;

public class MovingEntityPool
{
    private readonly MovingEntityData entityData;
    
    private readonly Transform parent;
    private readonly ObjectPool<MovingEntity> pool;
    
    public MovingEntityPool(MovingEntityData movingEntityData, int defaultSize = 5, int maxSize = 20)
    {
        entityData = movingEntityData;
        
        parent = new GameObject($"{entityData.prefab.name}Pool").transform;
        
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
        var movingEntity = Object.Instantiate(entityData.prefab, parent);
        movingEntity.Initialise(entityData);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    //public static ObjectPool Instance;
    public DictionaryPool<ObjectType, IObjectPool<GameObject>> testPool = 
        new DictionaryPool<ObjectType, IObjectPool<GameObject>>();

    public IObjectPool<GameObject> pool;

    // 실험용
    public GameObject poolingObject;
    
    public bool collectionChecks = true;
    public int maxPoolSize = 100;

    public DictionaryPool<ObjectType, IObjectPool<GameObject>> TestPool
    {
        get
        {
            if (pool == null)
            {
                pool = new ObjectPool<GameObject>(
                    CreateNewObject,
                    OnTakeFromPool,
                    OnReturnedToPool,
                    OnDestroyObject,
                    collectionChecks, 10, maxPoolSize);
            }
            return pool;
        }
    }

    // public IObjectPool<GameObject> Pool
    // {
    //     get
    //     {
    //         if (pool == null)
    //         {
    //             pool = new ObjectPool<GameObject>(
    //                 CreateNewObject,
    //                 OnTakeFromPool,
    //                 OnReturnedToPool,
    //                 OnDestroyObject,
    //                 collectionChecks, 10, maxPoolSize);
    //         }
    //         return pool;
    //     }
    // }

    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(poolingObject, transform);
        var returnToPool = newObj.AddComponent<ReturnToPool>();
        returnToPool.m_pool = Pool;
        return newObj;
    }

    public void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
        // Debug.Log("잘썼습니다");
    }

    public void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
        //Debug.Log("가져갑니다");
    }

    private void OnDestroyObject(GameObject obj)
    {
        Destroy(obj);
        // Debug.Log("Destroy object");
    }

    void OnGUI()
    {
        GUILayout.Label("Pool size: " + Pool.CountInactive);
        if (GUILayout.Button("Create Particles"))
        {
            var amount = Random.Range(1, 5);
            for (int i = 0; i < amount; ++i)
            {
                var ps = Pool.Get();
                ps.transform.position = Random.insideUnitSphere * 10;
            }
        }
    }
}

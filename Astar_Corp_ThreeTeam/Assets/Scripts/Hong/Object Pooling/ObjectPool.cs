using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum PoolName
{
    Monster,
    Footprint,
    Particle,
    Object,
    Sound,
}

[System.Serializable]
public class ObjectPool
{
    public int quantity;
    public PoolName poolName;
    public GameObject prefab;

    [HideInInspector]
    public Transform poolTr;
    [HideInInspector]
    public bool collectionChecks = true;
    public int maxPoolSize = 10;

    IObjectPool<GameObject> m_Pool;

    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new ObjectPool<GameObject>(
                    () => CreatePooledItem(prefab), OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, 
                    collectionChecks, quantity, maxPoolSize) ;
            }
            return m_Pool;
        }
    }

    GameObject CreatePooledItem(GameObject prefab)
    {
        var newObj = GameObject.Instantiate(prefab);
        newObj.transform.SetParent(poolTr);
        var returnToPool = newObj.AddComponent<ReturnToPool>();
        returnToPool.m_pool = Pool;
        return newObj;

        //Debug.Log("Created");
        //var go = GameObject.Instantiate(prefab);
        //go.transform.SetParent(poolTr);
        //return go;
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(GameObject go)
    {
        go.SetActive(false);
        //Debug.Log("Àß½è½À´Ï´Ù");
        
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GameObject go)
    {
        go.SetActive(true);
        //Debug.Log("ºô·Á°©´Ï´Ù");
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    public void OnDestroyPoolObject(GameObject go)
    {
        GameObject.Destroy(go);
        //Debug.Log("Destroy");
    }
}

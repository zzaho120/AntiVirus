using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum PoolName
{
    Particle,
    Object,
    Sound,
}

public class ObjectPool : MonoBehaviour
{
    public PoolName objectType;
    
    // public int poolCount;
    public IObjectPool<GameObject> pool;

    // �����
    public GameObject poolingObject;

    [HideInInspector]
    public bool collectionChecks = true;
    public int maxPoolSize = 100;

    public IObjectPool<GameObject> Pool
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

    private void Awake()
    {
        CreatePools();
    }

    private Transform temp; // �θ� Ʈ������ �ӽ� ����� -> ���߿� �����ϱ�

    private void CreatePools()
    {
        GameObject poolTr = new GameObject(objectType.ToString() + " Pool");
        poolTr.transform.SetParent(this.transform);
        temp = poolTr.transform;
        // return poolTr;
    }

    private GameObject CreateNewObject()
    {
        if (objectType == PoolName.Object)
        {
            var newObj = Instantiate(poolingObject, temp);
            var returnToPool = newObj.AddComponent<ReturnToPool>();
            returnToPool.m_pool = Pool;
            return newObj;
        }
        else return null;
    }

    public void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
        // Debug.Log("�߽���ϴ�");
    }

    public void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
        //Debug.Log("�������ϴ�");
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

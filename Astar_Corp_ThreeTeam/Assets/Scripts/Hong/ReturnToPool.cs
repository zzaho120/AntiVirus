using System;
using UnityEngine;
using UnityEngine.Pool;

// This component returns the particle system to the pool when the OnParticleSystemStopped event is received.
// [RequireComponent(typeof(ParticleSystem))]
public class ReturnToPool : MonoBehaviour
{
    //public DictionaryPool<ObjectType, IObjectPool<GameObject>> m_testPool = 
    //    new DictionaryPool<ObjectType, IObjectPool<GameObject>>();

    public IObjectPool<GameObject> m_pool;

    void Update()
    {
        // Pool�� Return ���� Ÿ�̹� üũ
        // Release �ؼ� �����ָ� ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_pool.Release(gameObject);
            Debug.Log("Returned at " + DateTime.Now.ToString());
        }
    }
}

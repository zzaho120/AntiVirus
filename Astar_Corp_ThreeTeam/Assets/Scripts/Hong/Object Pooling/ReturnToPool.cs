using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

// This component returns the particle system to the pool when the OnParticleSystemStopped event is received.
// [RequireComponent(typeof(ParticleSystem))]
public class ReturnToPool : MonoBehaviour
{
    public IObjectPool<GameObject> m_pool;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();    
    }

    public void Return()
    {
        if (agent != null)
            agent.enabled = false;

        m_pool.Release(gameObject);
        Debug.Log("Returned at " + DateTime.Now.ToString());
    }

}

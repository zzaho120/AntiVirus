using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSetting : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isAttackState;
    float extraRotationSpeed = 5f;

    [HideInInspector]
    public Vector3 targetPos;
    private Transform target;
    private NavMeshPath path;
    private float elapsed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        path = new NavMeshPath();
        elapsed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //elapsed += Time.deltaTime;
        ////if (elapsed > 1.0f)
        ////{
        ////    elapsed -= 1.0f;
        ////    //NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        ////    NavMesh.CalculatePath(transform.position, targetPos, NavMesh.AllAreas, path);
        ////
        ////}
        //
        //NavMesh.CalculatePath(transform.position, targetPos, NavMesh.AllAreas, path);
        //
        //if (elapsed > 5)
        //{
        //    if (path.corners.Length > 0)
        //        Debug.Log("코너? " + path.corners.Length);
        //
        //    elapsed = 0;
        //}

        

        //for (int i = 0; i < path.corners.Length - 1; i++)
        //{
        //    if (Vector3.Distance(agent.transform.position, path.corners[i]) <= 1)
        //    {
        //        agent.velocity = Vector3.zero;
        //        agent.ResetPath();
        //        Debug.Log("경로 재설정");
        //    }
        //}
        ////    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);

        // 회전 각 실시간 업데이트
        //Vector3 lookrotation = agent.steeringTarget - transform.position;
        //transform.rotation = Quaternion.Slerp(
        //    transform.rotation, 
        //    Quaternion.LookRotation(lookrotation), 
        //    extraRotationSpeed * Time.deltaTime);
    }

    private void TraceNavSetting()
    {
        agent.ResetPath();
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    private void AttackNavSetting()
    {
        agent.ResetPath();
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.velocity = Vector3.zero;
    }
}

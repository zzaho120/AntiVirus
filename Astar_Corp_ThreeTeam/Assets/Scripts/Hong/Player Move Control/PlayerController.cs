using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// NavMeshAgent 매뉴얼
// https://docs.unity3d.com/kr/530/ScriptReference/NavMeshAgent.html

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private CharacterController characterController;

    public GameObject panel;

    //private Vector3 calcVelocity = Vector3.zero;

    // 더블점프 방지용 -> 나중에 써야할지도?
    //private bool isGround = false;
    //[SerializeField]
    //float groundCheckDistance = 0.3f;

    // to set layers
    [SerializeField]
    LayerMask groundLayerMask;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        //panel = GetComponent<FadeOutTest>().gameObject;

        // 캐릭터 컨트롤러 포지션 설정할 때 사용
        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    void Update()
    {
        // [수정] 현재 -> 마우스 클릭 방향으로 이동하게
        // 나중에 터치 포지션으로 이동하도록 수정
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 100, groundLayerMask))
            {
                Debug.Log("충돌체 이름 : " + raycastHit.collider.name + ", 좌표 : " + raycastHit.point);
                agent.SetDestination(raycastHit.point);
            }
        }


        if (agent.remainingDistance > agent.stoppingDistance)
        {
            characterController.Move(agent.velocity * Time.deltaTime);
            // Debug.Log(agent.stoppingDistance);
            // RemainingDistance : 현재 경로(path)에서 남아있는 거리
        }
        else
        {
            characterController.Move(Vector3.zero);
        }
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }

    // 충돌 판별
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Debug.Log(hit.collider.name);
        
        // 씬 전환 시? FadeOut 효과
        panel.GetComponent<FadeOutTest>().StartFade();

        //if (agent.remainingDistance <= agent.stoppingDistance)
        //{
        //}
    }
}


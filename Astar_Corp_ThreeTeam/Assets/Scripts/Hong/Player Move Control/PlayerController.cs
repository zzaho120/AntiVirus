using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// NavMeshAgent 매뉴얼
// https://docs.unity3d.com/kr/530/ScriptReference/NavMeshAgent.html

public class PlayerController : MonoBehaviour
{
    //지은.
    public NonBattleMgr manager;

    private NavMeshAgent agent;
    private CharacterController characterController;

    public GameObject panel;
    public bool isMove;//지은.
    bool isBattle;
    float randomTimer;

    float originAgentSpeed;
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
        originAgentSpeed = agent.speed;
        //panel = GetComponent<FadeOutTest>().gameObject;

        // 캐릭터 컨트롤러 포지션 설정할 때 사용
        agent.updatePosition = false;
        agent.updateRotation = true;

        isMove = false;
        isBattle = false;



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
                //Debug.Log("충돌체 이름 : " + raycastHit.collider.name + ", 좌표 : " + raycastHit.point);
                agent.SetDestination(raycastHit.point);
            }
        }


        if (agent.remainingDistance > agent.stoppingDistance)
        {
            characterController.Move(agent.velocity * Time.deltaTime);
            isMove = true;
            // Debug.Log(agent.stoppingDistance);
            // RemainingDistance : 현재 경로(path)에서 남아있는 거리
        }
        else
        {
            characterController.Move(Vector3.zero);
            isMove = false;
        }
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }

    // 충돌 판별
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("RandomEvent")) return;

        // 씬 전환 시? FadeOut 효과
        //panel.GetComponent<FadeOutTest>().StartFade();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RandomEvent")) //랜덤 이벤트 발생.
        {
            //Debug.Log($"{manager.currentMapType}");
            agent.speed = 0;
            agent.destination = transform.position;
            agent.isStopped = true;

            foreach (var element in manager.randomEvents)
            {
                if (!element.Value.Contains(other.gameObject)) continue;

                manager.randomEvents[element.Key].Remove(other.gameObject);
                Destroy(other.gameObject);
            }

            //Debug.Log("랜덤이벤트 종료");
            agent.speed = originAgentSpeed;
            agent.isStopped = false;
        }
        else if (other.gameObject.CompareTag("NonBattleMap")) //현재 맵 갱신.
        {
            //Debug.Log($"{other.gameObject.name}에 들어옴");

            foreach (MapType element in Enum.GetValues(typeof(MapType)))
            {
                if (!element.ToString().Equals(other.gameObject.name)) continue;
                manager.currentMapType = element;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MonsterArea") && !isBattle) //몬스터와 전투.
        {
            randomTimer++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MonsterArea") && !isBattle) //몬스터와 전투.
        {
            randomTimer = 0;
        }
    }
}


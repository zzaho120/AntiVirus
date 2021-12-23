using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

// NavMeshAgent 매뉴얼
// https://docs.unity3d.com/kr/530/ScriptReference/NavMeshAgent.html

public class PlayerController : MonoBehaviour
{
    //지은.
    public MultiTouch multiTouch;
    public NonBattleMgr manager;

    private NavMeshAgent agent;
    private CharacterController characterController;

    public GameObject panel;
    public bool isMove;//지은.
    bool isBattle;
    float randomTimer;

    float pX;
    float pY;
    float pZ;
    bool saveMode;

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
        if ((PlayerPrefs.HasKey("p_x") || PlayerPrefs.HasKey("p_y") || PlayerPrefs.HasKey("p_z")))
        {
            pX = PlayerPrefs.GetFloat("p_x");
            pY = PlayerPrefs.GetFloat("p_y");
            pZ = PlayerPrefs.GetFloat("p_z");
            transform.position = new Vector3(pX, pY, pZ);
        }
        else
        {
            int randomNum = UnityEngine.Random.Range(0, manager.bunkerPos.Count);
            Vector3 pos = manager.bunkerPos[randomNum].transform.position;
            transform.position = pos;
        }

        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        originAgentSpeed = agent.speed;
        //panel = GetComponent<FadeOutTest>().gameObject;

        // 캐릭터 컨트롤러 포지션 설정할 때 사용
        agent.updatePosition = false;
        agent.updateRotation = true;

        isMove = false;
        isBattle = false;
        saveMode = true;
    }

    void Update()
    {
        if((PlayerPrefs.HasKey("p_x") || PlayerPrefs.HasKey("p_y") || PlayerPrefs.HasKey("p_z")) && saveMode)
            PlayerPrefs.DeleteAll();
        
        if (agent.velocity.magnitude > 0.15f) //움직이고 있을 때.
        {
            PlayerPrefs.SetFloat("p_x", transform.position.x);
            PlayerPrefs.SetFloat("p_y", transform.position.y);
            PlayerPrefs.SetFloat("p_z", transform.position.z);
        }

        // [수정] 현재 -> 마우스 클릭 방향으로 이동하게
        // 나중에 터치 포지션으로 이동하도록 수정
        if (multiTouch.DoubleTap)
        {
            Ray ray = Camera.main.ScreenPointToRay(multiTouch.curTouchPos);
            RaycastHit raycastHit;
            groundLayerMask = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out raycastHit, 100, groundLayerMask))
            {
                agent.SetDestination(raycastHit.point);
            }
        }
        else if (multiTouch.Tap)
        {
            Ray ray = Camera.main.ScreenPointToRay(multiTouch.curTouchPos);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 100))
            {
                Debug.Log($"{raycastHit.collider.gameObject.name}");
                if (raycastHit.collider.gameObject.name.Equals("Bunker"))
                {
                    pX = raycastHit.collider.gameObject.transform.position.x;
                    pY = raycastHit.collider.gameObject.transform.position.y;
                    pZ = raycastHit.collider.gameObject.transform.position.z;

                    saveMode = false;
                    PlayerPrefs.SetFloat("p_x", pX);
                    PlayerPrefs.SetFloat("p_y", pY);
                    PlayerPrefs.SetFloat("p_z", pZ);

                    SceneManager.LoadScene("Bunker");
                }
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
        else if (other.gameObject.CompareTag("VirusZonePhase1"))
        {
            Debug.Log("1단계 바이러스 영역");
        }
        else if (other.gameObject.CompareTag("VirusZonePhase2"))
        {
            Debug.Log("2단계 바이러스 영역");
        }
        else if (other.gameObject.CompareTag("VirusZonePhase3"))
        {
            Debug.Log("3단계 바이러스 영역");
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


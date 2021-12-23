using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

// NavMeshAgent �Ŵ���
// https://docs.unity3d.com/kr/530/ScriptReference/NavMeshAgent.html

public class PlayerController : MonoBehaviour
{
    //����.
    public MultiTouch multiTouch;
    public NonBattleMgr manager;

    private NavMeshAgent agent;
    private CharacterController characterController;

    public GameObject panel;
    public bool isMove;//����.
    bool isBattle;
    float randomTimer;

    float pX;
    float pY;
    float pZ;
    bool saveMode;

    float originAgentSpeed;
    //private Vector3 calcVelocity = Vector3.zero;

    // �������� ������ -> ���߿� ���������?
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

        // ĳ���� ��Ʈ�ѷ� ������ ������ �� ���
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
        
        if (agent.velocity.magnitude > 0.15f) //�����̰� ���� ��.
        {
            PlayerPrefs.SetFloat("p_x", transform.position.x);
            PlayerPrefs.SetFloat("p_y", transform.position.y);
            PlayerPrefs.SetFloat("p_z", transform.position.z);
        }

        // [����] ���� -> ���콺 Ŭ�� �������� �̵��ϰ�
        // ���߿� ��ġ ���������� �̵��ϵ��� ����
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
            // RemainingDistance : ���� ���(path)���� �����ִ� �Ÿ�
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

    // �浹 �Ǻ�
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("RandomEvent")) return;

        // �� ��ȯ ��? FadeOut ȿ��
        //panel.GetComponent<FadeOutTest>().StartFade();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RandomEvent")) //���� �̺�Ʈ �߻�.
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

            //Debug.Log("�����̺�Ʈ ����");
            agent.speed = originAgentSpeed;
            agent.isStopped = false;
        }
        else if (other.gameObject.CompareTag("VirusZonePhase1"))
        {
            Debug.Log("1�ܰ� ���̷��� ����");
        }
        else if (other.gameObject.CompareTag("VirusZonePhase2"))
        {
            Debug.Log("2�ܰ� ���̷��� ����");
        }
        else if (other.gameObject.CompareTag("VirusZonePhase3"))
        {
            Debug.Log("3�ܰ� ���̷��� ����");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MonsterArea") && !isBattle) //���Ϳ� ����.
        {
            randomTimer++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MonsterArea") && !isBattle) //���Ϳ� ����.
        {
            randomTimer = 0;
        }
    }
}


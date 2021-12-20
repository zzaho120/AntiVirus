using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// NavMeshAgent �Ŵ���
// https://docs.unity3d.com/kr/530/ScriptReference/NavMeshAgent.html

public class PlayerController : MonoBehaviour
{
    //����.
    public NonBattleMgr manager;

    private NavMeshAgent agent;
    private CharacterController characterController;

    public GameObject panel;
    public bool isMove;//����.
    bool isBattle;
    float randomTimer;

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
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        originAgentSpeed = agent.speed;
        //panel = GetComponent<FadeOutTest>().gameObject;

        // ĳ���� ��Ʈ�ѷ� ������ ������ �� ���
        agent.updatePosition = false;
        agent.updateRotation = true;

        isMove = false;
        isBattle = false;



    }

    void Update()
    {
        // [����] ���� -> ���콺 Ŭ�� �������� �̵��ϰ�
        // ���߿� ��ġ ���������� �̵��ϵ��� ����
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 100, groundLayerMask))
            {
                //Debug.Log("�浹ü �̸� : " + raycastHit.collider.name + ", ��ǥ : " + raycastHit.point);
                agent.SetDestination(raycastHit.point);
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
        else if (other.gameObject.CompareTag("NonBattleMap")) //���� �� ����.
        {
            //Debug.Log($"{other.gameObject.name}�� ����");

            foreach (MapType element in Enum.GetValues(typeof(MapType)))
            {
                if (!element.ToString().Equals(other.gameObject.name)) continue;
                manager.currentMapType = element;
            }
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// NavMeshAgent �Ŵ���
// https://docs.unity3d.com/kr/530/ScriptReference/NavMeshAgent.html

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private CharacterController characterController;

    public GameObject panel;

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
        //panel = GetComponent<FadeOutTest>().gameObject;

        // ĳ���� ��Ʈ�ѷ� ������ ������ �� ���
        agent.updatePosition = false;
        agent.updateRotation = true;
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
                Debug.Log("�浹ü �̸� : " + raycastHit.collider.name + ", ��ǥ : " + raycastHit.point);
                agent.SetDestination(raycastHit.point);
            }
        }


        if (agent.remainingDistance > agent.stoppingDistance)
        {
            characterController.Move(agent.velocity * Time.deltaTime);
            // Debug.Log(agent.stoppingDistance);
            // RemainingDistance : ���� ���(path)���� �����ִ� �Ÿ�
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

    // �浹 �Ǻ�
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Debug.Log(hit.collider.name);
        
        // �� ��ȯ ��? FadeOut ȿ��
        panel.GetComponent<FadeOutTest>().StartFade();

        //if (agent.remainingDistance <= agent.stoppingDistance)
        //{
        //}
    }
}


using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    // get NavMesh
    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    // to save and control player speed
    private float speed = 8f;
    private float speedUp = 16f;

    // to get CharacterController from the unity
    private CharacterController characterController;
    private PlayerController playerController;

    //to calculate
    //private Vector3 calcVelocity = Vector3.zero;

    // to set layers
    [SerializeField]
    LayerMask groundLayerMask;

    public void Init()
    {
        //to get CharacterController from the unity
        characterController = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerController = GetComponent<PlayerController>();

        // will use character controller position
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = true;
    }

    public void PlayerMoveUpdate()
    {
        int facilitiesLayer = LayerMask.GetMask("Facilities");
        int monsterLayer = LayerMask.GetMask("EliteMonster");

        // Mouse Click
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && playerController.timeController.isPause == false) 
        {
            // Make ray from screen to world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Check hit
            RaycastHit raycastHit;

            // 시설 혹은 몬스터 클릭 시 리턴
            if (Physics.Raycast(ray, out raycastHit, 1000f, facilitiesLayer) || Physics.Raycast(ray, out raycastHit, 1000f, monsterLayer))
                return;

            if (Physics.Raycast(ray, out raycastHit, 1000f, groundLayerMask))
            {
                //Debug.Log("We hit " + raycastHit.collider.name + " " + raycastHit.point);
                navMeshAgent.SetDestination(raycastHit.point);
            }
        }

        // New Input System
        if ((playerController.multiTouch.Tap && !EventSystem.current.IsPointerOverGameObject() && playerController.timeController.isPause == false))
        {
            // Make ray from screen to world
            Ray ray = Camera.main.ScreenPointToRay(playerController.multiTouch.curTouchPos);
            // Check hit
            RaycastHit raycastHit;

            // 시설 혹은 몬스터 클릭 시 리턴
            if (Physics.Raycast(ray, out raycastHit, 1000f, facilitiesLayer) || Physics.Raycast(ray, out raycastHit, 1000f, monsterLayer))
                return;

            if (Physics.Raycast(ray, out raycastHit, 1000f, groundLayerMask))
            {
                //Debug.Log("We hit " + raycastHit.collider.name + " " + raycastHit.point);
                navMeshAgent.SetDestination(raycastHit.point);
            }
        }

        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            characterController.Move(navMeshAgent.velocity * Time.deltaTime);
        }
        else
        {
            characterController.Move(Vector3.zero);
        }

        // 길 위에 있을 때 속도 조정
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(transform.position, out navHit, 0.8f, 8))    // 8번 AreaMask : Road
        {
            //Debug.Log(navHit.distance);
            //Debug.Log("Player가 Road 위에 있을때만 이 메시지가 뜨면 성공입니다");
            navMeshAgent.speed = speedUp;
        }
        else
        {
            navMeshAgent.speed = speed;
        }
    }

    public void PlayerMoveLateUpdate()
    {
        transform.position = navMeshAgent.nextPosition;
    }
}

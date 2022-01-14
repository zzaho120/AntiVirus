using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NewPlayerMove : MonoBehaviour
{
    //get NavMesh
    private NavMeshAgent navMeshAgent;
    private new Camera camera;

    //to get CharacterController from the unity
    private CharacterController characterController;

    //to calculate
    private Vector3 calcVelocity = Vector3.zero;

    //prevent double jump
    private bool isGround = false;
    [SerializeField]
    float groundCheckDistance = 0.3f;

    // to set layers
    [SerializeField]
    LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        //to get CharacterController from the unity
        characterController = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // will use character controller position
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = true;

        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        // 0 - left mouse click
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() /* && timeController.isPause == false */)
        {
            // Make ray from screen to world
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            // Check hit
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 10000f, groundLayerMask))
            {
                Debug.Log("We hit " + raycastHit.collider.name + " " + raycastHit.point);

                // Move our player to what we hit
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
    }
    private void LateUpdate()
    {
        transform.position = navMeshAgent.nextPosition;
    }
}

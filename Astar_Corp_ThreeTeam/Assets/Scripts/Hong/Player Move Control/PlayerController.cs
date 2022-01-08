using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //지은.
    public GameObject ui;

    public CharacterInfo character;
    public MultiTouch multiTouch;

    // 수정
    private GameObject nonBattleMgr;
    private PopUpMgr popUpMgr;

    //private NonBattlePopUps nonBattlePopUps;
    //public WindowManager windowManager;
    public TimeController timeController;

    private NavMeshAgent agent;

    //public GameObject panel;  //나중에 Fadeout 효과 넣을때
    [HideInInspector]
    public bool isMove;//지은.
    public bool isBunkerClikable;

    float pX, pY, pZ;
    bool saveMode;
    float originAgentSpeed;

    // to set layers
    [SerializeField]
    LayerMask groundLayerMask;

    void Start()
    {
        // 수정
        nonBattleMgr = GameObject.Find("NonBattleMgr");
        popUpMgr = nonBattleMgr.GetComponent<PopUpMgr>();

        if ((PlayerPrefs.HasKey("p_x") || PlayerPrefs.HasKey("p_y") || PlayerPrefs.HasKey("p_z")))
        {
            pX = PlayerPrefs.GetFloat("p_x");
            pY = PlayerPrefs.GetFloat("p_y");
            pZ = PlayerPrefs.GetFloat("p_z");
            transform.position = new Vector3(pX, pY, pZ);
        }
        else
        {
            // 수정
            //transform.position = manager.bunkerPos.position;
            transform.position = nonBattleMgr.GetComponent<NonBattleMgr>().bunkerPos.position;
        }

        agent = GetComponent<NavMeshAgent>();

        isMove = false;
        saveMode = true;
    }

    void Update()
    {
        // 뉴인풋시스템
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
        // if (multiTouch.Tap && !IsPointerOverUIObject(multiTouch.curTouchPos)            /*&& timeController.isPause == false*/)
        {
            Ray ray = Camera.main.ScreenPointToRay(multiTouch.curTouchPos);
            RaycastHit raycastHit;
            groundLayerMask = LayerMask.GetMask("Ground");
            int facilitiesLayer = LayerMask.GetMask("Facilities");

            // 전투발생
            if (Physics.Raycast(ray, out raycastHit, 100, LayerMask.GetMask("EliteMonster")))
            {
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // 렌더러가 활성화 되어있을때만 유효하게
                    if (raycastHit.collider.GetComponent<MeshRenderer>().enabled)
                    {
                        popUpMgr.OpenMonsterPopup();

                        timeController.PauseTime();
                        timeController.isPause = true;
                    }
                }
            }
            if (Physics.Raycast(ray, out raycastHit, 100, facilitiesLayer))
            {
                if (raycastHit.collider.gameObject.name.Equals("Bunker") && isBunkerClikable)
                {
                    pX = raycastHit.collider.gameObject.transform.position.x;
                    pY = raycastHit.collider.gameObject.transform.position.y;
                    pZ = raycastHit.collider.gameObject.transform.position.z;

                    saveMode = false;
                    PlayerPrefs.SetFloat("p_x", pX);
                    PlayerPrefs.SetFloat("p_y", pY);
                    PlayerPrefs.SetFloat("p_z", pZ);

                    //벙커 팝업창
                    popUpMgr.OpenBunkerPopup();
                }
                else if (raycastHit.collider.gameObject.name.Equals("Laboratory"))
                {
                    //pX = raycastHit.collider.gameObject.transform.position.x;
                    //pY = raycastHit.collider.gameObject.transform.position.y;
                    //pZ = raycastHit.collider.gameObject.transform.position.z;
                    //
                    //saveMode = false;
                    //PlayerPrefs.SetFloat("p_x", pX);
                    //PlayerPrefs.SetFloat("p_y", pY);
                    //PlayerPrefs.SetFloat("p_z", pZ);

                    //연구소 팝업창
                    popUpMgr.OpenLaboratoryPopup();
                }
                else if (raycastHit.collider.gameObject.name.Equals("Fog") ||
                   raycastHit.collider.gameObject.name.Equals("Plane"))
                {
                    agent.SetDestination(raycastHit.point);
                }
            }
            else if (Physics.Raycast(ray, out raycastHit, 100, groundLayerMask))
            {
                agent.SetDestination(raycastHit.point);
            }
        }


        // 마우스 클릭
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && timeController.isPause == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            groundLayerMask = LayerMask.GetMask("Ground");
            int facilitiesLayer = LayerMask.GetMask("Facilities");

            //전투발생
            if (Physics.Raycast(ray, out raycastHit, 100, LayerMask.GetMask("EliteMonster")))
            {
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // 렌더러가 활성화 되어있을때만 유효하게
                    if (raycastHit.collider.GetComponent<MeshRenderer>().enabled)
                    {
                        pX = raycastHit.collider.gameObject.transform.position.x;
                        pY = raycastHit.collider.gameObject.transform.position.y;
                        pZ = raycastHit.collider.gameObject.transform.position.z;

                        saveMode = false;
                        PlayerPrefs.SetFloat("p_x", pX);
                        PlayerPrefs.SetFloat("p_y", pY);
                        PlayerPrefs.SetFloat("p_z", pZ);

                        popUpMgr.OpenMonsterPopup();

                        timeController.PauseTime();
                        timeController.isPause = true;
                    }
                }
            }
            if (Physics.Raycast(ray, out raycastHit, 100, facilitiesLayer))
            {
                // 벙커로 이동
                if (raycastHit.collider.gameObject.name.Equals("Bunker") && isBunkerClikable)
                {
                    pX = raycastHit.collider.gameObject.transform.position.x;
                    pY = raycastHit.collider.gameObject.transform.position.y;
                    pZ = raycastHit.collider.gameObject.transform.position.z;

                    saveMode = false;
                    PlayerPrefs.SetFloat("p_x", pX);
                    PlayerPrefs.SetFloat("p_y", pY);
                    PlayerPrefs.SetFloat("p_z", pZ);

                    //벙커 팝업창
                    popUpMgr.OpenBunkerPopup();
                }
                // 연구소로 이동
                else if (raycastHit.collider.gameObject.name.Equals("Laboratory"))
                {
                    //pX = raycastHit.collider.gameObject.transform.position.x;
                    //pY = raycastHit.collider.gameObject.transform.position.y;
                    //pZ = raycastHit.collider.gameObject.transform.position.z;
                    //
                    //saveMode = false;
                    //PlayerPrefs.SetFloat("p_x", pX);
                    //PlayerPrefs.SetFloat("p_y", pY);
                    //PlayerPrefs.SetFloat("p_z", pZ);

                    //연구소 팝업창
                    popUpMgr.OpenLaboratoryPopup();
                }
                else if (raycastHit.collider.gameObject.name.Equals("Fog") ||
                   raycastHit.collider.gameObject.name.Equals("Plane"))
                {
                    agent.SetDestination(raycastHit.point);
                }
            }
            else if (Physics.Raycast(ray, out raycastHit, 100, groundLayerMask))
            {
                agent.SetDestination(raycastHit.point);
            }
        }
        if (agent.velocity.magnitude > 0.15f) //움직이고 있을 때.
        {
            PlayerPrefs.SetFloat("p_x", transform.position.x);
            PlayerPrefs.SetFloat("p_y", transform.position.y);
            PlayerPrefs.SetFloat("p_z", transform.position.z);
        }

        // 일시정지 상태일 때 목적지 값 변경 (현재 위치로)
        if (timeController.isPause)
        {
            agent.SetDestination(agent.transform.position);
        }
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }

    public void ChangeBattleScene()
    {
        timeController.PauseTime();
        timeController.isPause = false;
        SceneManager.LoadScene("BattleMap");
    }
}
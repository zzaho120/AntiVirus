using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// ※ 주의
// =======================================================
//  나중에 벙커 isClickable 작동되도록 -> 지금 주석 쳐놓음
// =======================================================

// 클래스 역할
// =======================================================
//  비전투씬 내 플레이어 위치 저장,
//  벙커, 연구소, 전투 팝업창 띄우기,
//  전투 씬으로 전환 등
// =======================================================
public class PlayerController : MonoBehaviour
{
    // MultiTouch
    //지은.
    [HideInInspector]
    public MultiTouch multiTouch;

    // Managers
    private NonBattleMgr nonBattleMgr;
    private PopUpMgr popUpMgr;

    // Time Controller
    [HideInInspector]
    public TimeController timeController;

    // Get NavMesh
    private PlayerMove playerNav;
    private NavMeshAgent agent;

    // Sight
    [HideInInspector]
    public bool isBunkerClikable = true;

    // SaveLoad
    private float pX, pY, pZ;
    private bool saveMode;

    public void Init()
    {
        // 오브젝트 찾기
        nonBattleMgr    = NonBattleMgr.Instance;
        multiTouch      = GameObject.Find("MultiTouch").GetComponent<MultiTouch>();
        popUpMgr        = nonBattleMgr.GetComponent<PopUpMgr>();
        timeController  = GameObject.Find("TimeController").GetComponent<TimeController>();
        playerNav       = GetComponent<PlayerMove>();
        agent           = playerNav.navMeshAgent;

        // 위치 저장
        if ((PlayerPrefs.HasKey("p_x") || PlayerPrefs.HasKey("p_y") || PlayerPrefs.HasKey("p_z")))
        {
            pX = PlayerPrefs.GetFloat("p_x");
            pY = PlayerPrefs.GetFloat("p_y");
            pZ = PlayerPrefs.GetFloat("p_z");
            transform.position = new Vector3(pX, pY, pZ);
        }
        else
        {
            transform.position = nonBattleMgr.bunkerPos.position;
        }
        saveMode = true;
    }

    //void Update()
    public void PlayerControllerUpdate()
    {
        // Raycast parameters
        int facilitiesLayer = LayerMask.GetMask("Facilities");
        int monsterLayer    = LayerMask.GetMask("EliteMonster");
        int rayRange = 1000;

        #region New Input System
        if ((multiTouch.Tap && !EventSystem.current.IsPointerOverGameObject() && timeController.isPause == false))              
        {
            Ray ray = Camera.main.ScreenPointToRay(multiTouch.curTouchPos);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, rayRange, monsterLayer))
            {
                // 전투발생
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // 렌더러가 활성화 되어있을때만 유효하게
                    if (raycastHit.collider.GetComponent<MeshRenderer>().enabled)
                    {
                        // 위치 저장? 해야되나
                        //SavePlayerPos(raycastHit);
                        popUpMgr.OpenMonsterPopup();

                        // 비전투씬 일시정지
                        timeController.PauseTime();
                        timeController.isPause = true;
                    }
                }
                // 발자국 클릭 시
                if (raycastHit.collider.CompareTag("Footprint"))
                {
                    popUpMgr.OpenFootprintPopup();
                }
                // 클릭하는 위치로 이동하는 것 방지
                agent.SetDestination(agent.transform.position);
            }
            if (Physics.Raycast(ray, out raycastHit, rayRange, facilitiesLayer))
            {
                if (raycastHit.collider.gameObject.name.Equals("Bunker") /*&& isBunkerClikable*/)
                {
                    SavePlayerPos(raycastHit);
                    popUpMgr.OpenBunkerPopup();
                    agent.SetDestination(agent.transform.position); 
                }
                if (raycastHit.collider.gameObject.name.Equals("Laboratory"))
                {
                    //SavePlayerPos(raycastHit);
                    popUpMgr.OpenLaboratoryPopup();
                    agent.SetDestination(agent.transform.position);
                }
            }
        }
        #endregion

        #region Mouse Click
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && timeController.isPause == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
        
            if (Physics.Raycast(ray, out raycastHit, rayRange, monsterLayer))
            {
                // 전투발생
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // 렌더러가 활성화 되어있을때만 유효하게
                    if (raycastHit.collider.GetComponent<MeshRenderer>().enabled)
                    {
                        // 위치 저장? 해야되나
                        //SavePlayerPos(raycastHit);
                        popUpMgr.OpenMonsterPopup();
                        
                        // 비전투맵 일시정지
                        timeController.PauseTime();
                        timeController.isPause = true;
                    }
                }
                // 발자국 클릭 시
                if (raycastHit.collider.CompareTag("Footprint"))
                {
                    popUpMgr.OpenFootprintPopup();
                }
                // 클릭하는 위치로 이동하는 것 방지
                agent.SetDestination(agent.transform.position);
            }
            if (Physics.Raycast(ray, out raycastHit, rayRange, facilitiesLayer))
            {
                if (raycastHit.collider.gameObject.name.Equals("Bunker") /*&& isBunkerClikable*/)
                {
                    SavePlayerPos(raycastHit);
                    popUpMgr.OpenBunkerPopup();

                    // 클릭하는 위치로 이동하는 것 방지
                    agent.SetDestination(agent.transform.position);
                }
                if (raycastHit.collider.gameObject.name.Equals("Laboratory"))
                {
                    popUpMgr.OpenLaboratoryPopup();
                    agent.SetDestination(agent.transform.position);
                }

            }
        }
        #endregion

        //움직이고 있을 때.
        if (agent.velocity.magnitude > 0.15f) 
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

        playerNav.PlayerMoveUpdate();
    }

    // 플레이어 위치 저장
    private void SavePlayerPos(RaycastHit raycastHit)
    {
        pX = raycastHit.collider.gameObject.transform.position.x;
        pY = raycastHit.collider.gameObject.transform.position.y;
        pZ = raycastHit.collider.gameObject.transform.position.z;

        saveMode = false;
        PlayerPrefs.SetFloat("p_x", pX);
        PlayerPrefs.SetFloat("p_y", pY);
        PlayerPrefs.SetFloat("p_z", pZ);
    }

    // 전투 씬으로 전환
    public void ChangeBattleScene()
    {
        timeController.PauseTime();
        timeController.isPause = false;
        SceneManager.LoadScene("BattleMap");
    }
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//  나중에 벙커 isClickable 작동되도록 -> 지금 주석 쳐놓음

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
    private TextMeshProUGUI timer;
    public float time;

    // Get NavMesh
    private PlayerMove playerMove;
    public NavMeshAgent agent;

    private GameObject footprintUI;
    private CameraDrag cameraMovement;
    private StartBattle startBattle;

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
        timer           = timeController.GetComponentInParent<TextMeshProUGUI>();
        playerMove      = GetComponent<PlayerMove>();
        cameraMovement  = Camera.main.GetComponent<CameraDrag>();
        footprintUI     = GameObject.Find("Footprint Info");
        startBattle = GetComponentInChildren<StartBattle>();

        playerMove.Init();
        agent = playerMove.navMeshAgent;

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
            transform.position = nonBattleMgr.playerStartPos.position;
        }
        saveMode = true;

        if (gameObject.GetComponent<NavMeshAgent>() != null)
            agent.enabled = true;
    }

    public void PlayerControllerUpdate()
    {
        time += Time.deltaTime;
        timer.text = string.Format("{0:N2}", time);

        // 조건 정리
        bool terms = !EventSystem.current.IsPointerOverGameObject() && timeController.isPause == false && !cameraMovement.isWorldMapMode;

        // Raycast parameters
        int facilitiesLayer = LayerMask.GetMask("Facilities");
        int monsterLayer    = LayerMask.GetMask("EliteMonster");
        int rayRange = 1000;

        #region New Input System
        if (multiTouch.Tap && terms)              
        {
            Ray ray = Camera.main.ScreenPointToRay(multiTouch.curTouchPos);
            RaycastHit raycastHit;
            // 1. 몬스터 클릭
            if (Physics.Raycast(ray, out raycastHit, rayRange, monsterLayer))
            {
                // 전투발생
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // 렌더러가 활성화 되어있을때만 유효하게 // 해당조건 잠깐 Off
                    if (raycastHit.collider.GetComponentInChildren<SkinnedMeshRenderer>().enabled)
                    {
                        // 위치 저장? 해야되나
                        SavePlayerPos(raycastHit);
                        popUpMgr.OpenMonsterPopup();

                        // 비전투씬 일시정지
                        timeController.Pause();
                        timeController.isPause = true;

                        // 몬스터 정보 출력
                        startBattle.PrintMonsterInfo(raycastHit.collider.GetComponent<WorldMonsterChar>());
                    }
                }
                //// 발자국 클릭 시
                //if (raycastHit.collider.CompareTag("Footprint"))
                //{
                //    popUpMgr.OpenFootprintPopup();
                //
                //    TextMeshProUGUI[] text = footprintUI.GetComponentsInChildren<TextMeshProUGUI>();
                //    text[1].text = raycastHit.collider.name;
                //}
                // 클릭하는 위치로 이동하는 것 방지
                agent.SetDestination(agent.transform.position);
            }
            // 2. 시설물 클릭 (벙커, 연구소)
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
                    SavePlayerPos(raycastHit);
                    popUpMgr.OpenLaboratoryPopup();
                    agent.SetDestination(agent.transform.position);
                }
            }
        }
        #endregion

        #region Mouse Click
        if (Input.GetMouseButtonDown(0) && terms)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
        
            if (Physics.Raycast(ray, out raycastHit, rayRange, monsterLayer))
            {
                // 전투발생
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // 렌더러가 활성화 되어있을때만 유효하게
                    if (raycastHit.collider.GetComponentInChildren<SkinnedMeshRenderer>().enabled)
                    {
                        Debug.Log("Click");
                        // 위치 저장? 해야되나
                        //SavePlayerPos(raycastHit);
                        popUpMgr.OpenMonsterPopup();
                        
                        // 비전투맵 일시정지
                        timeController.Pause();
                        timeController.isPause = true;

                    // 몬스터 정보 출력
                    startBattle.PrintMonsterInfo(raycastHit.collider.GetComponent<WorldMonsterChar>());
                    }
                }
                // 발자국 클릭 시
                //if (raycastHit.collider.CompareTag("Footprint"))
                //{
                //    // 발자국 팝업창 Open
                //    popUpMgr.OpenFootprintPopup();
                //    // 몬스터 정보 출력
                //    //PrintMonsterName(raycastHit);
                //}
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
        if (agent != null && agent.velocity.magnitude > 0.15f) 
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

        playerMove.PlayerMoveUpdate();
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

    private void LateUpdate()
    {
        playerMove.PlayerMoveLateUpdate();
    }

    // 몬스터 발자국 정보 출력
    //private void PrintMonsterName(RaycastHit hitInfo)
    //{
    //    // Text 정보 가져오기
    //    TextMeshProUGUI[] text = GameObject.Find("Footprint Info").GetComponentsInChildren<TextMeshProUGUI>();
    //
    //    if (hitInfo.collider.name == "Footprint_" + MonsterName.Wolf)
    //    {
    //        text[1].text = "Wolf";
    //    }
    //    else if (hitInfo.collider.name == "Footprint_" + MonsterName.Boar)
    //    {
    //        text[1].text = "Boar";
    //    }
    //    else if (hitInfo.collider.name == "Footprint_" + MonsterName.Bear)
    //    {
    //        text[1].text = "Bear";
    //    }
    //    else if (hitInfo.collider.name == "Footprint_" + MonsterName.Tiger)
    //    {
    //        text[1].text = "Tiger";
    //    }
    //    else if (hitInfo.collider.name == "Footprint_" + MonsterName.Jaguar)
    //    {
    //        text[1].text = "Jaguar";
    //    }
    //    else if (hitInfo.collider.name == "Footprint_" + MonsterName.Fox)
    //    {
    //        text[1].text = "Fox";
    //    }
    //    else if (hitInfo.collider.name == "Footprint_" + MonsterName.Spider)
    //    {
    //        text[1].text = "Spider";
    //    }
    //    else
    //    {
    //        Debug.LogError("뭐지");
    //    }
    //}

    // 전투 씬으로 전환

    public void ChangeBattleScene()
    {
        Debug.Log("Battle Squad : " + PlayerDataMgr.Instance.battleSquad.Count);
        if (PlayerDataMgr.Instance.battleSquad.Count < 1) return;
        
        timeController.Pause();
        SceneManager.LoadScene("BattleMap_Origin");
    }

    public void TimerReset()
    {
        time = 0;
    }

    public void SetPlayerPos()
    {
        agent.SetDestination(transform.position);
    }
}
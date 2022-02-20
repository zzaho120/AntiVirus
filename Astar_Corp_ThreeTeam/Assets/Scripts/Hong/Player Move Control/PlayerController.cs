using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//  ���߿� ��Ŀ isClickable �۵��ǵ��� -> ���� �ּ� �ĳ���

public class PlayerController : MonoBehaviour
{
    // MultiTouch
    //����.
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
        // ������Ʈ ã��
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

        // ��ġ ����
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

        // ���� ����
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
            // 1. ���� Ŭ��
            if (Physics.Raycast(ray, out raycastHit, rayRange, monsterLayer))
            {
                // �����߻�
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ� // �ش����� ��� Off
                    if (raycastHit.collider.GetComponentInChildren<SkinnedMeshRenderer>().enabled)
                    {
                        // ��ġ ����? �ؾߵǳ�
                        SavePlayerPos(raycastHit);
                        popUpMgr.OpenMonsterPopup();

                        // �������� �Ͻ�����
                        timeController.Pause();
                        timeController.isPause = true;

                        // ���� ���� ���
                        startBattle.PrintMonsterInfo(raycastHit.collider.GetComponent<WorldMonsterChar>());
                    }
                }
                //// ���ڱ� Ŭ�� ��
                //if (raycastHit.collider.CompareTag("Footprint"))
                //{
                //    popUpMgr.OpenFootprintPopup();
                //
                //    TextMeshProUGUI[] text = footprintUI.GetComponentsInChildren<TextMeshProUGUI>();
                //    text[1].text = raycastHit.collider.name;
                //}
                // Ŭ���ϴ� ��ġ�� �̵��ϴ� �� ����
                agent.SetDestination(agent.transform.position);
            }
            // 2. �ü��� Ŭ�� (��Ŀ, ������)
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
                // �����߻�
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ�
                    if (raycastHit.collider.GetComponentInChildren<SkinnedMeshRenderer>().enabled)
                    {
                        Debug.Log("Click");
                        // ��ġ ����? �ؾߵǳ�
                        //SavePlayerPos(raycastHit);
                        popUpMgr.OpenMonsterPopup();
                        
                        // �������� �Ͻ�����
                        timeController.Pause();
                        timeController.isPause = true;

                    // ���� ���� ���
                    startBattle.PrintMonsterInfo(raycastHit.collider.GetComponent<WorldMonsterChar>());
                    }
                }
                // ���ڱ� Ŭ�� ��
                //if (raycastHit.collider.CompareTag("Footprint"))
                //{
                //    // ���ڱ� �˾�â Open
                //    popUpMgr.OpenFootprintPopup();
                //    // ���� ���� ���
                //    //PrintMonsterName(raycastHit);
                //}
                // Ŭ���ϴ� ��ġ�� �̵��ϴ� �� ����
                agent.SetDestination(agent.transform.position);
            }
            if (Physics.Raycast(ray, out raycastHit, rayRange, facilitiesLayer))
            {
                if (raycastHit.collider.gameObject.name.Equals("Bunker") /*&& isBunkerClikable*/)
                {
                    SavePlayerPos(raycastHit);
                    popUpMgr.OpenBunkerPopup();

                    // Ŭ���ϴ� ��ġ�� �̵��ϴ� �� ����
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

        //�����̰� ���� ��.
        if (agent != null && agent.velocity.magnitude > 0.15f) 
        {
            PlayerPrefs.SetFloat("p_x", transform.position.x);
            PlayerPrefs.SetFloat("p_y", transform.position.y);
            PlayerPrefs.SetFloat("p_z", transform.position.z);
        }

        // �Ͻ����� ������ �� ������ �� ���� (���� ��ġ��)
        if (timeController.isPause)
        {
            agent.SetDestination(agent.transform.position);
        }

        playerMove.PlayerMoveUpdate();
    }

    // �÷��̾� ��ġ ����
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

    // ���� ���ڱ� ���� ���
    //private void PrintMonsterName(RaycastHit hitInfo)
    //{
    //    // Text ���� ��������
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
    //        Debug.LogError("����");
    //    }
    //}

    // ���� ������ ��ȯ

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
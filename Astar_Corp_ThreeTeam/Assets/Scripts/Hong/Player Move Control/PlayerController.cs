using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// �� ����
// =======================================================
//  ���߿� ��Ŀ isClickable �۵��ǵ��� -> ���� �ּ� �ĳ���
// =======================================================

// Ŭ���� ����
// =======================================================
//  �������� �� �÷��̾� ��ġ ����,
//  ��Ŀ, ������, ���� �˾�â ����,
//  ���� ������ ��ȯ ��
// =======================================================
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
        // ������Ʈ ã��
        nonBattleMgr    = NonBattleMgr.Instance;
        multiTouch      = GameObject.Find("MultiTouch").GetComponent<MultiTouch>();
        popUpMgr        = nonBattleMgr.GetComponent<PopUpMgr>();
        timeController  = GameObject.Find("TimeController").GetComponent<TimeController>();
        playerNav       = GetComponent<PlayerMove>();
        agent           = playerNav.navMeshAgent;

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
                // �����߻�
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ�
                    if (raycastHit.collider.GetComponent<MeshRenderer>().enabled)
                    {
                        // ��ġ ����? �ؾߵǳ�
                        //SavePlayerPos(raycastHit);
                        popUpMgr.OpenMonsterPopup();

                        // �������� �Ͻ�����
                        timeController.PauseTime();
                        timeController.isPause = true;
                    }
                }
                // ���ڱ� Ŭ�� ��
                if (raycastHit.collider.CompareTag("Footprint"))
                {
                    popUpMgr.OpenFootprintPopup();
                }
                // Ŭ���ϴ� ��ġ�� �̵��ϴ� �� ����
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
                // �����߻�
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ�
                    if (raycastHit.collider.GetComponent<MeshRenderer>().enabled)
                    {
                        // ��ġ ����? �ؾߵǳ�
                        //SavePlayerPos(raycastHit);
                        popUpMgr.OpenMonsterPopup();
                        
                        // �������� �Ͻ�����
                        timeController.PauseTime();
                        timeController.isPause = true;
                    }
                }
                // ���ڱ� Ŭ�� ��
                if (raycastHit.collider.CompareTag("Footprint"))
                {
                    popUpMgr.OpenFootprintPopup();
                }
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
        if (agent.velocity.magnitude > 0.15f) 
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

        playerNav.PlayerMoveUpdate();
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

    // ���� ������ ��ȯ
    public void ChangeBattleScene()
    {
        timeController.PauseTime();
        timeController.isPause = false;
        SceneManager.LoadScene("BattleMap");
    }
}
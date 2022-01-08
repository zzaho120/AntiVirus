using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //����.
    public GameObject ui;

    public CharacterInfo character;
    public MultiTouch multiTouch;

    // ����
    private GameObject nonBattleMgr;
    private PopUpMgr popUpMgr;

    //private NonBattlePopUps nonBattlePopUps;
    //public WindowManager windowManager;
    public TimeController timeController;

    private NavMeshAgent agent;

    //public GameObject panel;  //���߿� Fadeout ȿ�� ������
    [HideInInspector]
    public bool isMove;//����.
    public bool isBunkerClikable;

    float pX, pY, pZ;
    bool saveMode;
    float originAgentSpeed;

    // to set layers
    [SerializeField]
    LayerMask groundLayerMask;

    void Start()
    {
        // ����
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
            // ����
            //transform.position = manager.bunkerPos.position;
            transform.position = nonBattleMgr.GetComponent<NonBattleMgr>().bunkerPos.position;
        }

        agent = GetComponent<NavMeshAgent>();

        isMove = false;
        saveMode = true;
    }

    void Update()
    {
        // ����ǲ�ý���
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
        // if (multiTouch.Tap && !IsPointerOverUIObject(multiTouch.curTouchPos)            /*&& timeController.isPause == false*/)
        {
            Ray ray = Camera.main.ScreenPointToRay(multiTouch.curTouchPos);
            RaycastHit raycastHit;
            groundLayerMask = LayerMask.GetMask("Ground");
            int facilitiesLayer = LayerMask.GetMask("Facilities");

            // �����߻�
            if (Physics.Raycast(ray, out raycastHit, 100, LayerMask.GetMask("EliteMonster")))
            {
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ�
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

                    //��Ŀ �˾�â
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

                    //������ �˾�â
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


        // ���콺 Ŭ��
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && timeController.isPause == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            groundLayerMask = LayerMask.GetMask("Ground");
            int facilitiesLayer = LayerMask.GetMask("Facilities");

            //�����߻�
            if (Physics.Raycast(ray, out raycastHit, 100, LayerMask.GetMask("EliteMonster")))
            {
                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ�
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
                // ��Ŀ�� �̵�
                if (raycastHit.collider.gameObject.name.Equals("Bunker") && isBunkerClikable)
                {
                    pX = raycastHit.collider.gameObject.transform.position.x;
                    pY = raycastHit.collider.gameObject.transform.position.y;
                    pZ = raycastHit.collider.gameObject.transform.position.z;

                    saveMode = false;
                    PlayerPrefs.SetFloat("p_x", pX);
                    PlayerPrefs.SetFloat("p_y", pY);
                    PlayerPrefs.SetFloat("p_z", pZ);

                    //��Ŀ �˾�â
                    popUpMgr.OpenBunkerPopup();
                }
                // �����ҷ� �̵�
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

                    //������ �˾�â
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
        if (agent.velocity.magnitude > 0.15f) //�����̰� ���� ��.
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
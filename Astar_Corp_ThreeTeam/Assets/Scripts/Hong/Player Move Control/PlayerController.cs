using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CharacterInfo
{
    public string name;
    public int hp;
    public int stemina;
}

public class PlayerController : MonoBehaviour
{
    //����.
    public GameObject ui;

    public CharacterInfo character;
    public MultiTouch multiTouch;
    public NonBattleMgr manager;

    private NonBattlePopUps nonBattlePopUps;
    public WindowManager windowManager;

    private NavMeshAgent agent;
    private TimeController timeController;

    //public GameObject panel;  //���߿� Fadeout ȿ�� ������
    [HideInInspector]
    public bool isMove;//����.

    float pX, pY, pZ;
    bool saveMode;

    float originAgentSpeed;
    
    // to set layers
    [SerializeField]
    LayerMask groundLayerMask;

    void Start()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();

        character = new CharacterInfo();
        character.name = "����";
        character.hp = 5;
        character.stemina = 5;

        if ((PlayerPrefs.HasKey("p_x") || PlayerPrefs.HasKey("p_y") || PlayerPrefs.HasKey("p_z")))
        {
            pX = PlayerPrefs.GetFloat("p_x");
            pY = PlayerPrefs.GetFloat("p_y");
            pZ = PlayerPrefs.GetFloat("p_z");
            transform.position = new Vector3(pX, pY, pZ);
        }
        else
        {
            transform.position = manager.bunkerPos.position;
        }

        agent = GetComponent<NavMeshAgent>();

        isMove = false;
        saveMode = true;
    }



    void Update()
    {
        //if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //    return;

        // ����ǲ�ý���
		if (Input.touchCount == 1 && !this.IsPointerOverUIObject())
		// if (multiTouch.Tap && !IsPointerOverUIObject(multiTouch.curTouchPos)            /*&& timeController.isPause == false*/)
        {
            Ray ray = Camera.main.ScreenPointToRay(multiTouch.curTouchPos);
            RaycastHit raycastHit;
            groundLayerMask = LayerMask.GetMask("Ground");
            int facilitiesLayer = LayerMask.GetMask("Facilities");
            //int monsterLayer = LayerMask.GetMask("EliteMonster");

            // �����߻�
            if (Physics.Raycast(ray, out raycastHit, 100, LayerMask.GetMask("EliteMonster")))
            {
                //Debug.Log(raycastHit.collider.name);

                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ�
                    if (raycastHit.collider.GetComponent<MeshRenderer>().enabled)
                    {
                        var windowId = (int)Windows.MonsterWindow - 1;
                        nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;

                        timeController.PauseTime();
                        timeController.isPause = true;
                    }
                }
            }
            if (Physics.Raycast(ray, out raycastHit, 100, facilitiesLayer))
            {
                if (raycastHit.collider.gameObject.name.Equals("Bunker"))
                {
                    pX = raycastHit.collider.gameObject.transform.position.x;
                    pY = raycastHit.collider.gameObject.transform.position.y;
                    pZ = raycastHit.collider.gameObject.transform.position.z;

                    saveMode = false;
                    PlayerPrefs.SetFloat("p_x", pX);
                    PlayerPrefs.SetFloat("p_y", pY);
                    PlayerPrefs.SetFloat("p_z", pZ);

                    //��Ŀ �˾�â
                    var windowId = (int)Windows.BunkerWindow - 1;
                    nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
                    //MoveToBunker();
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
                    var windowId = (int)Windows.LaboratoryWindow - 1;
                    nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
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
                //Debug.Log(raycastHit.collider.name);

                if (raycastHit.collider.CompareTag("Enemy"))
                {
                    // �������� Ȱ��ȭ �Ǿ��������� ��ȿ�ϰ�
                    if (raycastHit.collider.GetComponent<MeshRenderer>().enabled)
                    {
                        var windowId = (int)Windows.MonsterWindow - 1;
                        nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;

                        timeController.PauseTime();
                        timeController.isPause = true;
                    }
                }
            }
            if (Physics.Raycast(ray, out raycastHit, 100, facilitiesLayer))
            {
                // ��Ŀ�� �̵�
                if (raycastHit.collider.gameObject.name.Equals("Bunker"))
                {
                    pX = raycastHit.collider.gameObject.transform.position.x;
                    pY = raycastHit.collider.gameObject.transform.position.y;
                    pZ = raycastHit.collider.gameObject.transform.position.z;

                    saveMode = false;
                    PlayerPrefs.SetFloat("p_x", pX);
                    PlayerPrefs.SetFloat("p_y", pY);
                    PlayerPrefs.SetFloat("p_z", pZ);

                    //��Ŀ �˾�â
                    var windowId = (int)Windows.BunkerWindow - 1;
                    nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
                    //MoveToBunker();
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
                    var windowId = (int)Windows.LaboratoryWindow - 1;
                    nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
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

    public void �ӽ���������()
    {
        Debug.Log("����");

        timeController.PauseTime();
        timeController.isPause = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //IEnumerator CoLoadScene()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}

    public void MoveToBunker()
    {
        SceneManager.LoadScene("Bunker");
    }

    public void MoveToLaboratory()
    {
        Debug.Log("�����ҷ� �̵�");
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }

    public void DecreaseHp(int i)
    {
        character.hp -= i;
        Debug.Log($"hp : {character.hp}");
    }

    public void DecreaseStemia(int i)
    {
        character.stemina -= i;
        Debug.Log($"stemina : {character.stemina}");
    }

    public bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition
            = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();


        EventSystem.current
        .RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    private bool IsPointerOverUIObject()
    {
        // get current pointer position and raycast it
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // check if the target is in the UI
        foreach (RaycastResult r in results)
        {
            bool isUIClick = r.gameObject.transform.IsChildOf(this.ui.transform);
            if (isUIClick)
            {
                return true;
            }
        }
        return false;
    }
}

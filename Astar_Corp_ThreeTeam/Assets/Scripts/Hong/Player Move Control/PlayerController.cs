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
    public CharacterInfo character;
    public MultiTouch multiTouch;
    public NonBattleMgr manager;

    private NavMeshAgent agent;
    private TimeController timeController;

    public GameObject panel;
    public bool isMove;//����.
    bool isBattle;
    float randomTimer;

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
        isBattle = false;
        saveMode = true;
    }

    //private WindowManager windowManager;
    private NonBattlePopUps nonBattlePopUps;
    public WindowManager windowManager;

    void Update()
    {
        if (agent.velocity.magnitude > 0.15f) //�����̰� ���� ��.
        {
            PlayerPrefs.SetFloat("p_x", transform.position.x);
            PlayerPrefs.SetFloat("p_y", transform.position.y);
            PlayerPrefs.SetFloat("p_z", transform.position.z);
        }

        // ����ǲ�ý���
        if (multiTouch.Tap &&
            !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
            /*&& timeController.isPause == false*/)
        {
            Ray ray = Camera.main.ScreenPointToRay(multiTouch.curTouchPos);
            RaycastHit raycastHit;
            groundLayerMask = LayerMask.GetMask("Ground");
            int facilitiesLayer = LayerMask.GetMask("Facilities");
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
                    //nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
                    MoveToBunker();
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
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Enemy"))
                    Debug.Log("���� �߻�");
            }
        }
        // ���콺 Ŭ��
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && timeController.isPause == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            groundLayerMask = LayerMask.GetMask("Ground");
            int facilitiesLayer = LayerMask.GetMask("Facilities");
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
                    //nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
                    MoveToBunker();
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
        // �Ͻ����� ������ �� ������ �� ���� (���� ��ġ��)
        if (timeController.isPause)
        {
            agent.SetDestination(agent.transform.position);
        }
    }

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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MonsterArea") && !isBattle) //���Ϳ� ����.
        {
            randomTimer++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MonsterArea") && !isBattle) //���Ϳ� ����.
        {
            randomTimer = 0;
        }
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
}

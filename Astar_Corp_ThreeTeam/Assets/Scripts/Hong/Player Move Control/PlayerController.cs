using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CharacterInfo
{
    public string name;
    public int hp;
    public int stemina;
}

public class PlayerController : MonoBehaviour
{
    //지은.
    public CharacterInfo character;

    public MultiTouch multiTouch;
    public NonBattleMgr manager;

    private NavMeshAgent agent;
    private CharacterController characterController;

    public GameObject panel;
    public bool isMove;//지은.
    bool isBattle;
    float randomTimer;

    float pX;
    float pY;
    float pZ;
    bool saveMode;

    private TimeController timeController;

    float originAgentSpeed;
    //private Vector3 calcVelocity = Vector3.zero;

    // to set layers
    [SerializeField]
    LayerMask groundLayerMask;

    void Start()
    {
        //timeController = GameObject.Find("TimeController").GetComponent<TimeController>();

        character = new CharacterInfo();
        character.name = "하이";
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

        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        originAgentSpeed = agent.speed;
        //panel = GetComponent<FadeOutTest>().gameObject;

        // 캐릭터 컨트롤러 포지션 설정할 때 사용
        agent.updatePosition = false;
        agent.updateRotation = true;

        isMove = false;
        isBattle = false;
        saveMode = true;
    }

    //private WindowManager windowManager;
    private NonBattlePopUps nonBattlePopUps;
    public WindowManager windowManager;

    void Update()
    {
        if (agent.velocity.magnitude > 0.15f) //움직이고 있을 때.
        {
            PlayerPrefs.SetFloat("p_x", transform.position.x);
            PlayerPrefs.SetFloat("p_y", transform.position.y);
            PlayerPrefs.SetFloat("p_z", transform.position.z);
        }

        if (multiTouch.Tap /*&& timeController.isPause == false*/)
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

                    //벙커 팝업창
                    //var windowId = (int)Windows.BunkerWindow - 1;
                    //nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
                    MoveToBunker();
                }

                else if (raycastHit.collider.gameObject.name.Equals("Laboratory"))
                {
                    pX = raycastHit.collider.gameObject.transform.position.x;
                    pY = raycastHit.collider.gameObject.transform.position.y;
                    pZ = raycastHit.collider.gameObject.transform.position.z;

                    saveMode = false;
                    PlayerPrefs.SetFloat("p_x", pX);
                    PlayerPrefs.SetFloat("p_y", pY);
                    PlayerPrefs.SetFloat("p_z", pZ);

                    Debug.Log("연구소 클릭");
                    //벙커 팝업창
                    var windowId = (int)Windows.LaboratoryWindow - 1;
                    nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
                }

                else if (raycastHit.collider.gameObject.name.Equals("Fog") ||
                   raycastHit.collider.gameObject.name.Equals("Plane"))
                {
                    agent.SetDestination(raycastHit.point);
                }

                // 수정
                // 엘리트몬스터 오브젝트 이름 확인하고 바꾸기
                //if (raycastHit.collider.gameObject.name.Equals("Elite Monster"))
                //{
                //    pX = raycastHit.collider.gameObject.transform.position.x;
                //    pY = raycastHit.collider.gameObject.transform.position.y;
                //    pZ = raycastHit.collider.gameObject.transform.position.z;
                //
                //    saveMode = false;
                //    PlayerPrefs.SetFloat("p_x", pX);
                //    PlayerPrefs.SetFloat("p_y", pY);
                //    PlayerPrefs.SetFloat("p_z", pZ);
                //
                //    //벙커 팝업창
                //    var windowId = (int)Windows.LaboratoryWindow - 1;
                //    nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
                //}
            }
            else if (Physics.Raycast(ray, out raycastHit, 100, groundLayerMask))
            {
                agent.SetDestination(raycastHit.point);
            }
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            characterController.Move(agent.velocity * Time.deltaTime);
            isMove = true;
            // Debug.Log(agent.stoppingDistance);
            // RemainingDistance : 현재 경로(path)에서 남아있는 거리
        }
        else
        {
            characterController.Move(Vector3.zero);
            isMove = false;
        }
    }

    public void MoveToBunker()
    {
        SceneManager.LoadScene("Bunker");
    }

    public void MoveToLaboratory()
    {
        Debug.Log("연구소로 이동");
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MonsterArea") && !isBattle) //몬스터와 전투.
        {
            randomTimer++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MonsterArea") && !isBattle) //몬스터와 전투.
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

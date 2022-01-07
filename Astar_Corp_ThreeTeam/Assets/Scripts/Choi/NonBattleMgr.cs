using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonBattleMgr : MonoBehaviour
{
    public GameObject player;
    public Transform bunkerPos;
    public List<GameObject> randomLaboratory;
    public List<GameObject> laboratoryObjs;
    public List<GameObject> virusZones1;
    public WindowManager windowManager;

    public GameObject monsterAreaPrefab;

    //������ ����.
    float Zone2Magnifi;
    float Zone3Magnifi;
    string[] virusType = { "E", "B", "P", "I", "T" };

    //���� ����.
    int monsterAreaCount;
    MonsterPool poolInfo;

    //��ũ ����.
    public List<Vector3> markList;

    PlayerController playerController;
    TimeController timeController;
    float timer;

    // Start is called before the first frame update
    private void Awake()
    {
        // ����
        // ���� ���� �� ����
        poolInfo = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
        monsterAreaCount = poolInfo.pools.Length;

        Zone2Magnifi = 2f;
        Zone3Magnifi = 4f;

        //ó�� �����Ҷ�.
        if (!PlayerPrefs.HasKey("MonsterAreaX0"))
        {
            var randomVirusType = RandomVirusType();

            //���� ������.
            int randomLaboratoryIndex = UnityEngine.Random.Range(0, randomLaboratory.Count);
            randomLaboratory[randomLaboratoryIndex].SetActive(true);
            string laboratoryIndexStr = "randomLaboratoryIndex";
            PlayerPrefs.SetInt(laboratoryIndexStr, randomLaboratoryIndex);
            laboratoryObjs.Add(randomLaboratory[randomLaboratoryIndex]);

            //������ ����.
            int i = 0;
            foreach (var element in laboratoryObjs)
            {
                //�ǹ��� ���� ŭ.
                var virusZone3 = element.transform.GetChild(1).gameObject;
                var virusZone2 = element.transform.GetChild(2).gameObject;
                var virusZone1 = element.transform.GetChild(3).gameObject;

                var script = virusZone1.GetComponent<LaboratoryInfo>();
                int randomNum = (!script.isSpareLab) ? 10 : 8;

                virusZone3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                virusZone2.transform.localScale = new Vector3(randomNum * Zone2Magnifi, randomNum * Zone2Magnifi, randomNum * Zone2Magnifi);
                virusZone1.transform.localScale = new Vector3(randomNum * Zone3Magnifi, randomNum * Zone3Magnifi, randomNum * Zone3Magnifi);

                var radius = virusZone3.GetComponent<SphereCollider>().radius;
                script.radiusZone3 = radius * virusZone3.transform.lossyScale.x;
                script.radiusZone2 = radius * virusZone2.transform.lossyScale.x;
                script.radiusZone1 = radius * virusZone1.transform.lossyScale.x;

                string str = $"VirusZone1Scale{i}";
                PlayerPrefs.SetInt(str, randomNum);

                script.isActiveZone2 = (Random.Range(0, 2) == 0) ? true : false;
                if (!script.isActiveZone2) virusZone2.SetActive(false);

                if (script.isActiveZone2) script.isActiveZone3 = (Random.Range(0, 2) == 0) ? true : false;
                if (!script.isActiveZone3) virusZone3.SetActive(false);

                str = $"Laboratory{i}Zone2";
                int num = (script.isActiveZone2 == true) ? 1 : 0;
                PlayerPrefs.SetInt(str, num);
                str = $"Laboratory{i}Zone3";
                num = (script.isActiveZone3 == true) ? 1 : 0;
                PlayerPrefs.SetInt(str, num);

                script.virusType = randomVirusType[i];
                str = $"Laboratory{i}VirusType";
                PlayerPrefs.SetString(str, randomVirusType[i]);

                i++;
            }

            //MonsterArea ����
            for (int j = 0; j < monsterAreaCount; j++)
            {
                int randX;
                int randScale;
                int randomIndex;
                Vector3 position;

                var radius = monsterAreaPrefab.GetComponent<SphereCollider>().radius;
                var monsterAreaLayer = LayerMask.GetMask("MonsterArea");
                var facilitiesLayer = LayerMask.GetMask("facilities");
                var virusZone = LayerMask.GetMask("VirusZone");
                var playerLayer = LayerMask.GetMask("Player");
                var boundaryLayer = LayerMask.GetMask("Boundary");
                do
                {
                    randomIndex = UnityEngine.Random.Range(0, virusZones1.Count);
                    var randLaboratoryPos = virusZones1[randomIndex].transform.position;

                    randX = UnityEngine.Random.Range(10, 20);
                    var pos = Random.onUnitSphere * randX + randLaboratoryPos;
                    position = new Vector3(pos.x, 0, pos.z);
                    randScale = UnityEngine.Random.Range(6, 10);

                } while ((Physics.OverlapSphere(position, radius * randScale, monsterAreaLayer).Length != 0)
                || (Physics.OverlapSphere(position, radius * randScale, playerLayer).Length != 0)
                || (Physics.OverlapSphere(position, radius * randScale, boundaryLayer).Length != 0)
                /*|| (Physics.OverlapSphere(position, radius * randScale, virusZone).Length == 0)*/);

                string str = $"MonsterAreaX{j}";
                PlayerPrefs.SetFloat(str, position.x);
                str = $"MonsterAreaZ{j}";
                PlayerPrefs.SetFloat(str, position.z);
                str = $"MonsterAreaScale{j}";
                PlayerPrefs.SetInt(str, randScale);

                var go = Instantiate(monsterAreaPrefab, position, Quaternion.identity);
                go.transform.SetParent(GameObject.Find("MonsterArea").transform);   // �θ������Ʈ ����
                //go.transform.localScale = new Vector3(randScale, randScale, randScale);
            }
        }
        else//�̾��ϱ�.
        {
            string laboratoryIndexStr = "randomLaboratoryIndex";
            int randomLaboratoryIndex = PlayerPrefs.GetInt(laboratoryIndexStr);
            randomLaboratory[randomLaboratoryIndex].SetActive(true);
            laboratoryObjs.Add(randomLaboratory[randomLaboratoryIndex]);

            int i = 0;
            foreach (var element in laboratoryObjs)
            {
                var virusZone3 = element.transform.GetChild(1).gameObject;
                var virusZone2 = element.transform.GetChild(2).gameObject;
                var virusZone1 = element.transform.GetChild(3).gameObject;

                string str = $"VirusZone1Scale{i}";
                int randomNum = PlayerPrefs.GetInt(str);

                var script = virusZone1.GetComponent<LaboratoryInfo>();
                var radius = virusZone3.GetComponent<SphereCollider>().radius;

                virusZone3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                virusZone2.transform.localScale = new Vector3(randomNum * Zone2Magnifi, randomNum * Zone2Magnifi, randomNum * Zone2Magnifi);
                virusZone1.transform.localScale = new Vector3(randomNum * Zone3Magnifi, randomNum * Zone3Magnifi, randomNum * Zone3Magnifi);

                script.radiusZone3 = radius * virusZone3.transform.lossyScale.x;
                script.radiusZone2 = radius * virusZone2.transform.lossyScale.x;
                script.radiusZone1 = radius * virusZone1.transform.lossyScale.x;

                str = $"Laboratory{i}Zone2";
                int num = PlayerPrefs.GetInt(str);
                if (num == 0) virusZone2.SetActive(false);
                else script.isActiveZone2 = true;

                str = $"Laboratory{i}Zone3";
                num = PlayerPrefs.GetInt(str);
                if (num == 0) virusZone3.SetActive(false);
                else script.isActiveZone3 = true;

                str = $"Laboratory{i}VirusType";
                var virusType = PlayerPrefs.GetString(str);
                script.virusType = virusType;

                i++;
            }
            //MonsterArea ����.
            for (int j = 0; j < monsterAreaCount; j++)
            {
                string str = $"MonsterAreaX{j}";
                var randX = PlayerPrefs.GetFloat(str);

                str = $"MonsterAreaZ{j}";
                var randZ = PlayerPrefs.GetFloat(str);

                var go = Instantiate(monsterAreaPrefab, new Vector3(randX, 0, randZ), Quaternion.identity);
                go.transform.SetParent(GameObject.Find("MonsterArea").transform);   // �θ������Ʈ ����
                str = $"MonsterAreaScale{j}";
                var randScale = PlayerPrefs.GetInt(str);
                //go.transform.localScale = new Vector3(randScale, randScale, randScale);
            }
        }

        playerController = player.GetComponent<PlayerController>();
        playerController.manager = this;

        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        playerController.timeController = timeController;


        //��ũ ����.
        markList = new List<Vector3>();
    }

    string[] RandomVirusType()
    {
        var randomVirusType = virusType;
        for (int i = 0; i < 50; i++)
        {
            var random1 = UnityEngine.Random.Range(0, virusType.Length);
            var random2 = UnityEngine.Random.Range(0, virusType.Length);
            var temp = randomVirusType[random1];
            randomVirusType[random1] = randomVirusType[random2];
            randomVirusType[random2] = temp;
        }

        return randomVirusType;
    }

    public void MoveToBunker()
    {
        SceneManager.LoadScene("Bunker");
    }

    public void OpenBunkerPopup()
    {
        var windowId = (int)Windows.BunkerWindow - 1;
        var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    }

    public void CloseBunkerPopup()
    {
        var windowId = (int)Windows.BunkerWindow - 1;
        windowManager.windows[windowId].Close();
    }

    public void OpenLaboratoryPopup()
    {
        var windowId = (int)Windows.LaboratoryWindow - 1;
        var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    }

    public void CloseLaboratoryPopup()
    {
        var windowId = (int)Windows.LaboratoryWindow - 1;
        windowManager.windows[windowId].Close();
    }

    public void OpenMonsterPopup()
    {
        var windowId = (int)Windows.MonsterWindow - 1;
        var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    }

    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}
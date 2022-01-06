using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattleMgr : MonoBehaviour
{
    public GameObject player;
    public Transform bunkerPos;
    public List<GameObject> randomLaboratory;
    public List<GameObject> laboratoryObjs;
    public List<GameObject> virusZones1;

    public GameObject monsterAreaPrefab;
    public GameObject eliteMonsterPrefab;

    //연구소 영역.
    float Zone2Magnifi;
    float Zone3Magnifi;
    string[] virusType = { "E", "B", "P", "I", "T" };
    
    //몬스터 영역.
    int monsterAreaCount;
    MonsterPool poolInfo;
    //엘리트 몬스터.
    int eliteMonsterCount;

    //마크 관리.
    public List<Vector3> markList;

    PlayerController playerController;
    float timer;

    // Start is called before the first frame update
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();

        // 수진
        // 몬스터 영역 수 설정
        poolInfo = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
        monsterAreaCount = poolInfo.pools.Length;
        //monsterAreaCount = 5;
        eliteMonsterCount = 5;

        Zone2Magnifi = 2f;
        Zone3Magnifi = 4f;


        //처음 시작할때.
        if (!PlayerPrefs.HasKey("MonsterAreaX0"))
        {
            var randomVirusType = RandomVirusType();

            //예비 연구소.
            int randomLaboratoryIndex = UnityEngine.Random.Range(0, randomLaboratory.Count);
            randomLaboratory[randomLaboratoryIndex].SetActive(true);
            string laboratoryIndexStr = "randomLaboratoryIndex";
            PlayerPrefs.SetInt(laboratoryIndexStr, randomLaboratoryIndex);
            laboratoryObjs.Add(randomLaboratory[randomLaboratoryIndex]);

            //연구소 영역.
            int i = 0;
            foreach (var element in laboratoryObjs)
            {
                //맨밑이 제일 큼.
                var virusZone3 = element.transform.GetChild(1).gameObject;
                var virusZone2 = element.transform.GetChild(2).gameObject;
                var virusZone1 = element.transform.GetChild(3).gameObject;

                var script = virusZone1.GetComponent<LaboratoryInfo>();
                int randomNum = (!script.isSpareLab)? 10 : 8;

                virusZone3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                virusZone2.transform.localScale = new Vector3(randomNum * Zone2Magnifi, randomNum * Zone2Magnifi, randomNum * Zone2Magnifi);
                virusZone1.transform.localScale = new Vector3(randomNum * Zone3Magnifi, randomNum * Zone3Magnifi, randomNum * Zone3Magnifi);

                var radius = virusZone3.GetComponent<SphereCollider>().radius;
                script.radiusZone3 = radius * virusZone3.transform.lossyScale.x;
                script.radiusZone2 = radius * virusZone2.transform.lossyScale.x;
                script.radiusZone1 = radius * virusZone1.transform.lossyScale.x;

                string str = $"VirusZone1Scale{i}";
                PlayerPrefs.SetInt(str, randomNum);

                script.isActiveZone2 = (Random.Range(0, 2) == 0)? true : false;
                if (!script.isActiveZone2) virusZone2.SetActive(false);
                    
                if(script.isActiveZone2) script.isActiveZone3 = (Random.Range(0, 2) == 0) ? true : false;
                if (!script.isActiveZone3) virusZone3.SetActive(false);
                
                str = $"Laboratory{i}Zone2";
                int num = (script.isActiveZone2 == true)? 1 : 0;
                PlayerPrefs.SetInt(str, num);
                str = $"Laboratory{i}Zone3";
                num = (script.isActiveZone3 == true) ? 1 : 0;
                PlayerPrefs.SetInt(str, num);

                script.virusType = randomVirusType[i];
                str = $"Laboratory{i}VirusType";
                PlayerPrefs.SetString(str, randomVirusType[i]);
                
                i++;
            }

            //MonsterArea 생성
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
                go.transform.SetParent(GameObject.Find("MonsterArea").transform);   // 부모오브젝트 설정
                //go.transform.localScale = new Vector3(randScale, randScale, randScale);
            }
            //EliteMonster 생성.
            for (int j = 0; j < eliteMonsterCount; j++)
            {
                int randX;
                int randomIndex;
                Vector3 position;

                var radius = monsterAreaPrefab.GetComponent<SphereCollider>().radius;

                var eliteMonsterLayer = LayerMask.GetMask("EliteMonster");
                var facilitiesLayer = LayerMask.GetMask("facilities");
                var virusZone = LayerMask.GetMask("VirusZone");
                var playerLayer = LayerMask.GetMask("Player");
                var boundaryLayer = LayerMask.GetMask("Boundary");
                do
                {
                randomIndex = UnityEngine.Random.Range(0, laboratoryObjs.Count);
                var randLaboratoryPos = laboratoryObjs[randomIndex].transform.position;

                randX = UnityEngine.Random.Range(10, 20);
                var pos = Random.onUnitSphere * randX + randLaboratoryPos;
                position = new Vector3(pos.x, 0, pos.z);

                } while ((Physics.OverlapSphere(position, radius, playerLayer).Length != 0)
                || (Physics.OverlapSphere(position, radius, eliteMonsterLayer).Length != 0)
                || (Physics.OverlapSphere(position, radius, boundaryLayer).Length != 0));

                string str = $"EliteMonsterX{j}";
                PlayerPrefs.SetFloat(str, position.x);
                str = $"EliteMonsterZ{j}";
                PlayerPrefs.SetFloat(str, position.z);
                
                var go = Instantiate(eliteMonsterPrefab, position, Quaternion.identity);
            }
        }
        else//이어하기.
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
                int num  = PlayerPrefs.GetInt(str);
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
            //MonsterArea 생성.
            for (int j = 0; j < monsterAreaCount; j++)
            {
                string str = $"MonsterAreaX{j}";
                var randX = PlayerPrefs.GetFloat(str);
            
                str = $"MonsterAreaZ{j}";
                var randZ = PlayerPrefs.GetFloat(str);
            
                var go = Instantiate(monsterAreaPrefab, new Vector3(randX, 0, randZ), Quaternion.identity);
                go.transform.SetParent(GameObject.Find("MonsterArea").transform);   // 부모오브젝트 설정
                str = $"MonsterAreaScale{j}";
                var randScale = PlayerPrefs.GetInt(str);
                //go.transform.localScale = new Vector3(randScale, randScale, randScale);
            }
            //EliteMonster 생성.
            for (int j = 0; j < eliteMonsterCount; j++)
            {
                string str = $"EliteMonsterX{j}";
                var randX = PlayerPrefs.GetFloat(str);

                str = $"EliteMonsterZ{j}";
                var randZ = PlayerPrefs.GetFloat(str);

                var go = Instantiate(eliteMonsterPrefab, new Vector3(randX, 0, randZ), Quaternion.identity);             // 부모오브젝트

            }
        }

        playerController = player.GetComponent<PlayerController>();

        //마크 관리.
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

    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}
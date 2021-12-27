using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattleMgr : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> maps;
    public List<Transform> bunkerPos;
    public List<GameObject> randomBunker;
    public List<GameObject> laboratoryObjs;
    public List<GameObject> virusZones1;

    public GameObject monsterAreaPrefab;
    public GameObject eliteMonsterPrefab;

    //연구소 영역.
    float Zoon2Magnifi = 2f;
    string[] virusType = { "E", "B", "P", "I", "T" };
    
    //몬스터 영역.
    int monsterAreaCount;
    //엘리트 몬스터.
    int eliteMonsterCount;

    //마크 관리.
    public List<Vector3> markList;

    PlayerController playerController;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();

        monsterAreaCount = 5;
        eliteMonsterCount = 5;

        Zoon2Magnifi = 2f;
        
        //처음 시작할때.
        if (!PlayerPrefs.HasKey("MonsterAreaX0"))
        {
            int eBunkerIndex = UnityEngine.Random.Range(0, randomBunker.Count);
            randomBunker[eBunkerIndex].SetActive(true);
            bunkerPos.Add(randomBunker[eBunkerIndex].transform);
            string eBunkerIndexStr = "EBunkerIndex";
            PlayerPrefs.SetInt(eBunkerIndexStr, eBunkerIndex);

            var randomVirusType = RandomVirusType();
            foreach (var element in randomVirusType)
            {
                Debug.Log($"{element}");
            }

            int i = 0;
            foreach (var element in laboratoryObjs)
            {
                //맨밑이 제일 큼.
                var virusZoon3 = element.transform.GetChild(1).gameObject;
                var virusZoon2 = element.transform.GetChild(2).gameObject;
                
                int randomNum = UnityEngine.Random.Range(5, 8);

                virusZoon3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                virusZoon2.transform.localScale = new Vector3(randomNum * Zoon2Magnifi, randomNum * Zoon2Magnifi, randomNum * Zoon2Magnifi);
                
                var radius = virusZoon3.GetComponent<SphereCollider>().radius;
                var script = virusZoon2.GetComponent<LaboratoryInfo>();
                script.radiusZone3 = radius * virusZoon3.transform.lossyScale.x;
                script.radiusZone2 = radius * virusZoon2.transform.lossyScale.x;
                script.virusType = randomVirusType[i];
                
                string str = $"VirusZone2Scale{i}";
                PlayerPrefs.SetInt(str, randomNum);
                i++;
            }

            i = 0;
            foreach (var element in virusZones1)
            {
                //범위 랜덤 설정.
                int randomNum = UnityEngine.Random.Range(14, 18);
                element.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                string str = $"VirusZone1Scale{i}";
                PlayerPrefs.SetInt(str, randomNum);

                //해당지역 연구소 정보 가져오기.
                var script = element.GetComponent<VirusZone1Info>();
                script.laboratoryPos = laboratoryObjs[i].transform.position;
                
                var virusZoon2 = laboratoryObjs[i].transform.GetChild(2).gameObject;
                var laboratoryInfo= virusZoon2.GetComponent<LaboratoryInfo>();
                script.virusZone2Radius = laboratoryInfo.radiusZone2;
                script.virusType = laboratoryInfo.virusType;
                
                i++;    
            }

            //MonsterArea 생성.
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
                go.transform.localScale = new Vector3(randScale, randScale, randScale);
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
            string eBunkerIndexStr = "EBunkerIndex";
            int eBunkerIndex = PlayerPrefs.GetInt(eBunkerIndexStr);
            randomBunker[eBunkerIndex].SetActive(true);
            bunkerPos.Add(randomBunker[eBunkerIndex].transform);
           
            int i = 0;
            foreach (var element in laboratoryObjs)
            {
                var virusZoon3 = element.transform.GetChild(1).gameObject;
                var virusZoon2 = element.transform.GetChild(2).gameObject;
                
                string str = $"VirusZone2Scale{i}";
                int randomNum = PlayerPrefs.GetInt(str);
                virusZoon3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                virusZoon2.transform.localScale = new Vector3(randomNum * Zoon2Magnifi, randomNum * Zoon2Magnifi, randomNum * Zoon2Magnifi);
                
                i++;

                var radius = virusZoon3.GetComponent<SphereCollider>().radius;
                var script = virusZoon2.GetComponent<LaboratoryInfo>();
                script.radiusZone3 = radius * virusZoon3.transform.lossyScale.x;
                script.radiusZone2 = radius * virusZoon2.transform.lossyScale.x;
            }
            i = 0;
            foreach (var element in virusZones1)
            {
                //범위 랜덤 설정.
                string str = $"VirusZone1Scale{i}";
                int randomNum = PlayerPrefs.GetInt(str);
                element.transform.localScale = new Vector3(randomNum, randomNum, randomNum);

                //해당지역 연구소 정보 가져오기.
                var script = element.GetComponent<VirusZone1Info>();
                script.laboratoryPos = laboratoryObjs[i].transform.position;

                var virusZoon2 = laboratoryObjs[i].transform.GetChild(2).gameObject;
                var laboratoryInfo = virusZoon2.GetComponent<LaboratoryInfo>();
                script.virusZone2Radius = laboratoryInfo.radiusZone2;
                i++;
            }

            for (int j = 0; j < monsterAreaCount; j++)
            {
                string str = $"MonsterAreaX{j}";
                var randX = PlayerPrefs.GetFloat(str);

                str = $"MonsterAreaZ{j}";
                var randZ = PlayerPrefs.GetFloat(str);

                var go = Instantiate(monsterAreaPrefab, new Vector3(randX, 0, randZ), Quaternion.identity);
                str = $"MonsterAreaScale{j}";
                var randScale = PlayerPrefs.GetInt(str);
                go.transform.localScale = new Vector3(randScale, randScale, randScale);
            }
            for (int j = 0; j < eliteMonsterCount; j++)
            {
                string str = $"EliteMonsterX{j}";
                var randX = PlayerPrefs.GetFloat(str);

                str = $"EliteMonsterZ{j}";
                var randZ = PlayerPrefs.GetFloat(str);

                var go = Instantiate(eliteMonsterPrefab, new Vector3(randX, 0, randZ), Quaternion.identity);
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
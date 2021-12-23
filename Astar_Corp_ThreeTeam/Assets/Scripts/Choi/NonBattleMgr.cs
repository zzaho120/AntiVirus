using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum MapType
//{
//    Seoul,
//    Suncheon,
//    Daegu,
//    None
//}

public class NonBattleMgr : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> maps;
    public List<Transform> bunkerPos;
    public List<GameObject> laboratoryObj;

    public Dictionary<GameObject, List<GameObject>> randomEvents;
    public GameObject randomEventPrefab;

    public GameObject monsterAreaPrefab;
    int monsterAreaCount;

    //付农 包府.
    public List<Vector3> markList;

    PlayerController playerController;
    float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        monsterAreaCount = 5;

        //贸澜 矫累且锭.
        if (!PlayerPrefs.HasKey("VirusCoverage0"))
        {
            int i = 0;
            foreach (var element in laboratoryObj)
            {
                //盖关捞 力老 怒.
                var virusZoon3 = element.transform.GetChild(1).gameObject;
                var virusZoon2 = element.transform.GetChild(2).gameObject;
                var virusZoon1 = element.transform.GetChild(3).gameObject;

                int randomNum = UnityEngine.Random.Range(2, 7);
                float Zoon2Magnifi = 2f;
                float Zoon3Magnifi = 3f;

                virusZoon3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                virusZoon2.transform.localScale = new Vector3(randomNum * Zoon2Magnifi, randomNum * Zoon2Magnifi, randomNum * Zoon2Magnifi);
                virusZoon1.transform.localScale = new Vector3(randomNum * Zoon3Magnifi, randomNum * Zoon3Magnifi, randomNum * Zoon3Magnifi);

                string str = $"VirusCoverage{i}";
                PlayerPrefs.SetInt(str, randomNum);
                i++;
            }

            for (int j = 0; j < monsterAreaCount; j++)
            {
                int randX;
                int randZ;
                int randScale;
                Vector3 position;

                var radius = monsterAreaPrefab.GetComponent<SphereCollider>().radius;
                var monsterAreaLayer = LayerMask.GetMask("MonsterArea");
                var facilitiesLayer = LayerMask.GetMask("facilities");
                var virusZone = LayerMask.GetMask("VirusZone");
                var playerLayer = LayerMask.GetMask("Player");
                do
                {
                    int randomIndex = UnityEngine.Random.Range(0, laboratoryObj.Count);
                    var randLaboratoryPos = laboratoryObj[randomIndex].transform.position;

                    var secondChild = laboratoryObj[randomIndex].transform.GetChild(3).gameObject;
                    var secondChildRadius = secondChild.GetComponent<SphereCollider>().radius;
                    var secondChildScale = secondChild.transform.localScale.x;
                    var range = secondChildRadius * secondChildScale;

                    Debug.Log($"randLaboratoryPos : {randLaboratoryPos.x}");
                    Debug.Log($"Random.onUnitSphere * range : {(Random.onUnitSphere * range).x}");
                    var pos = Random.onUnitSphere * range + randLaboratoryPos;
                    Debug.Log($"pos.x : {pos.x}");

                    randX = UnityEngine.Random.Range(-5, 5);
                    randZ = UnityEngine.Random.Range(-5, 5);

                    //position = new Vector3((randLaboratoryPos.x ) + randX, 0, (randLaboratoryPos.z) + randZ);
                    position = new Vector3(pos.x , 0, pos.z );
                    randScale = UnityEngine.Random.Range(6, 10);

                } while (/*(Physics.OverlapSphere(position, radius * randScale, monsterAreaLayer).Length != 0)
                ||*/ (Physics.OverlapSphere(position, radius * randScale, playerLayer).Length != 0)
                /*|| (Physics.OverlapSphere(position, radius * randScale, virusZone).Length == 0)*/);

                string str = $"MonsterAreaX{j}";
                PlayerPrefs.SetInt(str, randX);
                str = $"MonsterAreaZ{j}";
                PlayerPrefs.SetInt(str, randZ);
                str = $"MonsterAreaScale{j}";
                PlayerPrefs.SetInt(str, randScale);

                var go = Instantiate(monsterAreaPrefab, position, Quaternion.identity);
                go.transform.localScale = new Vector3(randScale, randScale, randScale);
            }
        }
        else
        {
            int i = 0;
            foreach (var element in laboratoryObj)
            {
                var coverage = element.transform.GetChild(1).gameObject;
                string str = $"VirusCoverage{i}";
                int randomNum = PlayerPrefs.GetInt(str);
                coverage.transform.localScale = new Vector3(randomNum, randomNum, randomNum);

                i++;
            }

            for (int j = 0; j < monsterAreaCount; j++)
            {
                string str = $"MonsterAreaX{j}";
                var randX = PlayerPrefs.GetInt(str);

                str = $"MonsterAreaZ{j}";
                var randZ = PlayerPrefs.GetInt(str);

                var go = Instantiate(monsterAreaPrefab, new Vector3(randX, 0, randZ), Quaternion.identity);
                str = $"MonsterAreaScale{j}";
                var randScale = PlayerPrefs.GetInt(str);
                go.transform.localScale = new Vector3(randScale, randScale, randScale);
            }
        }

        playerController = player.GetComponent<PlayerController>();

        randomEvents = new Dictionary<GameObject, List<GameObject>>();

        foreach (var element in maps)
        {
            randomEvents.Add(element, new List<GameObject>());
        }

        foreach (var element in maps)
        {
            var randomNum = Random.Range(0, 4);
            for (int i = 0; i < randomNum; i++)
            {
                var randomX = element.transform.position.x + Random.Range(0, 10);
                var randomZ = element.transform.position.z + Random.Range(0, 10);

                var go = Instantiate(randomEventPrefab, new Vector3(randomX, 0, randomZ), Quaternion.identity);
                go.tag = "RandomEvent";

                randomEvents[element].Add(go);
            }
        }

        //付农 包府.
        markList = new List<Vector3>();
    }

    void CreateEvents()
    {
        foreach (var element in maps)
        {
            randomEvents.Add(element, new List<GameObject>());
        }

        foreach (var element in maps)
        {
            var randomNum = Random.Range(0, 4);
            for (int i = 0; i < randomNum; i++)
            {
                var randomX = element.transform.position.x + Random.Range(0, 10);
                var randomZ = element.transform.position.z + Random.Range(0, 10);

                var go = Instantiate(randomEventPrefab, new Vector3(randomX, 0, randomZ), Quaternion.identity);
                go.tag = "RandomEvent";

                randomEvents[element].Add(go);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonBattleMgr : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> maps;
    public List<Transform> bunkerPos;
    public List<GameObject> laboratoryObj;

    public GameObject monsterAreaPrefab;
    public GameObject eliteMonsterPrefab;

    //楷备家 康开.
    float Zoon2Magnifi = 2f;
    float Zoon3Magnifi = 3f;
    //阁胶磐 康开.
    int monsterAreaCount;
    //郡府飘 阁胶磐.
    int eliteMonsterCount;

    //付农 包府.
    public List<Vector3> markList;

    PlayerController playerController;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();

        monsterAreaCount = 5;
        eliteMonsterCount = 5;

        Zoon2Magnifi = 2f;
        Zoon3Magnifi = 3f;

        //贸澜 矫累且锭.
        if (!PlayerPrefs.HasKey("MonsterAreaX0"))
        {
            int i = 0;
            foreach (var element in laboratoryObj)
            {
                //盖关捞 力老 怒.
                var virusZoon3 = element.transform.GetChild(1).gameObject;
                var virusZoon2 = element.transform.GetChild(2).gameObject;
                var virusZoon1 = element.transform.GetChild(3).gameObject;

                int randomNum = UnityEngine.Random.Range(2, 7);

                virusZoon3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                virusZoon2.transform.localScale = new Vector3(randomNum * Zoon2Magnifi, randomNum * Zoon2Magnifi, randomNum * Zoon2Magnifi);
                virusZoon1.transform.localScale = new Vector3(randomNum * Zoon3Magnifi, randomNum * Zoon3Magnifi, randomNum * Zoon3Magnifi);

                var radius = virusZoon3.GetComponent<SphereCollider>().radius;
                var script = virusZoon1.GetComponent<LaboratoryInfo>();
                script.radiusZone3 = radius * virusZoon3.transform.lossyScale.x;
                script.radiusZone2 = radius * virusZoon2.transform.lossyScale.x;
                script.radiusZone1 = radius * virusZoon1.transform.lossyScale.x;

                string str = $"VirusZoneScale{i}";
                PlayerPrefs.SetInt(str, randomNum);
                i++;
            }
            //MonsterArea 积己.
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
                do
                {
                    randomIndex = UnityEngine.Random.Range(0, laboratoryObj.Count);
                    var randLaboratoryPos = laboratoryObj[randomIndex].transform.position;

                    randX = UnityEngine.Random.Range(10, 20);
                    var pos = Random.onUnitSphere * randX + randLaboratoryPos;
                    position = new Vector3(pos.x, 0, pos.z);
                    randScale = UnityEngine.Random.Range(6, 10);

                } while (/*(Physics.OverlapSphere(position, radius * randScale, monsterAreaLayer).Length != 0)*/
                /*||*/ (Physics.OverlapSphere(position, radius * randScale, playerLayer).Length != 0)
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
            //EliteMonster 积己.
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
                do
                {
                randomIndex = UnityEngine.Random.Range(0, laboratoryObj.Count);
                var randLaboratoryPos = laboratoryObj[randomIndex].transform.position;

                randX = UnityEngine.Random.Range(10, 20);
                var pos = Random.onUnitSphere * randX + randLaboratoryPos;
                position = new Vector3(pos.x, 0, pos.z);

                } while (/*(Physics.OverlapSphere(position, radius, playerLayer).Length != 0)*/
                /*||*/ (Physics.OverlapSphere(position, radius, eliteMonsterLayer).Length != 0)
                /*|| (Physics.OverlapSphere(position, radius, virusZone).Length == 0)*/);

                string str = $"EliteMonsterX{j}";
                PlayerPrefs.SetFloat(str, position.x);
                str = $"EliteMonsterZ{j}";
                PlayerPrefs.SetFloat(str, position.z);
                
                var go = Instantiate(eliteMonsterPrefab, position, Quaternion.identity);
            }
        }
        else
        {
            int i = 0;
            foreach (var element in laboratoryObj)
            {
                var virusZoon3 = element.transform.GetChild(1).gameObject;
                var virusZoon2 = element.transform.GetChild(2).gameObject;
                var virusZoon1 = element.transform.GetChild(3).gameObject;

                string str = $"VirusZoneScale{i}";
                int randomNum = PlayerPrefs.GetInt(str);
                virusZoon3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                virusZoon2.transform.localScale = new Vector3(randomNum * Zoon2Magnifi, randomNum * Zoon2Magnifi, randomNum * Zoon2Magnifi);
                virusZoon1.transform.localScale = new Vector3(randomNum * Zoon3Magnifi, randomNum * Zoon3Magnifi, randomNum * Zoon3Magnifi);

                i++;

                var radius = virusZoon3.GetComponent<SphereCollider>().radius;
                var script = virusZoon1.GetComponent<LaboratoryInfo>();
                script.radiusZone3 = radius * virusZoon3.transform.lossyScale.x;
                script.radiusZone2 = radius * virusZoon2.transform.lossyScale.x;
                script.radiusZone1 = radius * virusZoon1.transform.lossyScale.x;
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

        //付农 包府.
        markList = new List<Vector3>();
    }

    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}
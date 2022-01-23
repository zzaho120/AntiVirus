using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateMonsterAreas : MonoBehaviour
{
    private NonBattleMgr nonBattleMgr;

    //몬스터 영역.
    public GameObject monsterAreaPrefab;
    private MonsterPool poolInfo;
    private int monsterAreaCount;
    private float posY = 10f;   // y좌표 설정
    public int labortoryNum;

    private GameObject fog;

    private List<GameObject> monsterAreaList = new List<GameObject>();
    private LaboratoryInfo labInfo;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.magenta;
    //    foreach (var go in monsterAreaList)
    //    {
    //        var col = go.GetComponentInChildren<SphereCollider>();
    //        Gizmos.DrawSphere(go.transform.position, go.transform.lossyScale.x * col.radius);
    //    }
    //}

    public void Init()
    {
        nonBattleMgr = NonBattleMgr.Instance;

        monsterAreaList.ToArray();
        //PlayerPrefs.DeleteAll();

        labInfo = GetComponentInChildren<LaboratoryInfo>();

        //Debug.Log("Zone2 : " + labInfo.isActiveZone2);
        //Debug.Log("Zone3 : " + labInfo.isActiveZone3);

        //// 몬스터 영역 수 설정
        if (labInfo != null)
        {
            if (labInfo.isActiveZone2 && !labInfo.isActiveZone3)
            {
                monsterAreaCount = 4;
            }
            else if (labInfo.isActiveZone2 && labInfo.isActiveZone3)
            {
                monsterAreaCount = 6;
            }
            else
            {
                monsterAreaCount = 2;
            }
            //Debug.Log(monsterAreaCount);
        }

        // 몬스터 영역 수 설정
        //poolInfo = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
        //monsterAreaCount = poolInfo.pools.Count;

        fog = GameObject.Find("Fog");

        // 처음 생성 시
        if (!PlayerPrefs.HasKey("MonsterAreaX0"))
        {
            //MonsterArea 생성
            for (int j = 0; j < monsterAreaCount; j++)
            {
                #region 기존 코드
                //int randX;
                //int randScale;
                //int randomIndex;
                //Vector3 position;
                //
                //var radius = monsterAreaPrefab.GetComponent<SphereCollider>().radius;
                //var monsterAreaLayer = LayerMask.GetMask("MonsterArea");
                //var virusZone = LayerMask.GetMask("VirusZone");
                //var playerLayer = LayerMask.GetMask("Player");
                //var boundaryLayer = LayerMask.GetMask("Boundary");
                //var groundLayer = LayerMask.GetMask("Ground");
                //
                //do
                //{
                //    randomIndex = Random.Range(0, nonBattleMgr.laboratoryArea.virusZones1.Count);
                //    var randLaboratoryPos = nonBattleMgr.laboratoryArea.virusZones1[randomIndex].transform.position;    // 연구소 위치 임의로 가져옴
                //
                //    randX = Random.Range(10, 20);
                //    var pos = Random.onUnitSphere * randX + randLaboratoryPos;
                //    position = new Vector3(pos.x, posY, pos.z);
                //    randScale = Random.Range(30, 50);
                //}
                //while ((Physics.OverlapSphere(position, radius * randScale, monsterAreaLayer).Length != 0));
                ////|| (Physics.OverlapSphere(position, radius * randScale, playerLayer).Length != 0)
                ////|| (Physics.OverlapSphere(position, radius * randScale, boundaryLayer).Length != 0)
                ///*|| (Physics.OverlapSphere(position, radius * randScale, virusZone).Length == 0)*/);
                #endregion

                // 위치 지정
                Vector3 position;
                int randScale;

                var bigRadius = nonBattleMgr.laboratoryArea.virusZones1[labortoryNum].GetComponent<SphereCollider>();
                var radius = monsterAreaPrefab.GetComponent<SphereCollider>(); //.radius;
                var monsterAreaLayer = LayerMask.GetMask("MonsterArea");
                var virusZone = LayerMask.GetMask("VirusZone");
                var playerLayer = LayerMask.GetMask("Player");

                Vector3 bigCenter = new Vector3(bigRadius.transform.position.x, 0, bigRadius.transform.position.z);
                Vector3 smallCenter;
                // 겹치지 않을 때까지 좌표 가져오기
                do
                {
                    Vector3 pos = RandomPosOnFog();
                    position = new Vector3(pos.x, posY, pos.z);
                    randScale = Random.Range(30, 50);
                    smallCenter = new Vector3(position.x, 0, position.z);
                }
                while ((Physics.OverlapSphere(position, radius.radius * monsterAreaPrefab.transform.lossyScale.x, monsterAreaLayer).Length != 0) ||
                       (Physics.OverlapSphere(position, radius.radius * monsterAreaPrefab.transform.lossyScale.x, playerLayer).Length != 0) ||
                       Vector3.Distance(bigCenter, smallCenter) > ((bigRadius.radius * bigRadius.transform.lossyScale.x) - (radius.radius * monsterAreaPrefab.transform.lossyScale.x)));

                // 원 생성 위치에 큐브 놔두기
                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //cube.transform.position = bigCenter;
                //cube.transform.localScale = new Vector3(5f, bigRadius.transform.localScale.y, 5f);
                //GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //cube2.transform.position = smallCenter; //radius.transform.position;
                //cube2.transform.localScale = new Vector3(3f, radius.transform.localScale.y, 3f);

                //// 몬스터 영역 저장
                //string str = $"MonsterAreaX{j}";
                //PlayerPrefs.SetFloat(str, position.x);
                //str = $"MonsterAreaZ{j}";
                //PlayerPrefs.SetFloat(str, position.z);
                //str = $"MonsterAreaScale{j}";
                //PlayerPrefs.SetFloat(str, monsterAreaPrefab.transform.lossyScale.x);
                ////PlayerPrefs.SetInt(str, randScale);

                var go = Instantiate(monsterAreaPrefab, position, Quaternion.identity);
                go.transform.SetParent(GameObject.Find("MonsterArea").transform);   // 부모오브젝트 설정
                //go.transform.localScale = new Vector3(randScale, randScale, randScale);

                monsterAreaList.Add(go);
            }
        }
        // 게임 이어서 할 때
        else
        {
            //MonsterArea 생성.
            for (int j = 0; j < monsterAreaCount; j++)
            {
                string str = $"MonsterAreaX{j}";
                var randX = PlayerPrefs.GetFloat(str);

                str = $"MonsterAreaZ{j}";
                var randZ = PlayerPrefs.GetFloat(str);

                var go = Instantiate(monsterAreaPrefab, new Vector3(randX, posY, randZ), Quaternion.identity);
                go.transform.SetParent(GameObject.Find("MonsterArea").transform);   // 부모오브젝트 설정
                //str = $"MonsterAreaScale{j}";
                //var randScale = PlayerPrefs.GetInt(str);
            }
        }
    }

    private Vector3 RandomPosOnFog()
    {
        // Fog Position을 기준으로
        Vector3 originPosition = new Vector3(fog.transform.position.x, fog.transform.position.y + 5f, fog.transform.position.z);

        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        var fogCollider = fog.GetComponent<BoxCollider>();
        float range_X = fogCollider.bounds.size.x;
        float range_Z = fogCollider.bounds.size.z;

        // 랜덤 포지션 뽑기
        //range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        //range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        float tempRange = 30.0f;
        range_X = Random.Range(((range_X / 2) * -1) + tempRange, (range_X / 2) - tempRange);
        range_Z = Random.Range(((range_Z / 2) * -1) + tempRange, (range_Z / 2) - tempRange);

        // 새로운 변수에 랜덤 포지션 담기
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        // 리턴
        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}

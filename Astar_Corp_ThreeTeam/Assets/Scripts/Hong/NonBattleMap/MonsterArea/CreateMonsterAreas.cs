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
    private float posY = 0f;   // y좌표 설정
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
    //        Gizmos.DrawSphere(go.transform.position, go.transform.GetChild(0).lossyScale.x * col.radius);
    //    }
    //}

    public void Init()
    {
        nonBattleMgr = NonBattleMgr.Instance;
        monsterAreaList.ToArray();
        labInfo = GetComponentInChildren<LaboratoryInfo>();

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
        }

        fog = GameObject.Find("Fog");

        // 처음 생성 시
        if (!PlayerPrefs.HasKey("MonsterAreaX0"))
        {
            //MonsterArea 생성
            for (int j = 0; j < monsterAreaCount; j++)
            {
                // 위치 저장
                Vector3 position;

                // 레이어
                int monsterAreaLayer = LayerMask.GetMask("MonsterArea");
                int playerLayer = LayerMask.GetMask("Player");
                //int virusZone = LayerMask.GetMask("VirusZone");

                // 원 반지름
                var bigRadius = nonBattleMgr.createLabArea.maxVirusZone[labortoryNum].GetComponent<SphereCollider>();
                var radius = monsterAreaPrefab.GetComponentInChildren<SphereCollider>();

                // 원 중심
                Vector3 bigCenter = new Vector3(bigRadius.transform.position.x, 0, bigRadius.transform.position.z);
                Vector3 smallCenter;

                // 겹치지 않을 때까지 좌표 가져오기
                do
                {
                    Vector3 pos = RandomPosOnFog();
                    position = new Vector3(pos.x, posY, pos.z);
                    smallCenter = new Vector3(position.x, 0, position.z);
                }
                while (/*(Physics.OverlapSphere(position, radius.radius * monsterAreaPrefab.transform.GetChild(0).lossyScale.x, monsterAreaLayer).Length != 0) ||*/
                       (Physics.OverlapSphere(position, radius.radius * monsterAreaPrefab.transform.GetChild(0).lossyScale.x, playerLayer).Length != 0) ||
                       Vector3.Distance(bigCenter, smallCenter) > 
                       ((bigRadius.radius * bigRadius.transform.lossyScale.x) - (radius.radius * monsterAreaPrefab.transform.GetChild(0).lossyScale.x)));

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
                //PlayerPrefs.SetInt(str, randScale);

                var go = Instantiate(monsterAreaPrefab, position, Quaternion.identity);
                go.transform.SetParent(GameObject.Find("MonsterPool").transform);
                //go.transform.localScale = new Vector3(randScale, randScale, randScale);

                // 리스트에 저장
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

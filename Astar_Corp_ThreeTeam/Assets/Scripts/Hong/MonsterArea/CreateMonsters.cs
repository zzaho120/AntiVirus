using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonsters : MonoBehaviour
{
    // ���� --> NonBattleMgr ���� �Ϸ�Ǹ� �̱������� ����� ��? �׶� Instance �ؼ� �ҷ����� �ɷ� �����ϱ�
    private NonBattleMgr nonBattleMgr;

    //���� ����.
    public GameObject monsterAreaPrefab;
    int monsterAreaCount;
    MonsterPool poolInfo;

    //private void Init()   // --> ���߿� NonBattleMgr ���� �Ϸ�Ǹ� ����
    private void Awake()
    {
        nonBattleMgr = GameObject.Find("NonBattleMgr").GetComponent<NonBattleMgr>();

        // ���� ���� �� ����
        poolInfo = GameObject.Find("MonsterPool").GetComponent<MonsterPool>();
        monsterAreaCount = poolInfo.pools.Length;

        // ó�� ���� ��
        if (!PlayerPrefs.HasKey("MonsterAreaX0"))
        {
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
                    randomIndex = UnityEngine.Random.Range(0, nonBattleMgr.virusZones1.Count);
                    var randLaboratoryPos = nonBattleMgr.virusZones1[randomIndex].transform.position;

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
        // ���� �̾ �� ��
        else
        {
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
            }
        }
    }

}

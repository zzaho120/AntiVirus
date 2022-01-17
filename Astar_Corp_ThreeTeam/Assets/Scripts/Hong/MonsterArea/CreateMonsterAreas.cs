using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonsterAreas : MonoBehaviour
{
    private NonBattleMgr nonBattleMgr;

    //���� ����.
    public GameObject monsterAreaPrefab;
    private MonsterPool poolInfo;
    private int monsterAreaCount;
    private float posY = 10f;   // y��ǥ ����

    public void Init()
    {
        nonBattleMgr = NonBattleMgr.Instance;

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
                    randomIndex = Random.Range(0, nonBattleMgr.laboratoryArea.virusZones1.Count);
                    var randLaboratoryPos = nonBattleMgr.laboratoryArea.virusZones1[randomIndex].transform.position;    // ������ ��ġ ���Ƿ� ������
                    
                    //randX = Random.Range(10, 20);
                    randX = Random.Range(40, 80);
                    var pos = Random.onUnitSphere * randX + randLaboratoryPos;
                    position = new Vector3(pos.x, posY, pos.z);
                    randScale = Random.Range(6, 10);

                    //if (randomIndex == 0)
                    //{
                    //
                    //}
                    //else if (randomIndex == 1)
                    //{
                    //
                    //}
                    //else if (randomIndex == 2)
                    //{
                    //
                    //}
                    //else
                    //{
                    //
                    //}

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

                var go = Instantiate(monsterAreaPrefab, new Vector3(randX, posY, randZ), Quaternion.identity);
                go.transform.SetParent(GameObject.Find("MonsterArea").transform);   // �θ������Ʈ ����
                str = $"MonsterAreaScale{j}";
                var randScale = PlayerPrefs.GetInt(str);
            }
        }
    }

}

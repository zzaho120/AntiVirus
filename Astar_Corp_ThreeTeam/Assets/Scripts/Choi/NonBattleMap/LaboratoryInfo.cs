using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryInfo : MonoBehaviour
{
    private NonBattleMgr nonBattleMgr;

    public bool isSpareLab;

    public float radiusZone1;
    public float radiusZone2;
    public float radiusZone3;

    //is Zone SetActive?
    public bool isActiveZone2;
    public bool isActiveZone3;

    public string virusType;
    string[] virusTypes = { "E", "B", "P", "I", "T" };
    int step;
    float timer;
    float turnTimer;
    float turnTime;

    GameObject player;
    PlayerController playerController;
    VirusData virusData;
    PlayerDataMgr playerDataMgr;

    private void Start()
    {
        nonBattleMgr = NonBattleMgr.Instance;
        //StartCoroutine(CoCheckActiveZone());
    }

    private IEnumerator CoCheckActiveZone()
    {
        yield return new WaitForSeconds(0.5f);

        // 잘 작동
        Debug.Log("isActiveZone2 : " + isActiveZone2);
        Debug.Log("isActiveZone3 : " + isActiveZone3);

        if (isActiveZone2)
        {
            // 몬스터 풀에 몬스터 영역 수 추가 (4)
            nonBattleMgr.monsterPool.pools.Add(nonBattleMgr.monsterPoolTemp.randMonsterPool);
            nonBattleMgr.monsterPool.pools.Add(nonBattleMgr.monsterPoolTemp.randMonsterPool);
        }
        else if (isActiveZone3)
        {
            // 몬스터 풀에 몬스터 영역 수 추가 (6)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timer = 0f;
            turnTimer = 0f;
            turnTime = 1f;

            Debug.Log("플레이어가 들어왔습니다.");
            Debug.Log($"바이러스 종류 : {virusType}");

            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();
            virusData = player.GetComponent<VirusData>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerDataMgr == null)
        {
            playerDataMgr = PlayerDataMgr.Instance;
        }

        // 플레이어
        if (other.gameObject.CompareTag("Player") && player != null)
        {
            var distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));

            if (distance < radiusZone3 && isActiveZone3)
            {
                timer += Time.deltaTime;
                if (timer > 1)
                {
                    timer = 0;
                    turnTimer++;
                }
                if (turnTimer > turnTime)
                {
                    turnTimer = 0;

                    foreach (var element in playerDataMgr.currentSquad)
                    {
                        if (element.Value.character.name == string.Empty) break;
                        var level = element.Value.virusPanalty[virusType].penaltyLevel;
                        element.Value.virusPanalty[virusType].Calculation(level + 1);
                    }
                }

                if (step != 3)
                {
                    virusData.None = false;
                    step = 3;
                    virusData.currentVirus[$"{virusType}"] = step;
                    virusData.Change();
                    Debug.Log("플레이어가 구역3에 들어왔습니다.");

                    timer = 0;
                    turnTimer = 0;
                }
            }
            else if (distance < radiusZone2 && isActiveZone2)
            {
                timer += Time.deltaTime;
                if (timer > 1)
                {
                    timer = 0;
                    turnTimer++;
                }
                if (turnTimer > turnTime)
                {
                    turnTimer = 0;

                    foreach (var element in playerDataMgr.currentSquad)
                    {
                        if (element.Value.character.name == string.Empty) break;
                        var level = element.Value.virusPanalty[virusType].penaltyLevel;
                        element.Value.virusPanalty[virusType].Calculation(level + 1);
                    }
                }

                if (step != 2)
                {
                    virusData.None = false;
                    step = 2;
                    virusData.currentVirus[$"{virusType}"] = step;
                    virusData.Change();
                    Debug.Log("플레이어가 구역2에 들어왔습니다.");

                    timer = 0;
                    turnTimer = 0;
                }
            }
            else if (distance < radiusZone1)
            {
                timer += Time.deltaTime;
                if (timer > 1)
                {
                    timer = 0;
                    turnTimer++;
                }
                if (turnTimer > turnTime)
                {
                    turnTimer = 0;

                    foreach (var element in playerDataMgr.currentSquad)
                    {
                        if (element.Value.character.name == string.Empty) break;
                        var level = element.Value.virusPanalty[virusType].penaltyLevel;
                        element.Value.virusPanalty[virusType].Calculation(level + 1);
                    }
                }

                if (step != 1)
                {
                    virusData.None = false;
                    step = 1;
                    virusData.currentVirus[$"{virusType}"] = step;
                    virusData.Change();
                    Debug.Log("플레이어가 구역1에 들어왔습니다.");

                    timer = 0;
                    turnTimer = 0;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player") && player != null)
        {
            Debug.Log("플레이어가 나갔습니다.");
            player = null;
            playerController = null;

            virusData.currentVirus[$"{virusType}"] = 0;
            step = 0;
            virusData.Change();

            bool isInZone = false;
            foreach (var element in virusTypes)
            {
                if (virusData.currentVirus[element] != 0)
                {
                    isInZone = true;
                    break;
                }
            }

            if (isInZone == false)
            {
                virusData.None = true;
                virusData.Init();
            }
        }
    }
}

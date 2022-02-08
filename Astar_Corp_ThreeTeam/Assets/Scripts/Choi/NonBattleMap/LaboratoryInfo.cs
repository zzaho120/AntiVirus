using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryInfo : MonoBehaviour
{
    public bool isSpareLab;

    // is Zone SetActive?
    public bool isActiveZone2;
    public bool isActiveZone3;

    // Virus Area
    public float radiusZone1;   //작은애
    public float radiusZone2;   //중간애
    public float radiusZone3;   //큰애

    // Virus Type
    public string virusType;
    public string[] virusTypes = { "E", "B", "P", "I", "T" };

    public int laboratoryNum;



    //private bool isPenaltyInit;
    //private GetVirusPenalty playerCollider;
    //
    //int step;
    //float timer;
    //float turnTimer;
    //float turnTime;
    //
    //GameObject player;
    //VirusData virusData;
    //PlayerController playerController;
    //PlayerDataMgr playerDataMgr;
/*
    public void Init()
    {
        //nonBattleMgr = NonBattleMgr.Instance;
        //scriptableMgr = ScriptableMgr.Instance;

        player = GameObject.Find("Player");
        playerCollider = player.GetComponentInChildren<GetVirusPenalty>();

    }

    private float distance;

    public void LaboratoryUpdate()
    {
        if (playerCollider.isPlayerInVirusZone)
        {
            if (!isPenaltyInit) {
                StartCoroutine(PenaltyStart());
            }

            // 플레이어, 바이러스 영역간 거리
            distance = Vector2.Distance(
                new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));

            // 최대 레벨 3
            if (isActiveZone3)
            {
                // 3단계
                if (distance < radiusZone1)
                {
                    StartCoroutine(Level3Penalty());
                }
                // 2단계
                else if (distance < radiusZone2)
                {
                    StartCoroutine(Level2Penalty());
                }
                // 1단계
                else if (distance < radiusZone3)
                {
                    StartCoroutine(Level1Penalty());
                }
            }
            // 최대 레벨 2
            else if (isActiveZone2)
            {
                // 2단계
                if (distance < radiusZone1)
                {
                    StartCoroutine(Level2Penalty());
                }
                // 1단계
                else if (distance < radiusZone2)
                {
                    StartCoroutine(Level2Penalty());
                }
            }
            // 최대 레벨 1
            else
            {
                if (distance < radiusZone1)
                {
                    StartCoroutine(Level1Penalty());
                }
            }
        }
        //else if (!playerCollider.isPlayerInVirusZone)
        if (playerCollider.isPlayerOut)
        {
            PenaltyEnd();
            isPenaltyInit = false;
        }
    }

    //private void PenaltyStart()
    private IEnumerator PenaltyStart()
    {
        yield return new WaitForSeconds(0.5f);

        timer = 0f;
        turnTimer = 0f;
        turnTime = 1f;

        Debug.Log("플레이어가 들어왔습니다.");
        Debug.Log($"바이러스 종류 : {virusType}");

        if (player != null)
            player = playerCollider.transform.parent.gameObject;

        virusData = player.GetComponent<VirusData>();

        executeOnce = false;
        isPenaltyInit = true;
    }

    private IEnumerator Level1Penalty()
    {
        yield return new WaitForSeconds(0.5f);

        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            turnTimer++;
        }
        if (turnTimer > turnTime)
        {
            turnTimer = 0;

            //foreach (var element in playerDataMgr.currentSquad)
            //{
            //    if (element.Value.character.name == string.Empty) break;
            //    var level = element.Value.virusPenalty[virusType].penaltyLevel;
            //    element.Value.virusPenalty[virusType].Calculation(level + 1);
            //}
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

    private IEnumerator Level2Penalty()
    {
        yield return new WaitForSeconds(0.5f);

        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            turnTimer++;
        }
        if (turnTimer > turnTime)
        {
            turnTimer = 0;

            //foreach (var element in playerDataMgr.currentSquad)
            //{
            //    if (element.Value.character.name == string.Empty) break;
            //    var level = element.Value.virusPenalty[virusType].penaltyLevel;
            //    element.Value.virusPenalty[virusType].Calculation(level + 1);
            //}
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

    private IEnumerator Level3Penalty()
    {
        yield return new WaitForSeconds(0.5f);

        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            turnTimer++;
        }
        if (turnTimer > turnTime)
        {
            turnTimer = 0;

            //foreach (var element in playerDataMgr.currentSquad)
            //{
            //    if (element.Value.character.name == string.Empty) break;
            //    var level = element.Value.virusPenalty[virusType].penaltyLevel;
            //    element.Value.virusPenalty[virusType].Calculation(level + 1);
            //}
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

    private bool executeOnce;

    private IEnumerator PenaltyEnd()
    {
        yield return new WaitForSeconds(0.5f);

        //if (!executeOnce)
        //{
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

            executeOnce = true;
        //}
    }
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVirusPenalty : MonoBehaviour
{
    private NonBattleMgr nonBattleMgr;
    private PlayerDataMgr playerDataMgr;

    private LaboratoryInfo labInfo;
    [SerializeField]
    private SquadStatusMgr squadStatus;

    private bool isPlayerIn;
    private bool isPlayerOut;
    
    private bool isPenaltyInit;
    private bool executeOnce;

    private float distance;

    private GameObject player;
    private GameObject virusZone;

    int step;
    float timer;
    float turnTimer;
    float turnTime;

    VirusData virusData;

    private void Start()
    {
        nonBattleMgr = NonBattleMgr.Instance;
        playerDataMgr = PlayerDataMgr.Instance;
        player = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 바이러스 영역 안에 들어오는지 확인
        // 어떤 원이든 안에 있으면 됨
        if (other.CompareTag("VirusZonePhase3") || other.CompareTag("VirusZonePhase2") || other.gameObject.CompareTag("VirusZonePhase1"))
        {
            virusZone = other.gameObject;
            labInfo = other.GetComponentInParent<LaboratoryInfo>();

            isPlayerIn = true;
            isPlayerOut = false;

            PenaltyStart();
            //Debug.Log(other.tag + " In");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 활성화된 바이러스 영역 중에서 제일 큰 원을 빠져나오는지 확인
        foreach (var element in nonBattleMgr.createLabArea.laboratoryObjs)
        {
            if (other.gameObject == nonBattleMgr.createLabArea.maxVirusZone[element.GetComponent<LaboratoryInfo>().laboratoryNum])
            {
                isPlayerIn = false;
                isPlayerOut = true;

                PenaltyEnd();
                StartCoroutine(VirusZoneNull());
                //Debug.Log(other.tag + " Out");
            }
        }
    }

    private IEnumerator VirusZoneNull()
    {
        yield return new WaitForSeconds(0.2f);
        virusZone = null;
    }

    public void VirusUpdate()
    {
        if (isPlayerIn)
        {
            // 플레이어, 바이러스 영역간 거리
            distance = Vector2.Distance(new Vector2(virusZone.transform.position.x, virusZone.transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));
            //distance = 5;

            // 최대 레벨 3
            if (labInfo.isActiveZone3)
            {
                // 3단계
                if (distance < labInfo.radiusZone1)
                {
                    Level3Penalty();
                }
                // 2단계
                else if (distance < labInfo.radiusZone2)
                {
                    Level2Penalty();
                }
                // 1단계
                else if (distance < labInfo.radiusZone3)
                {
                    Level1Penalty();
                }
            }
            // 최대 레벨 2
            else if (labInfo.isActiveZone2)
            {
                // 2단계
                if (distance < labInfo.radiusZone1)
                {
                    Level2Penalty();
                }
                // 1단계
                else if (distance < labInfo.radiusZone2)
                {
                    Level1Penalty();
                }
            }
            // 최대 레벨 1
            else
            {
                if (distance < labInfo.radiusZone1)
                {
                    Level1Penalty();
                }
            }

            //squadStatus.Init();
        }
    }

    private void PenaltyStart()
    {
        timer = 0f;
        turnTimer = 0f;
        turnTime = 1f;

        Debug.Log("플레이어가 들어왔습니다.");
        Debug.Log($"바이러스 종류 : {labInfo.virusType}");

        if (player == null)
            player = transform.parent.gameObject;

        virusData = player.GetComponent<VirusData>();

        executeOnce = false;
        isPenaltyInit = true;
    }

    private void Level1Penalty()
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

            foreach (var element in playerDataMgr.boardingSquad)
            {
                if (playerDataMgr.currentSquad[element.Value].character.name == string.Empty) break;
                var level = playerDataMgr.currentSquad[element.Value].virusPenalty[labInfo.virusType].penaltyLevel;
                playerDataMgr.currentSquad[element.Value].virusPenalty[labInfo.virusType].Calculation(level + 1);

                //var level = element.Value.virusPenalty[labInfo.virusType].penaltyLevel;
                //element.Value.virusPenalty[labInfo.virusType].Calculation(level + 1);
            }
        }

        if (step != 1)
        {
            virusData.None = false;
            step = 1;
            virusData.currentVirus[$"{labInfo.virusType}"] = step;
            virusData.Change();
            Debug.Log("플레이어가 구역1에 들어왔습니다.");

            timer = 0;
            turnTimer = 0;
        }
    }

    private void Level2Penalty()
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

            //foreach (var element in playerDataMgr.currentSquad)
            foreach (var element in playerDataMgr.boardingSquad)
            {
                if (playerDataMgr.currentSquad[element.Value].character.name == string.Empty) break;
                var level = playerDataMgr.currentSquad[element.Value].virusPenalty[labInfo.virusType].penaltyLevel;
                playerDataMgr.currentSquad[element.Value].virusPenalty[labInfo.virusType].Calculation(level + 1);

                //if (element.Value.character.name == string.Empty) break;
                //var level = element.Value.virusPenalty[labInfo.virusType].penaltyLevel;
                //element.Value.virusPenalty[labInfo.virusType].Calculation(level + 1);
            }
        }

        if (step != 2)
        {
            virusData.None = false;
            step = 2;
            virusData.currentVirus[$"{labInfo.virusType}"] = step;
            virusData.Change();
            Debug.Log("플레이어가 구역2에 들어왔습니다.");

            timer = 0;
            turnTimer = 0;
        }
    }

    private void Level3Penalty()
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

            //foreach (var element in playerDataMgr.currentSquad)
            foreach (var element in playerDataMgr.boardingSquad)
            {
                if (playerDataMgr.currentSquad[element.Value].character.name == string.Empty) break;
                var level = playerDataMgr.currentSquad[element.Value].virusPenalty[labInfo.virusType].penaltyLevel;
                playerDataMgr.currentSquad[element.Value].virusPenalty[labInfo.virusType].Calculation(level + 1);

                //if (element.Value.character.name == string.Empty) break;
                //var level = element.Value.virusPenalty[labInfo.virusType].penaltyLevel;
                //element.Value.virusPenalty[labInfo.virusType].Calculation(level + 1);
            }
        }

        if (step != 3)
        {
            virusData.None = false;
            step = 3;
            virusData.currentVirus[$"{labInfo.virusType}"] = step;
            virusData.Change();
            Debug.Log("플레이어가 구역3에 들어왔습니다.");

            timer = 0;
            turnTimer = 0;
        }
    }


    private void PenaltyEnd()
    {
        if (!executeOnce)
        {
            Debug.Log("플레이어가 나갔습니다.");
            player = null;

            virusData.currentVirus[$"{labInfo.virusType}"] = 0;
            step = 0;
            virusData.Change();

            bool isInZone = false;
            foreach (var element in labInfo.virusTypes)
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
        }
    }
}

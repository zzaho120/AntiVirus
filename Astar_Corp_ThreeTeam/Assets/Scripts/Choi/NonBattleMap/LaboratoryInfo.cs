using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryInfo : MonoBehaviour
{
    private NonBattleMgr nonBattleMgr;
    private ScriptableMgr scriptableMgr;

    public bool isSpareLab;

    public float radiusZone1;   //������
    public float radiusZone2;   //�߰���
    public float radiusZone3;   //ū��

    //is Zone SetActive?
    public bool isActiveZone2;
    public bool isActiveZone3;

    public int laboratoryNum;
    private bool isLevel1, isLevel2, isLevel3;
    private bool isPlayerIn;
    private string savePlayer;

    public string virusType;    // ���̷��� ����(Ÿ��) Ȯ��
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
        scriptableMgr = ScriptableMgr.Instance;

        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player In");

            timer = 0f;
            turnTimer = 0f;
            turnTime = 1f;

            Debug.Log("�÷��̾ ���Խ��ϴ�.");
            Debug.Log($"���̷��� ���� : {virusType}");

            player = other.gameObject;
            playerController = player.GetComponent<PlayerController>();
            virusData = player.GetComponent<VirusData>();
        }
    }

    private void Update()
    {

        // bool������
        // �÷��̾ ���� �ȿ� ������ �˻��ϵ��� �ٲ㺸�� (isPlayerIn)

        // �÷��̾�, ���̷��� ������ �Ÿ�
        float distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));

        // �ִ� ���� 3
        if (isActiveZone3)
        {
            // 3�ܰ�
            if (distance < radiusZone1)
            {
                //Debug.Log("3�ܰ�");
                Level3Penalty();
            }
            // 2�ܰ�
            else if (distance < radiusZone2)
            {
                //Debug.Log("2�ܰ�");
                //isLevel2 = true;
                Level2Penalty();
            }
            // 1�ܰ�
            else if (distance < radiusZone3)
            {
                //Debug.Log("1�ܰ�");
                if (!isPlayerIn)
                    VirusPenalty();

                Level1Penalty();
            }
        }
        // �ִ� ���� 2
        else if (isActiveZone2)
        {
            // 2�ܰ�
            if (distance < radiusZone1)
            {
                //Debug.Log("2�ܰ�");
                Level2Penalty();
            }
            // 1�ܰ�
            else if (distance < radiusZone2)
            {
                //Debug.Log("1�ܰ�");
                if (!isPlayerIn)
                    VirusPenalty();

                Level1Penalty();
            }
        }
        // �ִ� ���� 1
        else
        {
            if (distance < radiusZone1)
            {
                //Debug.Log("1�ܰ�");
                if (!isPlayerIn)
                    VirusPenalty();

                Level1Penalty();
            }
        }
    }

    private void VirusPenalty()
    {
        Debug.Log("Penalty Init");

        timer = 0f;
        turnTimer = 0f;
        turnTime = 1f;

        Debug.Log("�÷��̾ ���Խ��ϴ�.");
        Debug.Log($"���̷��� ���� : {virusType}");

        playerController = player.GetComponent<PlayerController>();
        virusData = player.GetComponent<VirusData>();

        isPlayerIn = true;
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
            Debug.Log("�÷��̾ ����1�� ���Խ��ϴ�.");

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
            Debug.Log("�÷��̾ ����2�� ���Խ��ϴ�.");

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
            Debug.Log("�÷��̾ ����3�� ���Խ��ϴ�.");

            timer = 0;
            turnTimer = 0;
        }
    }



    //private void OnTriggerStay(Collider other)
    //{
    //    if (playerDataMgr == null)
    //    {
    //        playerDataMgr = PlayerDataMgr.Instance;
    //    }
    //
    //    // �÷��̾�
    //    if (other.gameObject.CompareTag("Player") && player != null)
    //    {
    //        //// 1. Zone 1 Ȱ��ȭ (Level1)
    //        //if (distance < radiusZone1)
    //        //{
    //        //    timer += Time.deltaTime;
    //        //    if (timer > 1)
    //        //    {
    //        //        timer = 0;
    //        //        turnTimer++;
    //        //    }
    //        //    if (turnTimer > turnTime)
    //        //    {
    //        //        turnTimer = 0;
    //        //
    //        //        foreach (var element in playerDataMgr.currentSquad)
    //        //        {
    //        //            if (element.Value.character.name == string.Empty) break;
    //        //            var level = element.Value.virusPenalty[virusType].penaltyLevel;
    //        //            element.Value.virusPenalty[virusType].Calculation(level + 1);
    //        //        }
    //        //    }
    //        //
    //        //    if (step != 1)
    //        //    {
    //        //        virusData.None = false;
    //        //        step = 1;
    //        //        virusData.currentVirus[$"{virusType}"] = step;
    //        //        virusData.Change();
    //        //        Debug.Log("�÷��̾ ����1�� ���Խ��ϴ�.");
    //        //
    //        //        timer = 0;
    //        //        turnTimer = 0;
    //        //    }
    //        //}
    //        //// 2. Zone 2 Ȱ��ȭ (Level2)
    //        //else if (distance < radiusZone2 && isActiveZone2)
    //        //{
    //        //    Debug.Log("Zone2");
    //        //
    //        //    timer += Time.deltaTime;
    //        //    if (timer > 1)
    //        //    {
    //        //        timer = 0;
    //        //        turnTimer++;
    //        //    }
    //        //    if (turnTimer > turnTime)
    //        //    {
    //        //        turnTimer = 0;
    //        //
    //        //        foreach (var element in playerDataMgr.currentSquad)
    //        //        {
    //        //            if (element.Value.character.name == string.Empty) break;
    //        //            var level = element.Value.virusPenalty[virusType].penaltyLevel;
    //        //            element.Value.virusPenalty[virusType].Calculation(level + 1);
    //        //        }
    //        //    }
    //        //
    //        //    if (step != 2)
    //        //    {
    //        //        virusData.None = false;
    //        //        step = 2;
    //        //        virusData.currentVirus[$"{virusType}"] = step;
    //        //        virusData.Change();
    //        //        Debug.Log("�÷��̾ ����2�� ���Խ��ϴ�.");
    //        //
    //        //        timer = 0;
    //        //        turnTimer = 0;
    //        //    }
    //        //}
    //        //else if (distance < radiusZone3 && isActiveZone1)
    //        //{
    //        //    timer += Time.deltaTime;
    //        //    if (timer > 1)
    //        //    {
    //        //        timer = 0;
    //        //        turnTimer++;
    //        //    }
    //        //    if (turnTimer > turnTime)
    //        //    {
    //        //        turnTimer = 0;
    //        //
    //        //        foreach (var element in playerDataMgr.currentSquad)
    //        //        {
    //        //            if (element.Value.character.name == string.Empty) break;
    //        //            var level = element.Value.virusPenalty[virusType].penaltyLevel;
    //        //            element.Value.virusPenalty[virusType].Calculation(level + 1);
    //        //        }
    //        //    }
    //        //
    //        //    if (step != 1)
    //        //    {
    //        //        virusData.None = false;
    //        //        step = 1;
    //        //        virusData.currentVirus[$"{virusType}"] = step;
    //        //        virusData.Change();
    //        //        Debug.Log("�÷��̾ ����1�� ���Խ��ϴ�.");
    //        //
    //        //        timer = 0;
    //        //        turnTimer = 0;
    //        //    }
    //        //}
    //        //Debug.Log(distance < radiusZone2);
    //        //Debug.Log(distance < radiusZone3);
    //
    //
    //        ///// ����
    //        //if (distance < radiusZone3 && isActiveZone1)
    //        //{
    //        //    timer += Time.deltaTime;
    //        //    if (timer > 1)
    //        //    {
    //        //        timer = 0;
    //        //        turnTimer++;
    //        //    }
    //        //    if (turnTimer > turnTime)
    //        //    {
    //        //        turnTimer = 0;
    //        //
    //        //        foreach (var element in playerDataMgr.currentSquad)
    //        //        {
    //        //            if (element.Value.character.name == string.Empty) break;
    //        //            var level = element.Value.virusPenalty[virusType].penaltyLevel;
    //        //            element.Value.virusPenalty[virusType].Calculation(level + 1);
    //        //        }
    //        //    }
    //        //
    //        //    if (step != 3)
    //        //    {
    //        //        virusData.None = false;
    //        //        step = 3;
    //        //        virusData.currentVirus[$"{virusType}"] = step;
    //        //        virusData.Change();
    //        //        Debug.Log("�÷��̾ ����3�� ���Խ��ϴ�.");
    //        //
    //        //        timer = 0;
    //        //        turnTimer = 0;
    //        //    }
    //        //}
    //        //else if (distance < radiusZone2 && isActiveZone2)
    //        //{
    //        //    timer += Time.deltaTime;
    //        //    if (timer > 1)
    //        //    {
    //        //        timer = 0;
    //        //        turnTimer++;
    //        //    }
    //        //    if (turnTimer > turnTime)
    //        //    {
    //        //        turnTimer = 0;
    //        //
    //        //        foreach (var element in playerDataMgr.currentSquad)
    //        //        {
    //        //            if (element.Value.character.name == string.Empty) break;
    //        //            var level = element.Value.virusPenalty[virusType].penaltyLevel;
    //        //            element.Value.virusPenalty[virusType].Calculation(level + 1);
    //        //        }
    //        //    }
    //        //
    //        //    if (step != 2)
    //        //    {
    //        //        virusData.None = false;
    //        //        step = 2;
    //        //        virusData.currentVirus[$"{virusType}"] = step;
    //        //        virusData.Change();
    //        //        Debug.Log("�÷��̾ ����2�� ���Խ��ϴ�.");
    //        //
    //        //        timer = 0;
    //        //        turnTimer = 0;
    //        //    }
    //        //}
    //        //else if (distance < radiusZone1)
    //        //{
    //        //    timer += Time.deltaTime;
    //        //    if (timer > 1)
    //        //    {
    //        //        timer = 0;
    //        //        turnTimer++;
    //        //    }
    //        //    if (turnTimer > turnTime)
    //        //    {
    //        //        turnTimer = 0;
    //        //
    //        //        foreach (var element in playerDataMgr.currentSquad)
    //        //        {
    //        //            if (element.Value.character.name == string.Empty) break;
    //        //            var level = element.Value.virusPenalty[virusType].penaltyLevel;
    //        //            element.Value.virusPenalty[virusType].Calculation(level + 1);
    //        //        }
    //        //    }
    //        //
    //        //    if (step != 1)
    //        //    {
    //        //        virusData.None = false;
    //        //        step = 1;
    //        //        virusData.currentVirus[$"{virusType}"] = step;
    //        //        virusData.Change();
    //        //        Debug.Log("�÷��̾ ����1�� ���Խ��ϴ�.");
    //        //
    //        //        timer = 0;
    //        //        turnTimer = 0;
    //        //    }
    //        //}
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player") && player != null)
        {
            Debug.Log("�÷��̾ �������ϴ�.");
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

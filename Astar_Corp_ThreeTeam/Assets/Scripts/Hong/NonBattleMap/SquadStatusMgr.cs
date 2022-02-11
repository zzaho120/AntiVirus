using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadStatusMgr : MonoBehaviour
{
    private PlayerDataMgr playerDataMgr;
    private ScriptableMgr soMgr;
    private TimeController timeController;

    public GameObject characterPrefab;
    public GameObject infectedCharContent;

    public Sprite[] hpImgs;

    // 캐릭터 오브젝트 저장용
    private Dictionary<int, GameObject> charGoList = new Dictionary<int, GameObject>();
    private Dictionary<int, int> charKeyList = new Dictionary<int, int>();

    void Start()
    {
        playerDataMgr = PlayerDataMgr.Instance;
        soMgr = ScriptableMgr.Instance;

        // 임시 Sqaud 데이타 넣기
        //===================================================
        CharacterStats temp1 = new CharacterStats();
        temp1.character = soMgr.GetCharacter("CHAR_0001");
        CharacterStats temp2 = new CharacterStats();
        temp2.character = soMgr.GetCharacter("CHAR_0003");

        if (!playerDataMgr.currentSquad.ContainsKey(1))
            playerDataMgr.currentSquad.Add(1, temp1);

        if (!playerDataMgr.currentSquad.ContainsKey(2))
            playerDataMgr.currentSquad.Add(2, temp2);

        playerDataMgr.currentSquad[1].Init();
        playerDataMgr.currentSquad[1].level = 3;
        playerDataMgr.currentSquad[2].Init();
        playerDataMgr.currentSquad[2].level = 5;
        playerDataMgr.currentSquad[2].currentHp = 10;

        if (!playerDataMgr.boardingSquad.ContainsKey(0))
            playerDataMgr.boardingSquad.Add(0, 1);

        if (!playerDataMgr.boardingSquad.ContainsKey(1))
            playerDataMgr.boardingSquad.Add(1, 2);
        //===================================================

        Init();
    }

    private void Update()
    {
        if (charGoList.Count != 0)
        {
            foreach (var character in charGoList)
            {
                Destroy(character.Value);
            }
            charGoList.Clear();
            //characterListContent.transform.DetachChildren();
        }
        if (charKeyList.Count != 0) charKeyList.Clear();

        Init();
    }

    public void Init()
    {
        int i = 0;
        foreach (var element in playerDataMgr.boardingSquad)
        {
            var character = Instantiate(characterPrefab, infectedCharContent.transform);

            // 캐릭터 초상화
            Image charImg = character.transform.GetChild(0).GetComponent<Image>();
            charImg.sprite = playerDataMgr.currentSquad[element.Value].character.halfImg;

            // 캐릭터 상태들
            GameObject states = character.transform.GetChild(1).gameObject;

            // 0 풀피
            // 1 반피
            // 2 딸피

            // Hp
            var hpImg = states.transform.GetChild(0).GetComponent<Image>();
            var hpRate = playerDataMgr.currentSquad[element.Value].currentHp / playerDataMgr.currentSquad[element.Value].MaxHp;
            if (hpRate >= 0.5)
                hpImg.sprite = hpImgs[0];
            else if (hpRate >= 0.2)
                hpImg.sprite = hpImgs[1];
            else
                hpImg.sprite = hpImgs[2];

            // Virus
            //var virusGroup = child.transform.GetChild(2).gameObject;
            string[] virusName = new string[5];
            virusName[0] = "E";
            virusName[1] = "B";
            virusName[2] = "P";
            virusName[3] = "I";
            virusName[4] = "T";
            for (int j = 1; j < virusName.Length + 1; j++)
            {
                if (playerDataMgr.currentSquad[element.Value].virusPenalty[virusName[j - 1]].penaltyLevel >= 1)
                    states.transform.GetChild(j).gameObject.SetActive(true);
                else states.transform.GetChild(j).gameObject.SetActive(false);
            }

            int num = i;
            charGoList.Add(element.Value, character); // 오브젝트 추가
            charKeyList.Add(num, element.Key);      // 캐릭터 키 추가
            i++;
        }
    }
}

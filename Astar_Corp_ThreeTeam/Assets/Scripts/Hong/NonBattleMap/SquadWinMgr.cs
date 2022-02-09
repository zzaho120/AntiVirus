using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadWinMgr : UIManagerWindowManager
{
    private PlayerDataMgr playerDataMgr;
    private ScriptableMgr soMgr;

    public GameObject statePrefab;
    public GameObject characterListContent;

    // 선택된 캐릭터 개수
    private Text memberNumTxt;

    // 캐릭터 오브젝트 저장용
    private Dictionary<int, GameObject> characterList = new Dictionary<int, GameObject>();
    // 캐릭터 키 저장용
    private Dictionary<int, int> charKeyList = new Dictionary<int, int>();

    public override void Init()
    {
        Debug.Log("Init");

        playerDataMgr = PlayerDataMgr.Instance;
        soMgr = ScriptableMgr.Instance;

        memberNumTxt = GameObject.Find("CharNumTxt").GetComponent<Text>();

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

        if (!playerDataMgr.boardingSquad.ContainsKey(0))
            playerDataMgr.boardingSquad.Add(0, 1);

        if (!playerDataMgr.boardingSquad.ContainsKey(1))
            playerDataMgr.boardingSquad.Add(1, 2);
        //===================================================

        PrintCharacterList();
    }

    private void PrintCharacterList()
    {
        //Debug.Log(playerDataMgr.boardingSquad.Count);

        //이전 정보 삭제.
        if (characterList.Count != 0)
        {
            foreach (var character in characterList)
            {
                Destroy(character.Value);
            }
            characterList.Clear();
            //characterListContent.transform.DetachChildren();
        }
        if (charKeyList.Count != 0) charKeyList.Clear();

        //생성.
        int i = 0;
        foreach (var element in playerDataMgr.boardingSquad)
        {
            //Debug.Log(playerDataMgr.currentSquad[element.Value].Name);

            var character = Instantiate(statePrefab, characterListContent.transform);

            // 1. Character Contents (레벨)
            var child = character.transform.GetChild(1).gameObject;
            child.GetComponentInChildren<Text>().text = $"LV{playerDataMgr.currentSquad[element.Value].level}";
            
            // 2. Division Contents (분과)
            child = character.transform.GetChild(2).gameObject;
            child.GetComponentInChildren<Text>().text = playerDataMgr.currentSquad[element.Value].character.name;
            
            // 3. Name Contents (이름)
            child = character.transform.GetChild(3).gameObject;
            child.GetComponentInChildren<Text>().text = playerDataMgr.currentSquad[element.Value].Name;

            // 4. State Contents (상태)
            child = character.transform.GetChild(4).gameObject;
            // 4-1. Hp 슬라이더
            var slider = child.GetComponentInChildren<Slider>();
            slider.maxValue = playerDataMgr.currentSquad[element.Value].MaxHp;
            slider.value = playerDataMgr.currentSquad[element.Value].currentHp;
            // 4-2. Hp 표시
            child.GetComponentInChildren<Text>().text 
                = $"{playerDataMgr.currentSquad[element.Value].currentHp}/{playerDataMgr.currentSquad[element.Value].MaxHp}";
            // 4-3. Virus
            var virusGroup = child.transform.GetChild(2).gameObject;
            string[] virusName = new string[5];
            virusName[0] = "E";
            virusName[1] = "B";
            virusName[2] = "P";
            virusName[3] = "I";
            virusName[4] = "T";
            for (int j = 0; j < virusName.Length; j++)
            {
                if (playerDataMgr.currentSquad[element.Value].virusPenalty[virusName[j]].penaltyLevel >= 1)
                    virusGroup.transform.GetChild(j).gameObject.SetActive(true);
                else virusGroup.transform.GetChild(j).gameObject.SetActive(false);
            }
            
            // 5. Tolerance Contents (내성)
            child = character.transform.GetChild(5).gameObject;
            var toleranceGroup = child.transform.GetChild(0).gameObject;
            for (int j = 0; j < virusName.Length; j++)
            {
                child = toleranceGroup.transform.GetChild(j).gameObject;
                child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                    = $"Lv {playerDataMgr.currentSquad[element.Value].virusPenalty[virusName[j]].reductionLevel}";
            }
            
            int num = i;
            var button = character.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(element.Value); });

            characterList.Add(element.Value, character);    // 캐릭터 오브젝트 추가
            charKeyList.Add(num, element.Key);  // 캐릭터 키 추가

            i++;
        }
    }

    // 사용할 컬러 값 저장
    Color orginColor = new Color32(95, 100, 110, 91);
    Color selectedColor = new Color32(43, 76, 135, 91);

    private void SelectCharacter(int num)
    {
        // 선택된 오브젝트 가져오기
        var selectedObj = characterList[num];
        Toggle toggle = selectedObj.GetComponentInChildren<Toggle>();
        Image image = selectedObj.transform.GetComponent<Image>();



        // 중복 선택 시
        if (playerDataMgr.battleSquad.ContainsKey(num))
        {
            // 삭제
            Debug.Log(playerDataMgr.battleSquad[num].Name + " Removed");
            playerDataMgr.battleSquad.Remove(num);

            // 토글키 및 색상 값 변경
            toggle.isOn = false;
            image.color = orginColor;

            // 전투 참여 인원 표시
            memberNumTxt.text = $"전투 참여 {playerDataMgr.battleSquad.Count} / 4";
            return;
        }

        // 가득 찼을 때
        if (playerDataMgr.battleSquad.Count >= 4) return;

        // Truck Squad -> Battle Squad
        // 추가
        playerDataMgr.battleSquad.Add(num, playerDataMgr.currentSquad[num]);
        Debug.Log(playerDataMgr.battleSquad[num].Name + " Added");

        // 토글키 및 색상 값 변경
        toggle.isOn = true;
        image.color = selectedColor;

        // 전투 참여 인원 표시
        memberNumTxt.text = $"전투 참여 {playerDataMgr.battleSquad.Count} / 4";
    }

    public override void Open()
    {
        // 창 열기 + 초기화
        base.Open();
        Init();
    }

    public override void Close()
    {
        base.Close();
    }

    public void InactiveButton(Button button)
    {
        button.interactable = false;
    }
}

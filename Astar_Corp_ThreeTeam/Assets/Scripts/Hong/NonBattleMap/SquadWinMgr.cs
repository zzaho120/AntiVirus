using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SquadWinMgr : UIManagerWindowManager
{
    private PlayerDataMgr playerDataMgr;
    private ScriptableMgr soMgr;

    public GameObject statePrefab;
    public GameObject characterListContent;

    private Text memberNumTxt;

    private Dictionary<int, GameObject> characterList = new Dictionary<int, GameObject>();
    private Dictionary<int, int> charKeyList = new Dictionary<int, int>();

    public override void Init()
    {
        Debug.Log("Init");

        playerDataMgr = PlayerDataMgr.Instance;
        soMgr = ScriptableMgr.Instance;

        memberNumTxt = GameObject.Find("CharNumTxt").GetComponent<Text>();

        // �ӽ� Sqaud ����Ÿ �ֱ�
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

        playerDataMgr.boardingSquad.Add(0, 1);
        playerDataMgr.boardingSquad.Add(1, 2);

        PrintCharacterList();
    }

    private void PrintCharacterList()
    {
        Debug.Log(playerDataMgr.boardingSquad.Count);

        //���� ���� ����.
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

        //����.
        int i = 0;
        foreach (var element in playerDataMgr.boardingSquad)
        {
            //Debug.Log(playerDataMgr.currentSquad[element.Value].Name);

            var character = Instantiate(statePrefab, characterListContent.transform);

            // 1. Character Contents (����)
            var child = character.transform.GetChild(1).gameObject;
            child.GetComponentInChildren<Text>().text = $"LV{playerDataMgr.currentSquad[element.Value].level}";
            
            // 2. Division Contents (�а�)
            child = character.transform.GetChild(2).gameObject;
            child.GetComponentInChildren<Text>().text = playerDataMgr.currentSquad[element.Value].character.name;
            
            // 3. Name Contents (�̸�)
            child = character.transform.GetChild(3).gameObject;
            child.GetComponentInChildren<Text>().text = playerDataMgr.currentSquad[element.Value].Name;

            // 4. State Contents (����)
            child = character.transform.GetChild(4).gameObject;
            // 4-1. Hp �����̴�
            var slider = child.GetComponentInChildren<Slider>();
            slider.maxValue = playerDataMgr.currentSquad[element.Value].MaxHp;
            slider.value = playerDataMgr.currentSquad[element.Value].currentHp;
            // 4-2. Hp ǥ��
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
            
            // 5. Tolerance Contents (����)
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
            characterList.Add(element.Value, character);
            
            charKeyList.Add(num, element.Key);

            i++;
        }
    }



    private void SelectCharacter(int num)
    {
        // Key�� ã��
        //var key = charKeyList.FirstOrDefault(x => x.Value == num).Key;
        var selectedObj = characterList[num];
        var toggle = selectedObj.GetComponentInChildren<Toggle>();
        var image = selectedObj.transform.GetComponent<Image>();

        Color orginColor = new Color32(95, 100, 110, 91);
        Color selectedColor = new Color32(43, 76, 135, 91);

        // �ߺ� ���� ��
        if (playerDataMgr.battleSquad.ContainsKey(num))
        {
            Debug.Log(playerDataMgr.battleSquad[num].Name + " Removed");
            playerDataMgr.battleSquad.Remove(num);
            toggle.isOn = false;

            // �� ����
            image.color = orginColor;
            memberNumTxt.text = $"���� ���� {playerDataMgr.battleSquad.Count} / 4";
            return;
        }

        // ���� á�� ��
        if (playerDataMgr.battleSquad.Count >= 4) return;

        // Truck Squad -> Battle Squad
        playerDataMgr.battleSquad.Add(num, playerDataMgr.currentSquad[num]);
        Debug.Log(playerDataMgr.battleSquad[num].Name + " Added");
        toggle.isOn = true;
        //  �� ����
        image.color = selectedColor;

        // ���� ���� �ο�
        memberNumTxt.text = $"���� ���� {playerDataMgr.battleSquad.Count} / 4";
    }

    public override void Open()
    {
        // â ���� + �ʱ�ȭ
        base.Open();
        Init();
    }

    public override void Close()
    {
        base.Close();
    }
}
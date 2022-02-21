using Michsky.UI.ModernUIPack;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadWinMgr : UIManagerWindowManager
{
    private PlayerDataMgr playerDataMgr;
    private ScriptableMgr soMgr;
    public TimeController timeController;
    public bool isTimerOff;

    public TrunkWinMgr trunkWinMgr;

    public GameObject statePrefab;
    public GameObject equipPrefab;
    public GameObject skillPrefab;
    public GameObject characterListContent;

    // ���õ� ĳ���� ����
    public Text memberNumTxt;

    // ĳ���� ������Ʈ �����
    private Dictionary<int, GameObject> characterList = new Dictionary<int, GameObject>();
    // ĳ���� Ű �����
    private Dictionary<int, int> charKeyList = new Dictionary<int, int>();

    // ���� ���� ��
    public bool isSelectPossible;
    public GameObject[] battleButtons;


    public void Init()
    {
        //Debug.Log("Init");

        playerDataMgr = PlayerDataMgr.Instance;
        soMgr = ScriptableMgr.Instance;

        timeController.Pause();

        var button = GetComponentInChildren<Button>();
        button.Select();
        PrintCharacterList();
    }

    public void SelectMenu(int index)
    {
        //Debug.Log(index);

        if (index == 0)
            PrintCharacterList();
        if (index == 1)
            PrintEquipList();
    }

    private void PrintEquipList()
    {
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
            var charEquip = Instantiate(equipPrefab, characterListContent.transform);

            var child = charEquip.transform.GetChild(0).gameObject;
            child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                 = $"LV{playerDataMgr.currentSquad[element.Value].level}";

            var expSlider = child.transform.GetChild(3).gameObject.GetComponent<Slider>();
            expSlider.maxValue = playerDataMgr.currentSquad[element.Value].totalExp;
            expSlider.value = playerDataMgr.currentSquad[element.Value].currentExp;

            child = charEquip.transform.GetChild(1).gameObject;
            child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                = playerDataMgr.currentSquad[element.Value].character.icon;
            child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                 = playerDataMgr.currentSquad[element.Value].character.name;

            child = charEquip.transform.GetChild(2).gameObject;
            child.transform.GetChild(0).gameObject.GetComponent<Text>().text
                 = playerDataMgr.currentSquad[element.Value].characterName;

            child = charEquip.transform.GetChild(3).gameObject;
            child = child.transform.GetChild(0).gameObject;
            var childObj = child.transform.GetChild(0).gameObject;
            var color = childObj.GetComponent<Image>().color;
            if (playerDataMgr.currentSquad[element.Value].weapon.mainWeapon != null)
            {
                childObj.GetComponent<Image>().color
                    = new Color(color.r, color.g, color.b, 1);
                childObj.GetComponent<Image>().sprite
                    = playerDataMgr.currentSquad[element.Value].weapon.mainWeapon.img;
                childObj = child.transform.GetChild(1).gameObject;
                childObj.SetActive(true);
                childObj.transform.GetChild(0).GetComponent<Text>().text
                    = $"{GetTypeStr(playerDataMgr.currentSquad[element.Value].weapon.mainWeapon.kind)}";
            }
            else
            {
                childObj.GetComponent<Image>().color
                  = new Color(color.r, color.g, color.b, 0);
                childObj = child.transform.GetChild(1).gameObject;
                childObj.SetActive(false);
            }

            child = charEquip.transform.GetChild(4).gameObject;
            child = child.transform.GetChild(0).gameObject;
            childObj = child.transform.GetChild(0).gameObject;
            color = childObj.GetComponent<Image>().color;
            if (playerDataMgr.currentSquad[element.Value].weapon.subWeapon != null)
            {
                childObj.GetComponent<Image>().color
                    = new Color(color.r, color.g, color.b, 1);
                childObj.GetComponent<Image>().sprite
                    = playerDataMgr.currentSquad[element.Value].weapon.subWeapon.img;
                childObj = child.transform.GetChild(1).gameObject;
                childObj.SetActive(true);
                childObj.transform.GetChild(0).GetComponent<Text>().text
                    = $"{GetTypeStr(playerDataMgr.currentSquad[element.Value].weapon.subWeapon.kind)}";
            }
            else
            {
                childObj.GetComponent<Image>().color
                  = new Color(color.r, color.g, color.b, 0);
                childObj = child.transform.GetChild(1).gameObject;
                childObj.SetActive(false);
            }

            int num = i;
            //var button = go.AddComponent<Button>();
            //button.onClick.AddListener(delegate { SelectCharacter(num); });

            characterList.Add(element.Value, charEquip);
            charKeyList.Add(num, element.Key);

            i++;
        }
    }


    private void PrintCharacterList()
    {
        //Debug.Log(playerDataMgr.boardingSquad.Count);

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
            child.GetComponentInChildren<Image>().sprite = playerDataMgr.currentSquad[element.Value].character.icon;
            child.GetComponentInChildren<Text>().text = playerDataMgr.currentSquad[element.Value].character.name;
            
            // 3. Name Contents (�̸�)
            child = character.transform.GetChild(3).gameObject;
            child.GetComponentInChildren<Text>().text = playerDataMgr.currentSquad[element.Value].characterName;

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
                    = $"Lv {playerDataMgr.currentSquad[element.Value].virusPenalty[virusName[j]].resistLevel}";
            }

            int num = i;
            characterList.Add(element.Value, character);    // ĳ���� ������Ʈ �߰�
            charKeyList.Add(num, element.Key);  // ĳ���� Ű �߰�

            if (isSelectPossible)
            {
                var button = character.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectCharacter(element.Value); });

                //character.transform.GetChild(0).GetComponentInChildren<Toggle>().interactable = true;

                foreach (var go in battleButtons)
                    go.SetActive(true);
            }
            else
            {
                //character.transform.GetChild(0).GetComponentInChildren<Toggle>().interactable = false;
                foreach (var go in battleButtons)
                    go.SetActive(false);
            }
            i++;
        }
    }

    // ����� �÷� �� ����
    Color orginColor = new Color32(95, 100, 110, 91);
    Color selectedColor = new Color32(43, 76, 135, 91);

    private void SelectCharacter(int num)
    {
        // ���õ� ������Ʈ ��������
        var selectedObj = characterList[num];

        // ���Ű On
        //Toggle toggle = selectedObj.GetComponentInChildren<Toggle>();
        //toggle.interactable = true;
        Image image = selectedObj.transform.GetComponent<Image>();

        // �ߺ� ���� ��
        if (playerDataMgr.battleSquad.ContainsKey(num))
        {
            // ����
            Debug.Log(playerDataMgr.battleSquad[num].Name + " Removed");
            playerDataMgr.battleSquad.Remove(num);

            // ���Ű �� ���� �� ����
            //toggle.isOn = false;
            image.color = orginColor;

            // ���� ���� �ο� ǥ��
            memberNumTxt.text = $"���� ���� {playerDataMgr.battleSquad.Count} / 4";
            return;
        }

        // ���� á�� ��
        if (playerDataMgr.battleSquad.Count >= 4) return;

        // Truck Squad -> Battle Squad
        // �߰�
        playerDataMgr.battleSquad.Add(num, playerDataMgr.currentSquad[num]);
        Debug.Log(playerDataMgr.battleSquad[num].Name + " Added");

        // ���Ű �� ���� �� ����
        //toggle.isOn = true;
        image.color = selectedColor;

        // ���� ���� �ο� ǥ��
        memberNumTxt.text = $"���� ���� {playerDataMgr.battleSquad.Count} / 4";
    }

    string GetTypeStr(string kind)
    {
        string type = string.Empty;
        switch (kind)
        {
            case "1":
                type = "Handgun";
                break;
            case "2":
                type = "SG";
                break;
            case "3":
                type = "SMG";
                break;
            case "4":
                type = "AR";
                break;
            case "5":
                type = "LMG";
                break;
            case "6":
                type = "SR";
                break;
            case "7":
                type = "��������";
                break;
        }
        return type;
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
        if (trunkWinMgr.isOpen)
               Init();
        //timeController.Play();
        //timeController.isPause = false;
    }

    public void InactiveButton(Button button)
    {
        button.interactable = false;
    }

    public void SwitchMode(bool value)
    {
        isSelectPossible = value;
    }
}

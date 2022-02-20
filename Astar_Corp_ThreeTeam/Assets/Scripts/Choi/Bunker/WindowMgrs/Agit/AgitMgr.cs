using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgitMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;
    public BunkerMgr bunkerMgr;
    public SkillWinMgr skillWinMgr;
    public EquipmentMgr equipmentMgr;
    public ToleranceMgr toleranceMgr;
    public BagMgr bagMgr;

    public Animator menuAnim;
    bool isMenuOpen;
    public GameObject arrowImg;

    [Header("UI")]
    public GameObject upperUI;
    public GameObject belowUI;
    public GameObject characterListUpperUI;

    [Header("CharacterList Win")]
    public List<GameObject> chracterListMenus;
    public List<GameObject> menuObjs;
    public GameObject statePrefab;
    public GameObject equipPrefab;
    public GameObject abilityPrefab;
    int characterListMenu;

    [Header("Upgrade Win")]
    public Text agitLevelTxt;
    public Text capacityTxt;
    public Text materialTxt;

    [Header("CharacterInfo Win")]
    public GameObject skillInfoWin;
    public Text skillNameTxt;
    public Text skillDetailTxt;
    public GameObject fireWin;
    public Text characterName;
    public Image characterImg;
    public Image mainWeaponImg;
    public Image subWeaponImg;
    public Image nullImg;
    public Text bagNameTxt;
    public GameObject virusObj;
    public Slider hpBar;
    public Slider expBar;
    public List<Text> virusLevelTxt;
    public Text hpGaugeTxt;
    public Text expGaugeTxt;
    public Image skillImg;
    public List<Sprite> skillSprites;

    [Header("Ability Win")]
    public GameObject abilityButton;
    public List<GameObject> statImgs;
    public Text statExplanationTxt;
    public GameObject healthButton;
    public Text healthTxt;
    public GameObject concentrationButton;
    public Text concentrationText;
    public GameObject sensitiveButton;
    public Text sensitiveTxt;
    public GameObject mentalityButton;
    public Text mentalityTxt;
    int selectStat;

    public GameObject hpButton;
    public Text hpText;
    public GameObject weightButton;
    public Text weightTxt;
    public GameObject mpButton;
    public Text mpTxt;
    public GameObject sightButton;
    public Text sightTxt;

    public GameObject accuracyButton;
    public Text accuracyTxt;
    public GameObject avoidButton;
    public Text avoidTxt;
    public GameObject criButton;
    public Text criTxt;
    public GameObject criResistButton;
    public Text criResistTxt;

    [Header("Virus Win")]
    public GameObject virusPopup;
    public GameObject virusButton;
    public List<GameObject> virusImgs;
    public Text virusExplanationTxt;
    public Text EVirusLevelTxt;
    public Text EVirusGaugeTxt;
    public Text BVirusLevelTxt;
    public Text BVirusGaugeTxt;
    public Text PVirusLevelTxt;
    public Text PVirusGaugeTxt;
    public Text IVirusLevelTxt;
    public Text IVirusGaugeTxt;
    public Text TVirusLevelTxt;
    public Text TVirusGaugeTxt;
    public List<Slider> virusSliders;
    int selectVirus;
    bool isVirusPopupOpen;

    public GameObject characterListContent;
    public GameObject characterPrefab;
    public List<GameObject> characterObjs;

    public GameObject mainWin;
    public GameObject charcterListWin;
    public GameObject upgradeWin;
    public GameObject characterInfoWin;
    public GameObject abilityWin;
    bool isAbilityWinOpen;
    public GameObject virusWin;
    bool isVirusWinOpen;
    public Text memberNumTxt;

    //ĳ���� ���� Ȯ��.
    public Text nameTxt;
    public Text levelTxt;
    public Text hpTxt;
    public Text concentrationTxt;
    public Text sensitivityTxt;
    public Text willpowerTxt;

    // ����Ʈ ���� / PlayerDataMgr Key
    public Dictionary<int, int> characterInfo = new Dictionary<int, int>();
    int agitLevel;
    int currentMember;
    int maxMember;
    int nextMaxMember;
    int upgradeCost;
    bool isDeleteMode;
    int currentIndex;
    Color originColor;

    public void Init()
    {
        agitLevel = playerDataMgr.saveData.agitLevel;
        Bunker agitLevelInfo = playerDataMgr.bunkerList["BUN_0001"];
        switch (agitLevel)
        {
            case 1:
                maxMember = agitLevelInfo.level1;
                nextMaxMember = agitLevelInfo.level2;
                upgradeCost = agitLevelInfo.level2Cost;
                break;
            case 2:
                maxMember = agitLevelInfo.level2;
                nextMaxMember = agitLevelInfo.level3;
                upgradeCost = agitLevelInfo.level3Cost;
                break;
            case 3:
                maxMember = agitLevelInfo.level3;
                nextMaxMember = agitLevelInfo.level4;
                upgradeCost = agitLevelInfo.level4Cost;
                break;
            case 4:
                maxMember = agitLevelInfo.level4;
                nextMaxMember = agitLevelInfo.level5;
                upgradeCost = agitLevelInfo.level5Cost;
                break;
            case 5:
                maxMember = agitLevelInfo.level5;
                break;
        }

        currentIndex = -1;
        skillWinMgr.currentIndex = currentIndex;
        equipmentMgr.currentIndex = currentIndex;
        toleranceMgr.currentIndex = currentIndex;
        bagMgr.currentIndex = currentIndex;

        skillWinMgr.playerDataMgr = playerDataMgr;
        skillWinMgr.Init();

        equipmentMgr.playerDataMgr = playerDataMgr;
        equipmentMgr.agitMgr = this;
        equipmentMgr.Init();

        //toleranceMgr.playerDataMgr = playerDataMgr;
        //
        //bagMgr.playerDataMgr = playerDataMgr;
        //bagMgr.agitMgr = this;
        //bagMgr.Init();

        if (characterInfoWin.activeSelf) characterInfoWin.SetActive(false);
        if (skillWinMgr.skillPage.activeSelf) skillWinMgr.skillPage.SetActive(false);
        if (equipmentMgr.equipmentWin.activeSelf) equipmentMgr.equipmentWin.SetActive(false);
        if (toleranceMgr.toleranceWin.activeSelf) toleranceMgr.toleranceWin.SetActive(false);
        if (bagMgr.bagWin.activeSelf) bagMgr.bagWin.SetActive(false);

        originColor = characterPrefab.GetComponent<Image>().color;
        isDeleteMode = false;
        isMenuOpen = true;
        arrowImg.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
        selectStat = -1;
        selectVirus = -1;

        //��Ŀ �˶�.
        if (playerDataMgr.saveData.agitAlarm)
        {
            playerDataMgr.saveData.agitAlarm = false;
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
            bunkerMgr.bunkerObjs[4].transform.GetChild(1).gameObject.SetActive(true);
            bunkerMgr.quickButtons[0].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void SelectMenu(int index)
    {
        if (characterListMenu != -1)
        {
            chracterListMenus[characterListMenu].GetComponent<Image>().color = new Color(225f/255, 225f/255, 225f/255);
            menuObjs[characterListMenu].SetActive(false);
        }

        characterListMenu = index;
        chracterListMenus[characterListMenu].GetComponent<Image>().color = new Color(255f / 255, 192f / 255, 0f / 255);
        menuObjs[characterListMenu].SetActive(true);

        RefreshCharacterList(index);
    }

    public void RefreshCharacterList(int index)
    {
        //0.����
        //1.���
        //2.�ɷ�

        //���� ���� ����.
        if (characterObjs.Count != 0)
        {
            foreach (var element in characterObjs)
            {
                Destroy(element);
            }
            characterObjs.Clear();

            characterListContent.transform.DetachChildren();
        }
        if (characterInfo.Count != 0) characterInfo.Clear();

        if (index == 0)
        {
            //����.
            int i = 0;
            foreach (var element in playerDataMgr.currentSquad)
            {
                var go = Instantiate(statePrefab, characterListContent.transform);

                var child = go.transform.GetChild(0).gameObject;
                child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                     = $"LV{element.Value.level}";

                var expSlider = child.transform.GetChild(3).gameObject.GetComponent<Slider>();
                expSlider.maxValue = element.Value.totalExp;
                expSlider.value = element.Value.currentExp;

                child.transform.GetChild(4).gameObject.GetComponent<Text>().text
                    = $"{element.Value.currentExp}/{element.Value.totalExp}";

                child = go.transform.GetChild(1).gameObject;
                child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                    = element.Value.character.icon;
                child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                     = element.Value.character.name;

                child = go.transform.GetChild(2).gameObject;
                child.transform.GetChild(0).gameObject.GetComponent<Text>().text
                     = element.Value.characterName;

                child = go.transform.GetChild(3).gameObject;
                child.transform.GetChild(0).gameObject.GetComponent<Text>().text
                     = $"{element.Value.currentHp}/{element.Value.MaxHp}";

                var slider = child.transform.GetChild(1).gameObject.GetComponent<Slider>();
                slider.maxValue = element.Value.MaxHp;
                slider.value = element.Value.currentHp;

                var virusGroup = child.transform.GetChild(2).gameObject;
                string[] virusName = new string[5];
                virusName[0] = "E";
                virusName[1] = "B";
                virusName[2] = "P";
                virusName[3] = "I";
                virusName[4] = "T";

                for (int j = 0; j < virusName.Length; j++)
                {
                    if (element.Value.virusPenalty[virusName[j]].penaltyLevel >= 1)
                        virusGroup.transform.GetChild(j).gameObject.SetActive(true);
                    else virusGroup.transform.GetChild(j).gameObject.SetActive(false);
                }

                child = go.transform.GetChild(4).gameObject;
                var toleranceGroup = child.transform.GetChild(0).gameObject;
                for (int j = 0; j < virusName.Length; j++)
                {
                    child = toleranceGroup.transform.GetChild(j).gameObject;
                    child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                        = $"Lv {element.Value.virusPenalty[virusName[j]].resistLevel}";
                }

                int num = i;
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectCharacter(num); });
                characterObjs.Add(go);

                characterInfo.Add(num, element.Key);

                i++;
            }
        }
        else if (index == 1)
        {
            //����.
            int i = 0;
            foreach (var element in playerDataMgr.currentSquad)
            {
                var go = Instantiate(equipPrefab, characterListContent.transform);

                var child = go.transform.GetChild(0).gameObject;
                child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                     = $"LV{element.Value.level}";

                var expSlider = child.transform.GetChild(3).gameObject.GetComponent<Slider>();
                expSlider.maxValue = element.Value.totalExp;
                expSlider.value = element.Value.currentExp;

                child = go.transform.GetChild(1).gameObject;
                child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                    = element.Value.character.icon;
                child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                     = element.Value.character.name;

                child = go.transform.GetChild(2).gameObject;
                child.transform.GetChild(0).gameObject.GetComponent<Text>().text
                     = element.Value.characterName;

                child = go.transform.GetChild(3).gameObject;
                child = child.transform.GetChild(0).gameObject;
                var childObj = child.transform.GetChild(0).gameObject;
                var color = childObj.GetComponent<Image>().color;
                if (element.Value.weapon.mainWeapon != null)
                {
                    childObj.GetComponent<Image>().color
                        = new Color(color.r, color.g, color.b, 1);
                    childObj.GetComponent<Image>().sprite
                        = element.Value.weapon.mainWeapon.img;
                    childObj = child.transform.GetChild(1).gameObject;
                    childObj.SetActive(true);
                    childObj.transform.GetChild(0).GetComponent<Text>().text
                        = $"{GetTypeStr(element.Value.weapon.mainWeapon.kind)}";
                }
                else
                {
                    childObj.GetComponent<Image>().color
                      = new Color(color.r, color.g, color.b, 0);
                    childObj = child.transform.GetChild(1).gameObject;
                    childObj.SetActive(false);
                }

                child = go.transform.GetChild(4).gameObject;
                child = child.transform.GetChild(0).gameObject;
                childObj = child.transform.GetChild(0).gameObject;
                color = childObj.GetComponent<Image>().color;
                if (element.Value.weapon.subWeapon != null)
                {
                    childObj.GetComponent<Image>().color
                        = new Color(color.r, color.g, color.b, 1);
                    childObj.GetComponent<Image>().sprite
                        = element.Value.weapon.subWeapon.img;
                    childObj = child.transform.GetChild(1).gameObject;
                    childObj.SetActive(true);
                    childObj.transform.GetChild(0).GetComponent<Text>().text
                        = $"{GetTypeStr(element.Value.weapon.subWeapon.kind)}";
                }
                else
                {
                    childObj.GetComponent<Image>().color
                      = new Color(color.r, color.g, color.b, 0);
                    childObj = child.transform.GetChild(1).gameObject;
                    childObj.SetActive(false);
                }

                int num = i;
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectCharacter(num); });
                characterObjs.Add(go);

                characterInfo.Add(num, element.Key);

                i++;
            }
        }
        else if (index == 2)
        {
            //����.
            int i = 0;
            foreach (var element in playerDataMgr.currentSquad)
            {
                var go = Instantiate(abilityPrefab, characterListContent.transform);

                var child = go.transform.GetChild(0).gameObject;
                child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                     = $"LV{element.Value.level}";

                var expSlider = child.transform.GetChild(3).gameObject.GetComponent<Slider>();
                expSlider.maxValue = element.Value.totalExp;
                expSlider.value = element.Value.currentExp;

                child = go.transform.GetChild(1).gameObject;
                child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                    = element.Value.character.icon;
                child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                     = element.Value.character.name;

                child = go.transform.GetChild(2).gameObject;
                child.transform.GetChild(0).gameObject.GetComponent<Text>().text
                     = element.Value.characterName;

                child = go.transform.GetChild(3).gameObject;
                child = child.transform.GetChild(0).gameObject;
                var childObj = child.transform.GetChild(0).gameObject;
                childObj = childObj.transform.GetChild(1).gameObject;
                childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text
                    = $"{element.Value.currentHp}";

                childObj = child.transform.GetChild(1).gameObject;
                childObj = childObj.transform.GetChild(1).gameObject;
                childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text
                    = $"{element.Value.concentration}";

                childObj = child.transform.GetChild(2).gameObject;
                childObj = childObj.transform.GetChild(1).gameObject;
                childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text
                    = $"{element.Value.sensivity}";

                childObj = child.transform.GetChild(3).gameObject;
                childObj = childObj.transform.GetChild(1).gameObject;
                childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text
                    = $"{element.Value.willpower}";

                child = go.transform.GetChild(4).gameObject;
                childObj = child.transform.GetChild(0).gameObject;
                childObj = childObj.transform.GetChild(0).gameObject;

                Sprite skillSprite = null;
                switch (element.Value.character.name)
                {
                    case "Tanker":
                        skillSprite = skillSprites[0];
                        break;
                    case "Healer":
                        skillSprite = skillSprites[1];
                        break;
                    case "Sniper":
                        skillSprite = skillSprites[2];
                        break;
                    case "Scout":
                        skillSprite = skillSprites[3];
                        break;
                    case "Bombardier":
                        skillSprite = skillSprites[4];
                        break;
                }
                childObj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = skillSprite;

                int num = i;
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectCharacter(num); });
                characterObjs.Add(go);

                characterInfo.Add(num, element.Key);

                i++;
            }
        }

        int currentMemberNum = playerDataMgr.currentSquad.Count;
        memberNumTxt.text = $"�ִ� �뺴 ���뷮 {currentMemberNum} / {maxMember}";
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

    public void SelectCharacter(int index)
    {
        bunkerMgr.PlayClickSound();
        ChangeCharacter(index);
        OpenCharacterInfo();
    }

    public void NextCharacter()
    {
        if (currentIndex + 1 == playerDataMgr.currentSquad.Count) return;
        ChangeCharacter(currentIndex + 1);
        OpenCharacterInfo();
    }

    public void PreviousCharacter()
    {
        if (currentIndex - 1 < 0) return;
        ChangeCharacter(currentIndex - 1);
        OpenCharacterInfo();
    }

    void ChangeCharacter(int index)
    {
        currentIndex = index;
        skillWinMgr.currentIndex = currentIndex;
        equipmentMgr.currentIndex = currentIndex;
        //toleranceMgr.currentIndex = currentIndex;
        bagMgr.currentIndex = currentIndex;

        equipmentMgr.RefreshEquipList();
        //toleranceMgr.Refresh();
        //bagMgr.Init();
    }

    public void Fire()
    {
        if (isAnythingChecked() == false)
        {
            Debug.Log("����");
            return;
        }

        int count = characterObjs.Count;
        for (int i = count -1; i > -1; --i)
        {
            var child = characterObjs[i].transform.GetChild(0).gameObject;
            var toggle = child.transform.GetChild(2).gameObject.GetComponent<Toggle>();
                 
            if (toggle.isOn)
            {
                Destroy(characterObjs[i]);
                characterObjs.RemoveAt(i);

                playerDataMgr.RemoveCharacter(i);
            }
        }
        playerDataMgr.RefreshCurrentSquad();
        RefreshCharacterList(characterListMenu);
    }

    public void FireAll()
    {
        int count = characterObjs.Count;
        for (int i = count - 1; i > -1; --i)
        {
            Destroy(characterObjs[i]);
            characterObjs.RemoveAt(i);

            playerDataMgr.RemoveCharacter(i);
        }
        playerDataMgr.RefreshCurrentSquad();
        RefreshCharacterList(characterListMenu);
    }

    bool isAnythingChecked()
    {
        foreach (var element in characterObjs)
        {
            var child = element.transform.GetChild(0).gameObject;
            var toggle = child.transform.GetChild(2).gameObject.GetComponent<Toggle>();
            if (toggle.isOn == true) return true;
        }
        return false;
    }

    public void FireCharacter()
    {
        Destroy(characterObjs[currentIndex]);
        characterObjs.RemoveAt(currentIndex);

        playerDataMgr.RemoveCharacter(currentIndex);    
        playerDataMgr.RefreshCurrentSquad();
        RefreshCharacterList(characterListMenu);

        if (equipmentMgr.equipmentWin1.activeSelf) CloseEquipmentWin();
        CloseCharacterInfo();
    }

    public void SelectStat(int index)
    {
        if (selectStat != -1)
        {
            statImgs[selectStat].GetComponent<Image>().color = Color.white;
            var child = statImgs[selectStat].transform.GetChild(0).gameObject;
            child.GetComponent<Image>().color = Color.black;

            switch (selectStat)
            {
                case 0:
                    hpButton.GetComponent<Image>().color = Color.white;
                    hpText.color = Color.white;
                    weightButton.GetComponent<Image>().color = Color.white;
                    weightTxt.color = Color.white;
                    criResistButton.GetComponent<Image>().color = Color.white;
                    criResistTxt.color = Color.white;
                    break;
                case 1:
                    accuracyButton.GetComponent<Image>().color = Color.white;
                    accuracyTxt.color = Color.white;
                    //�����ž���.
                    sightButton.GetComponent<Image>().color = Color.white;
                    sightTxt.color = Color.white;
                    break;
                case 2:
                    avoidButton.GetComponent<Image>().color = Color.white;
                    avoidTxt.color = Color.white;
                    //�����ž���.
                    mpButton.GetComponent<Image>().color = Color.white;
                    mpTxt.color = Color.white;
                    break;
                case 3:
                    criButton.GetComponent<Image>().color = Color.white;
                    criTxt.color = Color.white;
                    break;
            }
        }

        selectStat = index;
        statImgs[selectStat].GetComponent<Image>().color = new Color(255f / 255, 192f / 255, 0f / 255);
        var childObj = statImgs[selectStat].transform.GetChild(0).gameObject;
        childObj.GetComponent<Image>().color = new Color(0f / 255, 112f / 255, 192f / 255);
        
        switch (selectStat)
        {
            case 0:
                hpButton.GetComponent<Image>().color = new Color(217f / 255, 231f / 255, 253f / 255);
                hpText.color = new Color(0f / 255, 112f / 255, 192f / 255);
                weightButton.GetComponent<Image>().color = new Color(217f / 255, 231f / 255, 253f / 255);
                weightTxt.color = new Color(0f / 255, 112f / 255, 192f / 255);
                criResistButton.GetComponent<Image>().color = new Color(217f / 255, 231f / 255, 253f / 255);
                criResistTxt.color = new Color(0f / 255, 112f / 255, 192f / 255);
                statExplanationTxt.text =
                    "�ǰ� ������ ĳ������ Hp�� ũ��Ƽ�� �������� ������ �ݴϴ�."
                    + "HP�� ������ ĳ���Ͱ� ����ִ� ���Կ� ������ �ݴϴ�.";
                break;
            case 1:
                accuracyButton.GetComponent<Image>().color = new Color(217f / 255, 231f / 255, 253f / 255);
                accuracyTxt.color = new Color(0f / 255, 112f / 255, 192f / 255);
                //�����ž���.
                sightButton.GetComponent<Image>().color = new Color(217f / 255, 231f / 255, 253f / 255);
                sightTxt.color = new Color(0f / 255, 112f / 255, 192f / 255);
                statExplanationTxt.text =
                     "���߷� ������ ��� ���߷��� ������ �ָ� Ư�� ����Ʈ���� �þ߹����� �����մϴ�."; 
                break;
            case 2:
                avoidButton.GetComponent<Image>().color = new Color(217f / 255, 231f / 255, 253f / 255);
                avoidTxt.color = new Color(0f / 255, 112f / 255, 192f / 255);
                //�����ž���.
                mpButton.GetComponent<Image>().color = new Color(217f / 255, 231f / 255, 253f / 255);
                mpTxt.color = new Color(0f / 255, 112f / 255, 192f / 255);
                statExplanationTxt.text =
                      "������ ������ ȸ������ ������ �ָ� Ư�� ����Ʈ���� 1ap�� �����ϴ� MP���� �����մϴ�."; 
                break;
            case 3:
                criButton.GetComponent<Image>().color = new Color(217f / 255, 231f / 255, 253f / 255);
                criTxt.color = new Color(0f / 255, 112f / 255, 192f / 255);
                statExplanationTxt.text =
                      "���ŷ� ������ ũ��Ƽ�� Ȯ���� ���ӻ�� �г�Ƽ ���ҿ� ������ �ݴϴ�.";
                break;
        }
    }

    public void SelectVirus(int index)
    {
        if (selectVirus != -1)
            virusImgs[selectVirus].GetComponent<Image>().color = Color.white;
        
        selectVirus = index;
        virusImgs[selectVirus].GetComponent<Image>().color = new Color(255f/255, 192f/255, 0f/255);

        switch (selectVirus)
        {
            case 0:
                virusExplanationTxt.text =
                    "���������� ���̷����� 2�������� �ϴ� HP�� 1�� �����մϴ�." +
                    "ĳ���� �������� ���ҽ�Ű�� ������ ���� ��ġ�� �����˴ϴ�.";
                break;
            case 1:
                virusExplanationTxt.text =
                    "���������� ���̷����� 2�������� �ϴ� HP�� 1�� �����մϴ�." +
                    "�г�Ƽ: ĳ���� HP�� ���ҽ�Ű�� ������ ���� ��ġ�� �����˴ϴ�.";
                break;
            case 2:
                virusExplanationTxt.text =
                    "���������� ���̷����� 2�������� �ϴ� HP�� 1�� �����մϴ�." +
                    "�г�Ƽ: ĳ���� ���� ��ü MP���� ���ҽ�Ű�� ������ ���� ��ġ�� �����˴ϴ�.";
                break;
            case 3:
                virusExplanationTxt.text =
                    "���������� ���̷����� 2�������� �ϴ� HP�� 1�� �����մϴ�." +
                    "�г�Ƽ: ĳ���� ���߷��� ���ҽ�Ű�� ������ ���� ��ġ�� �����˴ϴ�.";
                break;
            case 4:
                virusExplanationTxt.text =
                    "���������� ���̷����� 2�������� �ϴ� HP�� 1�� �����մϴ�." +
                    "T���̷����� ��� ���̷����� �г�Ƽ�� ���ÿ� ������ ������ ���� ��ġ�� �����˴ϴ�.";
                break;
        }
    }

    public void RefreshUpgradeWin()
    {
        if (agitLevel != 5)
        {
            agitLevelTxt.text = $"�ǹ� ���� {agitLevel}��{agitLevel + 1}";
            capacityTxt.text = $"�뺰 �ִ� ���� {maxMember}��{nextMaxMember}";
            materialTxt.text = $"{upgradeCost}";
        }
        else
        {
            agitLevelTxt.text = $"�ǹ� ����{agitLevel}�� -";
            capacityTxt.text = $"�뺰 �ִ� ���� {maxMember}�� -";
            materialTxt.text = $"-";
        }
    }

    //â ����.
    public void OpenMainWin()
    {
        //��Ŀ �˶�.
        if (bunkerMgr.bunkerObjs[4].transform.GetChild(1).gameObject.activeSelf)
        {
            bunkerMgr.bunkerObjs[4].transform.GetChild(1).gameObject.SetActive(false);
            bunkerMgr.quickButtons[0].transform.GetChild(1).gameObject.SetActive(false);
        }

        CloseSkillWin();
        CloseFireWin();

        if (bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(false);
        if (!upperUI.activeSelf) upperUI.SetActive(true);
        if (!belowUI.activeSelf) belowUI.SetActive(true);

        if (!mainWin.activeSelf) mainWin.SetActive(true);
        if (charcterListWin.activeSelf) charcterListWin.SetActive(false);
        if (characterListUpperUI.activeSelf) characterListUpperUI.SetActive(false);
        if (upgradeWin.activeSelf) upgradeWin.SetActive(false);
    }

    public void CloseMainWin()
    {
        if (!bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(true);
        if (upperUI.activeSelf) upperUI.SetActive(false);
        if (belowUI.activeSelf) belowUI.SetActive(false);
    }

    public void Menu()
    {
        arrowImg.GetComponent<RectTransform>().rotation = (isMenuOpen) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.Euler(0f, 0f, 0f);
        isMenuOpen = !isMenuOpen;
        menuAnim.SetBool("isOpen", isMenuOpen);
    }

    public void OpenCharacterListWin()
    {
        mainWin.SetActive(false);
        characterListUpperUI.SetActive(true);
        characterListMenu = 0;
        RefreshCharacterList(characterListMenu);
        charcterListWin.SetActive(true);

        foreach (var element in chracterListMenus)
        { 
            element.GetComponent<Image>().color = new Color(225f / 255, 225f / 255, 225f / 255);
        }
        foreach (var element in menuObjs)
        {
            element.SetActive(false);
        }

        characterListMenu = -1;
        SelectMenu(0);
    }

    public void CloseCharacterListWin()
    {
        characterListUpperUI.SetActive(false);
        charcterListWin.SetActive(false);
        mainWin.SetActive(true);
    }

    public void OpenUpgradeWin()
    {
        RefreshUpgradeWin();

        mainWin.SetActive(false);
        upgradeWin.SetActive(true);
    }

    public void CloseUpgradeWin()
    {
        upgradeWin.SetActive(false);
        mainWin.SetActive(true);
    }

    public void OpenCharacterInfo()
    {
        bunkerMgr.upperUI.SetActive(false);
        charcterListWin.SetActive(false);
        characterInfoWin.SetActive(true);

        if (abilityWin.activeSelf) abilityWin.SetActive(false);
        if (virusWin.activeSelf) virusWin.SetActive(false);
        if (virusPopup.activeSelf) virusPopup.SetActive(false);
        isAbilityWinOpen = false;
        isVirusWinOpen = false;
        isVirusPopupOpen = false;

        var character = playerDataMgr.currentSquad[currentIndex];
        characterName.text = $"{character.characterName} (LV.{character.level})";
        characterImg.sprite = character.character.halfImg;
        mainWeaponImg.sprite = (character.weapon.mainWeapon!=null)? 
            character.weapon.mainWeapon.img : nullImg.sprite;
        subWeaponImg.sprite = (character.weapon.subWeapon != null) ? 
            character.weapon.subWeapon.img : nullImg.sprite;
        bagNameTxt.text = $"Lv{character.bagLevel} ����";

        switch (character.character.name)
        {
            case "Tanker":
                skillImg.sprite = skillSprites[0];
                break;
            case "Healer":
                skillImg.sprite = skillSprites[1];
                break;
            case "Sniper":
                skillImg.sprite = skillSprites[2];
                break;
            case "Scout":
                skillImg.sprite = skillSprites[3];
                break;
            case "Bombardier":
                skillImg.sprite = skillSprites[4];
                break;
        }

        virusLevelTxt[0].text = $"{character.virusPenalty["E"].penaltyLevel}";
        virusLevelTxt[1].text = $"{character.virusPenalty["B"].penaltyLevel}";
        virusLevelTxt[2].text = $"{character.virusPenalty["P"].penaltyLevel}";
        virusLevelTxt[3].text = $"{character.virusPenalty["I"].penaltyLevel}";
        virusLevelTxt[4].text = $"{character.virusPenalty["T"].penaltyLevel}";

        hpBar.maxValue = character.MaxHp;
        hpBar.value = character.currentHp;
        hpGaugeTxt.text = $"{character.currentHp}/{character.MaxHp}";

        expBar.maxValue = character.totalExp;
        expBar.value = character.currentExp;
        expGaugeTxt.text = $"{character.currentExp}/{character.totalExp}";

        if (character.virusPenalty["E"].penaltyGauge > 0)
            virusObj.transform.GetChild(0).gameObject.SetActive(true);
        else virusObj.transform.GetChild(0).gameObject.SetActive(false);

        if (character.virusPenalty["B"].penaltyGauge > 0)
            virusObj.transform.GetChild(1).gameObject.SetActive(true);
        else virusObj.transform.GetChild(1).gameObject.SetActive(false);

        if (character.virusPenalty["P"].penaltyGauge > 0)
            virusObj.transform.GetChild(2).gameObject.SetActive(true);
        else virusObj.transform.GetChild(2).gameObject.SetActive(false);

        if (character.virusPenalty["I"].penaltyGauge > 0)
            virusObj.transform.GetChild(3).gameObject.SetActive(true);
        else virusObj.transform.GetChild(3).gameObject.SetActive(false);

        if (character.virusPenalty["T"].penaltyGauge > 0)
            virusObj.transform.GetChild(4).gameObject.SetActive(true);
        else virusObj.transform.GetChild(4).gameObject.SetActive(false);

        //Ability Win.
        healthTxt.text = $"{character.currentHp}";
        concentrationText.text = $"{character.concentration}";
        sensitiveTxt.text = $"{character.sensivity}";
        mentalityTxt.text = $"{character.willpower}";

        hpText.text = $"{character.currentHp}";
        //���߿� �����ؾߵ�.
        weightTxt.text = $"{character.Weight + playerDataMgr.bagList["BAG_0001"].weight}";
        mpTxt.text = $"3";

        sightTxt.text = $"{character.sightDistance}";
        accuracyTxt.text = $"{character.accuracy}";
        avoidTxt.text = $"{character.avoidRate}";
        criTxt.text = $"{character.critRate}";
        criResistTxt.text = $"{character.critResistRate}";

        //Virus Win.
        EVirusLevelTxt.text = $"Lv{character.virusPenalty["E"].resistLevel}";
        EVirusGaugeTxt.text = $"{character.virusPenalty["E"].resistGauge}/{character.virusPenalty["E"].GetMaxReductionGauge()}";
        BVirusLevelTxt.text = $"Lv{character.virusPenalty["B"].resistLevel}";
        BVirusGaugeTxt.text = $"{character.virusPenalty["B"].resistGauge}/{character.virusPenalty["B"].GetMaxReductionGauge()}";
        PVirusLevelTxt.text = $"Lv{character.virusPenalty["P"].resistLevel}";
        PVirusGaugeTxt.text = $"{character.virusPenalty["P"].resistGauge}/{character.virusPenalty["P"].GetMaxReductionGauge()}";
        IVirusLevelTxt.text = $"Lv{character.virusPenalty["I"].resistLevel}";
        IVirusGaugeTxt.text = $"{character.virusPenalty["I"].resistGauge}/{character.virusPenalty["I"].GetMaxReductionGauge()}";
        TVirusLevelTxt.text = $"Lv{character.virusPenalty["T"].resistLevel}";
        TVirusGaugeTxt.text = $"{character.virusPenalty["T"].resistGauge}/{character.virusPenalty["T"].GetMaxReductionGauge()}";

        virusSliders[0].maxValue = character.virusPenalty["E"].GetMaxReductionGauge();
        virusSliders[0].value = character.virusPenalty["E"].resistGauge;
        virusSliders[1].maxValue = character.virusPenalty["B"].GetMaxReductionGauge();
        virusSliders[1].value = character.virusPenalty["B"].resistGauge;
        virusSliders[2].maxValue = character.virusPenalty["P"].GetMaxReductionGauge();
        virusSliders[2].value = character.virusPenalty["P"].resistGauge;
        virusSliders[3].maxValue = character.virusPenalty["I"].GetMaxReductionGauge();
        virusSliders[3].value = character.virusPenalty["I"].resistGauge;
        virusSliders[4].maxValue = character.virusPenalty["T"].GetMaxReductionGauge();
        virusSliders[4].value = character.virusPenalty["T"].resistGauge;

        nameTxt.text = $"{character.character.name}";
        levelTxt.text = $"Level {character.level} / �а�";
        hpTxt.text = $"ü�� {character.currentHp}";
        concentrationTxt.text = $"���߷� {character.concentration}";
        sensitivityTxt.text = $"������ {character.sensivity}";
        willpowerTxt.text = $"���ŷ� {character.willpower}";
    }

    public void CloseCharacterInfo()
    {
        bunkerMgr.upperUI.SetActive(true);
        characterInfoWin.SetActive(false);
        charcterListWin.SetActive(true);
    }

    public void OpenSkillPage()
    {
        if(equipmentMgr.equipmentWin2.activeSelf)
            equipmentMgr.equipmentWin2.SetActive(false);

        characterInfoWin.SetActive(false);
        equipmentMgr.equipmentWin.SetActive(true);
        skillWinMgr.OpenSkillPage();
    }

    public void CloseSkillPage()
    {
        skillWinMgr.CloseSkillPage();
        equipmentMgr.equipmentWin.SetActive(false);
        characterInfoWin.SetActive(true);
    }

    public void OpenEquipmentWin()
    {
        CloseSkillWin();
        characterInfoWin.SetActive(false);
        equipmentMgr.equipmentWin.SetActive(true);
        equipmentMgr.OpenEquipWin1();
    }

    public void CloseEquipmentWin()
    {
        if (skillWinMgr.skillPage.activeSelf)
            skillWinMgr.skillPage.SetActive(false);

        equipmentMgr.equipmentWin.SetActive(false);
        characterInfoWin.SetActive(true);
    }

    public void OpenToleranceWin()
    {
        characterInfoWin.SetActive(false);
        toleranceMgr.toleranceWin.SetActive(true);
    }

    public void CloseToleranceWin()
    {
        toleranceMgr.toleranceWin.SetActive(false);
        characterInfoWin.SetActive(true);
    }

    public void OpenBagWin()
    {
        characterInfoWin.SetActive(false);
        bagMgr.bagWin.SetActive(true);
    }

    public void CloseBagWin()
    {
        bagMgr.bagWin.SetActive(false);
        characterInfoWin.SetActive(true);
    }

    public void AbilityWin()
    {
        if (isVirusWinOpen)
        {
            isVirusWinOpen = !isVirusWinOpen;
            virusWin.SetActive(false);
        }    
        isAbilityWinOpen = !isAbilityWinOpen;

        //�ʱ�ȭ.
        if (isAbilityWinOpen)
        {
            foreach (var element in statImgs)
            {
                if (element.GetComponent<Image>().color != Color.white)
                {
                    element.GetComponent<Image>().color = Color.white;
                    var child = element.transform.GetChild(0).gameObject;
                    child.GetComponent<Image>().color = Color.black ;
                }
            }
            if (hpButton.GetComponent<Image>().color != Color.white)
            {
                hpButton.GetComponent<Image>().color = Color.white;
                hpText.color = Color.white;
            }
            if (weightButton.GetComponent<Image>().color != Color.white)
            {
                weightButton.GetComponent<Image>().color = Color.white;
                weightTxt.color = Color.white;
            }
            if (mpButton.GetComponent<Image>().color != Color.white)
            {
                mpButton.GetComponent<Image>().color = Color.white;
                mpTxt.color = Color.white;
            }
            if (sightButton.GetComponent<Image>().color != Color.white)
            {
                sightButton.GetComponent<Image>().color = Color.white;
                sightTxt.color = Color.white;
            }

            if (accuracyButton.GetComponent<Image>().color != Color.white)
            {
                accuracyButton.GetComponent<Image>().color = Color.white;
                accuracyTxt.color = Color.white;
            }
            if (avoidButton.GetComponent<Image>().color != Color.white)
            {
                avoidButton.GetComponent<Image>().color = Color.white;
                avoidTxt.color = Color.white;
            }
            if (criButton.GetComponent<Image>().color != Color.white)
            {
                criButton.GetComponent<Image>().color = Color.white;
                criTxt.color = Color.white;
            }
            if (criResistButton.GetComponent<Image>().color != Color.white)
            {
                criResistButton.GetComponent<Image>().color = Color.white;
                criResistTxt.color = Color.white;
            }
        }
        statExplanationTxt.text = string.Empty;

        abilityWin.SetActive(isAbilityWinOpen);
    }

    public void VirusWin()
    {
        if (isAbilityWinOpen)
        {
            isAbilityWinOpen = !isAbilityWinOpen;
            abilityWin.SetActive(false);
        }
        isVirusWinOpen = !isVirusWinOpen;

        //�ʱ�ȭ.
        if (isVirusWinOpen)
        {
            foreach (var element in virusImgs)
            {
                if (element.GetComponent<Image>().color != Color.white)
                    element.GetComponent<Image>().color = Color.white;
            }
        }
        virusExplanationTxt.text = string.Empty;

        virusWin.SetActive(isVirusWinOpen);
    }

    public void VirusPopup()
    {
        isVirusPopupOpen = !isVirusPopupOpen;
        virusPopup.SetActive(isVirusPopupOpen);
    }

    public void OpenSkillWin()
    {
        if (!skillInfoWin.activeSelf) skillInfoWin.SetActive(true);
        var character = playerDataMgr.currentSquad[currentIndex];
        switch (character.character.name)
        {
            case "Bombardier":
                skillNameTxt.text = "�ѳ� ��� (��Ƽ�� ��ų)";
                skillDetailTxt.text = "6ap�� �Ҹ��ؼ� �̹��Ͽ� �ϳ��� ������ ź���� �����ؼ� ���� (��Ÿ�� 4��)";
                break;
            case "Healer":
                skillNameTxt.text = "�����Ѹ鿪ü�� (�нú� ��ų)";
                skillDetailTxt.text = "���� �޴� ���̷��� �г�Ƽ �������� 80%�� ����.";
                break;
            case "Scout":
                skillNameTxt.text = "ö���� ���� (�нú� ��ų)";
                skillDetailTxt.text = "ĳ���� ���� ���׶� �þ߰� 1ĭ �� �о�����.";
                break;
            case "Sniper":
                skillNameTxt.text = "ħ���� ��� (�нú� ��ų)";
                skillDetailTxt.text = "���� �Ͽ� �̵��� ���� �ʾҴٸ�, �̹� �� SR ���߷� 5%���";
                break;
            case "Tanker":
                skillNameTxt.text = "������ �߾� (�нú� ��ų)";
                skillDetailTxt.text = "ĳ������ ù �÷��̽� �̵��� �� �ִ� �ִ�Ÿ��� �̵� �ߴٸ�, ap �Ҹ� ���� ���� ��� 1ȸ ����";
                break;
        }
    }

    public void CloseSkillWin()
    {
        if (skillInfoWin.activeSelf) skillInfoWin.SetActive(false);
    }

    public void OpenFireWin()
    {
        if (!fireWin.activeSelf) fireWin.SetActive(true);
    }

    public void CloseFireWin()
    {
        if (fireWin.activeSelf) fireWin.SetActive(false);
    }
}

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

    [Header("Upgrade Win")]
    public Text agitLevelTxt;
    public Text capacityTxt;
    public Text materialTxt;

    [Header("CharacterInfo Win")]
    public Text characterName;
    public Image mainWeaponImg;
    public Image subWeaponImg;
    public Text bagNameTxt;
    public GameObject virusObj;
    public Slider hpBar;

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
    int selectVirus;

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

    

    //캐릭터 정보 확인.
    public Text nameTxt;
    public Text levelTxt;
    public Text hpTxt;
    public Text concentrationTxt;
    public Text sensitivityTxt;
    public Text willpowerTxt;

    // 리스트 순서 / PlayerDataMgr Key
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
       
        RefreshCharacterList();

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

        toleranceMgr.playerDataMgr = playerDataMgr;
        
        bagMgr.playerDataMgr = playerDataMgr;
        bagMgr.agitMgr = this;
        bagMgr.Init();

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

        //벙커 알람.
        if (playerDataMgr.saveData.agitAlarm)
        {
            playerDataMgr.saveData.agitAlarm = false;
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
            bunkerMgr.bunkerObjs[4].transform.GetChild(1).gameObject.SetActive(true);
            bunkerMgr.quickButtons[0].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void RefreshCharacterList()
    {
        //이전 정보 삭제.
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

        //생성.
        int i = 0;
        foreach (var element in playerDataMgr.currentSquad)
        {
            var go = Instantiate(characterPrefab, characterListContent.transform);

            var child = go.transform.GetChild(0).gameObject;
            child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                 = $"LV{element.Value.level}";

            child = go.transform.GetChild(1).gameObject;
            child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                 = element.Value.character.name;

            child = go.transform.GetChild(2).gameObject;
            child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                = (element.Value.weapon.mainWeapon != null) ?
                element.Value.weapon.mainWeapon.img : null;
            child.transform.GetChild(1).gameObject.GetComponent<Text>().text
                 = (element.Value.weapon.mainWeapon != null) ?
                 element.Value.weapon.mainWeapon.name : "비어있음";

            child = go.transform.GetChild(3).gameObject;
            child.transform.GetChild(0).gameObject.GetComponent<Text>().text
                 = element.Value.character.name;

            child = go.transform.GetChild(4).gameObject;
            child.transform.GetChild(0).gameObject.GetComponent<Text>().text
                 = $"{element.Value.currentHp}/{element.Value.MaxHp}";
            child.transform.GetChild(1).gameObject.GetComponent<Slider>().maxValue
                = element.Value.MaxHp;
            child.transform.GetChild(1).gameObject.GetComponent<Slider>().value
                = element.Value.currentHp;

            int num = i;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(num); });
            characterObjs.Add(go);

            characterInfo.Add(num, element.Key);

            i++;
        }
        int currentMemberNum = playerDataMgr.currentSquad.Count;
        memberNumTxt.text = $"최대 용병 수용량 {currentMemberNum} / {maxMember}";
    }

    public void SelectCharacter(int index)
    {
        ChangeCharacter(index);
        OpenCharacterInfo();
    }

    public void NextCharacter()
    {
        if (currentIndex + 1 == playerDataMgr.currentSquad.Count) return;
        ChangeCharacter(currentIndex + 1);
    }

    public void PreviousCharacter()
    {
        if (currentIndex - 1 < 0) return;
        ChangeCharacter(currentIndex - 1);
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
        bagMgr.Init();
    }

    public void Fire()
    {
        if (isAnythingChecked() == false)
        {
            Debug.Log("없음");
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
        RefreshCharacterList();
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
        RefreshCharacterList();
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
        RefreshCharacterList();

        CloseCharacterInfo();
    }

    public void SelectStat(int index)
    {
        if (selectStat != -1)
        {
            statImgs[selectStat].GetComponent<Image>().color = Color.white;
            var child = statImgs[selectVirus].transform.GetChild(0).gameObject;
            child.GetComponent<Image>().color = Color.black;

            switch (selectStat)
            {
                case 0:
                    hpText.color = Color.black;
                    weightTxt.color = Color.black;
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }

        selectStat = index;
        statImgs[selectVirus].GetComponent<Image>().color = new Color(255f / 255, 192f / 255, 0f / 255);
        var childObj = statImgs[selectVirus].transform.GetChild(0).gameObject;
        childObj.GetComponent<Image>().color = new Color(0f / 255, 112f / 255, 192f / 255);
        
        switch (selectStat)
        {
            case 0:
                hpButton.GetComponent<Image>().color = new Color(217f / 255, 231f / 255, 253f / 255);
                hpText.color = new Color(172f / 255, 201f / 255, 250f / 255);
                weightButton.GetComponent<Image>().color = new Color(217f / 255, 231f / 255, 253f / 255);
                weightTxt.color = new Color(172f / 255, 201f / 255, 250f / 255);
                statExplanationTxt.text =
                    "건강이 활성화 되어 있기 때문에 건강에 대한 설명이 들어가는 자리입니다."
                    +"활성화 되어 있는 스탯의 설명이 이 자리에 표시되게 됩니다.";
                break;
            case 1:
                statExplanationTxt.text =
                     "집중력이 활성화 되어 있기 때문에 건강에 대한 설명이 들어가는 자리입니다."
                    + "활성화 되어 있는 스탯의 설명이 이 자리에 표시되게 됩니다."; 
                break;
            case 2:
                statExplanationTxt.text =
                      "예민함이 활성화 되어 있기 때문에 건강에 대한 설명이 들어가는 자리입니다."
                    + "활성화 되어 있는 스탯의 설명이 이 자리에 표시되게 됩니다."; 
                break;
            case 3:
                statExplanationTxt.text =
                      "정신력이 활성화 되어 있기 때문에 건강에 대한 설명이 들어가는 자리입니다."
                    + "활성화 되어 있는 스탯의 설명이 이 자리에 표시되게 됩니다.";
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
                    "E바이러스가 활성화 되어 있기 때문에 E바이러스에 대한 설명이 들어가는 자리입니다.";
                break;
            case 1:
                virusExplanationTxt.text =
                    "B바이러스가 활성화 되어 있기 때문에 B바이러스에 대한 설명이 들어가는 자리입니다.";
                break;
            case 2:
                virusExplanationTxt.text =
                    "P바이러스가 활성화 되어 있기 때문에 P바이러스에 대한 설명이 들어가는 자리입니다.";
                break;
            case 3:
                virusExplanationTxt.text =
                    "I바이러스가 활성화 되어 있기 때문에 I바이러스에 대한 설명이 들어가는 자리입니다.";
                break;
            case 4:
                virusExplanationTxt.text =
                    "T바이러스가 활성화 되어 있기 때문에 T바이러스에 대한 설명이 들어가는 자리입니다.";
                break;
        }
    }

    public void RefreshUpgradeWin()
    {
        if (agitLevel != 5)
        {
            agitLevelTxt.text = $"건물 레벨 {agitLevel}→{agitLevel + 1}";
            capacityTxt.text = $"용별 최대 수량 {maxMember}→{nextMaxMember}";
            materialTxt.text = $"{upgradeCost}";
        }
        else
        {
            agitLevelTxt.text = $"건물 레벨{agitLevel}→ -";
            capacityTxt.text = $"용별 최대 수량 {maxMember}→ -";
            materialTxt.text = $"-";
        }
    }

    //창 관리.
    public void OpenMainWin()
    {
        //벙커 알람.
        if (bunkerMgr.bunkerObjs[4].transform.GetChild(1).gameObject.activeSelf)
        {
            bunkerMgr.bunkerObjs[4].transform.GetChild(1).gameObject.SetActive(false);
            bunkerMgr.quickButtons[0].transform.GetChild(1).gameObject.SetActive(false);
        }

        if (bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(false);
        if (bunkerMgr.mapButton.activeSelf) bunkerMgr.mapButton.SetActive(false);
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
        if (!bunkerMgr.mapButton.activeSelf) bunkerMgr.mapButton.SetActive(true);
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
        charcterListWin.SetActive(true);
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
        isAbilityWinOpen = false;
        isVirusWinOpen = false;

        var character = playerDataMgr.currentSquad[currentIndex];
        characterName.text = $"{character.character.name}";
        mainWeaponImg.sprite = (character.weapon.mainWeapon!=null)? 
            character.weapon.mainWeapon.img : null;
        subWeaponImg.sprite = (character.weapon.subWeapon != null) ? 
            character.weapon.subWeapon.img : null;
        bagNameTxt.text = $"Lv{character.bagLevel} 가방";

        hpBar.maxValue = character.MaxHp;
        hpBar.value = character.currentHp;

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
        //나중에 수정해야됨.
        weightTxt.text = $"{character.Weight + playerDataMgr.bagList["BAG_0001"].weight}";
        mpTxt.text = $"0";

        sightTxt.text = $"{character.sightDistance}";
        accuracyTxt.text = $"{character.accuracy}";
        avoidTxt.text = $"{character.avoidRate}";
        criTxt.text = $"{character.critRate}";
        criResistTxt.text = $"{character.critResistRate}";

        //Virus Win.
        EVirusLevelTxt.text = $"Lv{character.virusPenalty["E"].penaltyLevel}";
        EVirusGaugeTxt.text = $"{character.virusPenalty["E"].penaltyGauge}/{character.virusPenalty["E"].GetMaxGauge()}";
        BVirusLevelTxt.text = $"Lv{character.virusPenalty["B"].penaltyLevel}";
        BVirusGaugeTxt.text = $"{character.virusPenalty["B"].penaltyGauge}/{character.virusPenalty["B"].GetMaxGauge()}";
        PVirusLevelTxt.text = $"Lv{character.virusPenalty["P"].penaltyLevel}";
        PVirusGaugeTxt.text = $"{character.virusPenalty["P"].penaltyGauge}/{character.virusPenalty["P"].GetMaxGauge()}";
        IVirusLevelTxt.text = $"Lv{character.virusPenalty["I"].penaltyLevel}";
        IVirusGaugeTxt.text = $"{character.virusPenalty["I"].penaltyGauge}/{character.virusPenalty["I"].GetMaxGauge()}";
        TVirusLevelTxt.text = $"Lv{character.virusPenalty["T"].penaltyLevel}";
        TVirusGaugeTxt.text = $"{character.virusPenalty["T"].penaltyGauge}/{character.virusPenalty["T"].GetMaxGauge()}";

        nameTxt.text = $"{character.character.name}";
        levelTxt.text = $"Level {character.level} / 분과";
        hpTxt.text = $"체력 {character.currentHp}";
        concentrationTxt.text = $"집중력 {character.concentration}";
        sensitivityTxt.text = $"예민함 {character.sensivity}";
        willpowerTxt.text = $"정신력 {character.willpower}";
    }

    public void CloseCharacterInfo()
    {
        bunkerMgr.upperUI.SetActive(true);
        characterInfoWin.SetActive(false);
        charcterListWin.SetActive(true);
    }

    public void OpenSkillPage()
    {
        characterInfoWin.SetActive(false);
        skillWinMgr.OpenSkillPage();
    }

    public void CloseSkillPage()
    {
        skillWinMgr.CloseSkillPage();
        characterInfoWin.SetActive(true);
    }

    public void OpenEquipmentWin()
    {
        characterInfoWin.SetActive(false);
        equipmentMgr.equipmentWin.SetActive(true);
        equipmentMgr.OpenEquipWin1();
    }

    public void CloseEquipmentWin()
    {
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
        if (virusWin.activeSelf) virusWin.SetActive(false);
        isAbilityWinOpen = !isAbilityWinOpen;

        //초기화.
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
                hpText.color = Color.black;
            }
            if (weightButton.GetComponent<Image>().color != Color.white)
            {
                weightButton.GetComponent<Image>().color = Color.white;
                weightTxt.color = Color.black;
            }
            if (mpButton.GetComponent<Image>().color != Color.white)
            {
                mpButton.GetComponent<Image>().color = Color.white;
                mpTxt.color = Color.black;
            }
            if (sightButton.GetComponent<Image>().color != Color.white)
            {
                sightButton.GetComponent<Image>().color = Color.white;
                sightTxt.color = Color.black;
            }

            if (accuracyButton.GetComponent<Image>().color != Color.white)
            {
                accuracyButton.GetComponent<Image>().color = Color.white;
                accuracyTxt.color = Color.black;
            }
            if (avoidButton.GetComponent<Image>().color != Color.white)
            {
                avoidButton.GetComponent<Image>().color = Color.white;
                avoidTxt.color = Color.black;
            }
            if (criButton.GetComponent<Image>().color != Color.white)
            {
                criButton.GetComponent<Image>().color = Color.white;
                criTxt.color = Color.black;
            }
            if (criResistButton.GetComponent<Image>().color != Color.white)
            {
                criResistButton.GetComponent<Image>().color = Color.white;
                criResistTxt.color = Color.black;
            }
        }
        statExplanationTxt.text = string.Empty;

        abilityWin.SetActive(isAbilityWinOpen);
    }

    public void VirusWin()
    {
        if (abilityWin.activeSelf) abilityWin.SetActive(false);
        isVirusWinOpen = !isVirusWinOpen;

        //초기화.
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
}

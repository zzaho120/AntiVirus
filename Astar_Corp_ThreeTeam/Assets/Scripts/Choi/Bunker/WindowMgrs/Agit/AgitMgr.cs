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
    public GameObject healthButton;
    public Text healthTxt;
    public GameObject concentrationButton;
    public Text concentrationText;
    public GameObject sensitiveButton;
    public Text sensitiveTxt;
    public GameObject mentalityButton;
    public Text mentalityTxt;

    public GameObject hpButton;
    public Text hpText;
    public GameObject weightButton;
    public Text weight;
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
        weight.text = $"{character.Weight + playerDataMgr.bagList["BAG_0001"].weight}";
        mpTxt.text = $"0";

        sightTxt.text = $"{character.sightDistance}";
        accuracyTxt.text = $"{character.accuracy}";
        avoidTxt.text = $"{character.avoidRate}";
        criTxt.text = $"{character.critRate}";
        criResistTxt.text = $"{character.critResistRate}";

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
        isAbilityWinOpen = !isAbilityWinOpen;
        abilityWin.SetActive(isAbilityWinOpen);
    }

    public void VirusWin()
    {
        isVirusWinOpen = !isVirusWinOpen;
        virusWin.SetActive(isVirusWinOpen);
    }
}

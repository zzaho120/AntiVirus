using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HospitalMgr : MonoBehaviour
{
    public BunkerMgr bunkerMgr;
    public GameObject mainWin;
    public GameObject hospitalWin;
    public GameObject upgradeWin;

    public Animator menuAnim;
    bool isMenuOpen;
    public GameObject arrowImg;

    public GameObject characterListContent;
    public GameObject characterPrefab;

    public Text recoveryCostTxt;
    public GameObject recoveryButton;

    //상태창.
    public List<Slider> sliders = new List<Slider>();
    public List<Button> buttons = new List<Button>();
    public List<Text> virusTxts = new List<Text>();

    [Header("Upgrade Win")]
    public Text hospitalLevelTxt;
    public Text costTxt;
    public Text materialTxt;

    public PlayerDataMgr playerDataMgr;

    List<GameObject> characterObjs = new List<GameObject>();
    int hospitalLevel;
    int reductionPercentage;
    int nextReductionPercentage;
    int upgradeCost;

    int virusRecoveryCost;
    int hpRecoveryCost;
    
    int currentIndex;
    int currentKey;
    Color originColor;

    //기준 수치.
    Dictionary<int, int> standardHp = new Dictionary<int, int>();
    Dictionary<int, int> standardBGauge = new Dictionary<int, int>();
    Dictionary<int, int> standardEGauge = new Dictionary<int, int>();
    Dictionary<int, int> standardIGauge = new Dictionary<int, int>();
    Dictionary<int, int> standardPGauge = new Dictionary<int, int>();
    Dictionary<int, int> standardTGauge = new Dictionary<int, int>();

    public void Init()
    {
        if (standardHp.Count != 0) standardHp.Clear();
        if (standardBGauge.Count != 0) standardBGauge.Clear();
        if (standardEGauge.Count != 0) standardEGauge.Clear();
        if (standardIGauge.Count != 0) standardIGauge.Clear();
        if (standardPGauge.Count != 0) standardPGauge.Clear();
        if (standardTGauge.Count != 0) standardTGauge.Clear();

        hospitalLevel = playerDataMgr.saveData.hospitalLevel;
        Bunker hospitalLevelInfo = playerDataMgr.bunkerList["BUN_0005"];
        switch (hospitalLevel)
        {
            case 1:
                reductionPercentage = hospitalLevelInfo.level1;
                nextReductionPercentage = hospitalLevelInfo.level2;
                upgradeCost = hospitalLevelInfo.level2Cost;
                break;
            case 2:
                reductionPercentage = hospitalLevelInfo.level2;
                nextReductionPercentage = hospitalLevelInfo.level3;
                upgradeCost = hospitalLevelInfo.level3Cost;
                break;
            case 3:
                reductionPercentage = hospitalLevelInfo.level3;
                nextReductionPercentage = hospitalLevelInfo.level4;
                upgradeCost = hospitalLevelInfo.level4Cost;
                break;
            case 4:
                reductionPercentage = hospitalLevelInfo.level4;
                nextReductionPercentage = hospitalLevelInfo.level5;
                upgradeCost = hospitalLevelInfo.level5Cost;
                break;
            case 5:
                reductionPercentage = hospitalLevelInfo.level5;
                break;
        }

        foreach (var element in buttons)
        {
            if (element.transform.GetComponent<Image>().color == Color.red)
                element.transform.GetComponent<Image>().color = Color.white;
        }

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
        
        //생성.
        int i = 0;
        foreach (var element in playerDataMgr.currentSquad)
        {
            var go = Instantiate(characterPrefab, characterListContent.transform);
            var childObj = go.transform.GetChild(0).gameObject;
            childObj.transform.GetChild(1).gameObject.GetComponent<Text>().text 
                = $"LV{element.Value.level}";

            childObj = go.transform.GetChild(1).gameObject;
            childObj.transform.GetChild(1).gameObject.GetComponent<Text>().text
                = $"{element.Value.character.name}";

            childObj = go.transform.GetChild(2).gameObject;
            childObj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                = (element.Value.weapon.mainWeapon == null) ? null : element.Value.weapon.mainWeapon.img;
            string mainWeaponTxt = (element.Value.weapon.mainWeapon == null) ?
                "비어있음" : $"{element.Value.weapon.mainWeapon.name}";
            childObj.transform.GetChild(1).gameObject.GetComponent<Text>().text
                = $"{mainWeaponTxt}";

            childObj = go.transform.GetChild(3).gameObject;
            childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text
                = $"{element.Value.character.name}";

            childObj = go.transform.GetChild(4).gameObject;
            childObj.transform.GetChild(0).gameObject.GetComponent<Slider>().maxValue = element.Value.MaxHp;
            childObj.transform.GetChild(0).gameObject.GetComponent<Slider>().value = element.Value.currentHp;
            childObj.transform.GetChild(1).gameObject.GetComponent<Text>().text
                = $"{element.Value.currentHp}/{element.Value.MaxHp}";

            characterObjs.Add(go);

            int num = i;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(num); });

            standardHp.Add(num, element.Value.currentHp);
            standardBGauge.Add(num, element.Value.virusPenalty["B"].penaltyGauge);
            standardEGauge.Add(num, element.Value.virusPenalty["E"].penaltyGauge);
            standardIGauge.Add(num, element.Value.virusPenalty["I"].penaltyGauge);
            standardPGauge.Add(num, element.Value.virusPenalty["P"].penaltyGauge);
            standardTGauge.Add(num, element.Value.virusPenalty["T"].penaltyGauge);

            i++;
        }

        virusRecoveryCost = 0;
        hpRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage));
        recoveryCostTxt.text = $"회복 비용 nnn";

        currentIndex = -1;
        currentKey = -1;
        originColor = characterPrefab.GetComponent<Image>().color;
        isMenuOpen = true;
        arrowImg.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void SelectCharacter(int index)
    {
        if (currentIndex == index) return;
        if (currentIndex != -1)
        {
            characterObjs[currentIndex].GetComponent<Image>().color = originColor;
        }

        currentIndex = index;
        characterObjs[currentIndex].GetComponent<Image>().color = Color.red;

        var character = playerDataMgr.currentSquad[currentIndex];

        sliders[0].maxValue = character.MaxHp;
        sliders[0].value = character.currentHp;

        sliders[1].maxValue = character.virusPenalty["E"].GetMaxGauge();
        sliders[1].value = character.virusPenalty["E"].penaltyGauge;
        virusTxts[0].text = $"E바이러스\n레벨 {character.virusPenalty["E"].penaltyLevel}";

        sliders[2].maxValue = character.virusPenalty["B"].GetMaxGauge();
        sliders[2].value = character.virusPenalty["B"].penaltyGauge;
        virusTxts[1].text = $"B바이러스\n레벨 {character.virusPenalty["B"].penaltyLevel}";

        sliders[3].maxValue = character.virusPenalty["P"].GetMaxGauge();
        sliders[3].value = character.virusPenalty["P"].penaltyGauge;
        virusTxts[2].text = $"P바이러스\n레벨 {character.virusPenalty["P"].penaltyLevel}";

        sliders[4].maxValue = character.virusPenalty["I"].GetMaxGauge();
        sliders[4].value = character.virusPenalty["I"].penaltyGauge;
        virusTxts[3].text = $"I바이러스\n레벨 {character.virusPenalty["I"].penaltyLevel}";

        sliders[5].maxValue = character.virusPenalty["T"].GetMaxGauge();
        sliders[5].value = character.virusPenalty["T"].penaltyGauge;
        virusTxts[4].text = $"T바이러스\n레벨 {character.virusPenalty["T"].penaltyLevel}";
    }

    public void SelectButton(int index)
    {
        if (currentKey != -1)
        {
            buttons[currentKey].gameObject.GetComponent<Image>().color = Color.white;
        }

        currentKey = index;
        buttons[currentKey].gameObject.GetComponent<Image>().color = Color.red;

        switch (index)
        {
            case 0:
                break;
            case 1:
                int level = playerDataMgr.currentSquad[currentIndex].virusPenalty["E"].penaltyLevel;
                virusRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage)) * level;
                break;
            case 2:
                level = playerDataMgr.currentSquad[currentIndex].virusPenalty["B"].penaltyLevel;
                virusRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage)) * level;
                break;
            case 3:
                level = playerDataMgr.currentSquad[currentIndex].virusPenalty["P"].penaltyLevel;
                virusRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage)) * level;
                break;
            case 4:
                level = playerDataMgr.currentSquad[currentIndex].virusPenalty["I"].penaltyLevel;
                virusRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage)) * level;
                break;
            case 5:
                level = playerDataMgr.currentSquad[currentIndex].virusPenalty["T"].penaltyLevel;
                virusRecoveryCost = (int)(400 * (1 - 0.01 * reductionPercentage)) * level;
                break;
        }
        
        if(index == 0)recoveryCostTxt.text = $"회복비용 {hpRecoveryCost}";
        else recoveryCostTxt.text = $"회복비용 {virusRecoveryCost}";
    }

    public void ClickRecoveryButton()
    {
        if (currentKey == 0) RecoveryHp();
        else RecoveryVirusGauge();
    }

    public void RecoveryHp()
    {
        if (currentIndex == -1) return;
        int cost = (int)(100 * (1 - 0.01 * reductionPercentage));
        if (playerDataMgr.saveData.money - cost < 0) return;
        
        int key = currentIndex;
        int loss = playerDataMgr.currentSquad[key].MaxHp - standardHp[key];
        int recoveryAmount = Mathf.FloorToInt(loss * 0.1f);
        playerDataMgr.currentSquad[key].currentHp += recoveryAmount;
        if (playerDataMgr.currentSquad[key].currentHp >= playerDataMgr.currentSquad[key].MaxHp)
            playerDataMgr.currentSquad[key].currentHp = playerDataMgr.currentSquad[key].MaxHp;

        playerDataMgr.saveData.hp[key] = playerDataMgr.currentSquad[key].currentHp;
        playerDataMgr.saveData.money -= 100;
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        bunkerMgr.moneyTxt.text = playerDataMgr.saveData.money.ToString();

        //체력 UI변경.
        sliders[0].maxValue = playerDataMgr.currentSquad[key].MaxHp;
        sliders[0].value = playerDataMgr.currentSquad[key].currentHp;
    }

    public void RecoveryVirusGauge()
    {
        if (currentIndex == -1) return;
        if (currentKey == -1) return;
        
        if (playerDataMgr.saveData.money - virusRecoveryCost < 0 ) return;
        playerDataMgr.saveData.money -= virusRecoveryCost;
        bunkerMgr.moneyTxt.text = playerDataMgr.saveData.money.ToString();

        int gauge = 0;
        int recoveryAmount = 0;
        int key = currentIndex;
        string currentVirus = null;
        switch (currentKey)
        {
            case 1:
                currentVirus = "E";
                gauge = standardEGauge[key];
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (playerDataMgr.currentSquad[key].virusPenalty["E"].penaltyGauge - recoveryAmount <= 0
                    && playerDataMgr.currentSquad[key].virusPenalty["E"].penaltyLevel != 1)
                    standardEGauge[key] = 100;

                playerDataMgr.currentSquad[key].virusPenalty["E"].ReductionCalculation(recoveryAmount);
                playerDataMgr.saveData.gaugeB[key] = playerDataMgr.currentSquad[key].virusPenalty["E"].penaltyGauge;
                playerDataMgr.saveData.levelB[key] = playerDataMgr.currentSquad[key].virusPenalty["E"].penaltyLevel;
                break;
            case 2:
                currentVirus = "B";
                gauge = standardBGauge[key];
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (playerDataMgr.currentSquad[key].virusPenalty["B"].penaltyGauge - recoveryAmount <= 0
                    && playerDataMgr.currentSquad[key].virusPenalty["B"].penaltyLevel != 1)
                    standardBGauge[key] = 100;

                playerDataMgr.currentSquad[key].virusPenalty["B"].ReductionCalculation(recoveryAmount);
                playerDataMgr.saveData.gaugeE[key] = playerDataMgr.currentSquad[key].virusPenalty["B"].penaltyGauge;
                playerDataMgr.saveData.levelE[key] = playerDataMgr.currentSquad[key].virusPenalty["B"].penaltyLevel;
                break;
            case 3:
                currentVirus = "P";
                gauge = standardPGauge[key];
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (playerDataMgr.currentSquad[key].virusPenalty["P"].penaltyGauge - recoveryAmount <= 0
                    && playerDataMgr.currentSquad[key].virusPenalty["P"].penaltyLevel != 1)
                    standardPGauge[key] = 100;

                playerDataMgr.currentSquad[key].virusPenalty["P"].ReductionCalculation(recoveryAmount);
                playerDataMgr.saveData.gaugeP[key] = playerDataMgr.currentSquad[key].virusPenalty["P"].penaltyGauge;
                playerDataMgr.saveData.levelP[key] = playerDataMgr.currentSquad[key].virusPenalty["P"].penaltyLevel;
                break;
            case 4:
                currentVirus = "I";
                gauge = standardIGauge[key];
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (playerDataMgr.currentSquad[key].virusPenalty["I"].penaltyGauge - recoveryAmount <= 0
                    && playerDataMgr.currentSquad[key].virusPenalty["I"].penaltyLevel != 1)
                    standardIGauge[key] = 100;

                playerDataMgr.currentSquad[key].virusPenalty["I"].ReductionCalculation(recoveryAmount);
                playerDataMgr.saveData.gaugeI[key] = playerDataMgr.currentSquad[key].virusPenalty["I"].penaltyGauge;
                playerDataMgr.saveData.levelI[key] = playerDataMgr.currentSquad[key].virusPenalty["I"].penaltyLevel;
                break;
            case 5:
                currentVirus = "T";
                gauge = standardTGauge[key];
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (playerDataMgr.currentSquad[key].virusPenalty["T"].penaltyGauge - recoveryAmount <= 0
                    && playerDataMgr.currentSquad[key].virusPenalty["T"].penaltyLevel != 1)
                    standardTGauge[key] = 100;

                playerDataMgr.currentSquad[key].virusPenalty["T"].ReductionCalculation(recoveryAmount);
                playerDataMgr.saveData.gaugeT[key] = playerDataMgr.currentSquad[key].virusPenalty["T"].penaltyGauge;
                playerDataMgr.saveData.levelT[key] = playerDataMgr.currentSquad[key].virusPenalty["T"].penaltyLevel;
                break;
        }
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        //UI변경.
        sliders[currentKey].maxValue = playerDataMgr.currentSquad[key].virusPenalty[currentVirus].GetMaxGauge();
        sliders[currentKey].value = playerDataMgr.currentSquad[key].virusPenalty[currentVirus].penaltyGauge;
    }

    public void RefreshUpgradeWin()
    {
        if (hospitalLevel != 5)
        {
            hospitalLevelTxt.text = $"건물 레벨 {hospitalLevel}→{hospitalLevel + 1}";
            costTxt.text = $"체력 회복 비용 감소 {reductionPercentage}%→{nextReductionPercentage}%\n"
                +$"바이러스 회복 비용 감소 {reductionPercentage}%→{nextReductionPercentage}%";
            materialTxt.text = $"{upgradeCost}";
        }
        else
        {
            hospitalLevelTxt.text = $"건물 레벨{hospitalLevel}→ -";
            costTxt.text = $"체력 회복 비용 감소 {reductionPercentage}%→ -%\n"
                + $"바이러스 회복 비용 감소 {reductionPercentage}%→ -%";
            materialTxt.text = $"-";
        }
    }

    //창 관련.
    public void OpenMainWin()
    {
        if (bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(false);
        if (bunkerMgr.mapButton.activeSelf) bunkerMgr.mapButton.SetActive(false);
        if (!mainWin.activeSelf) mainWin.SetActive(true);
        if (hospitalWin.activeSelf) hospitalWin.SetActive(false);
        if (upgradeWin.activeSelf) upgradeWin.SetActive(false);
    }

    public void CloseMainWin()
    {
        if (!bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(true);
        if (!bunkerMgr.mapButton.activeSelf) bunkerMgr.mapButton.SetActive(true);
    }

    public void Menu()
    {
        arrowImg.GetComponent<RectTransform>().rotation = (isMenuOpen) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.Euler(0f, 0f, 0f);
        isMenuOpen = !isMenuOpen;
        menuAnim.SetBool("isOpen", isMenuOpen);
    }

    public void OpenHospitalWin()
    {
        mainWin.SetActive(false);
        hospitalWin.SetActive(true);
    }

    public void CloseHospitalWin()
    {
        hospitalWin.SetActive(false);
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
}
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
    public GameObject newCharacterPrefab;

    public Text moneyText;
    public Text selectedCost;
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

    int EVirusRecoveryCost;
    int BVirusRecoveryCost;
    int PVirusRecoveryCost;
    int IVirusRecoveryCost;
    int TVirusRecoveryCost;
    int hpRecoveryCost;
    
    int currentIndex;
    int currentKey;
    //Color originColor;

    //기준 수치.
    Dictionary<int, int> standardHp = new Dictionary<int, int>();
    Dictionary<int, int> standardBGauge = new Dictionary<int, int>();
    Dictionary<int, int> standardEGauge = new Dictionary<int, int>();
    Dictionary<int, int> standardIGauge = new Dictionary<int, int>();
    Dictionary<int, int> standardPGauge = new Dictionary<int, int>();
    Dictionary<int, int> standardTGauge = new Dictionary<int, int>();

    Dictionary<int, int> costs = new Dictionary<int, int>();

    public void Init()
    {
        if (standardHp.Count != 0) standardHp.Clear();
        if (standardBGauge.Count != 0) standardBGauge.Clear();
        if (standardEGauge.Count != 0) standardEGauge.Clear();
        if (standardIGauge.Count != 0) standardIGauge.Clear();
        if (standardPGauge.Count != 0) standardPGauge.Clear();
        if (standardTGauge.Count != 0) standardTGauge.Clear();
        if (costs.Count != 0) costs.Clear();

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
            var go = Instantiate(newCharacterPrefab, characterListContent.transform);
            var childObj = go.transform.GetChild(0).gameObject;
            childObj.transform.GetChild(1).gameObject.GetComponent<Text>().text 
                = $"LV{element.Value.level}";
            var toggle = childObj.transform.GetChild(2).gameObject.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate { CauculateTotal(); });
            var expSlider = childObj.transform.GetChild(3).gameObject.GetComponent<Slider>();
            expSlider.maxValue = element.Value.totalExp;
            expSlider.value = element.Value.currentExp;
            childObj.transform.GetChild(4).gameObject.GetComponent<Text>().text
                = $"{element.Value.currentExp}/{element.Value.totalExp}";

            childObj = go.transform.GetChild(1).gameObject;
            childObj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite
                = element.Value.character.icon;
            childObj.transform.GetChild(1).gameObject.GetComponent<Text>().text
                = $"{element.Value.character.name}";

            childObj = go.transform.GetChild(2).gameObject;
            childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text
                = $"{element.Value.character.name}";

            childObj = go.transform.GetChild(3).gameObject;
            childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text
                = $"{element.Value.currentHp}/{element.Value.MaxHp}";
            var slider = childObj.transform.GetChild(1).gameObject.GetComponent<Slider>();
            slider.maxValue = element.Value.MaxHp;
            slider.value = element.Value.currentHp;

            var virusGroup = childObj.transform.GetChild(2).gameObject;
            string[] virusName = new string[5];
            virusName[0] = "E";
            virusName[1] = "B";
            virusName[2] = "P";
            virusName[3] = "I";
            virusName[4] = "T";

            for (int j = 0; j < virusName.Length; j++)
            {
                if (!(element.Value.virusPenalty[virusName[j]].penaltyLevel == 0 &&
                    element.Value.virusPenalty[virusName[j]].penaltyGauge ==0))
                    virusGroup.transform.GetChild(j).gameObject.SetActive(true);
                else virusGroup.transform.GetChild(j).gameObject.SetActive(false);
            }

            childObj = go.transform.GetChild(4).gameObject;
            childObj = childObj.transform.GetChild(0).gameObject;
            var button = childObj.GetComponent<Button>();
            button.onClick.AddListener(TreatAll);
            
            int num = i;

            standardHp.Add(num, element.Value.currentHp);
            standardBGauge.Add(num, element.Value.virusPenalty["B"].penaltyGauge);
            standardEGauge.Add(num, element.Value.virusPenalty["E"].penaltyGauge);
            standardIGauge.Add(num, element.Value.virusPenalty["I"].penaltyGauge);
            standardPGauge.Add(num, element.Value.virusPenalty["P"].penaltyGauge);
            standardTGauge.Add(num, element.Value.virusPenalty["T"].penaltyGauge);

            var cost = CalculateCost(num);
            childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"G {cost}";
            costs.Add(num, cost);

            characterObjs.Add(go);

            button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(num); });

            i++;
        }

        EVirusRecoveryCost = 0;
        BVirusRecoveryCost = 0;
        PVirusRecoveryCost = 0;
        IVirusRecoveryCost = 0;
        TVirusRecoveryCost = 0;
        hpRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage));
        recoveryCostTxt.text = $"회복 비용 nnn";

        currentIndex = -1;
        currentKey = -1;
        //originColor = characterPrefab.GetComponent<Image>().color;
        isMenuOpen = true;
        moneyText.text = $"보유 금액 G {playerDataMgr.saveData.money}";

        arrowImg.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void SelectCharacter(int index)
    {
        if (currentIndex == index) return;
        //if (currentIndex != -1)
        //{
        //    characterObjs[currentIndex].GetComponent<Image>().color = Color.black;
        //}

        currentIndex = index;
        //characterObjs[currentIndex].GetComponent<Image>().color = Color.red;

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
                EVirusRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage)) * level;
                break;
            case 2:
                level = playerDataMgr.currentSquad[currentIndex].virusPenalty["B"].penaltyLevel;
                BVirusRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage)) * level;
                break;
            case 3:
                level = playerDataMgr.currentSquad[currentIndex].virusPenalty["P"].penaltyLevel;
                PVirusRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage)) * level;
                break;
            case 4:
                level = playerDataMgr.currentSquad[currentIndex].virusPenalty["I"].penaltyLevel;
                IVirusRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage)) * level;
                break;
            case 5:
                level = playerDataMgr.currentSquad[currentIndex].virusPenalty["T"].penaltyLevel;
                TVirusRecoveryCost = (int)(400 * (1 - 0.01 * reductionPercentage)) * level;
                break;
        }
        
        //if(index == 0)recoveryCostTxt.text = $"회복비용 {hpRecoveryCost}";
        //else recoveryCostTxt.text = $"회복비용 {virusRecoveryCost}";
    }

    //public void ClickRecoveryButton()
    //{
    //    if (currentKey == 0) RecoveryHp();
    //    else RecoveryVirusGauge();
    //}

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
        moneyText.text = $"보유 금액 G {playerDataMgr.saveData.money}";

        //체력 UI변경.
        sliders[0].maxValue = playerDataMgr.currentSquad[key].MaxHp;
        sliders[0].value = playerDataMgr.currentSquad[key].currentHp;
    }

    public int CalculateCost(int key)
    {
        int totalCost = 0;

        int currentHp = playerDataMgr.currentSquad[key].currentHp;
        while (true)
        {
            if (currentHp >= playerDataMgr.currentSquad[key].MaxHp)
            {
                break;
            }

            int cost = (int)(100 * (1 - 0.01 * reductionPercentage));
            totalCost += cost;
            
            int loss = playerDataMgr.currentSquad[key].MaxHp - standardHp[key];
            int recoveryHpAmount = Mathf.FloorToInt(loss * 0.1f);
            currentHp += recoveryHpAmount;
        }

        int gauge = 0;
        int recoveryAmount = 0;
        
        string[] virusName = new string[5];
        virusName[0] = "E";
        virusName[1] = "B";
        virusName[2] = "P";
        virusName[3] = "I";
        virusName[4] = "T";

        for (int i = 0; i < virusName.Length; i++)
        {
            int viruslevel = playerDataMgr.currentSquad[key].virusPenalty[virusName[i]].penaltyLevel;
            int virusGauge = playerDataMgr.currentSquad[key].virusPenalty[virusName[i]].penaltyGauge;
            int standard = 0;
            switch (i)
            {
                case 0:
                    standard = standardEGauge[key];
                    break;
                case 1:
                    standard = standardBGauge[key];
                    break;
                case 2:
                    standard = standardPGauge[key];
                    break;
                case 3:
                    standard = standardIGauge[key];
                    break;
                case 4:
                    standard = standardTGauge[key];
                    break;
            }
            
            while (true)
            {
                if (viruslevel == 0 && virusGauge == 0) break;

                int cost = 0;
                if(i != 4) cost = (int)(100 * (1 - 0.01 * reductionPercentage)) * viruslevel;
                else cost = (int)(400 * (1 - 0.01 * reductionPercentage)) * viruslevel;
                totalCost += cost;

                gauge = standard;
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (virusGauge - recoveryAmount <= 0 && viruslevel != 0) standard = 100;

                virusGauge -= recoveryAmount;
                var previousMax = 100 * viruslevel;
                if (virusGauge < 0)
                {
                    if (viruslevel != 0)
                    {
                        virusGauge += previousMax;
                        viruslevel--;
                    }
                    else if (viruslevel == 0)
                    {
                        virusGauge = 0;
                        viruslevel = 0;
                    }
                }
            }
        }

        return totalCost;
    }

    public void TreatAll()
    {
        if (currentIndex == -1) return;
        int totalCost = costs[currentIndex];
        if (totalCost == 0) return;
        if (playerDataMgr.saveData.money - totalCost < 0) return;

        int key = currentIndex;
        playerDataMgr.currentSquad[key].currentHp = playerDataMgr.currentSquad[key].MaxHp;
        playerDataMgr.saveData.hp[key] = playerDataMgr.currentSquad[key].currentHp;
        playerDataMgr.saveData.money -= totalCost;
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        bunkerMgr.moneyTxt.text = playerDataMgr.saveData.money.ToString();
        moneyText.text = $"보유 금액 G {playerDataMgr.saveData.money}";

        var child = characterObjs[key].transform.GetChild(3).gameObject;
        child.transform.GetChild(0).gameObject.GetComponent<Text>().text
            = $"{playerDataMgr.currentSquad[key].currentHp}/{playerDataMgr.currentSquad[key].MaxHp}";
        var slider = child.transform.GetChild(1).gameObject.GetComponent<Slider>();
        slider.maxValue = playerDataMgr.currentSquad[key].MaxHp;
        slider.value = playerDataMgr.currentSquad[key].currentHp;

        var virusGroup = child.transform.GetChild(2).gameObject;
        string[] virusName = new string[5];
        virusName[0] = "E";
        virusName[1] = "B";
        virusName[2] = "P";
        virusName[3] = "I";
        virusName[4] = "T";

        for (int j = 0; j < virusName.Length; j++)
        {
            virusGroup.transform.GetChild(j).gameObject.SetActive(false);
        }

        child = characterObjs[key].transform.GetChild(4).gameObject;
        child = child.transform.GetChild(0).gameObject;
        child.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"G {0}";
        costs[currentIndex] = 0;

        for (int j = 0; j < virusName.Length; j++)
        {
            playerDataMgr.currentSquad[key].virusPenalty[virusName[j]].penaltyLevel = 0;
            playerDataMgr.currentSquad[key].virusPenalty[virusName[j]].penaltyGauge = 0;

            switch (j)
            {
                case 0:
                    playerDataMgr.saveData.levelE[key] = 0;
                    playerDataMgr.saveData.gaugeE[key] = 0;
                    break;
                case 1:
                    playerDataMgr.saveData.levelB[key] = 0;
                    playerDataMgr.saveData.gaugeB[key] = 0;
                    break;
                case 2:
                    playerDataMgr.saveData.levelP[key] = 0;
                    playerDataMgr.saveData.gaugeP[key] = 0;
                    break;
                case 3:
                    playerDataMgr.saveData.levelI[key] = 0;
                    playerDataMgr.saveData.gaugeI[key] = 0;
                    break;
                case 4:
                    playerDataMgr.saveData.levelT[key] = 0;
                    playerDataMgr.saveData.gaugeT[key] = 0;
                    break;
            }
        }
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
    }

    public void CauculateTotal()
    {
        bunkerMgr.PlayClickSound();

        int sum = 0;
        int i = 0;
        foreach (var element in characterObjs)
        { 
            var child  = element.transform.GetChild(0).gameObject;
            var toggle = child.transform.GetChild(2).gameObject.GetComponent<Toggle>();
            if (toggle.isOn) sum += costs[i];
            i++;
        }

        selectedCost.text = $"G {sum}";
        var button = recoveryButton.GetComponent<Button>();
        button.enabled = (sum==0)?false : true;
    }

    public void SelectTreat()
    {
        string[] splits = selectedCost.text.Split(' ');
        int cost = int.Parse(splits[1]);
        if (cost == 0) return;

        int i = 0;
        foreach (var element in characterObjs)
        {
            var child = element.transform.GetChild(0).gameObject;
            var toggle = child.transform.GetChild(2).gameObject.GetComponent<Toggle>();
            if (toggle.isOn)
            {
                currentIndex = i;
                TreatAll();
            }
            i++;
        }

        foreach (var element in characterObjs)
        {
            var child = element.transform.GetChild(0).gameObject;
            var toggle = child.transform.GetChild(2).gameObject.GetComponent<Toggle>();
            if (toggle.isOn) toggle.isOn = false;
        }
        selectedCost.text = $"G 0";

        currentIndex = -1;
    }

    //public void RecoveryVirusGauge()
    //{
    //    if (currentIndex == -1) return;
    //    if (currentKey == -1) return;
        
    //    if (playerDataMgr.saveData.money - virusRecoveryCost < 0 ) return;
    //    playerDataMgr.saveData.money -= virusRecoveryCost;
    //    bunkerMgr.moneyTxt.text = playerDataMgr.saveData.money.ToString();

    //    int gauge = 0;
    //    int recoveryAmount = 0;
    //    int key = currentIndex;
    //    string currentVirus = null;
    //    switch (currentKey)
    //    {
    //        case 1:
    //            currentVirus = "E";
    //            gauge = standardEGauge[key];
    //            recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
    //            if (playerDataMgr.currentSquad[key].virusPenalty["E"].penaltyGauge - recoveryAmount <= 0
    //                && playerDataMgr.currentSquad[key].virusPenalty["E"].penaltyLevel != 1)
    //                standardEGauge[key] = 100;

    //            playerDataMgr.currentSquad[key].virusPenalty["E"].ReductionCalculation(recoveryAmount);
    //            playerDataMgr.saveData.gaugeB[key] = playerDataMgr.currentSquad[key].virusPenalty["E"].penaltyGauge;
    //            playerDataMgr.saveData.levelB[key] = playerDataMgr.currentSquad[key].virusPenalty["E"].penaltyLevel;
    //            break;
    //        case 2:
    //            currentVirus = "B";
    //            gauge = standardBGauge[key];
    //            recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
    //            if (playerDataMgr.currentSquad[key].virusPenalty["B"].penaltyGauge - recoveryAmount <= 0
    //                && playerDataMgr.currentSquad[key].virusPenalty["B"].penaltyLevel != 1)
    //                standardBGauge[key] = 100;

    //            playerDataMgr.currentSquad[key].virusPenalty["B"].ReductionCalculation(recoveryAmount);
    //            playerDataMgr.saveData.gaugeE[key] = playerDataMgr.currentSquad[key].virusPenalty["B"].penaltyGauge;
    //            playerDataMgr.saveData.levelE[key] = playerDataMgr.currentSquad[key].virusPenalty["B"].penaltyLevel;
    //            break;
    //        case 3:
    //            currentVirus = "P";
    //            gauge = standardPGauge[key];
    //            recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
    //            if (playerDataMgr.currentSquad[key].virusPenalty["P"].penaltyGauge - recoveryAmount <= 0
    //                && playerDataMgr.currentSquad[key].virusPenalty["P"].penaltyLevel != 1)
    //                standardPGauge[key] = 100;

    //            playerDataMgr.currentSquad[key].virusPenalty["P"].ReductionCalculation(recoveryAmount);
    //            playerDataMgr.saveData.gaugeP[key] = playerDataMgr.currentSquad[key].virusPenalty["P"].penaltyGauge;
    //            playerDataMgr.saveData.levelP[key] = playerDataMgr.currentSquad[key].virusPenalty["P"].penaltyLevel;
    //            break;
    //        case 4:
    //            currentVirus = "I";
    //            gauge = standardIGauge[key];
    //            recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
    //            if (playerDataMgr.currentSquad[key].virusPenalty["I"].penaltyGauge - recoveryAmount <= 0
    //                && playerDataMgr.currentSquad[key].virusPenalty["I"].penaltyLevel != 1)
    //                standardIGauge[key] = 100;

    //            playerDataMgr.currentSquad[key].virusPenalty["I"].ReductionCalculation(recoveryAmount);
    //            playerDataMgr.saveData.gaugeI[key] = playerDataMgr.currentSquad[key].virusPenalty["I"].penaltyGauge;
    //            playerDataMgr.saveData.levelI[key] = playerDataMgr.currentSquad[key].virusPenalty["I"].penaltyLevel;
    //            break;
    //        case 5:
    //            currentVirus = "T";
    //            gauge = standardTGauge[key];
    //            recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
    //            if (playerDataMgr.currentSquad[key].virusPenalty["T"].penaltyGauge - recoveryAmount <= 0
    //                && playerDataMgr.currentSquad[key].virusPenalty["T"].penaltyLevel != 1)
    //                standardTGauge[key] = 100;

    //            playerDataMgr.currentSquad[key].virusPenalty["T"].ReductionCalculation(recoveryAmount);
    //            playerDataMgr.saveData.gaugeT[key] = playerDataMgr.currentSquad[key].virusPenalty["T"].penaltyGauge;
    //            playerDataMgr.saveData.levelT[key] = playerDataMgr.currentSquad[key].virusPenalty["T"].penaltyLevel;
    //            break;
    //    }
    //    PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

    //    //UI변경.
    //    sliders[currentKey].maxValue = playerDataMgr.currentSquad[key].virusPenalty[currentVirus].GetMaxGauge();
    //    sliders[currentKey].value = playerDataMgr.currentSquad[key].virusPenalty[currentVirus].penaltyGauge;
    //}

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
        if (!mainWin.activeSelf) mainWin.SetActive(true);
        if (hospitalWin.activeSelf) hospitalWin.SetActive(false);
        if (upgradeWin.activeSelf) upgradeWin.SetActive(false);

       var button = recoveryButton.GetComponent<Button>();
        button.enabled = false;
    }

    public void CloseMainWin()
    {
        if (!bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(true);
    }

    public void Menu()
    {
        arrowImg.GetComponent<RectTransform>().rotation = (isMenuOpen) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.Euler(0f, 0f, 0f);
        isMenuOpen = !isMenuOpen;
        menuAnim.SetBool("isOpen", isMenuOpen);
    }

    public void OpenHospitalWin()
    {
        //foreach (var elememt in characterObjs)
        //{
        //    if(elememt.GetComponent<Image>().color == Color.red)
        //        elememt.GetComponent<Image>().color = Color.black;
        //}

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
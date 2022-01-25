using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HospitalMgr : MonoBehaviour
{
    public GameObject hospitalWin;
    public GameObject characterListContent;
    public GameObject characterPrefab;
    public GameObject stateWin;
    public GameObject hpRecoveryButton;
    public GameObject virusRecoveryButton;

    //상태창.
    public Slider hpSlider;
    public Slider eVirusSlider;
    public Slider bVirusSlider;
    public Slider pVirusSlider;
    public Slider iVirusSlider;
    public Slider tVirusSlider;
    public Dictionary<string, Slider> sliders = new Dictionary<string, Slider>();

    public PlayerDataMgr playerDataMgr;

    List<GameObject> characterObjs = new List<GameObject>();
    int hospitalLevel;

    int virusRecoveryCost;
    int hpRecoveryCost;
    int reductionPercentage;
    
    int currentIndex;
    string currentVirus;
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
                break;
            case 2:
                reductionPercentage = hospitalLevelInfo.level2;
                break;
            case 3:
                reductionPercentage = hospitalLevelInfo.level3;
                break;
            case 4:
                reductionPercentage = hospitalLevelInfo.level4;
                break;
            case 5:
                reductionPercentage = hospitalLevelInfo.level5;
                break;
        }

        if (sliders.Count == 0) 
        {
            sliders.Add("E", eVirusSlider);
            sliders.Add("B", bVirusSlider);
            sliders.Add("P", pVirusSlider);
            sliders.Add("I", iVirusSlider);
            sliders.Add("T", tVirusSlider);
        }

        foreach (var element in sliders)
        {
            if (element.Value.transform.GetChild(0).GetComponent<Image>().color == Color.red)
                element.Value.transform.GetChild(0).GetComponent<Image>().color = Color.white;
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
        
        if (stateWin.activeSelf) stateWin.SetActive(false);

        //생성.
        int i = 0;
        foreach (var element in playerDataMgr.currentSquad)
        {
            var go = Instantiate(characterPrefab, characterListContent.transform);
            go.transform.GetChild(1).GetComponent<Text>().text = element.Value.character.name;
            characterObjs.Add(go);

            int num = i;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(num); });

            standardHp.Add(num, element.Value.currentHp);
            standardBGauge.Add(num, element.Value.virusPanalty["B"].penaltyGauge);
            standardEGauge.Add(num, element.Value.virusPanalty["E"].penaltyGauge);
            standardIGauge.Add(num, element.Value.virusPanalty["I"].penaltyGauge);
            standardPGauge.Add(num, element.Value.virusPanalty["P"].penaltyGauge);
            standardTGauge.Add(num, element.Value.virusPanalty["T"].penaltyGauge);

            i++;
        }

        virusRecoveryCost = 0;
        hpRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage));
        var child = hpRecoveryButton.transform.GetChild(1).gameObject;
        child.GetComponent<Text>().text = $"{ hpRecoveryCost}";

        currentIndex = -1;
        currentVirus = null;
        originColor = characterPrefab.GetComponent<Image>().color;
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

        //상태창 관리.
        stateWin.SetActive(true);

        var character = playerDataMgr.currentSquad[currentIndex];
        
        //테스트.
        //character.currentHp = 20;
        //hpSlider.value = (float)character.currentHp / character.maxHp;

        //테스트.
        //character.virusPanalty["E"].penaltyGauge = 30;
        //character.virusPanalty["B"].penaltyGauge = 50;
        //character.virusPanalty["P"].penaltyGauge = 100;
        //character.virusPanalty["I"].penaltyGauge = 140;
        //character.virusPanalty["T"].penaltyGauge = 180;

        eVirusSlider.value = (float)character.virusPanalty["E"].penaltyGauge / character.virusPanalty["E"].GetMaxGauge();
        bVirusSlider.value = (float)character.virusPanalty["B"].penaltyGauge / character.virusPanalty["B"].GetMaxGauge();
        pVirusSlider.value = (float)character.virusPanalty["P"].penaltyGauge / character.virusPanalty["P"].GetMaxGauge();
        iVirusSlider.value = (float)character.virusPanalty["I"].penaltyGauge / character.virusPanalty["I"].GetMaxGauge();
        tVirusSlider.value = (float)character.virusPanalty["T"].penaltyGauge / character.virusPanalty["T"].GetMaxGauge();
    }

    public void SelectVirus(string str)
    {
        GameObject child;
        if (currentVirus != null)
        {
            child = sliders[currentVirus].transform.GetChild(0).gameObject;
            child.GetComponent<Image>().color = Color.white;
        }

        currentVirus = str;
        child = sliders[currentVirus].transform.GetChild(0).gameObject;
        child.GetComponent<Image>().color = Color.red;

        int level = level = playerDataMgr.currentSquad[currentIndex].virusPanalty[str].penaltyLevel;
        switch (str)
        {
            case "E":
            case "B":
            case "P":
            case "I":
                 virusRecoveryCost = (int)(100 * (1 - 0.01 * reductionPercentage)) * level;
                break;
            case "T":
                virusRecoveryCost = (int)(400 * (1 - 0.01 * reductionPercentage)) * level;
                break;
        }

        child = virusRecoveryButton.transform.GetChild(1).gameObject;
        child.GetComponent<Text>().text = $"{virusRecoveryCost}";
    }

    public void RecoveryHp()
    {
        if (currentIndex == -1) return;
        if (playerDataMgr.saveData.money - 100 < 0) return;
        
        int key = currentIndex;
        int loss = playerDataMgr.currentSquad[key].MaxHp - standardHp[key];
        int recoveryAmount = Mathf.FloorToInt(loss * 0.1f);
        playerDataMgr.currentSquad[key].currentHp += recoveryAmount;
        if (playerDataMgr.currentSquad[key].currentHp >= playerDataMgr.currentSquad[key].MaxHp)
            playerDataMgr.currentSquad[key].currentHp = playerDataMgr.currentSquad[key].MaxHp;

        playerDataMgr.saveData.hp[key] = playerDataMgr.currentSquad[key].currentHp;
        playerDataMgr.saveData.money -= 100;
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        //체력 UI변경.
        hpSlider.value = (float)playerDataMgr.currentSquad[key].currentHp / playerDataMgr.currentSquad[key].MaxHp;
    }

    public void RecoveryVirusGauge()
    {
        if (currentIndex == -1) return;
        if (currentVirus == null) return;
        
        int key = currentIndex;
        int level = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel;

        if (playerDataMgr.saveData.money - (100 * level) < 0 && !currentVirus.Equals("T")) return;
        if (playerDataMgr.saveData.money - (400 * level) < 0 && currentVirus.Equals("T")) return;

        if (!currentVirus.Equals("T")) playerDataMgr.saveData.money -= (100 * level);
        if (currentVirus.Equals("T")) playerDataMgr.saveData.money -= (400 * level);

        int gauge = 0;
        int recoveryAmount = 0;
        switch (currentVirus)
        {
            case "B":
                gauge = standardBGauge[key];
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge - recoveryAmount <= 0
                    && playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel != 1)
                    standardBGauge[key] = 100;

                playerDataMgr.currentSquad[key].virusPanalty[currentVirus].ReductionCalculation(recoveryAmount);
                playerDataMgr.saveData.gaugeB[key] = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge;
                playerDataMgr.saveData.levelB[key] = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel;
                break;
            case "E":
                gauge = standardEGauge[key];
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge - recoveryAmount <= 0
                    && playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel != 1)
                    standardEGauge[key] = 100;

                playerDataMgr.currentSquad[key].virusPanalty[currentVirus].ReductionCalculation(recoveryAmount);
                playerDataMgr.saveData.gaugeE[key] = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge;
                playerDataMgr.saveData.levelE[key] = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel;
                break;
            case "P":
                gauge = standardPGauge[key];
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge - recoveryAmount <= 0
                    && playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel != 1)
                    standardPGauge[key] = 100;

                playerDataMgr.currentSquad[key].virusPanalty[currentVirus].ReductionCalculation(recoveryAmount);
                playerDataMgr.saveData.gaugeP[key] = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge;
                playerDataMgr.saveData.levelP[key] = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel;
                break;
            case "I":
                gauge = standardIGauge[key];
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge - recoveryAmount <= 0
                    && playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel != 1)
                    standardIGauge[key] = 100;

                playerDataMgr.currentSquad[key].virusPanalty[currentVirus].ReductionCalculation(recoveryAmount);
                playerDataMgr.saveData.gaugeI[key] = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge;
                playerDataMgr.saveData.levelI[key] = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel;
                break;
            case "T":
                gauge = standardTGauge[key];
                recoveryAmount = Mathf.FloorToInt(gauge * 0.1f);
                if (playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge - recoveryAmount <= 0
                    && playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel != 1)
                    standardTGauge[key] = 100;

                playerDataMgr.currentSquad[key].virusPanalty[currentVirus].ReductionCalculation(recoveryAmount);
                playerDataMgr.saveData.gaugeT[key] = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge;
                playerDataMgr.saveData.levelT[key] = playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyLevel;
                break;
        }
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        //UI변경.
        sliders[currentVirus].value
            = (float)playerDataMgr.currentSquad[key].virusPanalty[currentVirus].penaltyGauge
            / playerDataMgr.currentSquad[key].virusPanalty[currentVirus].GetMaxGauge();
    }
}
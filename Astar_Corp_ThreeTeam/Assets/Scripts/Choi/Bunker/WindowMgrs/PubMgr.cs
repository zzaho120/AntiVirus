using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PubMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;

    public GameObject mainWin;
    public GameObject recruitmentWin;
    public GameObject hintCollectionWin;

    //용병 모집 창.
    public GameObject characterListContent;
    public GameObject characterPrefab;
    public GameObject DetailInfoWin;
    public Text hpTxt;
    public Text conTxt;
    public Text senTxt;
    public Text willPowerTxt;
    Dictionary<int, GameObject> characterObjs = new Dictionary<int, GameObject>();

    //힌트 수집 창.
    public List<GameObject> hintList;
    public GameObject hintPopup;
    public Text hintContents;

    int pubLevel;
    int maxSoldierNum;
    int maxHintNum;
    int currentIndex;
    int currentHintIndex;
    Color originColor;

    List<string> characterName = new List<string>();
    Dictionary<int, CharacterStats> soliders = new Dictionary<int, CharacterStats>();
    Dictionary<int, int> costs = new Dictionary<int, int>();

    public void Init()
    {
        pubLevel = playerDataMgr.saveData.pubLevel;
        Bunker pubSoldierLevelInfo = playerDataMgr.bunkerList["BUN_0007"];
        Bunker pubHintLevelInfo = playerDataMgr.bunkerList["BUN_0008"];
        switch (pubLevel)
        {
            case 1:
                maxSoldierNum = pubSoldierLevelInfo.level1;
                maxHintNum = pubHintLevelInfo.level1;
                break;
            case 2:
                maxSoldierNum = pubSoldierLevelInfo.level2;
                maxHintNum = pubHintLevelInfo.level2;
                break;
            case 3:
                maxSoldierNum = pubSoldierLevelInfo.level3;
                maxHintNum = pubHintLevelInfo.level3;
                break;
            case 4:
                maxSoldierNum = pubSoldierLevelInfo.level4;
                maxHintNum = pubHintLevelInfo.level4;
                break;
            case 5:
                maxSoldierNum = pubSoldierLevelInfo.level5;
                maxHintNum = pubHintLevelInfo.level5;
                break;
        }

        if (!mainWin.activeSelf) mainWin.SetActive(true);
        if (recruitmentWin.activeSelf) recruitmentWin.SetActive(false);
        if (hintCollectionWin.activeSelf) hintCollectionWin.SetActive(false);

        //이전 정보 삭제.
        if (characterObjs.Count != 0)
        {
            foreach (var element in characterObjs)
            {
                Destroy(element.Value);
            }
            characterObjs.Clear();

            characterListContent.transform.DetachChildren();
        }
        
        //랜덤 용병 생성.
        foreach (var element in playerDataMgr.characterList)
        {
            characterName.Add(element.Key); 
        }

        if (soliders.Count != 0) soliders.Clear();
        if (costs.Count != 0) costs.Clear();
        for (int j = 0; j < maxSoldierNum; j++)
        {
            int randomIndex = Random.Range(0, playerDataMgr.characterList.Count);
            var key = characterName[randomIndex];

            CharacterStats stat = new CharacterStats();
            stat.VirusPanaltyInit();
            var character = playerDataMgr.characterList[key];
            stat.level = 0;
            stat.currentHp = Random.Range(character.minHp, character.maxHp);
            stat.MaxHp = stat.currentHp;
            stat.sensivity = Random.Range(character.minSensitivity, character.maxSensitivity);
            stat.concentration = Random.Range(character.minConcentration, character.maxConcentration);
            stat.willpower = Random.Range(character.minWillpower, character.maxWillpower);
            stat.bagLevel = 1;

            stat.character = character;
            stat.character.id = character.id;

            stat.virusPanalty["E"].penaltyGauge =0;
            stat.virusPanalty["B"].penaltyGauge = 0;
            stat.virusPanalty["P"].penaltyGauge = 0;
            stat.virusPanalty["I"].penaltyGauge = 0;
            stat.virusPanalty["T"].penaltyGauge = 0;

            stat.virusPanalty["E"].penaltyLevel = 0;
            stat.virusPanalty["B"].penaltyLevel = 0;
            stat.virusPanalty["P"].penaltyLevel = 0;
            stat.virusPanalty["I"].penaltyLevel = 0;
            stat.virusPanalty["T"].penaltyLevel = 0;

            stat.weapon = new WeaponStats();
            stat.weapon.mainWeapon = null;
            stat.weapon.subWeapon =  null;

            stat.skillMgr = new SkillMgr();
            stat.skillMgr.activeSkills = new List<ActiveSkill>();
            for (int k = 0; k < 5; k++) stat.skillMgr.activeSkills.Add(null);
            stat.skillMgr.passiveSkills = new List<PassiveSkill>();
            for (int k = 0; k < 5; k++) stat.skillMgr.passiveSkills.Add(null);

            int num = j;
            soliders.Add(num, stat);
            var cost = Random.Range(character.minCharCost, character.maxCharCost);
            costs.Add(num, cost);
        }

        int i = 0;
        foreach (var element in soliders)
        {
            var go = Instantiate(characterPrefab, characterListContent.transform);

            go.transform.GetChild(1).GetComponent<Text>().text
                = element.Value.Name;
            go.transform.GetChild(2).GetComponent<Text>().text
                = $"Lv {element.Value.level}";
            go.transform.GetChild(3).GetComponent<Text>().text
                = "무기 미장착";
            go.transform.GetChild(4).GetComponent<Text>().text
                = $"분과 {element.Value.character.name}";
            go.transform.GetChild(5).GetComponent<Text>().text
                = $"금액 {costs[i]}";

            int num = i;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(num); });
            characterObjs.Add(num, go);

            i++;
        }

        currentIndex = -1;
        currentHintIndex = -1;
        originColor = characterPrefab.GetComponent<Image>().color;
    }

    public void SelectCharacter(int index)
    {
        if (currentIndex != -1)
            characterObjs[currentIndex].GetComponent<Image>().color = originColor;

        currentIndex = index;
        characterObjs[currentIndex].GetComponent<Image>().color = Color.red;

        OpenDetailInfo();
    }

    public void OpenDetailInfo()
    {
        if(!DetailInfoWin.activeSelf) DetailInfoWin.SetActive(true);

        hpTxt.text = $"체력{soliders[currentIndex].currentHp}";
        conTxt.text = $"집중력{soliders[currentIndex].concentration}";
        senTxt.text = $"예민함{soliders[currentIndex].sensivity}";
        willPowerTxt.text = $"정신력{soliders[currentIndex].willpower}";
    }

    public void Hire()
    {
        if (playerDataMgr.saveData.money - costs[currentIndex] < 0) return;

        int agitLevel = playerDataMgr.saveData.agitLevel;
        Bunker agitLevelInfo = playerDataMgr.bunkerList["BUN_0001"];
        int maxMember = 0;
        switch (agitLevel)
        {
            case 1:
                maxMember = agitLevelInfo.level1;
                break;
            case 2:
                maxMember = agitLevelInfo.level2;
                break;
            case 3:
                maxMember = agitLevelInfo.level3;
                break;
            case 4:
                maxMember = agitLevelInfo.level4;
                break;
            case 5:
                maxMember = agitLevelInfo.level5;
                break;
        }
        int currentMemberNum = playerDataMgr.currentSquad.Count;

        if (currentMemberNum + 1 > maxMember) return;

        string squadNum = "SquadNum";
        int currentNum = PlayerPrefs.HasKey(squadNum) ? PlayerPrefs.GetInt(squadNum) : 0;
        playerDataMgr.AddCharacter(currentNum, soliders[currentIndex]);
        playerDataMgr.saveData.money -= costs[currentIndex];
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        PlayerPrefs.SetInt(squadNum, currentNum + 1);

        Refresh();
        DetailInfoWin.SetActive(false);
    }

    public void Refresh()
    {
        Destroy(characterObjs[currentIndex]);
        characterObjs.Remove(currentIndex);
        costs.Remove(currentIndex);
        currentIndex = -1;
    }

    //힌트 수집 창 관련.
    public void SelectHint(int index)
    {
        if (currentHintIndex != -1) hintList[currentHintIndex].GetComponent<Image>().color = Color.white;

        currentHintIndex = index;
        hintList[currentHintIndex].GetComponent<Image>().color = Color.red;
    }

    public void OpenHintPopup()
    {
        if (currentHintIndex == -1) return;

        if (!hintPopup.activeSelf) hintPopup.SetActive(true);
        hintContents.text = "랜덤 힌트 내용";
    }

    public void CloseHintPopup()
    {
        hintList[currentHintIndex].GetComponent<Image>().color = Color.white;
        currentHintIndex = -1;

        if (hintPopup.activeSelf) hintPopup.SetActive(false);
    }
    
    //창이동.
    public void MoveToMainWin()
    {
        if (recruitmentWin.activeSelf) recruitmentWin.SetActive(false);
        if (hintCollectionWin.activeSelf) hintCollectionWin.SetActive(false);
        mainWin.SetActive(true);
    }

    public void MoveToRecruitWin()
    {
        mainWin.SetActive(false);
        recruitmentWin.SetActive(true);
    }

    public void MoveToHintCollectionWin()
    {
        mainWin.SetActive(false);
        hintCollectionWin.SetActive(true);
        if (hintPopup.activeSelf) hintPopup.SetActive(false);

        foreach (var element in hintList)
        {
            if (element.GetComponent<Image>().color == Color.red)
                element.GetComponent<Image>().color = Color.white;
        }
    }
}

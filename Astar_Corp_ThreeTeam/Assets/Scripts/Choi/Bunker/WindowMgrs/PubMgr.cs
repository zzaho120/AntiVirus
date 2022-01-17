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
    int currentIndex;
    int currentHintIndex;
    Color originColor;

    List<string> characterName = new List<string>();
    Dictionary<int, CharacterStats> soliders = new Dictionary<int, CharacterStats>();
    public void Init()
    {
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

        pubLevel = (PlayerPrefs.HasKey("PubLevel"))? PlayerPrefs.GetInt("PubLevel") : 1;
        int soliderNum=0;
        switch (pubLevel)
        {
            case 1:
                soliderNum = 4;
                break;
            case 2:
                soliderNum = 6;
                break;
            case 3:
                soliderNum = 8;
                break;
        }

        if (soliders.Count != 0) soliders.Clear();
        for (int j = 0; j < soliderNum; j++)
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

            stat.skills = new CharacterSkillList();
            stat.skills.activeSkills = new List<ActiveSkill>();
            for (int k = 0; k < 5; k++) stat.skills.activeSkills.Add(null);
            stat.skills.passiveSkills = new List<PassiveSkill>();
            for (int k = 0; k < 5; k++) stat.skills.passiveSkills.Add(null);

            int num = j;
            soliders.Add(num, stat);
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
                = $"분과";
            go.transform.GetChild(5).GetComponent<Text>().text
                = $"금액";

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
        string squadNum = "SquadNum";
        int currentNum = PlayerPrefs.HasKey(squadNum) ? PlayerPrefs.GetInt(squadNum) : 0;
        playerDataMgr.AddCharacter(currentNum, soliders[currentIndex]);
        PlayerPrefs.SetInt(squadNum, currentNum + 1);

        Refresh();
        DetailInfoWin.SetActive(false);
    }

    public void Refresh()
    {
        Destroy(characterObjs[currentIndex]);
        characterObjs.Remove(currentIndex);
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

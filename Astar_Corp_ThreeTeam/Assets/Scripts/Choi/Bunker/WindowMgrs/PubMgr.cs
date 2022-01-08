using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PubMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;

    public GameObject mainWin;
    public GameObject recruitmentWin;

    public GameObject characterListContent;
    public GameObject characterPrefab;
    public GameObject DetailInfoWin;
    public Text hpTxt;
    public Text conTxt;
    public Text senTxt;
    public Text willPowerTxt;
    Dictionary<int, GameObject> characterObjs = new Dictionary<int, GameObject>();

    int pubLevel;
    int currentIndex;
    Color originColor;

    List<string> characterName = new List<string>();
    Dictionary<int, CharacterStats> soliders = new Dictionary<int, CharacterStats>();
    public void Init()
    {
        if (!mainWin.activeSelf) mainWin.SetActive(true);
        if (recruitmentWin.activeSelf) recruitmentWin.SetActive(false);

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
            stat.currentHp = Random.Range(character.min_Hp, character.max_Hp);
            stat.maxHp = character.max_Hp;
            stat.sensivity = Random.Range(character.min_Sensitivity, character.max_Sensitivity);
            stat.concentration = Random.Range(character.min_Concentration, character.max_Concentration);
            stat.willpower = Random.Range(character.min_Willpower, character.max_Willpower);

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
        Refresh();
    }

    public void Refresh()
    {
        Destroy(characterObjs[currentIndex]);
        characterObjs.Remove(currentIndex);
        currentIndex = -1;
    }

    //창이동.
    public void MoveToMainWin()
    {
        recruitmentWin.SetActive(false);
        mainWin.SetActive(true);
    }

    public void MoveToRecruitWin()
    {
        mainWin.SetActive(false);
        recruitmentWin.SetActive(true);
    }
}

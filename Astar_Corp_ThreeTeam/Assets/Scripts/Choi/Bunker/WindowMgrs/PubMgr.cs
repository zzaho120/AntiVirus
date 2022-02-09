using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PubMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;
    public BunkerMgr bunkerMgr;

    public GameObject mainWin;
    public GameObject recruitmentWin;
    public GameObject popupWin;
    public GameObject upgradeWin;
    public GameObject hintCollectionWin;

    public Animator menuAnim;
    bool isMenuOpen;
    public GameObject arrowImg;

    //용병 모집 창.
    public GameObject characterListContent;
    public GameObject characterPrefab;
    public GameObject DetailInfoWin;
    public Text hpTxt;
    public Text conTxt;
    public Text senTxt;
    public Text willPowerTxt;
    Dictionary<int, GameObject> characterObjs = new Dictionary<int, GameObject>();

    [Header("Detail Win")]
    public Text nameTxt;
    public Text levelTxt;
    public List<GameObject> mainWeaponList;
    public Text simpleStat;
    public Text detailStat;

    //힌트 수집 창.
    public List<GameObject> hintList;
    public GameObject hintPopup;
    public Text hintContents;

    [Header("Upgrade Win")]
    public Text pubLevelTxt;
    public Text capacityTxt;
    public Text materialTxt;

    //아지트 관련.
    int agitLevel;
    int maxMember;

    //선술집 관련.
    int pubLevel;
    int maxSoldierNum;
    int nextMaxSoldier;
    int upgradeCost;

    int maxHintNum;
    int currentIndex;
    int currentHintIndex;
    Color originColor;

    List<string> characterName = new List<string>();
    Dictionary<int, CharacterStats> soldiers = new Dictionary<int, CharacterStats>();
    Dictionary<int, int> costs = new Dictionary<int, int>();

    public void Init()
    {
        agitLevel = playerDataMgr.saveData.agitLevel;
        Bunker agitLevelInfo = playerDataMgr.bunkerList["BUN_0001"];
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
        
        pubLevel = playerDataMgr.saveData.pubLevel;
        Bunker pubSoldierLevelInfo = playerDataMgr.bunkerList["BUN_0007"];
        Bunker pubHintLevelInfo = playerDataMgr.bunkerList["BUN_0008"];
        switch (pubLevel)
        {
            case 1:
                maxSoldierNum = pubSoldierLevelInfo.level1;
                nextMaxSoldier = pubSoldierLevelInfo.level2;
                upgradeCost = pubSoldierLevelInfo.level2Cost;
                maxHintNum = pubHintLevelInfo.level1;
                break;
            case 2:
                maxSoldierNum = pubSoldierLevelInfo.level2;
                nextMaxSoldier = pubSoldierLevelInfo.level3;
                upgradeCost = pubSoldierLevelInfo.level3Cost;
                maxHintNum = pubHintLevelInfo.level2;
                break;
            case 3:
                maxSoldierNum = pubSoldierLevelInfo.level3;
                nextMaxSoldier = pubSoldierLevelInfo.level4;
                upgradeCost = pubSoldierLevelInfo.level4Cost;
                maxHintNum = pubHintLevelInfo.level3;
                break;
            case 4:
                maxSoldierNum = pubSoldierLevelInfo.level4;
                nextMaxSoldier = pubSoldierLevelInfo.level5;
                upgradeCost = pubSoldierLevelInfo.level5Cost;
                maxHintNum = pubHintLevelInfo.level4;
                break;
            case 5:
                maxSoldierNum = pubSoldierLevelInfo.level5;
                maxHintNum = pubHintLevelInfo.level5;
                break;
        }

        SelectSoldier();

        currentIndex = -1;
        currentHintIndex = -1;
        originColor = characterPrefab.GetComponent<Image>().color;
    }

    public void SelectSoldier()
    {
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
        if (soldiers.Count != 0) soldiers.Clear();
        if (costs.Count != 0) costs.Clear();

        if (playerDataMgr.saveData.pubReset == true)
        {
            //벙커 알람.
            bunkerMgr.bunkerObjs[2].transform.GetChild(1).gameObject.SetActive(true);
            bunkerMgr.quickButtons[3].transform.GetChild(1).gameObject.SetActive(true);

            //랜덤 용병 생성.
            playerDataMgr.saveData.pubReset = false;
            if (playerDataMgr.saveData.soldierName.Count != 0) playerDataMgr.saveData.soldierName.Clear();
            if (playerDataMgr.saveData.soldierHp.Count != 0) playerDataMgr.saveData.soldierHp.Clear();
            if (playerDataMgr.saveData.soldierSensitivity.Count != 0) playerDataMgr.saveData.soldierSensitivity.Clear();
            if (playerDataMgr.saveData.soldierAvoidRate.Count != 0) playerDataMgr.saveData.soldierAvoidRate.Clear();
            if (playerDataMgr.saveData.soldierConcentration.Count != 0) playerDataMgr.saveData.soldierConcentration.Clear();
            if (playerDataMgr.saveData.soldierWillPower.Count != 0) playerDataMgr.saveData.soldierWillPower.Clear();
            if (playerDataMgr.saveData.soldierSightDistance.Count != 0) playerDataMgr.saveData.soldierSightDistance.Clear();
            if (playerDataMgr.saveData.soldierMainWeapon.Count != 0) playerDataMgr.saveData.soldierMainWeapon.Clear();

            //랜덤 용병 생성.
            foreach (var element in playerDataMgr.characterList)
            {
                characterName.Add(element.Key);
            }

            for (int j = 0; j < maxSoldierNum; j++)
            {
                int randomIndex = Random.Range(0, playerDataMgr.characterList.Count);
                var key = characterName[randomIndex];

                CharacterStats stat = new CharacterStats();
                var character = playerDataMgr.characterList[key];
                stat.character = character;
                stat.Init();
                stat.bagLevel = 1;

                stat.weapon = new WeaponStats();
                stat.weapon.mainWeapon = null;

                int num = j;
                soldiers.Add(num, stat);
                var cost = Random.Range(character.minCharCost, character.maxCharCost);
                costs.Add(num, cost);

                playerDataMgr.saveData.soldierName.Add(stat.character.name);
                playerDataMgr.saveData.soldierHp.Add(stat.currentHp);
                playerDataMgr.saveData.soldierSensitivity.Add(stat.sensivity);
                playerDataMgr.saveData.soldierAvoidRate.Add(stat.avoidRate);
                playerDataMgr.saveData.soldierConcentration.Add(stat.concentration);
                playerDataMgr.saveData.soldierWillPower.Add(stat.willpower);
                playerDataMgr.saveData.soldierSightDistance.Add(stat.sightDistance);
                playerDataMgr.saveData.soldierMainWeapon.Add(null);
                playerDataMgr.saveData.soldierCost.Add(cost);
            }
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            int i = 0;
            foreach (var element in soldiers)
            {
                var go = Instantiate(characterPrefab, characterListContent.transform);
                var child = go.transform.GetChild(1).gameObject;

                var childObj = child.transform.GetChild(0).gameObject;
                childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text
                    = $"가격 {costs[i]}";

                childObj = child.transform.GetChild(1).gameObject;
                childObj.GetComponent<Text>().text = $"{element.Value.character.name}/성별";

                childObj = child.transform.GetChild(2).gameObject;
                childObj.GetComponent<Text>().text = $"{element.Value.character.name}/Lv{element.Value.level}/착용 중인 주무기";

                childObj = child.transform.GetChild(3).gameObject;
                var slider = childObj.GetComponent<Slider>();
                slider.maxValue = element.Value.MaxHp;
                slider.value = element.Value.currentHp;

                int num = i;
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectCharacter(num); });
                characterObjs.Add(num, go);

                i++;
            }
        }
        else
        {
            for (int j = 0; j < playerDataMgr.saveData.soldierName.Count; j++)
            {
                CharacterStats stat = new CharacterStats();
                var key = playerDataMgr.characterList.FirstOrDefault(x => x.Value.name == playerDataMgr.saveData.soldierName[j]).Key;
                var character = playerDataMgr.characterList[key];
                stat.character = character;
                stat.currentHp = playerDataMgr.saveData.soldierHp[j];
                stat.MaxHp = playerDataMgr.saveData.soldierHp[j];
                stat.sensivity = playerDataMgr.saveData.soldierSensitivity[j];
                stat.avoidRate = playerDataMgr.saveData.soldierAvoidRate[j];
                stat.concentration = playerDataMgr.saveData.soldierConcentration[j];
                stat.willpower = playerDataMgr.saveData.soldierWillPower[j];
                stat.level = 1;
                stat.currentExp = 0;
                stat.sightDistance = playerDataMgr.saveData.soldierSightDistance[j];
                stat.Weight = stat.character.minHp;
                stat.bagLevel = 1;
             
                stat.Setting();

                stat.weapon = new WeaponStats();
                if (playerDataMgr.saveData.soldierMainWeapon[j] == null) stat.weapon.mainWeapon = null;

                int num = j;
                soldiers.Add(num, stat);
                var cost = playerDataMgr.saveData.soldierCost[j];
                costs.Add(num, cost);
            }
            
            int i = 0;
            foreach (var element in soldiers)
            {
                var go = Instantiate(characterPrefab, characterListContent.transform);
                var child = go.transform.GetChild(1).gameObject;

                var childObj = child.transform.GetChild(0).gameObject;
                childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text
                    = $"가격 {costs[i]}";

                childObj = child.transform.GetChild(1).gameObject;
                childObj.GetComponent<Text>().text = $"{element.Value.character.name}/성별";

                childObj = child.transform.GetChild(2).gameObject;
                childObj.GetComponent<Text>().text = $"{element.Value.character.name}/Lv{element.Value.level}/착용 중인 주무기";

                childObj = child.transform.GetChild(3).gameObject;
                var slider = childObj.GetComponent<Slider>();
                slider.maxValue = element.Value.MaxHp;
                slider.value = element.Value.currentHp;

                int num = i;
                var button = go.AddComponent<Button>();
                button.onClick.AddListener(delegate { SelectCharacter(num); });
                characterObjs.Add(num, go);

                i++;
            }
        }
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

        nameTxt.text = $"{soldiers[currentIndex].character.name}";
        levelTxt.text = $"Lv{soldiers[currentIndex].level}";

        foreach (var element in mainWeaponList)
        {
            element.SetActive(false);
        }

        int i = 0;
        foreach (var element in soldiers[currentIndex].character.weapons)
        {
            if (element.Equals("1") || element.Equals("7")) continue;
            
            mainWeaponList[i].SetActive(true);
            var child = mainWeaponList[i].transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = $"{GetTypeStr(element)}";
            i++;
        }

        simpleStat.text = $"체력{soldiers[currentIndex].currentHp} 예민함 {soldiers[currentIndex].sensivity}\n"
            + $"집중력 {soldiers[currentIndex].concentration} 정신력 {soldiers[currentIndex].willpower}";

        detailStat.text = $"HP {soldiers[currentIndex].currentHp} 무게 {soldiers[currentIndex].character.minHp}\n"
        + $"회피율 {soldiers[currentIndex].avoidRate} 시야범위 {soldiers[currentIndex].sightDistance}\n"
        + $"명중률 {soldiers[currentIndex].accuracy} 경계명중률 {soldiers[currentIndex].alertAccuracy} 크리티컬 확률 {soldiers[currentIndex].critRate}";
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
                type = "근접무기";
                break;
        }
        return type;
    }

    public void Hire()
    {
        if (playerDataMgr.saveData.money - costs[currentIndex] < 0) return;

        int currentMemberNum = playerDataMgr.currentSquad.Count;
        if (currentMemberNum + 1 > maxMember) return;

        //json.
        playerDataMgr.saveData.soldierName.RemoveAt(currentIndex);
        playerDataMgr.saveData.soldierHp.RemoveAt(currentIndex);
        playerDataMgr.saveData.soldierSensitivity.RemoveAt(currentIndex);
        playerDataMgr.saveData.soldierAvoidRate.RemoveAt(currentIndex);
        playerDataMgr.saveData.soldierConcentration.RemoveAt(currentIndex);
        playerDataMgr.saveData.soldierWillPower.RemoveAt(currentIndex);
        playerDataMgr.saveData.soldierSightDistance.RemoveAt(currentIndex);
        playerDataMgr.saveData.soldierMainWeapon.RemoveAt(currentIndex);
        playerDataMgr.saveData.soldierCost.RemoveAt(currentIndex);

        string squadNum = "SquadNum";
        int currentNum = PlayerPrefs.HasKey(squadNum) ? PlayerPrefs.GetInt(squadNum) : 0;
        playerDataMgr.AddCharacter(currentNum, soldiers[currentIndex]);
        PlayerPrefs.SetInt(squadNum, currentNum + 1);

        playerDataMgr.saveData.money -= costs[currentIndex];
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        bunkerMgr.moneyTxt.text = playerDataMgr.saveData.money.ToString();
       
        currentIndex = -1;
        SelectSoldier();
        DetailInfoWin.SetActive(false);
    }

    public void ElectiveEmployment()
    {
        int totalCost = 0;
        int totalNum = 0;
        bool isAnythingChecked = false;
        foreach (var element in characterObjs)
        {
            var child = element.Value.transform.GetChild(1).gameObject;
            var toggle = child.transform.GetChild(4).gameObject.GetComponent<Toggle>();
            if (toggle.isOn == true)
            {
                isAnythingChecked = true;
                totalCost += costs[element.Key];
                totalNum++;
            }
        }
        if (isAnythingChecked == false) return;
        if (playerDataMgr.saveData.money - totalCost < 0) return;

        int currentMemberNum = playerDataMgr.currentSquad.Count;
        if (currentMemberNum + totalNum > maxMember) return;

        int count = characterObjs.Count;
        for (int i = count - 1; i > -1; --i)
        {
            var child = characterObjs[i].transform.GetChild(1).gameObject;
            var toggle = child.transform.GetChild(4).gameObject.GetComponent<Toggle>();

            if (toggle.isOn)
            {
                //json.
                playerDataMgr.saveData.soldierName.RemoveAt(i);
                playerDataMgr.saveData.soldierHp.RemoveAt(i);
                playerDataMgr.saveData.soldierSensitivity.RemoveAt(i);
                playerDataMgr.saveData.soldierAvoidRate.RemoveAt(i);
                playerDataMgr.saveData.soldierConcentration.RemoveAt(i);
                playerDataMgr.saveData.soldierWillPower.RemoveAt(i);
                playerDataMgr.saveData.soldierSightDistance.RemoveAt(i);
                playerDataMgr.saveData.soldierMainWeapon.RemoveAt(i);
                playerDataMgr.saveData.soldierCost.RemoveAt(i);

                string squadNum = "SquadNum";
                int currentNum = PlayerPrefs.HasKey(squadNum) ? PlayerPrefs.GetInt(squadNum) : 0;
                playerDataMgr.AddCharacter(currentNum, soldiers[i]);
                PlayerPrefs.SetInt(squadNum, currentNum + 1);

                playerDataMgr.saveData.money -= costs[i];
            }
        }
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        bunkerMgr.moneyTxt.text = playerDataMgr.saveData.money.ToString();
        currentIndex = -1;
        SelectSoldier();
        DetailInfoWin.SetActive(false);
    }

    public void SelectAll()
    {
        foreach (var element in characterObjs)
        {
            var child = element.Value.transform.GetChild(1).gameObject;
            var toggle = child.transform.GetChild(4).gameObject.GetComponent<Toggle>();
            toggle.isOn = true;
        }
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

    public void RefreshUpgradeWin()
    {
        if (pubLevel != 5)
        {
            pubLevelTxt.text = $"건물 레벨 {pubLevel}→{pubLevel + 1}";
            capacityTxt.text = $"등장 용병 숫자 {maxSoldierNum}→{nextMaxSoldier}";
            materialTxt.text = $"{upgradeCost}";
        }
        else
        {
            pubLevelTxt.text = $"건물 레벨{pubLevel}→ -";
            capacityTxt.text = $"등장 용병 숫자 {maxSoldierNum}→ -";
            materialTxt.text = $"-";
        }
    }

    //창이동.
    public void OpenMainWin()
    {
        //벙커 알람.
        if (bunkerMgr.bunkerObjs[2].transform.GetChild(1).gameObject.activeSelf)
        {
            bunkerMgr.bunkerObjs[2].transform.GetChild(1).gameObject.SetActive(false);
            bunkerMgr.quickButtons[3].transform.GetChild(1).gameObject.SetActive(false);
        }

        if (bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(false);
        
        if (recruitmentWin.activeSelf) recruitmentWin.SetActive(false);
        if (upgradeWin.activeSelf) upgradeWin.SetActive(false);
        if (popupWin.activeSelf) popupWin.SetActive(false);
        if (hintCollectionWin.activeSelf) hintCollectionWin.SetActive(false);
        if (!mainWin.activeSelf) mainWin.SetActive(true);

        isMenuOpen = true;
        arrowImg.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
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

    public void MoveToRecruitWin()
    {
        mainWin.SetActive(false);
        recruitmentWin.SetActive(true);
    }

    public void OpenPopup()
    {
        popupWin.SetActive(true);
    }

    public void ClosePopup()
    {
        popupWin.SetActive(false);
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

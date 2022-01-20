using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgitMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;
    public SkillWinMgr skillWinMgr;
    public EquipmentMgr equipmentMgr;
    public ToleranceMgr toleranceMgr;
    public BagMgr bagMgr;

    public GameObject characterListContent;
    public GameObject characterPrefab;
    public List<GameObject> characterObjs;

    public GameObject charcterListWin;
    public GameObject characterInfoWin;
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
        memberNumTxt.text = $"팀원 {currentMemberNum} / {maxMember}";

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

            go.transform.GetChild(1).GetComponent<Text>().text
                = element.Value.character.name;
            go.transform.GetChild(2).GetComponent<Text>().text
                = $"{element.Value.level}";
            go.transform.GetChild(3).GetComponent<Text>().text
                = (element.Value.weapon.mainWeapon != null)?
                $"{element.Value.weapon.mainWeapon.name}" : "무기 미장착";
            go.transform.GetChild(4).GetComponent<Text>().text
                = $"분과";
            //go.transform.GetChild(5).GetComponent<Slider>().value
            //   = element.Value.currentHp/ element.Value.maxHp;
            
            int num = i;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(num); });
            characterObjs.Add(go);

            characterInfo.Add(num, element.Key);

            i++;
        }

        currentIndex = -1;
        skillWinMgr.currentIndex = currentIndex;
        equipmentMgr.currentIndex = currentIndex;
        toleranceMgr.currentIndex = currentIndex;
        bagMgr.currentIndex = currentIndex;

        skillWinMgr.playerDataMgr = playerDataMgr;
        skillWinMgr.Init();

        equipmentMgr.playerDataMgr = playerDataMgr;
        equipmentMgr.Init();

        toleranceMgr.playerDataMgr = playerDataMgr;
        
        bagMgr.playerDataMgr = playerDataMgr;
        bagMgr.Init();

        if (!charcterListWin.activeSelf) charcterListWin.SetActive(true);
        if (characterInfoWin.activeSelf) characterInfoWin.SetActive(false);
        if (skillWinMgr.skillPage.activeSelf) skillWinMgr.skillPage.SetActive(false);
        if (equipmentMgr.equipmentWin.activeSelf) equipmentMgr.equipmentWin.SetActive(false);
        if (toleranceMgr.toleranceWin.activeSelf) toleranceMgr.toleranceWin.SetActive(false);
        if (bagMgr.bagWin.activeSelf) bagMgr.bagWin.SetActive(false);

        originColor = characterPrefab.GetComponent<Image>().color;
        isDeleteMode = false;
    }

    public void SelectCharacter(int index)
    {
        //if (currentIndex != -1)
        //    characterObjs[currentIndex].GetComponent<Image>().color = originColor;

        //currentIndex = index;
        //characterObjs[currentIndex].GetComponent<Image>().color = Color.red;

        currentIndex = index;
        skillWinMgr.currentIndex = currentIndex;
        equipmentMgr.currentIndex = currentIndex;
        toleranceMgr.currentIndex = currentIndex;
        bagMgr.currentIndex = currentIndex;

        equipmentMgr.RefreshEquipList();
        toleranceMgr.Refresh();
        bagMgr.Init();
        
        OpenCharacterInfo();
    }

    public void EnableCheckBox()
    {
        if (!isDeleteMode)
        {
            isDeleteMode = true;
            foreach (var element in characterObjs)
            {
                element.GetComponent<Button>().enabled = false;

                var go = element.transform.GetChild(6).gameObject;
                var toggle = go.GetComponent<Toggle>().isOn = false;
                go.SetActive(true);
            }
        }
    }

    public void DisableCheckBox()
    {
        isDeleteMode = false;
        foreach (var element in characterObjs)
        {
            element.GetComponent<Button>().enabled = true;

            var go = element.transform.GetChild(6).gameObject;
            go.SetActive(false);
        }
    }

    public void AllCheckBox(bool value)
    {
        if (!isDeleteMode) return;
        foreach (var element in characterObjs)
        {
            var go = element.transform.GetChild(6).gameObject;
            var toggle = go.GetComponent<Toggle>().isOn = value;
        }
    }

    public void CheckAll()
    {
        AllCheckBox(true);
    }

    public void UncheckAll()
    {
        AllCheckBox(false);
    }

    public void Fire()
    {
        if (isAnythingChecked() == false) return;

        int count = characterObjs.Count;
        for (int i = count -1; i > -1; --i)
        {
            var toggle = characterObjs[i].transform.GetChild(6).GetComponent<Toggle>();
            if (toggle.isOn)
            {
                Destroy(characterObjs[i]);
                characterObjs.RemoveAt(i);

                //데이터 삭제.
                playerDataMgr.saveData.id.RemoveAt(i);
                playerDataMgr.saveData.name.RemoveAt(i);
                playerDataMgr.saveData.hp.RemoveAt(i);
                playerDataMgr.saveData.maxHp.RemoveAt(i);
                playerDataMgr.saveData.sensitivity.RemoveAt(i);
                playerDataMgr.saveData.concentration.RemoveAt(i);
                playerDataMgr.saveData.willPower.RemoveAt(i);

                playerDataMgr.saveData.gaugeE.RemoveAt(i);
                playerDataMgr.saveData.gaugeB.RemoveAt(i);
                playerDataMgr.saveData.gaugeP.RemoveAt(i);
                playerDataMgr.saveData.gaugeI.RemoveAt(i);
                playerDataMgr.saveData.gaugeT.RemoveAt(i);

                playerDataMgr.saveData.levelE.RemoveAt(i);
                playerDataMgr.saveData.levelB.RemoveAt(i);
                playerDataMgr.saveData.levelP.RemoveAt(i);
                playerDataMgr.saveData.levelI.RemoveAt(i);
                playerDataMgr.saveData.levelT.RemoveAt(i);

                int firstIndex = playerDataMgr.saveData.bagEquippableFirstIndex[i];
                int lastIndex = playerDataMgr.saveData.bagEquippableLastIndex[i];
                int difference = lastIndex - firstIndex;
                for (int j = i; j < playerDataMgr.saveData.bagEquippableFirstIndex.Count; j++)
                {
                    playerDataMgr.saveData.bagEquippableFirstIndex[j] -= difference;
                    playerDataMgr.saveData.bagEquippableLastIndex[j] -= difference;
                }

                for (int j = firstIndex; j < lastIndex; j++)
                {
                    playerDataMgr.saveData.bagEquippableList.RemoveAt(i);
                    playerDataMgr.saveData.bagEquippableNumList.RemoveAt(i);
                }
                playerDataMgr.saveData.bagEquippableFirstIndex.RemoveAt(i);
                playerDataMgr.saveData.bagEquippableLastIndex.RemoveAt(i);
                
                firstIndex = playerDataMgr.saveData.bagConsumableFirstIndex[i];
                lastIndex = playerDataMgr.saveData.bagConsumableLastIndex[i];
                difference = lastIndex - firstIndex;
                for (int j = i; j < playerDataMgr.saveData.bagConsumableFirstIndex.Count; j++)
                {
                    playerDataMgr.saveData.bagConsumableFirstIndex[j] -= difference;
                    playerDataMgr.saveData.bagConsumableLastIndex[j] -= difference;
                }

                for (int j = firstIndex; j < lastIndex; j++)
                {
                    playerDataMgr.saveData.bagConsumableList.RemoveAt(i);
                    playerDataMgr.saveData.bagConsumableNumList.RemoveAt(i);
                }
                playerDataMgr.saveData.bagConsumableFirstIndex.RemoveAt(i);
                playerDataMgr.saveData.bagConsumableLastIndex.RemoveAt(i);

                playerDataMgr.saveData.mainWeapon.RemoveAt(i);
                playerDataMgr.saveData.subWeapon.RemoveAt(i);
                
                int activeSkillNum = playerDataMgr.activeSkillList.Count;
                
                for (int j = activeSkillNum-1; j>-1; j--) 
                {
                    playerDataMgr.saveData.activeSkillList.RemoveAt(i * activeSkillNum + j); 
                }

                int passiveSkillNum = playerDataMgr.passiveSkillList.Count;
                for (int j = passiveSkillNum-1; j >-1; j--)
                {
                    playerDataMgr.saveData.passiveSkillList.RemoveAt(i * passiveSkillNum + j);
                }

                playerDataMgr.currentSquad.Remove(i);

                PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
            }
        }
        playerDataMgr.RefreshCurrentSquad();
    }

    bool isAnythingChecked()
    {
        if (!isDeleteMode) return false;
        foreach (var element in characterObjs)
        {
            var go = element.transform.GetChild(6).gameObject;
            if (go.GetComponent<Toggle>().isOn == true) return true;
        }
        return false;
    }

    public void OpenCharacterInfo()
    {
        charcterListWin.SetActive(false);
        characterInfoWin.SetActive(true);

        var character = playerDataMgr.currentSquad[currentIndex];
        nameTxt.text = $"{character.character.name}";
        levelTxt.text = $"Level {character.level} / 분과";
        hpTxt.text = $"체력 {character.currentHp}";
        concentrationTxt.text = $"집중력 {character.concentration}";
        sensitivityTxt.text = $"예민함 {character.sensivity}";
        willpowerTxt.text = $"정신력 {character.willpower}";
    }

    //창 관리.
    public void CloseCharacterInfo()
    {
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
}

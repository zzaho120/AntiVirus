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

    public GameObject characterListContent;
    public GameObject characterPrefab;
    public List<GameObject> characterObjs;

    public GameObject charcterListWin;
    public GameObject characterInfoWin;

    //캐릭터 정보 확인.
    public Text nameTxt;
    public Text levelTxt;
    public Text hpTxt;
    public Text concentrationTxt;
    public Text sensitivityTxt;
    public Text willpowerTxt;

    // 리스트 순서 / PlayerDataMgr Key
    Dictionary<int, int> characterInfo = new Dictionary<int, int>();
    int currentIndex;
    Color originColor;
    public void Init()
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

        skillWinMgr.playerDataMgr = playerDataMgr;
        skillWinMgr.Init();

        equipmentMgr.playerDataMgr = playerDataMgr;
        equipmentMgr.Init();

        toleranceMgr.playerDataMgr = playerDataMgr;
        toleranceMgr.Init();

        if (!charcterListWin.activeSelf) charcterListWin.SetActive(true);
        if (characterInfoWin.activeSelf) characterInfoWin.SetActive(false);
        if (skillWinMgr.skillPage.activeSelf) skillWinMgr.skillPage.SetActive(false);
        if (equipmentMgr.equipmentWin.activeSelf) equipmentMgr.equipmentWin.SetActive(false);
        if (toleranceMgr.toleranceWin.activeSelf) toleranceMgr.toleranceWin.SetActive(false);

        originColor = characterPrefab.GetComponent<Image>().color;
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
        equipmentMgr.RefreshEquipList();
        toleranceMgr.currentIndex = currentIndex; 
        OpenCharacterInfo();
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
}

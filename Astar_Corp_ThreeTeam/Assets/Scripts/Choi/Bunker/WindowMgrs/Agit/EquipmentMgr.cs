using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipKind
{ 
    None,
    MainWeapon,
    SubWeapon,
    UtilityItem
}

public class EquipmentMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;

    public GameObject equipmentWin;
    public GameObject equipmentWin1;
    public GameObject equipmentWin2;

    //창1 관련.
    public Text mainWeaponTxt;
    public Text subWeaponTxt;
    public Text utilityItemTxt;

    //창2 관련.
    public GameObject WeaponWin;
    public GameObject SpecCompareWin;
    public Text weaponSpecTxt;
    public Text currentWeaSpecTxt;
    public Text changeWeaSpecTxt;

    public GameObject itemListContents;
    public GameObject itemPrefab;
    public Dictionary<int, GameObject> itemObjs = new Dictionary<int, GameObject>();
    public Dictionary<int, string> itemInfo = new Dictionary<int, string>();

    public int currentIndex;
    int currentItemIndex;
    EquipKind currentKind;

    public void Init()
    {
        OpenEquipWin1();
        mainWeaponTxt.text =
            (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null) ?
            playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.name : "비어있음";
        subWeaponTxt.text =
            (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null) ?
            playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.name : "비어있음";
        //utilityItemTxt.text =
        //            (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null) ?
        //            playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.name : "비어있음";

        if (itemObjs.Count != 0)
        {
            foreach (var element in itemObjs)
            {
                Destroy(element.Value);
            }
            itemListContents.transform.DetachChildren();
            itemObjs.Clear();
        }
        if (itemInfo.Count != 0) itemInfo.Clear();

        int i = 0;
        foreach (var element in playerDataMgr.currentEquippables)
        {
            var go = Instantiate(itemPrefab, itemListContents.transform);
            var button = go.AddComponent<Button>();
            int num = i;
            button.onClick.AddListener(delegate { SelectItem(num); });

            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Key;

            itemObjs.Add(num,go);
            itemInfo.Add(num, element.Key);

            i++;
        }

        currentItemIndex = -1;
        currentKind = EquipKind.None;
    }

    public void SelectEquipKind(int kind)
    {
        switch (kind)
        {
            case 0:
                currentKind = EquipKind.MainWeapon;
                break;
            case 1:
                currentKind = EquipKind.SubWeapon;
                break;
            case 2:
                currentKind = EquipKind.UtilityItem;
                break;
        }
        OpenEquipWin2();
    }

    public void SelectItem(int index)
    {
        if (currentItemIndex != -1) 
            itemObjs[currentItemIndex].GetComponent<Image>().color = Color.white;
        
        currentItemIndex = index;
        itemObjs[currentItemIndex].GetComponent<Image>().color = Color.red;

        OpenWeaponInfo();
    }

    public void OpenWeaponInfo()
    {
        if(!WeaponWin.activeSelf) WeaponWin.SetActive(true);
        if (!SpecCompareWin.activeSelf) SpecCompareWin.SetActive(true);

        var key = itemInfo[currentItemIndex];
        var weapon = playerDataMgr.currentEquippables[key];
        string selectedWeaStr = $"기본 명중률 : {weapon.accur_Rate_Base}% \n" +
            $"데미지 : {weapon.min_damage} ~ {weapon.max_damage} \n" +
            $"탄창 클립수 : {weapon.bullet} \n" +
            $"명중률 감소 : {weapon.accur_Rate_Dec}%";
        weaponSpecTxt.text = selectedWeaStr;

        Weapon currentWeapon;
        if (currentKind == EquipKind.MainWeapon)
        {
            if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null)
            {
                currentWeapon = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon;
                string str = $"기본 명중률 : {currentWeapon.accur_Rate_Base}% \n" +
                   $"데미지 : {currentWeapon.min_damage} ~ {currentWeapon.max_damage} \n" +
                   $"탄창 클립수 : {currentWeapon.bullet} \n" +
                   $"명중률 감소 : {currentWeapon.accur_Rate_Dec}%";
                currentWeaSpecTxt.text = str;

                str = $"기본 명중률 : {currentWeapon.accur_Rate_Base - weapon.accur_Rate_Base}% \n" +
                   $"데미지 : {currentWeapon.min_damage - weapon.min_damage} ~ {currentWeapon.max_damage - weapon.max_damage} \n" +
                   $"탄창 클립수 : {currentWeapon.bullet - weapon.bullet} \n" +
                   $"명중률 감소 : {currentWeapon.accur_Rate_Dec - weapon.accur_Rate_Dec}%";
                changeWeaSpecTxt.text = str;
            }
            else
            {
                string str = $"기본 명중률 : -% \n" +
                   $"데미지 : - \n" +
                   $"탄창 클립수 : - \n" +
                   $"명중률 감소 : -%";
                currentWeaSpecTxt.text = str;
                changeWeaSpecTxt.text = selectedWeaStr;
            }
        }
        else if (currentKind == EquipKind.SubWeapon)
        {
            if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null)
            {
                currentWeapon = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon;
                string str = $"기본 명중률 : {currentWeapon.accur_Rate_Base}% \n" +
                   $"데미지 : {currentWeapon.min_damage} ~ {currentWeapon.max_damage} \n" +
                   $"탄창 클립수 : {currentWeapon.bullet} \n" +
                   $"명중률 감소 : {currentWeapon.accur_Rate_Dec}%";
                currentWeaSpecTxt.text = str;

                str = $"기본 명중률 : {currentWeapon.accur_Rate_Base - weapon.accur_Rate_Base}% \n" +
                   $"데미지 : {currentWeapon.min_damage - weapon.min_damage} ~ {currentWeapon.max_damage - weapon.max_damage} \n" +
                   $"탄창 클립수 : {currentWeapon.bullet - weapon.bullet} \n" +
                   $"명중률 감소 : {currentWeapon.accur_Rate_Dec - weapon.accur_Rate_Dec}%";
                changeWeaSpecTxt.text = str;
            }
            else
            {
                string str = $"기본 명중률 : -% \n" +
                   $"데미지 : - \n" +
                   $"탄창 클립수 : - \n" +
                   $"명중률 감소 : -%";
                currentWeaSpecTxt.text = str;
                changeWeaSpecTxt.text = selectedWeaStr;
            }
        }
    }

    public void Equip()
    {
        Disarm();
        if (currentKind == EquipKind.MainWeapon)
        {
            var key = itemInfo[currentItemIndex];
            var weapon = playerDataMgr.currentEquippables[key];
            playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon = weapon;

            playerDataMgr.currentEquippables.Remove(key);
            playerDataMgr.currentEquippablesNum.Remove(key);

            var index = currentIndex;
            playerDataMgr.saveData.mainWeapon[index] = key;
            index = playerDataMgr.saveData.equippableList.IndexOf(key);
            playerDataMgr.saveData.equippableList.Remove(key);
            playerDataMgr.saveData.equippableNumList.RemoveAt(index);
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            Destroy(itemObjs[currentItemIndex]);
            itemInfo.Remove(currentItemIndex);
        }
        else if (currentKind == EquipKind.SubWeapon)
        {
            var key = itemInfo[currentItemIndex];
            var weapon = playerDataMgr.currentEquippables[key];
            playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = weapon;

            playerDataMgr.currentEquippables.Remove(key);
            playerDataMgr.currentEquippablesNum.Remove(key);

            var index = currentIndex;
            playerDataMgr.saveData.subWeapon[index] = key;
            index = playerDataMgr.saveData.equippableList.IndexOf(key);
            playerDataMgr.saveData.equippableList.Remove(key);
            playerDataMgr.saveData.equippableNumList.RemoveAt(index);
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            Destroy(itemObjs[currentItemIndex]);
            itemInfo.Remove(currentItemIndex);
        }
    }

    public void Disarm()
    {
        if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null && currentKind == EquipKind.MainWeapon)
        {
            var id = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.id;
            
            playerDataMgr.saveData.equippableList.Add(id);
            playerDataMgr.saveData.equippableNumList.Add(1);
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon = null;
            playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
            playerDataMgr.currentEquippablesNum.Add(id, 1);
        }
        else if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null && currentKind == EquipKind.SubWeapon)
        {
            var id = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.id;
            
            playerDataMgr.saveData.equippableList.Add(id);
            playerDataMgr.saveData.equippableNumList.Add(1);
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = null;
            playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
            playerDataMgr.currentEquippablesNum.Add(id, 1);
        }
    }

    //창 설정.
    public void OpenEquipWin1()
    {
        if (equipmentWin2.activeSelf) equipmentWin2.SetActive(false);
        if (!equipmentWin1.activeSelf) equipmentWin1.SetActive(true);
    }

    public void CloseEquipWin1()
    {
        equipmentWin1.SetActive(false);
    }

    public void OpenEquipWin2()
    {
        equipmentWin1.SetActive(false);
        equipmentWin2.SetActive(true);

        if (WeaponWin.activeSelf) WeaponWin.SetActive(false);
        if (SpecCompareWin.activeSelf) SpecCompareWin.SetActive(false);
    }
}

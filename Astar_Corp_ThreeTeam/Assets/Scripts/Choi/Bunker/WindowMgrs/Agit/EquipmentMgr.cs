using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipKind
{ 
    None,
    MainWeapon,
    SubWeapon,
    Bag
}

public class EquipmentMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;

    public GameObject equipmentWin;
    public GameObject equipmentWin1;
    public GameObject equipmentWin2;
    public GameObject detailWin;

    //창1 관련.
    public Text mainWeaponTxt;
    public Text subWeaponTxt;
    
    //창2 관련.
    public GameObject SpecCompareWin;
    public Text currentWeaSpecTxt;
    public Text changeWeaSpecTxt;
    public Text weaponKindTxt;

    [Header("Detail Win")]
    public Text weaponName;
    public Text typeNameTxt;
    public Text accuracyNumTxt;
    public Text damageNumTxt;
    public Text criticalNumTxt;
    public Text bulletNumTxt;
    public Text shootingRangeNumTxt;

    public GameObject itemListContents;
    public GameObject itemPrefab;
    public Dictionary<string, GameObject> itemObjs = new Dictionary<string, GameObject>();
    //public Dictionary<string, int> itemInfo = new Dictionary<string, int>();
   
    public int currentIndex;
    string currentKey;
    EquipKind currentKind;

    public void Init()
    {
        OpenEquipWin1();

        currentKey = null;
        currentKind = EquipKind.None;
    }

    public void WeaponList(int index)
    {
        //1. 주무기
        //2. 보조무기

        if (itemObjs.Count != 0)
        {
            foreach (var element in itemObjs)
            {
                Destroy(element.Value);
            }
            itemListContents.transform.DetachChildren();
            itemObjs.Clear();
        }
        //if (itemInfo.Count != 0) itemInfo.Clear();

        foreach (var element in playerDataMgr.currentEquippables)
        {
            if (index != int.Parse(element.Value.type)) continue;
            var go = Instantiate(itemPrefab, itemListContents.transform);
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(element.Key); });

            var child = go.transform.GetChild(0).gameObject;
            child.transform.GetChild(0).GetComponent<Text>().text = element.Value.name;

            string type = GetTypeStr(element.Value.kind);

            child = go.transform.GetChild(1).gameObject;
            child.transform.GetChild(0).GetComponent<Text>().text = $"{type}";

            itemObjs.Add(element.Key, go);
            //itemInfo.Add(element.Key, playerDataMgr.currentEquippablesNum[element.Key]);
        }
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

    public void RefreshEquipList()
    {
        if (currentIndex != -1)
        {
            mainWeaponTxt.text =
                (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null) ?
                playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.name : "비어있음";
            subWeaponTxt.text =
                (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null) ?
                playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.name : "비어있음";
        }
    }

    public void SelectEquipKind(int kind)
    {
        switch (kind)
        {
            case 0:
                currentKind = EquipKind.MainWeapon;
                weaponKindTxt.text = "주무기";
                WeaponList(1);
                break;
            case 1:
                currentKind = EquipKind.SubWeapon;
                weaponKindTxt.text = "보조무기";
                WeaponList(2);
                break;
            case 2:
                currentKind = EquipKind.Bag;
                break;
        }
        OpenEquipWin2();
    }

    public void SelectItem(string key)
    {
        if (currentKey != null) 
            itemObjs[currentKey].GetComponent<Image>().color = Color.white;

        currentKey = key;
        itemObjs[currentKey].GetComponent<Image>().color = Color.red;

        OpenWeaponInfo();
    }

    public void OpenWeaponInfo()
    {
        if(!detailWin.activeSelf) detailWin.SetActive(true);
        if (!SpecCompareWin.activeSelf) SpecCompareWin.SetActive(true);

        RefreshSpec();
    }

    public void Equip()
    {
        var weaponType = playerDataMgr.equippableList[currentKey].type;
        if (!playerDataMgr.currentSquad[currentIndex].character.weapons.Contains(weaponType))
            return;

        Disarm();
        if (currentKind == EquipKind.MainWeapon)
        {
            playerDataMgr.saveData.mainWeapon[currentIndex] = currentKey;
            var weapon = playerDataMgr.equippableList[currentKey];
            playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon = weapon;

            mainWeaponTxt.text = weapon.name;

            //json.
            var id = currentKey;
            var index = playerDataMgr.saveData.equippableList.IndexOf(id);
            if (playerDataMgr.saveData.equippableNumList[index] - 1 == 0)
            {
                playerDataMgr.saveData.equippableList.Remove(id);
                playerDataMgr.saveData.equippableNumList.RemoveAt(index);
            }
            else
            {
                playerDataMgr.saveData.equippableNumList[index] -= 1;
            }
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            if (playerDataMgr.currentEquippablesNum[id] - 1 == 0)
            {
                //현재 데이터.
                //itemInfo.Remove(currentKey);
                Destroy(itemObjs[currentKey]);
                itemObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Remove(id);
                playerDataMgr.currentEquippablesNum.Remove(id);

                currentKey = null;
                currentKind = EquipKind.None;
            }
            else
            {
                //현재 데이터.
                //itemInfo[currentKey] -= 1;

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] -= 1;
                //var child = itemObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{playerDataMgr.currentEquippablesNum[id]}개";
            }
        }
        else if (currentKind == EquipKind.SubWeapon)
        {
            playerDataMgr.saveData.subWeapon[currentIndex] = currentKey;
            var weapon = playerDataMgr.equippableList[currentKey];
            playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = weapon;

            subWeaponTxt.text = weapon.name;

            //json.
            var id = currentKey;
            var index = playerDataMgr.saveData.equippableList.IndexOf(id);
            if (playerDataMgr.saveData.equippableNumList[index] - 1 == 0)
            {
                playerDataMgr.saveData.equippableList.Remove(id);
                playerDataMgr.saveData.equippableNumList.RemoveAt(index);
            }
            else
            {
                playerDataMgr.saveData.equippableNumList[index] -= 1;
            }
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            if (playerDataMgr.currentEquippablesNum[id] - 1 == 0)
            {
                //현재 데이터.
                //itemInfo.Remove(currentKey);
                Destroy(itemObjs[currentKey]);
                itemObjs.Remove(currentKey);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Remove(id);
                playerDataMgr.currentEquippablesNum.Remove(id);

                currentKey = null;
                currentKind = EquipKind.None;
            }
            else
            {
                //현재 데이터.
                //itemInfo[currentKey] -= 1;

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] -= 1;
                //var child = itemObjs[currentKey].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{playerDataMgr.currentEquippablesNum[id]}개";
            }
        }
        CloseEquipWin2();
    }

    public void ClickDisarmButton()
    {
        Disarm();
        switch (currentKind)
        {
            case EquipKind.MainWeapon:
                mainWeaponTxt.text = "비어있음";
                break;
            case EquipKind.SubWeapon:
                subWeaponTxt.text = "비어있음";
                break;
        }

        CloseEquipWin2();
    }

    public void Disarm()
    {
        if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null && currentKind == EquipKind.MainWeapon)
        {
            var id = playerDataMgr.saveData.mainWeapon[currentIndex];
            var weapon = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon;
            playerDataMgr.saveData.mainWeapon[currentIndex] = null;
            playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon = null;

            //json.
            int index = 0;
            if (!playerDataMgr.saveData.equippableList.Contains(id))
            {
                playerDataMgr.saveData.equippableList.Add(id);
                playerDataMgr.saveData.equippableNumList.Add(1);
            }
            else
            {
                index = playerDataMgr.saveData.equippableList.IndexOf(id);
                playerDataMgr.saveData.equippableNumList[index] += 1;
            }
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentEquippables.ContainsKey(id))
            {
                //현재데이터 관련.
                //itemInfo.Add(id, 1);
                
                //var go = Instantiate(itemPrefab, itemListContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = weapon.name;
                //child = go.transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"1개";

                //var button = go.AddComponent<Button>();
                //button.onClick.AddListener(delegate { SelectItem(id); });
                //itemObjs.Add(id, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, 1);
            }
            else
            {
                //현재데이터 관련.
                //itemInfo[id] += 1;
                
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] += 1;
                //var child = itemObjs[id].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{playerDataMgr.currentEquippablesNum[id]}개";
            }
            WeaponList(1);
        }
        else if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null && currentKind == EquipKind.SubWeapon)
        {
            var id = playerDataMgr.saveData.subWeapon[currentIndex];
            var weapon = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon;
            playerDataMgr.saveData.subWeapon[currentIndex] = null;
            playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = null;

            //json.
            int index = 0;
            if (!playerDataMgr.saveData.equippableList.Contains(id))
            {
                playerDataMgr.saveData.equippableList.Add(id);
                playerDataMgr.saveData.equippableNumList.Add(1);
            }
            else
            {
                index = playerDataMgr.saveData.equippableList.IndexOf(id);
                playerDataMgr.saveData.equippableNumList[index] += 1;
            }
            PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            //playerDataMgr.
            if (!playerDataMgr.currentEquippables.ContainsKey(id))
            {
                //현재데이터 관련.
                //itemInfo.Add(id, 1);

                //var go = Instantiate(itemPrefab, itemListContents.transform);
                //var child = go.transform.GetChild(0).gameObject;
                //child.GetComponent<Text>().text = weapon.name;
                //child = go.transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"1개";

                //var button = go.AddComponent<Button>();
                //button.onClick.AddListener(delegate { SelectItem(id); });
                //itemObjs.Add(id, go);

                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, 1);
            }
            else
            {
                //현재데이터 관련.
                //itemInfo[id] += 1;
            
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] += 1;
                //var child = itemObjs[id].transform.GetChild(1).gameObject;
                //child.GetComponent<Text>().text = $"{playerDataMgr.currentEquippablesNum[id]}개";
            }
            WeaponList(2);
        }
        else if (playerDataMgr.currentSquad[currentIndex].bagLevel != 0 && currentKind == EquipKind.Bag)
        {
            playerDataMgr.saveData.bagLevel[currentIndex] = 0;
            playerDataMgr.currentSquad[currentIndex].bagLevel = 0;

            //json.
            //int index = 0;
            //if (!playerDataMgr.saveData.equippableList.Contains(id))
            //{
            //    playerDataMgr.saveData.equippableList.Add(id);
            //    playerDataMgr.saveData.equippableNumList.Add(1);
            //}
            //else
            //{
            //    index = playerDataMgr.saveData.equippableList.IndexOf(id);
            //    playerDataMgr.saveData.equippableNumList[index] += 1;
            //}
            //PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

            ////playerDataMgr.
            //if (!playerDataMgr.currentEquippables.ContainsKey(id))
            //{
            //    //현재데이터 관련.
            //    itemInfo.Add(id, 1);

            //    var go = Instantiate(itemPrefab, itemListContents.transform);
            //    var child = go.transform.GetChild(0).gameObject;
            //    child.GetComponent<Text>().text = weapon.name;
            //    child = go.transform.GetChild(1).gameObject;
            //    child.GetComponent<Text>().text = $"1개";

            //    var button = go.AddComponent<Button>();
            //    button.onClick.AddListener(delegate { SelectItem(id); });
            //    itemObjs.Add(id, go);

            //    //플레이어 데이터 매니저 관련.
            //    playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
            //    playerDataMgr.currentEquippablesNum.Add(id, 1);
            //}
            //else
            //{
            //    //현재데이터 관련.
            //    itemInfo[id] += 1;
            //    var child = itemObjs[id].transform.GetChild(1).gameObject;
            //    child.GetComponent<Text>().text = $"{itemInfo[id]}개";

            //    //플레이어 데이터 매니저 관련.
            //    playerDataMgr.currentEquippablesNum[id] += 1;
            //}
        }
    }

    // 홍수진_스탯 수정
    // 명중률 스탯 주석처리
    public void RefreshSpec()
    {
        var weapon = playerDataMgr.equippableList[currentKey];
        string selectedWeaStr = $"기본 명중률 : {weapon.accurRateBase}% \n" +
            $"데미지 : {weapon.minDamage} ~ {weapon.maxDamage} \n" +
            $"탄창 클립수 : {weapon.bullet} \n" /*+
            $"명중률 감소 : {weapon.accur_Rate_Dec}%"*/;

        weaponName.text = $"{weapon.name}";
        typeNameTxt.text = $"{GetTypeStr(weapon.kind)}";
        accuracyNumTxt.text = $"{weapon.accurRateBase}%";
        damageNumTxt.text = $"{weapon.minDamage} ~ {weapon.maxDamage}";
        criticalNumTxt.text = $"{weapon.critRate}%";
        bulletNumTxt.text = $"{weapon.bullet}";
        shootingRangeNumTxt.text = $"{weapon.minRange} ~ {weapon.maxRange}";

        Weapon currentWeapon;
        if (currentKind == EquipKind.MainWeapon)
        {

            if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null)
            {
                currentWeapon = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon;
                string str = $"기본 명중률 : {currentWeapon.accurRateBase}%\n" +
                   $"데미지 : {currentWeapon.minDamage} ~ {currentWeapon.maxDamage}\n" +
                   $"탄창 클립수 : {currentWeapon.bullet}\n" /*+
                   $"명중률 감소 : {currentWeapon.accur_Rate_Dec}%"*/;
                currentWeaSpecTxt.text = str;

                str = $"기본 명중률 : {weapon.accurRateBase - currentWeapon.accurRateBase}%\n" +
                   $"데미지 : {weapon.minDamage - currentWeapon.minDamage} ~ {weapon.maxDamage - currentWeapon.maxDamage}\n" +
                   $"탄창 클립수 : {weapon.bullet - currentWeapon.bullet} \n" /*+
                   $"명중률 감소 : {weapon.accur_Rate_Dec - currentWeapon.accur_Rate_Dec}%"*/;
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
                string str = $"기본 명중률 : {currentWeapon.accurRateBase}%\n" +
                   $"데미지 : {currentWeapon.minDamage} ~ {currentWeapon.maxDamage}\n" +
                   $"탄창 클립수 : {currentWeapon.bullet}\n"; // +
                   //$"명중률 감소 : {currentWeapon.accur_Rate_Dec}%";
                currentWeaSpecTxt.text = str;

                str = $"기본 명중률 : {weapon.accurRateBase - currentWeapon.accurRateBase}%\n" +
                   $"데미지 : {weapon.minDamage - currentWeapon.minDamage} ~ {weapon.maxDamage - currentWeapon.maxDamage}\n" +
                   $"탄창 클립수 : {weapon.bullet - currentWeapon.bullet}\n"; // +
                   //$"명중률 감소 : {weapon.accur_Rate_Dec - currentWeapon.accur_Rate_Dec}%";
                changeWeaSpecTxt.text = str;
            }
            else
            {
                string str = $"기본 명중률 : -% \n" +
                   $"데미지 : - \n" +
                   $"탄창 클립수 : - \n"; // +
                   //$"명중률 감소 : -%";
                currentWeaSpecTxt.text = str;
                changeWeaSpecTxt.text = selectedWeaStr;
            }
        }
    }

    //창 설정.
    public void OpenEquipWin1()
    {
        if (equipmentWin2.activeSelf) equipmentWin2.SetActive(false);
        if (!equipmentWin1.activeSelf) equipmentWin1.SetActive(true);
    }

    public void OpenEquipWin2()
    {
        equipmentWin1.SetActive(false);
        equipmentWin2.SetActive(true);

        if (detailWin.activeSelf) detailWin.SetActive(false);
        if (SpecCompareWin.activeSelf) SpecCompareWin.SetActive(false);
    }

    public void CloseEquipWin2()
    {
        if (currentKey != null && itemObjs.ContainsKey(currentKey))
        {
            if (itemObjs[currentKey].GetComponent<Image>().color == Color.red)
                itemObjs[currentKey].GetComponent<Image>().color = Color.white;
            currentKey = null;
        }

        equipmentWin2.SetActive(false);
        equipmentWin1.SetActive(true);
    }
}

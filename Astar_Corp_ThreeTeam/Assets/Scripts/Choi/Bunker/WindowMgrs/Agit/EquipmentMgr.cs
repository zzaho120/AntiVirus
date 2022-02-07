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
    public AgitMgr agitMgr;

    public GameObject equipmentWin;
    public GameObject equipmentWin1;
    public GameObject equipmentWin2;
    public GameObject detailWin;

    //창1 관련.
    public Image mainWeaponImg;
    public Image subWeaponImg;
    public Text mainWeaponTxt;
    public Text subWeaponTxt;

    //창2 관련.
    public List<GameObject> menuObjs;
    int weaponType;
    public GameObject SpecCompareWin;
    public Text currentWeaSpecTxt;
    public Text changeWeaSpecTxt;
    public Text weaponKindTxt;
    public Image currentWeaponImg;
    public Image changeWeaponImg;

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
        int weaponKind = -1;
        if (currentKind == EquipKind.MainWeapon) weaponKind = 1;
        else if (currentKind == EquipKind.SubWeapon) weaponKind = 2;

        //1.Handgun 6 
        //2.SG 2
        //3.SMG 3
        //4.AR 1
        //5.LMG 4
        //6.SR 5
        //7.근접무기 6
        if (weaponType != -1)
        {
            menuObjs[weaponType].GetComponent<Image>().color = Color.white;
        }
        weaponType = index;
        menuObjs[weaponType].GetComponent<Image>().color = new Color(255f / 255, 192f / 255, 0f / 255);

        if (itemObjs.Count != 0)
        {
            foreach (var element in itemObjs)
            {
                Destroy(element.Value);
            }
            itemListContents.transform.DetachChildren();
            itemObjs.Clear();
        }
        
        switch (weaponKind)
        {
            case 1:
                if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null)
                {
                    var weapon = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon;
                    var go = Instantiate(itemPrefab, itemListContents.transform);

                    var child = go.transform.GetChild(4).gameObject;
                    var button = child.AddComponent<Button>();
                    button.onClick.AddListener(delegate { SelectItem("mainWeapon"); });

                    go.GetComponent<Image>().sprite = weapon.img;

                    child = go.transform.GetChild(0).gameObject;
                    var childObj = child.transform.GetChild(1).gameObject;
                    string type = GetTypeStr(weapon.kind);
                    childObj.transform.GetChild(0).GetComponent<Text>().text = $"{type}";

                    child = go.transform.GetChild(1).gameObject;
                    childObj = child.transform.GetChild(1).gameObject;
                    childObj.transform.GetChild(0).GetComponent<Text>().text =
                        (weapon.bulletType == 0) ? "-" : $"{GetBulletType(weapon.bulletType)}";

                    child = go.transform.GetChild(2).gameObject;
                    child.SetActive(true);

                    child = go.transform.GetChild(3).gameObject;
                    child.SetActive(false);

                    child = go.transform.GetChild(4).gameObject;
                    child.GetComponent<Text>().text = $"{weapon.name}";

                    child = go.transform.GetChild(5).gameObject;
                    child.SetActive(false);

                    itemObjs.Add("mainWeapon", go);
                }
                break;
            case 2:
                if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null)
                {
                    var weapon = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon;
                    var go = Instantiate(itemPrefab, itemListContents.transform);

                    var child = go.transform.GetChild(4).gameObject;
                    var button = child.AddComponent<Button>();
                    button.onClick.AddListener(delegate { SelectItem("subWeapon"); });

                    go.GetComponent<Image>().sprite = weapon.img;

                    child = go.transform.GetChild(0).gameObject;
                    var childObj = child.transform.GetChild(1).gameObject;
                    string type = GetTypeStr(weapon.kind);
                    childObj.transform.GetChild(0).GetComponent<Text>().text = $"{type}";

                    child = go.transform.GetChild(1).gameObject;
                    childObj = child.transform.GetChild(1).gameObject;
                    childObj.transform.GetChild(0).GetComponent<Text>().text =
                        (weapon.bulletType == 0) ? "-" : $"{GetBulletType(weapon.bulletType)}";

                    child = go.transform.GetChild(2).gameObject;
                    child.SetActive(true);

                    child = go.transform.GetChild(3).gameObject;
                    child.SetActive(false);

                    child = go.transform.GetChild(4).gameObject;
                    child.GetComponent<Text>().text = $"{weapon.name}";

                    child = go.transform.GetChild(5).gameObject;
                    child.SetActive(false);

                    itemObjs.Add("subWeapon", go);
                }
                break;
        }

        foreach (var element in playerDataMgr.currentEquippables)
        {
            if (weaponKind != int.Parse(element.Value.type)) continue;
            if (index != 0 && index != 7 &&  int.Parse(element.Value.kind) != index) continue;
            if (index == 7 && (int.Parse(element.Value.kind) != 1 || int.Parse(element.Value.kind) != 7)) continue;
            string type = GetTypeStr(element.Value.kind);

            var go = Instantiate(itemPrefab, itemListContents.transform);
            var child = go.transform.GetChild(4).gameObject;
            var button = child.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectItem(element.Key); });

            go.GetComponent<Image>().sprite = element.Value.img;

            child = go.transform.GetChild(0).gameObject;
            var childObj = child.transform.GetChild(1).gameObject;
            childObj.transform.GetChild(0).GetComponent<Text>().text = $"{type}";

            child = go.transform.GetChild(1).gameObject;
            childObj = child.transform.GetChild(1).gameObject;
            childObj.transform.GetChild(0).GetComponent<Text>().text =
                (element.Value.bulletType == 0) ? "-" : $"{GetBulletType(element.Value.bulletType)}";

            child = go.transform.GetChild(2).gameObject;
            child.SetActive(false);

            child = go.transform.GetChild(3).gameObject;
            child.GetComponent<Text>().text = $"X {playerDataMgr.currentEquippablesNum[element.Key]}";

            child = go.transform.GetChild(4).gameObject;
            child.GetComponent<Text>().text = $"{element.Value.name}";

            child = go.transform.GetChild(5).gameObject;
            child.SetActive(false);

            itemObjs.Add(element.Key, go);
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

    string GetBulletType(int kind)
    {
        string type = string.Empty;
        switch (kind)
        {
            case 1:
                type = "12";
                break;
            case 2:
                type = "0.45";
                break;
            case 3:
                type = "5.56";
                break;
            case 4:
                type = "7.62";
                break;
            case 5:
                type = "9";
                break;
        }
        return type;
    }

    public void RefreshEquipList()
    {
        if (currentIndex != -1)
        {
            mainWeaponImg.sprite =
                 (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null) ?
                 playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.img : null;
            mainWeaponTxt.text =
                (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null) ?
                playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.name : "비어있음";
            
            subWeaponImg.sprite =
                (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null) ?
                playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.img : null;
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
                WeaponList(0);
                break;
            case 1:
                currentKind = EquipKind.SubWeapon;
                weaponKindTxt.text = "보조무기";
                WeaponList(0);
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
        {
            var child = itemObjs[currentKey].transform.GetChild(6).gameObject;
            child.GetComponent<Image>().color = Color.white;
        }

        currentKey = key;
        var childObj = itemObjs[currentKey].transform.GetChild(6).gameObject;
        childObj.GetComponent<Image>().color = new Color(255f / 255, 192f / 255, 0f / 255); ;

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
            mainWeaponImg.sprite = weapon.img;
            agitMgr.mainWeaponImg.sprite = weapon.img;

            var go = agitMgr.characterObjs[currentIndex];
            var child = go.transform.GetChild(2).gameObject;
            child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = mainWeaponImg.sprite;
            child.transform.GetChild(1).gameObject.GetComponent<Text>().text = weapon.name;

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
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] -= 1;
            }
        }
        else if (currentKind == EquipKind.SubWeapon)
        {
            playerDataMgr.saveData.subWeapon[currentIndex] = currentKey;
            var weapon = playerDataMgr.equippableList[currentKey];
            playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = weapon;

            subWeaponTxt.text = weapon.name;
            subWeaponImg.sprite = weapon.img;
            agitMgr.subWeaponImg.sprite = weapon.img;

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
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] -= 1;
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
                mainWeaponImg.sprite = null;
                mainWeaponTxt.text = "비어있음";

                var go = agitMgr.characterObjs[currentIndex];
                var child = go.transform.GetChild(2).gameObject;
                child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
                child.transform.GetChild(1).gameObject.GetComponent<Text>().text = "비어있음";
                break;
            case EquipKind.SubWeapon:
                subWeaponImg.sprite = null;
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
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, 1);
            }
            else
            {
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] += 1;
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
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, 1);
            }
            else
            {
                //플레이어 데이터 매니저 관련.
                playerDataMgr.currentEquippablesNum[id] += 1;
            }
            WeaponList(2);
        }
        else if (playerDataMgr.currentSquad[currentIndex].bagLevel != 0 && currentKind == EquipKind.Bag)
        {
            playerDataMgr.saveData.bagLevel[currentIndex] = 0;
            playerDataMgr.currentSquad[currentIndex].bagLevel = 0;
        }
    }

    // 홍수진_스탯 수정
    // 명중률 스탯 주석처리
    public void RefreshSpec()
    {
        var weapon = playerDataMgr.equippableList[currentKey];
        changeWeaponImg.sprite = weapon.img;
        string selectedWeaStr = $"명중률{weapon.accurRateBase}%\n" +
            $"최소 데미지{weapon.minDamage} 최대 데미지{weapon.maxDamage}\n" +
            $"크리 확률{weapon.critRate}% 크리데미지{weapon.critDamage}\n"+
            $"탄창수(클립수){weapon.bullet}\n" +
            $"최소 사거리{weapon.minRange} 최대 사거리{weapon.maxRange}";

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
                currentWeaponImg.sprite = currentWeapon.img;
               
                string str = $"명중률{currentWeapon.accurRateBase}%\n" +
                   $"최소 데미지{currentWeapon.minDamage} 최대 데미지{currentWeapon.maxDamage}\n" +
                   $"크리 확률{currentWeapon.critRate}% 크리데미지{currentWeapon.critDamage}\n" +
                   $"탄창수(클립수){currentWeapon.bullet}\n" +
                   $"최소 사거리{currentWeapon.minRange} 최대 사거리{currentWeapon.maxRange}";
                currentWeaSpecTxt.text = str;

                string accurRateDiff = (weapon.accurRateBase - currentWeapon.accurRateBase < 0) ?
                    $"{weapon.accurRateBase - currentWeapon.accurRateBase}" : $"+{weapon.accurRateBase - currentWeapon.accurRateBase}";
                string minDamageDiff = (weapon.minDamage - currentWeapon.minDamage < 0) ?
                    $"{weapon.minDamage - currentWeapon.minDamage}" : $"+{weapon.minDamage - currentWeapon.minDamage}";
                string maxDamageDiff = (weapon.maxDamage - currentWeapon.maxDamage < 0) ?
                    $"{weapon.maxDamage - currentWeapon.maxDamage}" : $"+{weapon.maxDamage - currentWeapon.maxDamage}";
                string criRateDiff = (weapon.critRate - currentWeapon.critRate < 0) ?
                    $"{weapon.critRate - currentWeapon.critRate}" : $"+{weapon.critRate - currentWeapon.critRate}";
                string critDamageDiff = (weapon.critDamage - currentWeapon.critDamage < 0) ?
                    $"{weapon.critDamage - currentWeapon.critDamage}" : $"+{weapon.critDamage - currentWeapon.critDamage}";
                string bulletDiff = (weapon.bullet - currentWeapon.bullet < 0) ?
                   $"{weapon.bullet - currentWeapon.bullet}" : $"+{weapon.bullet - currentWeapon.bullet}";
                string minRangeDiff = (weapon.minRange - currentWeapon.minRange < 0) ?
                   $"{weapon.minRange - currentWeapon.minRange}" : $"+{weapon.minRange - currentWeapon.minRange}";
                string maxRangeDiff = (weapon.maxRange - currentWeapon.maxRange < 0) ?
                  $"{weapon.maxRange - currentWeapon.maxRange}" : $"+{weapon.maxRange - currentWeapon.maxRange}";

                str = $"명중률{accurRateDiff}%\n" +
                   $"최소 데미지{minDamageDiff} 최대 데미지{maxDamageDiff}\n" +
                   $"크리 확률{criRateDiff}% 크리데미지{critDamageDiff}\n" +
                   $"탄창수(클립수){bulletDiff} \n" +
                   $"최소 사거리{minRangeDiff} 최대 사거리{maxRangeDiff}";
                changeWeaSpecTxt.text = str;
            }
            else
            {
                currentWeaponImg.sprite = null;
                string str = $"명중률 : -% \n" +
                  $"최소 데미지- 최대 데미지-\n" +
                   $"크리 확률-% 크리데미지-\n" +
                   $"탄창수(클립수)-\n" +
                   $"최소 사거리- 최대 사거리-";
                currentWeaSpecTxt.text = str;
                changeWeaSpecTxt.text = selectedWeaStr;
            }
        }
        else if (currentKind == EquipKind.SubWeapon)
        {
            if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null)
            {
                currentWeapon = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon;
                currentWeaponImg.sprite = currentWeapon.img;
                
                string str = $"명중률 : {currentWeapon.accurRateBase}%\n" +
                      $"최소 데미지{currentWeapon.minDamage} 최대 데미지{currentWeapon.maxDamage}\n" +
                   $"크리 확률{currentWeapon.critRate}% 크리데미지{currentWeapon.critDamage}\n" +
                   $"탄창수(클립수){currentWeapon.bullet}\n" +
                   $"최소 사거리{currentWeapon.minRange} 최대 사거리{currentWeapon.maxRange}";
                currentWeaSpecTxt.text = str;

                string accurRateDiff = (weapon.accurRateBase - currentWeapon.accurRateBase < 0) ?
                    $"{weapon.accurRateBase - currentWeapon.accurRateBase}" : $"+{weapon.accurRateBase - currentWeapon.accurRateBase}";
                string minDamageDiff = (weapon.minDamage - currentWeapon.minDamage < 0) ?
                    $"{weapon.minDamage - currentWeapon.minDamage}" : $"+{weapon.minDamage - currentWeapon.minDamage}";
                string maxDamageDiff = (weapon.maxDamage - currentWeapon.maxDamage < 0) ?
                    $"{weapon.maxDamage - currentWeapon.maxDamage}" : $"+{weapon.maxDamage - currentWeapon.maxDamage}";
                string criRateDiff = (weapon.critRate - currentWeapon.critRate < 0) ?
                    $"{weapon.critRate - currentWeapon.critRate}" : $"+{weapon.critRate - currentWeapon.critRate}";
                string critDamageDiff = (weapon.critDamage - currentWeapon.critDamage < 0) ?
                    $"{weapon.critDamage - currentWeapon.critDamage}" : $"+{weapon.critDamage - currentWeapon.critDamage}";
                string bulletDiff = (weapon.bullet - currentWeapon.bullet < 0) ?
                   $"{weapon.bullet - currentWeapon.bullet}" : $"+{weapon.bullet - currentWeapon.bullet}";
                string minRangeDiff = (weapon.minRange - currentWeapon.minRange < 0) ?
                   $"{weapon.minRange - currentWeapon.minRange}" : $"+{weapon.minRange - currentWeapon.minRange}";
                string maxRangeDiff = (weapon.maxRange - currentWeapon.maxRange < 0) ?
                  $"{weapon.maxRange - currentWeapon.maxRange}" : $"+{weapon.maxRange - currentWeapon.maxRange}";

                str = $"명중률{accurRateDiff}%\n" +
                   $"최소 데미지{minDamageDiff} 최대 데미지{maxDamageDiff}\n" +
                   $"크리 확률{criRateDiff}% 크리데미지{critDamageDiff}\n" +
                   $"탄창수(클립수){bulletDiff} \n" +
                   $"최소 사거리{minRangeDiff} 최대 사거리{maxRangeDiff}";
                changeWeaSpecTxt.text = str;
            }
            else
            {
                currentWeaponImg.sprite = null;
                string str = $"명중률 : -% \n" +
                  $"최소 데미지- 최대 데미지-\n" +
                   $"크리 확률-% 크리데미지-\n" +
                   $"탄창수(클립수)-\n" +
                   $"최소 사거리- 최대 사거리-";
                currentWeaSpecTxt.text = str;
                changeWeaSpecTxt.text = selectedWeaStr;
            }
        }
    }

    //창 설정.
    public void OpenEquipWin1()
    {
        weaponType = -1;


        if (equipmentWin2.activeSelf) equipmentWin2.SetActive(false);
        if (!equipmentWin1.activeSelf) equipmentWin1.SetActive(true);
        RefreshEquipList();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

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
    public GameObject weaponImg;
    public Text damageTxt;
    public Text criTxt;
    public Text bulletTypeTxt;
    public Text loadTxt;
    public Text clipTxt;
    public Text shotTxt;
    public Text accuracyTxt;
    public Text continuousShotTxt;
    public Text recoilTxt;

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

        //0.All
        //1.AR 
        //2.SG
        //3.SMG
        //4.LMG
        //5.SR
        //6.ETC

        //초기화.
        Color originButtonColor = new Color(27f / 255, 40f / 255, 52f / 255);
        foreach(var element in menuObjs)
        {
            if(element.GetComponent<Image>().color != originButtonColor) 
                element.GetComponent<Image>().color = originButtonColor;
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

        //0.All
        //1.AR 
        //2.SG
        //3.SMG
        //4.LMG
        //5.SR
        //6.ETC
        int weaponKindIndex = -1;
        switch (index)
        {
            case 1:
                weaponKindIndex = 4;
                break;
            case 2:
                weaponKindIndex = 2;
                break;
            case 3:
                weaponKindIndex = 3;
                break;
            case 4:
                weaponKindIndex = 5;
                break;
            case 5:
                weaponKindIndex = 6;
                break;
            case 6:
                weaponKindIndex = 7;
                break;
        }

        switch (weaponKind)
        {
            case 1:
                var weapon = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon;
                bool condition = true;
                //if (index != 0 && index != 6 && weapon != null)
                //{
                //    if (int.Parse(weapon.kind) != weaponKindIndex)
                //    {
                //        condition = false;                    
                //    }
                //}
                //if (weaponKindIndex == 7 && weapon != null)
                //{
                //    if (!(int.Parse(weapon.kind) == 1 || int.Parse(weapon.kind) == 7))
                //    {
                //        condition = false;
                //    }
                //}

                if (weapon != null && condition)
                {
                    var go = Instantiate(itemPrefab, itemListContents.transform);

                    var child = go.transform.GetChild(7).gameObject;
                    var button = child.AddComponent<Button>();
                    string key = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.id;
                    button.onClick.AddListener(delegate { SelectItem(key); });

                    //go.GetComponent<Image>().sprite = weapon.img;

                    child = go.transform.GetChild(1).gameObject;
                    var childObj = child.transform.GetChild(1).gameObject;
                    string type = GetTypeStr(weapon.kind);
                    childObj.GetComponent<Text>().text = $"{type}";

                    child = go.transform.GetChild(2).gameObject;
                    childObj = child.transform.GetChild(1).gameObject;
                    childObj.GetComponent<Text>().text =
                        (weapon.bulletType == 0) ? "-" : $"{GetBulletType(weapon.bulletType)}";

                    child = go.transform.GetChild(3).gameObject;
                    child.SetActive(true);

                    child = go.transform.GetChild(4).gameObject;
                    child.SetActive(false);

                    child = go.transform.GetChild(5).gameObject;
                    child.GetComponent<Text>().text = $"{weapon.name}";

                    child = go.transform.GetChild(6).gameObject;
                    child.SetActive(false);

                    child = go.transform.GetChild(0).gameObject;
                    child.GetComponent<Image>().sprite = weapon.img;

                    itemObjs.Add(key, go);
                }
                break;

            case 2:
                weapon = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon;

                condition = true;
                //if (index != 0 && index != 6 && weapon != null)
                //{
                //    if (int.Parse(weapon.kind) != weaponKindIndex)
                //    {
                //        condition = false;
                //    }
                //}
                //if (weaponKindIndex == 7 && weapon != null)
                //{
                //    if (!(int.Parse(weapon.kind) == 1 || int.Parse(weapon.kind) == 7))
                //    {
                //        condition = false;
                //    }
                //}

                if (weapon != null && condition)
                {
                    var go = Instantiate(itemPrefab, itemListContents.transform);

                    var child = go.transform.GetChild(7).gameObject;
                    var button = child.AddComponent<Button>();
                    string key = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.id;
                    button.onClick.AddListener(delegate { SelectItem(key); });

                    //go.GetComponent<Image>().sprite = weapon.img;

                    child = go.transform.GetChild(1).gameObject;
                    var childObj = child.transform.GetChild(1).gameObject;
                    string type = GetTypeStr(weapon.kind);
                    childObj.GetComponent<Text>().text = $"{type}";

                    child = go.transform.GetChild(2).gameObject;
                    childObj = child.transform.GetChild(1).gameObject;
                    childObj.GetComponent<Text>().text =
                        (weapon.bulletType == 0) ? "-" : $"{GetBulletType(weapon.bulletType)}";

                    child = go.transform.GetChild(3).gameObject;
                    child.SetActive(true);

                    child = go.transform.GetChild(4).gameObject;
                    child.SetActive(false);

                    child = go.transform.GetChild(5).gameObject;
                    child.GetComponent<Text>().text = $"{weapon.name}";

                    child = go.transform.GetChild(6).gameObject;
                    child.SetActive(false);

                    child = go.transform.GetChild(0).gameObject;
                    child.GetComponent<Image>().sprite = weapon.img;

                    itemObjs.Add(key, go);
                }
                break;
        }

        foreach (var element in playerDataMgr.currentEquippables)
        {
            if (weaponKind != int.Parse(element.Value.type)) continue;
            if (index != 0 && index != 6)
            {
                if (int.Parse(element.Value.kind) != weaponKindIndex) continue;
            }
            if (weaponKindIndex == 7)
            {
                if (!(int.Parse(element.Value.kind) == 1 || int.Parse(element.Value.kind) == 7)) continue;
            }
            
            string type = GetTypeStr(element.Value.kind);

            var go = Instantiate(itemPrefab, itemListContents.transform);

            var child = go.transform.GetChild(7).gameObject;
            var button = child.AddComponent<Button>();
            string key = element.Key;
            button.onClick.AddListener(delegate { SelectItem(key); });
           
            //go.GetComponent<Image>().sprite = element.Value.img;

            child = go.transform.GetChild(1).gameObject;
            var childObj = child.transform.GetChild(1).gameObject;
            childObj.GetComponent<Text>().text = $"{type}";

            child = go.transform.GetChild(2).gameObject;
            childObj = child.transform.GetChild(1).gameObject;
            childObj.GetComponent<Text>().text =
                (element.Value.bulletType == 0) ? "-" : $"{GetBulletType(element.Value.bulletType)}";

            child = go.transform.GetChild(3).gameObject;
            child.SetActive(false);

            child = go.transform.GetChild(4).gameObject;
            child.GetComponent<Text>().text = $"X {playerDataMgr.currentEquippablesNum[element.Key]}";

            child = go.transform.GetChild(5).gameObject;
            child.GetComponent<Text>().text = $"{element.Value.name}";

            child = go.transform.GetChild(6).gameObject;
            child.SetActive(false);

            child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Image>().sprite = element.Value.img;

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
            mainWeaponImg.color = new Color(mainWeaponImg.color.r, mainWeaponImg.color.g, mainWeaponImg.color.b,
               (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null) ? 1 : 0);
            mainWeaponTxt.text =
                (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null) ?
                playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.name : "비어있음";
            
            subWeaponImg.sprite =
                (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null) ?
                playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.img :null;
            subWeaponImg.color = new Color(subWeaponImg.color.r, subWeaponImg.color.g, subWeaponImg.color.b,
              (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null) ? 1 : 0);
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
        if (currentKey != null && itemObjs.ContainsKey(currentKey))
        {
            var child = itemObjs[currentKey].transform.GetChild(7).gameObject;
            child.GetComponent<Button>().enabled = true;
            child.GetComponent<Image>().color = Color.white;

            child = itemObjs[currentKey].transform.GetChild(6).gameObject;
            child.SetActive(false);
        }

        currentKey = key;
        var childObj = itemObjs[currentKey].transform.GetChild(7).gameObject;
        childObj.GetComponent<Image>().color = new Color(255f / 255, 192f / 255, 0f / 255);

        childObj = itemObjs[currentKey].transform.GetChild(6).gameObject;
        childObj.SetActive(true);
        var button = childObj.GetComponent<Button>();

        var weapon = playerDataMgr.equippableList[currentKey];
        bool isEquip = false;
        switch (currentKind)
        {
            case EquipKind.MainWeapon:
                if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null
                    && playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.name.Equals(weapon.name))
                    isEquip = true;
                break;
            case EquipKind.SubWeapon:
                if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null
                    && playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.name.Equals(weapon.name))
                    isEquip = true;
                break;
        }

        if (isEquip)
        {
            childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "장착 해제";

            childObj = itemObjs[currentKey].transform.GetChild(6).gameObject;
            childObj.GetComponent<Button>().enabled = false;
            button.onClick.AddListener(() => { Disarm(); });
            OpenWeaponInfo(isEquip);
        }
        else
        {
            childObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "장착";

            childObj = itemObjs[currentKey].transform.GetChild(6).gameObject;
            childObj.GetComponent<Button>().enabled = false;
            button.onClick.AddListener(() => { Equip(); });
            OpenWeaponInfo(isEquip);
        }
    }

    public void OpenWeaponInfo(bool isEquip)
    {
        var weapon = playerDataMgr.equippableList[currentKey];
        Weapon equipedWeapon = null; 
        switch (currentKind)
        {
            case EquipKind.MainWeapon:
                equipedWeapon = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon;
                break;
            case EquipKind.SubWeapon:
                equipedWeapon = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon;
                break;
        }

        //Detail Info.
        if (equipedWeapon != null && !equipedWeapon.name.Equals(weapon.name))
        {
            damageTxt.text = $"{weapon.minDamage}-{weapon.maxDamage}";

            int criDiff =  weapon.critRate - equipedWeapon.critRate;
            if (criDiff > 0)
            {
                criTxt.color = Color.green;
                criTxt.text = $"{weapon.critRate}%({criDiff}%▲)";
            }
            else if (criDiff < 0)
            {
                criTxt.color = Color.red;
                criTxt.text = $"{weapon.critRate}%({criDiff}%▼)";
            }
            else
            {
                criTxt.color = Color.white;
                criTxt.text = $"{weapon.critRate}%(-)";
            }

            bulletTypeTxt.text = (weapon.bulletType == 0) ? "-" : $"{GetBulletType(weapon.bulletType)}";

            int loadDiff =  weapon.reloadBullet- equipedWeapon.reloadBullet;
            if (loadDiff > 0)
            {
                loadTxt.color = Color.red;
                loadTxt.text = $"{weapon.reloadBullet}({loadDiff}▲)";
            }
            else if (loadDiff < 0)
            {
                loadTxt.color = Color.green;
                loadTxt.text = $"{weapon.reloadBullet}({loadDiff}▼)";
            }
            else
            {
                loadTxt.color = Color.white;
                loadTxt.text = $"{weapon.reloadBullet}(-)";
            }

            int clipDiff = weapon.bullet - equipedWeapon.bullet;
            if (clipDiff > 0)
            {
                clipTxt.color = Color.green;
                clipTxt.text = $"{weapon.bullet}({clipDiff}▲)";
            }
            else if (clipDiff < 0)
            {
                clipTxt.color = Color.red;
                clipTxt.text = $"{weapon.bullet}({clipDiff}▼)";
            }
            else
            {
                clipTxt.color = Color.white;
                clipTxt.text = $"{weapon.bullet}(-)";
            }

            int shotDiff =  weapon.firstShotAp - equipedWeapon.firstShotAp;
            StringBuilder shotGauge = new StringBuilder();
            for (int i = 0; i < weapon.firstShotAp; i++) shotGauge.Append("■");
            for (int i = weapon.firstShotAp; i < 5; i++) shotGauge.Append("□");

            if (shotDiff > 0) shotTxt.color = Color.red;
            else if (shotDiff < 0) shotTxt.color = Color.green;
            else shotTxt.color = Color.white;
            shotTxt.text = $"{shotGauge.ToString()}";

            int accuracyDiff =  weapon.accurRateBase - equipedWeapon.accurRateBase;
            if (accuracyDiff > 0)
            {
                accuracyTxt.color = Color.green;
                accuracyTxt.text = $"{weapon.accurRateBase}%({accuracyDiff}%▲)";
            }
            else if (accuracyDiff < 0)
            {
                accuracyTxt.color = Color.red;
                accuracyTxt.text = $"{weapon.accurRateBase}%({accuracyDiff}%▼)";
            }
            else
            {
                accuracyTxt.color = Color.white;
                accuracyTxt.text = $"{weapon.accurRateBase}%(-)";
            }

            int continuousShotDiff =  weapon.otherShotAp - equipedWeapon.otherShotAp;
            StringBuilder continuousShotGauge = new StringBuilder();
            for (int i = 0; i < weapon.otherShotAp; i++) continuousShotGauge.Append("■");
            for (int i = weapon.otherShotAp; i < 5; i++) continuousShotGauge.Append("□");

            if (continuousShotDiff > 0) continuousShotTxt.color = Color.red;
            else if (continuousShotDiff < 0) continuousShotTxt.color = Color.green;
            else continuousShotTxt.color = Color.white;
            continuousShotTxt.text = $"{continuousShotGauge.ToString()}";

            int recoilDiff =  weapon.continuousShootingPenalty - equipedWeapon.continuousShootingPenalty;
            if (recoilDiff > 0)
            {
                recoilTxt.color = Color.red;
                recoilTxt.text = $"{weapon.continuousShootingPenalty}%({recoilDiff}%▲)";
            }
            else if (recoilDiff < 0)
            {
                recoilTxt.color = Color.green;
                recoilTxt.text = $"{weapon.continuousShootingPenalty}%({recoilDiff}%▼)";
            }
            else
            {
                recoilTxt.color = Color.white;
                recoilTxt.text = $"{weapon.continuousShootingPenalty}%(-)";
            }
        }
        else
        {
            damageTxt.text = $"{weapon.minDamage}-{weapon.maxDamage}";

            criTxt.color = Color.white;
            criTxt.text = $"{weapon.critRate}%";

            bulletTypeTxt.text = (weapon.bulletType == 0) ? "-" : $"{GetBulletType(weapon.bulletType)}";

            loadTxt.color = Color.white;
            loadTxt.text = $"{weapon.reloadBullet}";

            clipTxt.color = Color.white;
            clipTxt.text = $"{weapon.bullet}";

            StringBuilder shotStr = new StringBuilder();
            for (int i = 0; i < weapon.firstShotAp; i++) shotStr.Append("■");
            for (int i = weapon.firstShotAp; i < 5; i++) shotStr.Append("□");
            shotTxt.color = Color.white;
            shotTxt.text = $"{shotStr.ToString()}";

            accuracyTxt.color = Color.white;
            accuracyTxt.text = $"{weapon.accurRateBase}%";

            StringBuilder continuousShotStr = new StringBuilder();
            for (int i = 0; i < weapon.otherShotAp; i++) continuousShotStr.Append("■");
            for (int i = weapon.otherShotAp; i < 5; i++) continuousShotStr.Append("□");
            continuousShotTxt.color = Color.white;
            continuousShotTxt.text = $"{continuousShotStr.ToString()}";

            recoilTxt.color = Color.white;
            recoilTxt.text = $"{weapon.continuousShootingPenalty}%";
        }


        if (isEquip)
        {
            //weaponImg.GetComponent<Image>().sprite = weapon.img;

            var child = weaponImg.transform.GetChild(1).gameObject;
            var childObj = child.transform.GetChild(1).gameObject;
            string type = GetTypeStr(weapon.kind);
            childObj.GetComponent<Text>().text = $"{type}";

            child = weaponImg.transform.GetChild(2).gameObject;
            childObj = child.transform.GetChild(1).gameObject;
            childObj.GetComponent<Text>().text =
                (weapon.bulletType == 0) ? "-" : $"{GetBulletType(weapon.bulletType)}";

            child = weaponImg.transform.GetChild(3).gameObject;
            child.SetActive(true);

            child = weaponImg.transform.GetChild(4).gameObject;
            child.SetActive(false);

            child = weaponImg.transform.GetChild(5).gameObject;
            child.GetComponent<Text>().text = $"{weapon.name}";

            child = weaponImg.transform.GetChild(6).gameObject;
            child.SetActive(false);

            child = weaponImg.transform.GetChild(0).gameObject;
            child.GetComponent<Image>().sprite = weapon.img;
        }
        else
        {
            //weaponImg.GetComponent<Image>().sprite = weapon.img;

            var child = weaponImg.transform.GetChild(1).gameObject;
            var childObj = child.transform.GetChild(1).gameObject;
            string type = GetTypeStr(weapon.kind);
            childObj.GetComponent<Text>().text = $"{type}";

            child = weaponImg.transform.GetChild(2).gameObject;
            childObj = child.transform.GetChild(1).gameObject;
            childObj.GetComponent<Text>().text =
                (weapon.bulletType == 0) ? "-" : $"{GetBulletType(weapon.bulletType)}";

            child = weaponImg.transform.GetChild(3).gameObject;
            child.SetActive(false);

            child = weaponImg.transform.GetChild(4).gameObject;
            child.GetComponent<Text>().text = $"X {playerDataMgr.currentEquippablesNum[currentKey]}";

            child = weaponImg.transform.GetChild(5).gameObject;
            child.GetComponent<Text>().text = $"{weapon.name}";

            child = weaponImg.transform.GetChild(6).gameObject;
            child.SetActive(false);

            child = weaponImg.transform.GetChild(0).gameObject;
            child.GetComponent<Image>().sprite = weapon.img;
        }

        if (!detailWin.activeSelf) detailWin.SetActive(true);
        //if (!SpecCompareWin.activeSelf) SpecCompareWin.SetActive(true);

        //RefreshSpec();
    }

    public void Equip()
    {
        var weaponType = playerDataMgr.equippableList[currentKey].kind;
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
            mainWeaponImg.color = Color.white;
            agitMgr.mainWeaponImg.sprite = weapon.img;
            agitMgr.subWeaponImg.color = new Color(agitMgr.subWeaponImg.color.r, agitMgr.subWeaponImg.color.g, agitMgr.subWeaponImg.color.b, 1);

            //var go = agitMgr.characterObjs[currentIndex];
            //var child = go.transform.GetChild(2).gameObject;
            //child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = mainWeaponImg.sprite;
            //child.transform.GetChild(1).gameObject.GetComponent<Text>().text = weapon.name;

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
            agitMgr.subWeaponImg.color = new Color(agitMgr.subWeaponImg.color.r, agitMgr.subWeaponImg.color.g, agitMgr.subWeaponImg.color.b,1);

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
                mainWeaponImg.color = new Color(mainWeaponImg.color.r, mainWeaponImg.color.g, mainWeaponImg.color.b, 0 );
                mainWeaponTxt.text = "비어있음";

                //var go = agitMgr.characterObjs[currentIndex];
                //var child = go.transform.GetChild(2).gameObject;
                //child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
                //child.transform.GetChild(1).gameObject.GetComponent<Text>().text = "비어있음";
                break;
            case EquipKind.SubWeapon:
                subWeaponImg.color = new Color(subWeaponImg.color.r, subWeaponImg.color.g, subWeaponImg.color.b, 0);
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
            mainWeaponImg.color = new Color(mainWeaponImg.color.r, mainWeaponImg.color.g, mainWeaponImg.color.b, 0);
            agitMgr.mainWeaponImg.color = new Color(agitMgr.mainWeaponImg.color.r, agitMgr.mainWeaponImg.color.g,
                agitMgr.mainWeaponImg.color.b, 0);

            //var go = agitMgr.characterObjs[currentIndex];
            //var child = go.transform.GetChild(2).gameObject;
            //child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
            //child.transform.GetChild(1).gameObject.GetComponent<Text>().text = "비어있음";

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
            WeaponList(weaponType);
        }
        else if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null && currentKind == EquipKind.SubWeapon)
        {
            var id = playerDataMgr.saveData.subWeapon[currentIndex];
            var weapon = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon;
            playerDataMgr.saveData.subWeapon[currentIndex] = null;
            playerDataMgr.currentSquad[currentIndex].weapon.subWeapon = null;
            subWeaponImg.color = new Color(subWeaponImg.color.r, subWeaponImg.color.g, subWeaponImg.color.b, 0);
            agitMgr.subWeaponImg.color = new Color(agitMgr.subWeaponImg.color.r, agitMgr.subWeaponImg.color.g,
              agitMgr.subWeaponImg.color.b, 0);

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
            WeaponList(weaponType);
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
        //RefreshEquipList();
    }

    public void OpenEquipWin2()
    {
        if (agitMgr.skillWinMgr.skillPage.activeSelf)
            agitMgr.skillWinMgr.skillPage.SetActive(false);

        if (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon != null)
        {
            var weapon = playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon;
            mainWeaponImg.sprite = weapon.img;
            mainWeaponImg.color = new Color(mainWeaponImg.color.r, mainWeaponImg.color.g, mainWeaponImg.color.b, 1);
        }
        else mainWeaponImg.color = new Color(mainWeaponImg.color.r, mainWeaponImg.color.g, mainWeaponImg.color.b, 0);

        if (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null)
        {
            var weapon = playerDataMgr.currentSquad[currentIndex].weapon.subWeapon;
            subWeaponImg.sprite = weapon.img;
            subWeaponImg.color = new Color(subWeaponImg.color.r, subWeaponImg.color.g, subWeaponImg.color.b,1 );
        }
        else subWeaponImg.color = new Color(subWeaponImg.color.r, subWeaponImg.color.g, subWeaponImg.color.b, 0);
        
        equipmentWin2.SetActive(true);

        if (detailWin.activeSelf) detailWin.SetActive(false);
        //if (SpecCompareWin.activeSelf) SpecCompareWin.SetActive(false);
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

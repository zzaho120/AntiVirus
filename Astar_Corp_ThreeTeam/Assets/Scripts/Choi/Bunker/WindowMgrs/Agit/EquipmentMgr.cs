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

    //â1 ����.
    public Image mainWeaponImg;
    public Image subWeaponImg;
    public Text mainWeaponTxt;
    public Text subWeaponTxt;
    
    //â2 ����.
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
        //1. �ֹ���
        //2. ��������

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
            child.GetComponent<Image>().sprite = element.Value.img;

            child = go.transform.GetChild(1).gameObject;
            child.transform.GetChild(0).GetComponent<Text>().text = element.Value.name;

            string type = GetTypeStr(element.Value.kind);

            child = go.transform.GetChild(2).gameObject;
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
                type = "��������";
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
                playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.name : "�������";
            
            subWeaponImg.sprite =
                (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null) ?
                playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.img : null;
            subWeaponTxt.text =
                (playerDataMgr.currentSquad[currentIndex].weapon.subWeapon != null) ?
                playerDataMgr.currentSquad[currentIndex].weapon.subWeapon.name : "�������";
        }
    }

    public void SelectEquipKind(int kind)
    {
        switch (kind)
        {
            case 0:
                currentKind = EquipKind.MainWeapon;
                weaponKindTxt.text = "�ֹ���";
                WeaponList(1);
                break;
            case 1:
                currentKind = EquipKind.SubWeapon;
                weaponKindTxt.text = "��������";
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
                //���� ������.
                Destroy(itemObjs[currentKey]);
                itemObjs.Remove(currentKey);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentEquippables.Remove(id);
                playerDataMgr.currentEquippablesNum.Remove(id);

                currentKey = null;
                currentKind = EquipKind.None;
            }
            else
            {
                //�÷��̾� ������ �Ŵ��� ����.
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
                //���� ������.
                Destroy(itemObjs[currentKey]);
                itemObjs.Remove(currentKey);

                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentEquippables.Remove(id);
                playerDataMgr.currentEquippablesNum.Remove(id);

                currentKey = null;
                currentKind = EquipKind.None;
            }
            else
            {
                //�÷��̾� ������ �Ŵ��� ����.
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
                mainWeaponTxt.text = "�������";

                var go = agitMgr.characterObjs[currentIndex];
                var child = go.transform.GetChild(2).gameObject;
                child.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
                child.transform.GetChild(1).gameObject.GetComponent<Text>().text = "�������";
                break;
            case EquipKind.SubWeapon:
                subWeaponImg.sprite = null;
                subWeaponTxt.text = "�������";
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
                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, 1);
            }
            else
            {
                //�÷��̾� ������ �Ŵ��� ����.
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
                //�÷��̾� ������ �Ŵ��� ����.
                playerDataMgr.currentEquippables.Add(id, playerDataMgr.equippableList[id]);
                playerDataMgr.currentEquippablesNum.Add(id, 1);
            }
            else
            {
                //�÷��̾� ������ �Ŵ��� ����.
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

    // ȫ����_���� ����
    // ���߷� ���� �ּ�ó��
    public void RefreshSpec()
    {
        var weapon = playerDataMgr.equippableList[currentKey];
        changeWeaponImg.sprite = weapon.img;
        string selectedWeaStr = $"���߷�{weapon.accurRateBase}%\n" +
            $"�ּ� ������{weapon.minDamage} �ִ� ������{weapon.maxDamage}\n" +
            $"ũ�� Ȯ��{weapon.critRate}% ũ��������{weapon.critDamage}\n"+
            $"źâ��(Ŭ����){weapon.bullet}\n" +
            $"�ּ� ��Ÿ�{weapon.minRange} �ִ� ��Ÿ�{weapon.maxRange}";

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
               
                string str = $"���߷�{currentWeapon.accurRateBase}%\n" +
                   $"�ּ� ������{currentWeapon.minDamage} �ִ� ������{currentWeapon.maxDamage}\n" +
                   $"ũ�� Ȯ��{currentWeapon.critRate}% ũ��������{currentWeapon.critDamage}\n" +
                   $"źâ��(Ŭ����){currentWeapon.bullet}\n" +
                   $"�ּ� ��Ÿ�{currentWeapon.minRange} �ִ� ��Ÿ�{currentWeapon.maxRange}";
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

                str = $"���߷�{accurRateDiff}%\n" +
                   $"�ּ� ������{minDamageDiff} �ִ� ������{maxDamageDiff}\n" +
                   $"ũ�� Ȯ��{criRateDiff}% ũ��������{critDamageDiff}\n" +
                   $"źâ��(Ŭ����){bulletDiff} \n" +
                   $"�ּ� ��Ÿ�{minRangeDiff} �ִ� ��Ÿ�{maxRangeDiff}";
                changeWeaSpecTxt.text = str;
            }
            else
            {
                currentWeaponImg.sprite = null;
                string str = $"���߷� : -% \n" +
                  $"�ּ� ������- �ִ� ������-\n" +
                   $"ũ�� Ȯ��-% ũ��������-\n" +
                   $"źâ��(Ŭ����)-\n" +
                   $"�ּ� ��Ÿ�- �ִ� ��Ÿ�-";
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
                
                string str = $"���߷� : {currentWeapon.accurRateBase}%\n" +
                      $"�ּ� ������{currentWeapon.minDamage} �ִ� ������{currentWeapon.maxDamage}\n" +
                   $"ũ�� Ȯ��{currentWeapon.critRate}% ũ��������{currentWeapon.critDamage}\n" +
                   $"źâ��(Ŭ����){currentWeapon.bullet}\n" +
                   $"�ּ� ��Ÿ�{currentWeapon.minRange} �ִ� ��Ÿ�{currentWeapon.maxRange}";
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

                str = $"���߷�{accurRateDiff}%\n" +
                   $"�ּ� ������{minDamageDiff} �ִ� ������{maxDamageDiff}\n" +
                   $"ũ�� Ȯ��{criRateDiff}% ũ��������{critDamageDiff}\n" +
                   $"źâ��(Ŭ����){bulletDiff} \n" +
                   $"�ּ� ��Ÿ�{minRangeDiff} �ִ� ��Ÿ�{maxRangeDiff}";
                changeWeaSpecTxt.text = str;
            }
            else
            {
                currentWeaponImg.sprite = null;
                string str = $"���߷� : -% \n" +
                  $"�ּ� ������- �ִ� ������-\n" +
                   $"ũ�� Ȯ��-% ũ��������-\n" +
                   $"źâ��(Ŭ����)-\n" +
                   $"�ּ� ��Ÿ�- �ִ� ��Ÿ�-";
                currentWeaSpecTxt.text = str;
                changeWeaSpecTxt.text = selectedWeaStr;
            }
        }
    }

    //â ����.
    public void OpenEquipWin1()
    {
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

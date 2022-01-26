using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //테스터
using System.Linq;

public class PlayerDataMgr : Singleton<PlayerDataMgr>
{
    //플레이어 데이터.
    public PlayerSaveData saveData = new PlayerSaveData();
    string filePath;

    //SO데이터.
    public Dictionary<string, Character> characterList = new Dictionary<string, Character>();
    public Dictionary<string, Weapon> equippableList = new Dictionary<string, Weapon>();
    public Dictionary<string, Consumable> consumableList = new Dictionary<string, Consumable>();
    public Dictionary<string, OtherItem> otherItemList = new Dictionary<string, OtherItem>();
    public Dictionary<string, Monster> monsterList = new Dictionary<string, Monster>();
    public Dictionary<string, Virus> virusList = new Dictionary<string, Virus>();
    public Dictionary<string, ActiveSkill> activeSkillList = new Dictionary<string, ActiveSkill>();
    public Dictionary<string, PassiveSkill> passiveSkillList = new Dictionary<string, PassiveSkill>();
    public Dictionary<string, Truck> truckList = new Dictionary<string, Truck>();
    public Dictionary<string, Inventory> bagList = new Dictionary<string, Inventory>();
    public Dictionary<string, Bunker> bunkerList = new Dictionary<string, Bunker>();

    ScriptableMgr scriptableMgr;

    //아지트 아이템 데이터.
    public Dictionary<string, Weapon> currentEquippables = new Dictionary<string, Weapon>();
    public Dictionary<string, int> currentEquippablesNum = new Dictionary<string, int>();
    public Dictionary<string, Consumable> currentConsumables = new Dictionary<string, Consumable>();
    public Dictionary<string, int> currentConsumablesNum = new Dictionary<string, int>();
    public Dictionary<string, OtherItem> currentOtherItems = new Dictionary<string, OtherItem>();
    public Dictionary<string, int> currentOtherItemsNum = new Dictionary<string, int>();

    //트럭 데이터.
    public Dictionary<string, Weapon> truckEquippables = new Dictionary<string, Weapon>();
    public Dictionary<string, int> truckEquippablesNum = new Dictionary<string, int>();
    public Dictionary<string, Consumable> truckConsumables = new Dictionary<string, Consumable>();
    public Dictionary<string, int> truckConsumablesNum = new Dictionary<string, int>();
    public Dictionary<string, OtherItem> truckOtherItems = new Dictionary<string, OtherItem>();
    public Dictionary<string, int> truckOtherItemsNum = new Dictionary<string, int>();

    //캐릭터 데이터.
    public Dictionary<int, CharacterStats> currentSquad = new Dictionary<int, CharacterStats>();
    public Dictionary<int, int> boardingSquad = new Dictionary<int, int>();//트럭에 있는 캐릭터들.(좌석 번호/ 인덱스)
    public Dictionary<int, CharacterStats> battleSquad = new Dictionary<int, CharacterStats>();//전투에 나갈 캐릭터들.

    public bool ableToExit;//벙커 밖으로 나갈 수 있는지.
    public bool isFirst;
    public bool isMonsterAtk;
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        scriptableMgr = ScriptableMgr.Instance;

        characterList = scriptableMgr.characterList;
        equippableList = scriptableMgr.equippableList;
        consumableList = scriptableMgr.consumableList;
        otherItemList = scriptableMgr.otherItemList;
        monsterList = scriptableMgr.monsterList;
        virusList = scriptableMgr.virusList;
        activeSkillList = scriptableMgr.activeSkillList;
        passiveSkillList = scriptableMgr.passiveSkillList;
        truckList = scriptableMgr.truckList;
        bagList = scriptableMgr.bagList;
        bunkerList = scriptableMgr.bunkerList;

        filePath = @$"{Application.persistentDataPath}\PlayerData.json";
        if (saveData.id == null)
        {
            //saveData.bunkerKind = new List<int>();
            saveData.cars = new List<string>();
            saveData.speedLv = new List<int>();
            saveData.sightLv = new List<int>();
            saveData.weightLv = new List<int>();
            saveData.boarding = new List<int>();
            saveData.currentCar = null;

            saveData.storeItem = new List<string>();
            saveData.storeItemNum = new List<int>();

            saveData.id = new List<string>();
            saveData.name = new List<string>();
            saveData.hp = new List<int>();
            saveData.maxHp = new List<int>();
            saveData.sensitivity = new List<int>();
            saveData.concentration = new List<int>();
            saveData.willPower = new List<int>();
            saveData.bagLevel = new List<int>();

            saveData.mainWeapon = new List<string>();
            saveData.subWeapon = new List<string>();
            saveData.activeSkillList = new List<string>();
            saveData.passiveSkillList = new List<string>();

            saveData.bagEquippableList = new List<string>();
            saveData.bagEquippableNumList = new List<int>();
            saveData.bagEquippableFirstIndex = new List<int>();
            saveData.bagEquippableLastIndex = new List<int>();

            saveData.bagConsumableList = new List<string>();
            saveData.bagConsumableNumList = new List<int>();
            saveData.bagConsumableFirstIndex = new List<int>();
            saveData.bagConsumableLastIndex = new List<int>();

            saveData.bagOtherItemList = new List<string>();
            saveData.bagOtherItemNumList = new List<int>();
            saveData.bagOtherItemFirstIndex = new List<int>();
            saveData.bagOtherItemLastIndex = new List<int>();

            saveData.equippableList = new List<string>();
            saveData.equippableNumList = new List<int>();
            saveData.consumableList = new List<string>();
            saveData.consumableNumList = new List<int>();
            saveData.otherItemList = new List<string>();
            saveData.otherItemNumList = new List<int>();

            saveData.gaugeE = new List<int>();
            saveData.gaugeB = new List<int>();
            saveData.gaugeP = new List<int>();
            saveData.gaugeI = new List<int>();
            saveData.gaugeT = new List<int>();

            saveData.levelE = new List<int>();
            saveData.levelB = new List<int>();
            saveData.levelP = new List<int>();
            saveData.levelI = new List<int>();
            saveData.levelT = new List<int>();

            saveData.truckEquippableList = new List<string>();
            saveData.truckEquippableNumList = new List<int>();
            saveData.truckConsumableList = new List<string>();
            saveData.truckConsumableNumList = new List<int>();
            saveData.truckOtherItemList = new List<string>();
            saveData.truckOtherItemNumList = new List<int>();
        }

        var obj = FindObjectsOfType<PlayerDataMgr>();
        if (obj.Length == 1)
        {
            //처음하기.
            if (!PlayerPrefs.HasKey("Continue"))
            {
                isFirst = true;
                
                string str = "Continue";
                PlayerPrefs.SetInt(str, 1);

                saveData.storeReset = true;

                saveData.money = 1000000;
                saveData.bunkerExitNum = 0;
                //for (int i = 0; i < 20; i++)
                //{
                //    switch (i)
                //    {
                //        case 2:
                //            saveData.bunkerKind.Add(5);
                //            break;
                //        case 7:
                //            saveData.bunkerKind.Add(4);
                //            break;
                //        case 11:
                //            saveData.bunkerKind.Add(3);
                //            break;
                //        case 12:
                //            saveData.bunkerKind.Add(2);
                //            break;
                //        case 13:
                //            saveData.bunkerKind.Add(1);
                //            break;
                //        case 14:
                //            saveData.bunkerKind.Add(6);
                //            break;
                //        default:
                //            saveData.bunkerKind.Add(7);
                //            break;
                //    }
                //}

                saveData.agitLevel = 1;
                saveData.storageLevel = 1;
                saveData.garageLevel = 1;
                saveData.hospitalLevel = 1;
                saveData.storeLevel = 1;
                saveData.pubLevel = 1;

                saveData.cars.Add("TRU_0004");
                saveData.currentCar = "TRU_0004";
                saveData.speedLv.Add(1);
                saveData.sightLv.Add(1);
                saveData.weightLv.Add(1);

                //테스트용.
                //////////////////////////////
                //foreach (var element in equippableList)
                //{
                //    saveData.equippableList.Add(element.Key);
                //    int random = Random.Range(1, 3);
                //    saveData.equippableNumList.Add(random);
                //    currentEquippables.Add(element.Key, element.Value);
                //    currentEquippablesNum.Add(element.Key, random);
                //}

                //foreach (var element in consumableList)
                //{
                //    saveData.consumableList.Add(element.Key);
                //    int random = Random.Range(1, 3);
                //    saveData.consumableNumList.Add(random);
                //    currentConsumables.Add(element.Key, element.Value);
                //    currentConsumablesNum.Add(element.Key, random);
                //}

                //foreach (var element in otherItemList)
                //{
                //    saveData.otherItemList.Add(element.Key);
                //    int random = Random.Range(1, 3);
                //    saveData.otherItemNumList.Add(random);
                //    currentOtherItems.Add(element.Key, element.Value);
                //    currentOtherItemsNum.Add(element.Key, random);
                //}

                //foreach (var element in equippableList)
                //{
                //    saveData.truckEquippableList.Add(element.Key);
                //    int random = Random.Range(1, 3);
                //    saveData.truckEquippableNumList.Add(random);
                //    truckEquippables.Add(element.Key, element.Value);
                //    truckEquippablesNum.Add(element.Key, random);
                //}

                //foreach (var element in consumableList)
                //{
                //    saveData.truckConsumableList.Add(element.Key);
                //    int random = Random.Range(1, 3);
                //    saveData.truckConsumableNumList.Add(random);
                //    truckConsumables.Add(element.Key, element.Value);
                //    truckConsumablesNum.Add(element.Key, random);
                //}

                //foreach (var element in otherItemList)
                //{
                //    saveData.truckOtherItemList.Add(element.Key);
                //    int random = Random.Range(1, 3);
                //    saveData.truckOtherItemNumList.Add(random);
                //    truckOtherItems.Add(element.Key, element.Value);
                //    truckOtherItemsNum.Add(element.Key, random);
                //}

                //PlayerSaveLoadSystem.Save(saveData);
                //////////////////////////////
            }
            //이어하기.
            else
            {
                saveData = PlayerSaveLoadSystem.Load(filePath);

                //아이템 설정.
                int k = 0;
                foreach (var element in saveData.equippableList)
                {
                    currentEquippables.Add(element, equippableList[element]);
                    int num = k;
                    currentEquippablesNum.Add(element, saveData.equippableNumList[num]);
                    k++;
                }
                k = 0;
                foreach (var element in saveData.consumableList)
                {
                    currentConsumables.Add(element, consumableList[element]);
                    int num = k;
                    currentConsumablesNum.Add(element, saveData.consumableNumList[num]);
                    k++;
                }
                k = 0;
                foreach (var element in saveData.otherItemList)
                {
                    currentOtherItems.Add(element, otherItemList[element]);
                    int num = k;
                    currentOtherItemsNum.Add(element, saveData.otherItemNumList[num]);
                    k++;
                }
                //트럭.
                k = 0;
                foreach (var element in saveData.truckEquippableList)
                {
                    truckEquippables.Add(element, equippableList[element]);
                    int num = k;
                    truckEquippablesNum.Add(element, saveData.truckEquippableNumList[num]);
                    k++;
                }
                k = 0;
                foreach (var element in saveData.truckConsumableList)
                {
                    truckConsumables.Add(element, consumableList[element]);
                    int num = k;
                    truckConsumablesNum.Add(element, saveData.truckConsumableNumList[num]);
                    k++;
                }
                k = 0;
                foreach (var element in saveData.truckOtherItemList)
                {
                    truckOtherItems.Add(element, otherItemList[element]);
                    int num = k;
                    truckOtherItemsNum.Add(element, saveData.truckOtherItemNumList[num]);
                    k++;
                }

                //캐릭터 설정.
                for (int i = 0; i < saveData.name.Count; i++)
                {
                    //게임상 관리하기 쉽도록.
                    CharacterStats stat = new CharacterStats();
                    stat.saveId = i;
                    stat.currentHp = saveData.hp[i];
                    stat.MaxHp = saveData.maxHp[i];
                    stat.sensivity = saveData.sensitivity[i];
                    stat.concentration = saveData.concentration[i];
                    stat.willpower = saveData.willPower[i];
                    stat.character = characterList[saveData.id[i]];
                    stat.character.id = saveData.id[i];
                    stat.bagLevel = saveData.bagLevel[i];
                    stat.VirusPanaltyInit();

                    stat.weapon = new WeaponStats();
                    stat.weapon.mainWeapon = (saveData.mainWeapon[i] == null) ? null : equippableList[saveData.mainWeapon[i]];
                    stat.weapon.subWeapon = (saveData.subWeapon[i] == null) ? null : equippableList[saveData.subWeapon[i]];

                    stat.virusPenalty["E"].penaltyLevel = saveData.levelE[i];
                    stat.virusPenalty["B"].penaltyLevel = saveData.levelB[i];
                    stat.virusPenalty["P"].penaltyLevel = saveData.levelP[i];
                    stat.virusPenalty["I"].penaltyLevel = saveData.levelI[i];
                    stat.virusPenalty["T"].penaltyLevel = saveData.levelT[i];

                    stat.virusPenalty["E"].penaltyGauge = saveData.gaugeE[i];
                    stat.virusPenalty["B"].penaltyGauge = saveData.gaugeB[i];
                    stat.virusPenalty["P"].penaltyGauge = saveData.gaugeP[i];
                    stat.virusPenalty["I"].penaltyGauge = saveData.gaugeI[i];
                    stat.virusPenalty["T"].penaltyGauge = saveData.gaugeT[i];

                    //List<string> activeSkill = new List<string>();
                    //int activeSkillNum = activeSkillList.Count;
                    //for (int j = 0; j < activeSkillNum; j++) { activeSkill.Add(saveData.activeSkillList[i * activeSkillNum + j]); }
                    //foreach (var element in activeSkill)
                    //{
                    //    if (element == null) continue;
                    //    stat.skills.activeSkills.Add(activeSkillList[element]);
                    //}

                    //List<string> passiveSkill = new List<string>();
                    //int passiveSkillNum = passiveSkillList.Count;
                    //for (int j = 0; j < passiveSkillNum; j++) { passiveSkill.Add(saveData.passiveSkillList[i * passiveSkillNum + j]); }
                    //foreach (var element in passiveSkill)
                    //{
                    //    if (element == null) continue;
                    //    stat.skills.passiveSkills.Add(passiveSkillList[element]);
                    //}

                    for (int j = saveData.bagEquippableFirstIndex[i]; j < saveData.bagEquippableLastIndex[i]; j++)
                    {
                        var key = saveData.bagEquippableList[j];
                        var weapon = equippableList[key];
                        
                        stat.bag.Add(saveData.bagEquippableList[j], saveData.bagEquippableNumList[j]);

                    }

                    for (int j = saveData.bagConsumableFirstIndex[i]; j < saveData.bagConsumableLastIndex[i]; j++)
                    {
                        stat.bag.Add(saveData.bagConsumableList[j], saveData.bagConsumableNumList[j]);
                    }

                    for (int j = saveData.bagOtherItemFirstIndex[i]; j < saveData.bagOtherItemLastIndex[i]; j++)
                    {
                        stat.bag.Add(saveData.bagOtherItemList[j], saveData.bagOtherItemNumList[j]);
                    }

                    currentSquad.Add(i, stat);

                    if (saveData.boarding[i] != -1) boardingSquad.Add(saveData.boarding[i], i);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RefreshCurrentSquad()
    {
        currentSquad.Clear();
        //캐릭터 설정.
        for (int i = 0; i < saveData.name.Count; i++)
        {
            //게임상 관리하기 쉽도록.
            CharacterStats stat = new CharacterStats();
            stat.saveId = i;
            stat.VirusPanaltyInit();
            stat.currentHp = saveData.hp[i];
            stat.MaxHp = saveData.maxHp[i];
            stat.sensivity = saveData.sensitivity[i];
            stat.concentration = saveData.concentration[i];
            stat.willpower = saveData.willPower[i];
            stat.character = characterList[saveData.id[i]];
            stat.character.id = saveData.id[i];
            stat.bagLevel = saveData.bagLevel[i];

            stat.weapon = new WeaponStats();
            stat.weapon.mainWeapon = (saveData.mainWeapon[i] == null) ? null : equippableList[saveData.mainWeapon[i]];
            stat.weapon.subWeapon = (saveData.subWeapon[i] == null) ? null : equippableList[saveData.subWeapon[i]];

            //List<string> activeSkill = new List<string>();
            //int activeSkillNum = activeSkillList.Count;
            //for (int j = 0; j < activeSkillNum; j++) { activeSkill.Add(saveData.activeSkillList[i * activeSkillNum + j]); }
            //foreach (var element in activeSkill)
            //{
            //    if (element == null) continue;
            //    stat.skillMgr.activeSkills.Add(activeSkillList[element]);
            //}

            //List<string> passiveSkill = new List<string>();
            //int passiveSkillNum = passiveSkillList.Count;
            //for (int j = 0; j < passiveSkillNum; j++) { passiveSkill.Add(saveData.passiveSkillList[i * passiveSkillNum + j]); }
            //foreach (var element in passiveSkill)
            //{
            //    if (element == null) continue;
            //    stat.skillMgr.passiveSkills.Add(passiveSkillList[element]);
            //}

            for (int j = saveData.bagEquippableFirstIndex[i]; j < saveData.bagEquippableLastIndex[i]; j++)
            {
                stat.bag.Add(saveData.bagEquippableList[j], saveData.bagEquippableNumList[j]);
            }

            for (int j = saveData.bagConsumableFirstIndex[i]; j < saveData.bagConsumableLastIndex[i]; j++)
            {
                stat.bag.Add(saveData.bagConsumableList[j], saveData.bagConsumableNumList[j]);
            }

            for (int j = saveData.bagOtherItemFirstIndex[i]; j < saveData.bagOtherItemLastIndex[i]; j++)
            {
                stat.bag.Add(saveData.bagOtherItemList[j], saveData.bagOtherItemNumList[j]);
            }

            currentSquad.Add(i, stat);
        }
    }

    public void AddCharacter(int num, CharacterStats stat)
    {
        string str = "SquadNum";
        int totalSquadNum = (PlayerPrefs.HasKey(str)) ? PlayerPrefs.GetInt(str) : 0;

        //인원 추가.
        if (num > totalSquadNum - 1)
        {
            saveData.boarding.Add(-1);
            saveData.id.Add(stat.character.id);
            saveData.name.Add(stat.character.name);
            saveData.hp.Add(stat.currentHp);
            saveData.maxHp.Add(stat.MaxHp);
            saveData.sensitivity.Add(stat.sensivity);
            saveData.concentration.Add(stat.concentration);
            saveData.willPower.Add(stat.willpower);
            saveData.bagLevel.Add(1);

            saveData.gaugeE.Add(stat.virusPenalty["E"].penaltyGauge);
            saveData.gaugeB.Add(stat.virusPenalty["B"].penaltyGauge);
            saveData.gaugeP.Add(stat.virusPenalty["P"].penaltyGauge);
            saveData.gaugeI.Add(stat.virusPenalty["I"].penaltyGauge);
            saveData.gaugeT.Add(stat.virusPenalty["T"].penaltyGauge);

            saveData.levelE.Add(stat.virusPenalty["E"].penaltyLevel);
            saveData.levelB.Add(stat.virusPenalty["B"].penaltyLevel);
            saveData.levelP.Add(stat.virusPenalty["P"].penaltyLevel);
            saveData.levelI.Add(stat.virusPenalty["I"].penaltyLevel);
            saveData.levelT.Add(stat.virusPenalty["T"].penaltyLevel);

            string mainWeaponStr = (stat.weapon.mainWeapon != null) ? stat.weapon.mainWeapon.id : null;
            saveData.mainWeapon.Add(mainWeaponStr);
            string subWeaponStr = (stat.weapon.subWeapon != null) ? stat.weapon.subWeapon.id : null;
            saveData.subWeapon.Add(subWeaponStr);
            //for (int k = 0; k < 5; k++) 
            //{
            //    string activeSkillStr = (stat.skillMgr.activeSkills[k] != null) ? stat.skillMgr.activeSkills[k].id : null;
            //    saveData.activeSkillList.Add(activeSkillStr); 
            //}
            //for (int k = 0; k < 5; k++) 
            //{
            //    string passiveSkillStr = (stat.skillMgr.passiveSkills[k] != null) ? stat.skillMgr.passiveSkills[k].id : null;
            //    saveData.passiveSkillList.Add(passiveSkillStr); 
            //}

            if (num == 0)
            {
                saveData.bagEquippableFirstIndex.Add(0);
                saveData.bagEquippableLastIndex.Add(0);
                saveData.bagConsumableFirstIndex.Add(0);
                saveData.bagConsumableLastIndex.Add(0);
                saveData.bagOtherItemFirstIndex.Add(0);
                saveData.bagOtherItemLastIndex.Add(0);
            }
            else
            {
                int preFirstIndex = saveData.bagEquippableFirstIndex[num - 1];
                saveData.bagEquippableFirstIndex.Add(preFirstIndex);
                saveData.bagEquippableLastIndex.Add(preFirstIndex);
                preFirstIndex = saveData.bagConsumableFirstIndex[num - 1];
                saveData.bagConsumableFirstIndex.Add(preFirstIndex);
                saveData.bagConsumableLastIndex.Add(preFirstIndex);
                preFirstIndex = saveData.bagOtherItemFirstIndex[num - 1];
                saveData.bagOtherItemFirstIndex.Add(preFirstIndex);
                saveData.bagOtherItemLastIndex.Add(preFirstIndex);
            }

            stat.saveId = num;
            currentSquad.Add(num, stat);
        }
        PlayerSaveLoadSystem.Save(saveData);
    }

    public void RemoveCharacter(int i)
    {
        saveData.boarding.RemoveAt(i);
        saveData.id.RemoveAt(i);
        saveData.name.RemoveAt(i);
        saveData.hp.RemoveAt(i);
        saveData.maxHp.RemoveAt(i);
        saveData.sensitivity.RemoveAt(i);
        saveData.concentration.RemoveAt(i);
        saveData.willPower.RemoveAt(i);
        saveData.bagLevel.RemoveAt(i);

        saveData.gaugeE.RemoveAt(i);
        saveData.gaugeB.RemoveAt(i);
        saveData.gaugeP.RemoveAt(i);
        saveData.gaugeI.RemoveAt(i);
        saveData.gaugeT.RemoveAt(i);

        saveData.levelE.RemoveAt(i);
        saveData.levelB.RemoveAt(i);
        saveData.levelP.RemoveAt(i);
        saveData.levelI.RemoveAt(i);
        saveData.levelT.RemoveAt(i);

        saveData.mainWeapon.RemoveAt(i);
        saveData.subWeapon.RemoveAt(i);

        //int activeSkillNum = activeSkillList.Count;
        //for (int j = activeSkillNum - 1; j > -1; j--)
        //{
        //    saveData.activeSkillList.RemoveAt(i * activeSkillNum + j);
        //}

        //int passiveSkillNum = passiveSkillList.Count;
        //for (int j = passiveSkillNum - 1; j > -1; j--)
        //{
        //    saveData.passiveSkillList.RemoveAt(i * passiveSkillNum + j);
        //}

        int firstIndex = saveData.bagEquippableFirstIndex[i];
        int lastIndex = saveData.bagEquippableLastIndex[i];
        int difference = lastIndex - firstIndex;
        for (int j = i; j < saveData.bagEquippableFirstIndex.Count; j++)
        {
            saveData.bagEquippableFirstIndex[j] -= difference;
            saveData.bagEquippableLastIndex[j] -= difference;
        }

        for (int j = firstIndex; j < lastIndex; j++)
        {
            saveData.bagEquippableList.RemoveAt(i);
            saveData.bagEquippableNumList.RemoveAt(i);
        }
        saveData.bagEquippableFirstIndex.RemoveAt(i);
        saveData.bagEquippableLastIndex.RemoveAt(i);

        firstIndex = saveData.bagConsumableFirstIndex[i];
        lastIndex = saveData.bagConsumableLastIndex[i];
        difference = lastIndex - firstIndex;
        for (int j = i; j < saveData.bagConsumableFirstIndex.Count; j++)
        {
            saveData.bagConsumableFirstIndex[j] -= difference;
            saveData.bagConsumableLastIndex[j] -= difference;
        }

        for (int j = firstIndex; j < lastIndex; j++)
        {
            saveData.bagConsumableList.RemoveAt(i);
            saveData.bagConsumableNumList.RemoveAt(i);
        }
        saveData.bagConsumableFirstIndex.RemoveAt(i);
        saveData.bagConsumableLastIndex.RemoveAt(i);

        firstIndex = saveData.bagOtherItemFirstIndex[i];
        lastIndex = saveData.bagOtherItemLastIndex[i];
        difference = lastIndex - firstIndex;
        for (int j = i; j < saveData.bagOtherItemFirstIndex.Count; j++)
        {
            saveData.bagOtherItemFirstIndex[j] -= difference;
            saveData.bagOtherItemLastIndex[j] -= difference;
        }

        for (int j = firstIndex; j < lastIndex; j++)
        {
            saveData.bagOtherItemList.RemoveAt(i);
            saveData.bagOtherItemNumList.RemoveAt(i);
        }
        saveData.bagOtherItemFirstIndex.RemoveAt(i);
        saveData.bagOtherItemLastIndex.RemoveAt(i);

        currentSquad.Remove(i);

        PlayerSaveLoadSystem.Save(saveData);
    }
}

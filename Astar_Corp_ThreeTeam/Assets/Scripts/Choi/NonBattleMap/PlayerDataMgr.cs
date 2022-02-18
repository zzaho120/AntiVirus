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
    public Dictionary<string, Name> nameList = new Dictionary<string, Name>();

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
    public bool isBattleTutorial;

    public WorldMonsterChar worldMonster;
    public int virusLevel;
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
        nameList = scriptableMgr.nameList;

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
            saveData.characterName = new List<string>();
            saveData.hp = new List<int>();
            saveData.maxHp = new List<int>();
            saveData.sensitivity = new List<int>();
            saveData.avoidRate = new List<int>();
            saveData.concentration = new List<int>();
            saveData.willPower = new List<int>();
            saveData.level = new List<int>();
            saveData.currentExp = new List<int>();
            saveData.sightDistance = new List<int>();
            saveData.weight = new List<int>();
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

            saveData.soldierName = new List<string>();
            saveData.soldierCharacterName = new List<string>();
            saveData.soldierHp = new List<int>();
            saveData.soldierSensitivity = new List<int>();
            saveData.soldierAvoidRate = new List<int>();
            saveData.soldierConcentration = new List<int>();
            saveData.soldierWillPower = new List<int>();
            saveData.soldierSightDistance = new List<int>();
            saveData.soldierMainWeapon = new List<string>();
            saveData.soldierCost = new List<int>();

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

                saveData.agitAlarm = false;
                saveData.storeReset = true;
                saveData.pubReset = true;

                saveData.money = 100000;
                saveData.bunkerExitNum = 0;
               
                saveData.agitLevel = 1;
                saveData.storageLevel = 1;
                saveData.carCenterLevel = 1;
                saveData.hospitalLevel = 1;
                saveData.storeLevel = 1;
                saveData.pubLevel = 1;

                //AddCharacter(3);

                saveData.cars.Add("TRU_0004");
                saveData.currentCar = "TRU_0004";
                saveData.speedLv.Add(1);
                saveData.sightLv.Add(1);
                saveData.weightLv.Add(1);

                saveData.consumableList.Add("CON_0017");
                saveData.consumableNumList.Add(1);
                saveData.consumableList.Add("CON_0011");
                saveData.consumableNumList.Add(1);
                saveData.consumableList.Add("CON_0003");
                saveData.consumableNumList.Add(1);
                int k = 0;
                foreach (var element in saveData.consumableList)
                {
                    currentConsumables.Add(element, consumableList[element]);
                    int num = k;
                    currentConsumablesNum.Add(element, saveData.consumableNumList[num]);
                    k++;
                }

                saveData.otherItemList.Add("BUL_0001");
                saveData.otherItemNumList.Add(50);
                saveData.otherItemList.Add("BUL_0002");
                saveData.otherItemNumList.Add(50);
                saveData.otherItemList.Add("BUL_0003");
                saveData.otherItemNumList.Add(50);
                saveData.otherItemList.Add("BUL_0004");
                saveData.otherItemNumList.Add(50);
                saveData.otherItemList.Add("BUL_0005");
                saveData.otherItemNumList.Add(50);

                saveData.otherItemList.Add("DROP_0001");
                saveData.otherItemNumList.Add(1);
                saveData.otherItemList.Add("DROP_0002");
                saveData.otherItemNumList.Add(1);
                saveData.otherItemList.Add("DROP_0003");
                saveData.otherItemNumList.Add(1);
                k = 0;
                foreach (var element in saveData.otherItemList)
                {
                    currentOtherItems.Add(element, otherItemList[element]);
                    int num = k;
                    currentOtherItemsNum.Add(element, saveData.otherItemNumList[num]);
                    k++;
                }
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
                    stat.characterName = saveData.characterName[i];
                    stat.currentHp = saveData.hp[i];
                    stat.MaxHp = saveData.maxHp[i];
                    stat.sensivity = saveData.sensitivity[i];
                    stat.avoidRate = saveData.avoidRate[i];
                    stat.concentration = saveData.concentration[i];
                    stat.willpower = saveData.willPower[i];
                    stat.level = saveData.level[i];
                    stat.currentExp = saveData.currentExp[i];
                    stat.sightDistance = saveData.sightDistance[i];
                    stat.Weight = saveData.weight[i];
                    stat.character = characterList[saveData.id[i]];
                    stat.Setting();
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
            stat.characterName = saveData.characterName[i];
            stat.VirusPenaltyInit();
            stat.currentHp = saveData.hp[i];
            stat.MaxHp = saveData.maxHp[i];
            stat.sensivity = saveData.sensitivity[i];
            stat.avoidRate = saveData.avoidRate[i];
            stat.concentration = saveData.concentration[i];
            stat.willpower = saveData.willPower[i];
            stat.level = saveData.level[i];
            stat.currentExp = saveData.currentExp[i];
            stat.sightDistance = saveData.sightDistance[i];
            stat.Weight = saveData.weight[i];
            stat.character = characterList[saveData.id[i]];
            stat.Setting();
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

            if (saveData.boarding[i] != -1) boardingSquad.Add(saveData.boarding[i], i);
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
            saveData.characterName.Add(stat.characterName);
            saveData.hp.Add(stat.currentHp);
            saveData.maxHp.Add(stat.MaxHp);
            saveData.sensitivity.Add(stat.sensivity);
            saveData.avoidRate.Add(stat.avoidRate);
            saveData.concentration.Add(stat.concentration);
            saveData.willPower.Add(stat.willpower);
            saveData.level.Add(stat.level);
            saveData.currentExp.Add(stat.currentExp);
            saveData.sightDistance.Add(stat.sightDistance);
            saveData.weight.Add(stat.Weight);

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

            saveData.bagLevel.Add(1);

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
            stat.Setting();
            currentSquad.Add(num, stat);
        }
        PlayerSaveLoadSystem.Save(saveData);
    }

    public void AddCharacter(int number)
    {
        string str = "SquadNum";
        PlayerPrefs.SetInt(str, number-1);

        //string str = "SquadNum";
        //int totalSquadNum = (PlayerPrefs.HasKey(str)) ? PlayerPrefs.GetInt(str) : 0;

        List<string> randomCharacter = new List<string>();
        List<string> randomName = new List<string>();
        //랜덤 용병 생성.
        foreach (var element in characterList)
        {
            randomCharacter.Add(element.Key);
        }
        //캐릭터 이름.
        foreach (var element in nameList)
        {
            randomName.Add(element.Key);
        }

        for (int j = 0; j < number; j++)
        {
            int randomIndex = Random.Range(0, characterList.Count);
            var key = randomCharacter[randomIndex];

            CharacterStats stat = new CharacterStats();
            var character = characterList[key];
            stat.character = character;
            stat.Init();

            randomIndex = Random.Range(0, nameList.Count);
            var nameKey = randomName[randomIndex];
            stat.characterName = nameList[nameKey].name;

            stat.bagLevel = 1;

            //랜덤 주무기 장착.
            List<string> mainWeapons = new List<string>();
            foreach (var mainWeapon in equippableList)
            {
                if (mainWeapon.Value.kind.Equals("1") || mainWeapon.Value.kind.Equals("7")
                    || mainWeapon.Value.kind.Equals("8")) continue;
                if (!stat.character.weapons.Contains(mainWeapon.Value.kind)) continue;
                mainWeapons.Add(mainWeapon.Key);
            }
            randomIndex = Random.Range(0, mainWeapons.Count);
            var mainWeaponKey = mainWeapons[randomIndex];

            stat.weapon = new WeaponStats();
            stat.weapon.mainWeapon = equippableList[mainWeaponKey];

            saveData.boarding.Add(-1);
            saveData.id.Add(stat.character.id);
            saveData.name.Add(stat.character.name);
            saveData.characterName.Add(stat.characterName);
            saveData.hp.Add(stat.currentHp);
            saveData.maxHp.Add(stat.MaxHp);
            saveData.sensitivity.Add(stat.sensivity);
            saveData.avoidRate.Add(stat.avoidRate);
            saveData.concentration.Add(stat.concentration);
            saveData.willPower.Add(stat.willpower);
            saveData.level.Add(stat.level);
            saveData.currentExp.Add(stat.currentExp);
            saveData.sightDistance.Add(stat.sightDistance);
            saveData.weight.Add(stat.Weight);

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

            saveData.bagLevel.Add(1);

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

            //if (num == 0)
            //{
            //    saveData.bagEquippableFirstIndex.Add(0);
            //    saveData.bagEquippableLastIndex.Add(0);
            //    saveData.bagConsumableFirstIndex.Add(0);
            //    saveData.bagConsumableLastIndex.Add(0);
            //    saveData.bagOtherItemFirstIndex.Add(0);
            //    saveData.bagOtherItemLastIndex.Add(0);
            //}
            //else
            //{
            //    int preFirstIndex = saveData.bagEquippableFirstIndex[num - 1];
            //    saveData.bagEquippableFirstIndex.Add(preFirstIndex);
            //    saveData.bagEquippableLastIndex.Add(preFirstIndex);
            //    preFirstIndex = saveData.bagConsumableFirstIndex[num - 1];
            //    saveData.bagConsumableFirstIndex.Add(preFirstIndex);
            //    saveData.bagConsumableLastIndex.Add(preFirstIndex);
            //    preFirstIndex = saveData.bagOtherItemFirstIndex[num - 1];
            //    saveData.bagOtherItemFirstIndex.Add(preFirstIndex);
            //    saveData.bagOtherItemLastIndex.Add(preFirstIndex);
            //}

            int index= currentSquad.Count;
            stat.saveId = index;
            stat.Setting();
            currentSquad.Add(index, stat);
        }
        PlayerSaveLoadSystem.Save(saveData);
    }

    public void RemoveCharacter(int i)
    {
        saveData.boarding.RemoveAt(i);
        saveData.id.RemoveAt(i);
        saveData.name.RemoveAt(i);
        saveData.characterName.RemoveAt(i);
        saveData.hp.RemoveAt(i);
        saveData.maxHp.RemoveAt(i);
        saveData.sensitivity.RemoveAt(i);
        saveData.avoidRate.RemoveAt(i);
        saveData.concentration.RemoveAt(i);
        saveData.willPower.RemoveAt(i);
        saveData.level.RemoveAt(i);
        saveData.currentExp.RemoveAt(i);
        saveData.sightDistance.RemoveAt(i);
        saveData.weight.RemoveAt(i);

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

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}

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
    public Dictionary<string, Consumable> consumableList = new Dictionary<string, Consumable>();
    public Dictionary<string, Weapon> equippableList = new Dictionary<string, Weapon>();
    public Dictionary<string, Monster> monsterList = new Dictionary<string, Monster>();
    public Dictionary<string, Virus> virusList = new Dictionary<string, Virus>();
    public Dictionary<string, ActiveSkill> activeSkillList = new Dictionary<string, ActiveSkill>();
    public Dictionary<string, PassiveSkill> passiveSkillList = new Dictionary<string, PassiveSkill>();
    public Dictionary<string, Truck> truckList = new Dictionary<string, Truck>();
    ScriptableMgr scriptableMgr;

    //아지트 아이템 데이터.
    public Dictionary<string, Weapon> currentEquippables = new Dictionary<string, Weapon>();
    public Dictionary<string, int> currentEquippablesNum = new Dictionary<string, int>();
    public Dictionary<string, Consumable> currentConsumables = new Dictionary<string, Consumable>();
    public Dictionary<string, int> currentConsumablesNum = new Dictionary<string, int>();

    //트럭 데이터.
    public Dictionary<string, Weapon> truckEquippables = new Dictionary<string, Weapon>();
    public Dictionary<string, int> truckEquippablesNum = new Dictionary<string, int>();
    public Dictionary<string, Consumable> truckConsumables = new Dictionary<string, Consumable>();
    public Dictionary<string, int> truckConsumablesNum = new Dictionary<string, int>();

    //캐릭터 데이터.
    public Dictionary<int, CharacterStats> currentSquad = new Dictionary<int, CharacterStats>();
    public Dictionary<int, int> boardingSquad = new Dictionary<int, int>();//트럭에 있는 캐릭터들.(좌석 번호/ 인덱스)
    public Dictionary<int, CharacterStats> battleSquad = new Dictionary<int, CharacterStats>();//전투에 나갈 캐릭터들.
                                                                                               
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        scriptableMgr = ScriptableMgr.Instance;

        characterList = scriptableMgr.characterList;
        consumableList = scriptableMgr.consumableList;
        equippableList = scriptableMgr.equippableList;
        monsterList = scriptableMgr.monsterList;
        virusList = scriptableMgr.virusList;
        activeSkillList = scriptableMgr.activeSkillList;
        passiveSkillList = scriptableMgr.passiveSkillList;
        truckList = scriptableMgr.truckList;

        filePath = @$"{Application.persistentDataPath}\PlayerData.json";
        if (saveData.id == null)
        {
            saveData.id = new List<string>();
            saveData.boarding = new List<int>();
            saveData.name = new List<string>();
            saveData.hp = new List<int>();
            saveData.maxHp = new List<int>();
            saveData.sensitivity = new List<int>();
            saveData.concentration = new List<int>();
            saveData.willPower = new List<int>();

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

            saveData.equippableList = new List<string>();
            saveData.equippableNumList = new List<int>();
            saveData.consumableList = new List<string>();
            saveData.consumableNumList = new List<int>();

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
        }

        var obj = FindObjectsOfType<PlayerDataMgr>();
        if (obj.Length == 1)
        {
            //처음하기.
            if (!PlayerPrefs.HasKey("Continue"))
            {
                string str = "Continue";
                PlayerPrefs.SetInt(str, 1);

                //테스트용.
                //////////////////////////////
                foreach (var element in equippableList)
                {
                    saveData.equippableList.Add(element.Key);
                    int random = Random.Range(1, 3);
                    saveData.equippableNumList.Add(random);
                    currentEquippables.Add(element.Key, element.Value);
                    currentEquippablesNum.Add(element.Key, random);
                }

                foreach (var element in consumableList)
                {
                    saveData.consumableList.Add(element.Key);
                    int random = Random.Range(1, 3);
                    saveData.consumableNumList.Add(random);
                    currentConsumables.Add(element.Key, element.Value);
                    currentConsumablesNum.Add(element.Key, random);
                }

                PlayerSaveLoadSystem.Save(saveData);
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

                    stat.weapon = new WeaponStats();
                    stat.weapon.mainWeapon = (saveData.mainWeapon[i] == null) ? null : equippableList[saveData.mainWeapon[i]];
                    stat.weapon.subWeapon = (saveData.subWeapon[i] == null) ? null : equippableList[saveData.subWeapon[i]];

                    stat.virusPanalty["E"].penaltyLevel = saveData.levelE[i];
                    stat.virusPanalty["B"].penaltyLevel = saveData.levelB[i];
                    stat.virusPanalty["P"].penaltyLevel = saveData.levelP[i];
                    stat.virusPanalty["I"].penaltyLevel = saveData.levelI[i];
                    stat.virusPanalty["T"].penaltyLevel = saveData.levelT[i];

                    stat.virusPanalty["E"].penaltyGauge = saveData.gaugeE[i];
                    stat.virusPanalty["B"].penaltyGauge = saveData.gaugeB[i];
                    stat.virusPanalty["P"].penaltyGauge = saveData.gaugeP[i];
                    stat.virusPanalty["I"].penaltyGauge = saveData.gaugeI[i];
                    stat.virusPanalty["T"].penaltyGauge = saveData.gaugeT[i];

                    // 홍수진_캐릭터 스킬 수정 중이라 잠시 주석
                    //List<string> activeSkill = new List<string>();
                    //int activeSkillNum = activeSkillList.Count;
                    //for (int j = 0; j < activeSkillNum; j++) { activeSkill.Add(saveData.activeSkillList[i * activeSkillNum + j]); }
                    //foreach (var element in activeSkill)
                    //{
                    //    if (element == null) continue;
                    //    stat.skills.activeSkills.Add(activeSkillList[element]);
                    //}
                    //
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
                        stat.bag.Add(saveData.bagEquippableList[j], saveData.bagEquippableNumList[j]);
                    }

                    for (int j = saveData.bagConsumableFirstIndex[i]; j < saveData.bagConsumableLastIndex[i]; j++)
                    {
                        stat.bag.Add(saveData.bagConsumableList[j], saveData.bagConsumableNumList[j]);
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

            stat.weapon = new WeaponStats();
            stat.weapon.mainWeapon = (saveData.mainWeapon[i] == null) ? null : equippableList[saveData.mainWeapon[i]];
            stat.weapon.subWeapon = (saveData.subWeapon[i] == null) ? null : equippableList[saveData.subWeapon[i]];

            List<string> activeSkill = new List<string>();
            int activeSkillNum = activeSkillList.Count;
            for (int j = 0; j < activeSkillNum; j++) { activeSkill.Add(saveData.activeSkillList[i * activeSkillNum + j]); }
            foreach (var element in activeSkill)
            {
                if (element == null) continue;
                stat.skills.activeSkills.Add(activeSkillList[element]);
            }

            List<string> passiveSkill = new List<string>();
            int passiveSkillNum = passiveSkillList.Count;
            for (int j = 0; j < passiveSkillNum; j++) { passiveSkill.Add(saveData.passiveSkillList[i * passiveSkillNum + j]); }
            foreach (var element in passiveSkill)
            {
                if (element == null) continue;
                stat.skills.passiveSkills.Add(passiveSkillList[element]);
            }

            for (int j = saveData.bagEquippableFirstIndex[i]; j < saveData.bagEquippableLastIndex[i]; j++)
            {
                stat.bag.Add(saveData.bagEquippableList[j], saveData.bagEquippableNumList[j]);
            }

            for (int j = saveData.bagConsumableFirstIndex[i]; j < saveData.bagConsumableLastIndex[i]; j++)
            {
                stat.bag.Add(saveData.bagConsumableList[j], saveData.bagConsumableNumList[j]);
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

            saveData.gaugeE.Add(stat.virusPanalty["E"].penaltyGauge);
            saveData.gaugeB.Add(stat.virusPanalty["B"].penaltyGauge);
            saveData.gaugeP.Add(stat.virusPanalty["P"].penaltyGauge);
            saveData.gaugeI.Add(stat.virusPanalty["I"].penaltyGauge);
            saveData.gaugeT.Add(stat.virusPanalty["T"].penaltyGauge);

            saveData.levelE.Add(stat.virusPanalty["E"].penaltyLevel);
            saveData.levelB.Add(stat.virusPanalty["B"].penaltyLevel);
            saveData.levelP.Add(stat.virusPanalty["P"].penaltyLevel);
            saveData.levelI.Add(stat.virusPanalty["I"].penaltyLevel);
            saveData.levelT.Add(stat.virusPanalty["T"].penaltyLevel);

            string mainWeaponStr = (stat.weapon.mainWeapon != null) ? stat.weapon.mainWeapon.id : null;
            saveData.mainWeapon.Add(mainWeaponStr);
            string subWeaponStr = (stat.weapon.subWeapon != null) ? stat.weapon.subWeapon.id : null;
            saveData.subWeapon.Add(subWeaponStr);

            
            for (int k = 0; k < 5; k++) 
            {
                string activeSkillStr = (stat.skills.activeSkills[k] != null) ? stat.skills.activeSkills[k].id : null;
                saveData.activeSkillList.Add(activeSkillStr); 
            }
            for (int k = 0; k < 5; k++) 
            {
                string passiveSkillStr = (stat.skills.passiveSkills[k] != null) ? stat.skills.passiveSkills[k].id : null;
                saveData.passiveSkillList.Add(passiveSkillStr); 
            }

            if (num == 0)
            {
                saveData.bagEquippableFirstIndex.Add(0);
                saveData.bagEquippableLastIndex.Add(0);
                saveData.bagConsumableFirstIndex.Add(0);
                saveData.bagConsumableLastIndex.Add(0);
            }
            else
            {
                int preFirstIndex = saveData.bagEquippableFirstIndex[num - 1];
                saveData.bagEquippableFirstIndex.Add(preFirstIndex);
                saveData.bagEquippableLastIndex.Add(preFirstIndex);
                preFirstIndex = saveData.bagConsumableFirstIndex[num - 1];
                saveData.bagConsumableFirstIndex.Add(preFirstIndex);
                saveData.bagConsumableLastIndex.Add(preFirstIndex);
            }

            stat.saveId = num;
            currentSquad.Add(num, stat);
        }
        //else //기존거 변경.
        //{
        //    saveData.id[num] = stat.character.id;
        //    saveData.name[num] = stat.character.name;
        //    saveData.hp[num] = stat.currentHp;
        //    saveData.maxHp[num] = stat.maxHp;
        //    saveData.sensitivity[num] = stat.sensivity;
        //    saveData.concentration[num] =stat.concentration;
        //    saveData.willPower[num] =stat.willpower;

        //    saveData.gaugeE[num] = stat.virusPanalty["E"].penaltyGauge;
        //    saveData.gaugeB[num] = stat.virusPanalty["B"].penaltyGauge;
        //    saveData.gaugeP[num] = stat.virusPanalty["P"].penaltyGauge;
        //    saveData.gaugeI[num] = stat.virusPanalty["I"].penaltyGauge;
        //    saveData.gaugeT[num] = stat.virusPanalty["T"].penaltyGauge;

        //    saveData.levelE[num] = stat.virusPanalty["E"].penaltyLevel;
        //    saveData.levelB[num] = stat.virusPanalty["B"].penaltyLevel;
        //    saveData.levelP[num] = stat.virusPanalty["P"].penaltyLevel;
        //    saveData.levelI[num] = stat.virusPanalty["I"].penaltyLevel;
        //    saveData.levelT[num] = stat.virusPanalty["T"].penaltyLevel;

        //    string mainWeaponStr = (stat.weapon.mainWeapon != null) ? stat.weapon.mainWeapon.id : null;
        //    saveData.mainWeapon[num] = mainWeaponStr;
        //    string subWeaponStr = (stat.weapon.subWeapon != null) ? stat.weapon.subWeapon.id : null;
        //    saveData.subWeapon[num] = subWeaponStr;

        //    for (int k = 0; k < 5; k++)
        //    {
        //        string activeSkillStr = (stat.skills.activeSkills[k] != null) ? stat.skills.activeSkills[k].id : null;
        //        saveData.activeSkillList[num * 5 + k] = activeSkillStr;
        //    }
        //    for (int k = 0; k < 5; k++)
        //    {
        //        string passiveSkillStr = (stat.skills.passiveSkills[k] != null) ? stat.skills.passiveSkills[k].id : null;
        //        saveData.passiveSkillList[num * 5 + k] =passiveSkillStr;
        //    }

        //    currentSquad[num] = stat;
        //}

        PlayerSaveLoadSystem.Save(saveData);
    }
}

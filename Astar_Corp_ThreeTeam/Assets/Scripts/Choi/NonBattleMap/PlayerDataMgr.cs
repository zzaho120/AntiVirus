using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //테스터
using System.Linq;

public class PlayerDataMgr : MonoBehaviour
{
    //테스트 출력
    public Text[] text;

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
    ScriptableMgr scriptableMgr;

    //아이템 데이터.
    public Dictionary<string, Weapon> currentEquippables = new Dictionary<string, Weapon>();
    public Dictionary<string, int> currentEquippablesNum = new Dictionary<string, int>();
    public Dictionary<string, Consumable> currentConsumables = new Dictionary<string, Consumable>();
    public Dictionary<string, int> currentConsumablesNum = new Dictionary<string, int>();

    //캐릭터 데이터.
    public Dictionary<int, CharacterStats> currentSquad = new Dictionary<int, CharacterStats>();//현재 소유한 캐릭터들.
    public Dictionary<int, CharacterStats> boardingSquad = new Dictionary<int, CharacterStats>();//트럭에 탑승한 캐릭터들.
    public Dictionary<int, CharacterStats> battleSquad = new Dictionary<int, CharacterStats>();//전투에 나갈 캐릭터들.

    // 주수, 프로토 타입용
    public int virusAreaLevel;
    public string virusAreaType;

    private void Start()
    {
        scriptableMgr = ScriptableMgr.Instance;

        characterList = scriptableMgr.characterList;
        consumableList = scriptableMgr.consumableList;
        equippableList = scriptableMgr.equippableList;
        monsterList = scriptableMgr.monsterList;
        virusList = scriptableMgr.virusList;
        activeSkillList = scriptableMgr.activeSkillList;
        passiveSkillList = scriptableMgr.passiveSkillList;

        filePath = @$"{Application.persistentDataPath}\PlayerData.json";
        if (saveData.id == null)
        {
            saveData.id = new List<string>();
            saveData.name = new List<string>();
            saveData.hp = new List<int>();
            saveData.sensitivity = new List<int>();
            saveData.concentration = new List<int>();
            saveData.willPower = new List<int>();

            saveData.mainWeapon = new List<string>();
            saveData.subWeapon = new List<string>();
            saveData.activeSkillList = new List<string>();
            saveData.passiveSkillList = new List<string>();
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
        }

        var obj = FindObjectsOfType<PlayerDataMgr>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            //처음하기.
            if (!PlayerPrefs.HasKey("Continue"))
            {
                string str = "Continue";
                PlayerPrefs.SetInt(str, 1);

                
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

                //테스트용.
                //////////////////////////////
                foreach (var element in equippableList)
                {
                    currentEquippables.Add(element.Key, element.Value);
                }
                //////////////////////////////

                //캐릭터 설정.
                for (int i = 0; i < saveData.name.Count; i++)
                {
                    //게임상 관리하기 쉽도록.
                    CharacterStats stat = new CharacterStats();
                    stat.VirusPanaltyInit();
                    stat.currentHp = saveData.hp[i];
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

                    currentSquad.Add(i, stat);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCharacter(int num,CharacterStats stat)
    {
        string str = "SquadNum";
        int totalSquadNum = (PlayerPrefs.HasKey(str)) ? PlayerPrefs.GetInt(str) : 0;

        //인원 추가.
        if (num > totalSquadNum - 1)
        {
            saveData.id.Add(stat.character.id);
            saveData.name.Add(stat.character.name);
            saveData.hp.Add(stat.currentHp);
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

            currentSquad.Add(num, stat);
        }
        else //기존거 변경.
        {
            saveData.id[num] = stat.character.id;
            saveData.name[num] = stat.character.name;
            saveData.hp[num] = stat.currentHp;
            saveData.sensitivity[num] = stat.sensivity;
            saveData.concentration[num] =stat.concentration;
            saveData.willPower[num] =stat.willpower;

            saveData.gaugeE[num] = stat.virusPanalty["E"].penaltyGauge;
            saveData.gaugeB[num] = stat.virusPanalty["B"].penaltyGauge;
            saveData.gaugeP[num] = stat.virusPanalty["P"].penaltyGauge;
            saveData.gaugeI[num] = stat.virusPanalty["I"].penaltyGauge;
            saveData.gaugeT[num] = stat.virusPanalty["T"].penaltyGauge;

            saveData.levelE[num] = stat.virusPanalty["E"].penaltyLevel;
            saveData.levelB[num] = stat.virusPanalty["B"].penaltyLevel;
            saveData.levelP[num] = stat.virusPanalty["P"].penaltyLevel;
            saveData.levelI[num] = stat.virusPanalty["I"].penaltyLevel;
            saveData.levelT[num] = stat.virusPanalty["T"].penaltyLevel;

            string mainWeaponStr = (stat.weapon.mainWeapon != null) ? stat.weapon.mainWeapon.id : null;
            saveData.mainWeapon[num] = mainWeaponStr;
            string subWeaponStr = (stat.weapon.subWeapon != null) ? stat.weapon.subWeapon.id : null;
            saveData.subWeapon[num] = subWeaponStr;

            for (int k = 0; k < 5; k++)
            {
                string activeSkillStr = (stat.skills.activeSkills[k] != null) ? stat.skills.activeSkills[k].id : null;
                saveData.activeSkillList[num * 5 + k] = activeSkillStr;
            }
            for (int k = 0; k < 5; k++)
            {
                string passiveSkillStr = (stat.skills.passiveSkills[k] != null) ? stat.skills.passiveSkills[k].id : null;
                saveData.passiveSkillList[num * 5 + k] =passiveSkillStr;
            }

            currentSquad[num] = stat;
        }

        PlayerSaveLoadSystem.Save(saveData);
    }

    public void BattleStartTester()
    {
        foreach (var chars in battleSquad)
        {
            Debug.Log("Selected Character : " + chars.Value.Name);
            //Debug.Log(battleSquad.Count);
        }
    }
}

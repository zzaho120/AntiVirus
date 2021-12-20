using System.Collections.Generic;
using UnityEngine;

public class DataCharacter
{
    public int id; //유저가 보유한 id

    public CharacterTableElem tableElem;

    // public int exp;
    // public int level;

    public List<EquippableTableElem> listDataArmor = new List<EquippableTableElem>();

    // Stats

    public float LEVEL
    {
        get
        {
            var baseLevel = tableElem.level;  // 테이블에 있는 기본 스탯
            var level = baseLevel;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //ad += itemelem.ad;


            // 무기에 의한 추가 수치
            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return level;
        }
    }

    public float DAMAGE
    {
        get
        {
            var baseDamage = tableElem.damage;  // 테이블에 있는 기본 스탯
            var damage = baseDamage;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //ap += itemelem.ap;


            // 무기에 의한 추가 수치
            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return damage;
        }
    }

    public float RANGE
    {
        get
        {
            var baseRange = tableElem.range;  // 테이블에 있는 기본 스탯
            var range = baseRange;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //df += itemelem.df;


            // 무기에 의한 추가 수치
            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return range;
        }
    }

    public float HP
    {
        get
        {
            var baseHp = tableElem.hp;  // 테이블에 있는 기본 스탯
            var hp = baseHp;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //hp += itemelem.hp;


            // 무기에 의한 추가 수치
            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return hp;
        }
    }

    public float CRIT_RATE
    {
        get
        {
            var baseRate = tableElem.crit_rate;  // 테이블에 있는 기본 스탯
            var rate = baseRate;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //mp += itemelem.mp;


            // 무기에 의한 추가 수치
            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return rate;
        }
    }

    public int WILLPOWER
    {
        get
        {
            var baseWillpower = tableElem.willpower;  // 테이블에 있는 기본 스탯
            var willpower = baseWillpower;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //statStr += itemelem.stat_str;


            // 무기에 의한 추가 수치
            //statStr += dataWeapon.tableElem.weapon_str;

            // 방어구들 추가 수치
            //foreach (var element in listDataArmor)
            //{
            //    statStr += element.def;
            //}

            // 버프, 디버프 보정
            return willpower;
        }
    }

    public int STAMINA
    {
        get
        {
            var baseStamina = tableElem.stamina;  // 테이블에 있는 기본 스탯
            var stamina = baseStamina;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //statDex += itemelem.stat_dex;

            // 무기에 의한 추가 수치

            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return stamina;
        }
    }

    public int RESISTANCE
    {
        get
        {
            var baseResist = tableElem.resistance;  // 테이블에 있는 기본 스탯
            var resistance = baseResist;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //statInt += itemelem.stat_int;

            // 무기에 의한 추가 수치
            //statInt += dataWeapon.tableElem.weapon_int;

            // 방어구들 추가 수치
            //foreach (var element in listDataArmor)
            //{
            //    statInt += element.intellet;
            //}

            // 버프, 디버프 보정
            return resistance;
        }
    }


    public DataCharacter(string charID)
    {
        var table = DataTableMgr.GetTable<CharacterTable>();
        tableElem = table.GetData<CharacterTableElem>(charID);
    }

    public DataCharacter(CharacterTableElem tableElem)
    {
        this.tableElem = tableElem;
    }
}


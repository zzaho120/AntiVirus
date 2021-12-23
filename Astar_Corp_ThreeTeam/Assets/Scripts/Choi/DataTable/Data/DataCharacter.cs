using System.Collections.Generic;
using UnityEngine;

public class DataCharacter
{
    public int id; //유저가 보유한 id

    public CharacterTableElem tableElem;

    public List<EquippableTableElem> listDataArmor = new List<EquippableTableElem>();

    // Stats

    public float DAMAGE
    {
        get
        {
            var baseDamage = tableElem.damage;  // 테이블에 있는 기본 스탯
            var damage = baseDamage;

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
            var baseHp = tableElem.max_Hp;  // 테이블에 있는 기본 스탯
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
            var baseWillpower = tableElem.min_Willpower;  // 테이블에 있는 기본 스탯
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
            var baseStamina = tableElem.min_Stamina;  // 테이블에 있는 기본 스탯
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


// 추후 수정?
// 아이템, 레벨에 따른 추가 스탯 부여하기

//var itemTable = DataTableMgr.GetTable<LevelTable>();
//var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

// 레벨에 따른 추가 수치
//ad += itemelem.ad;


// 무기에 의한 추가 수치
// 방어구들 추가 수치
// 버프, 디버프 보정
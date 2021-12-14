using System.Collections.Generic;
using UnityEngine;

public class DataCharacter
{
    public int id; //유저가 보유한 id

    public CharacterTableElem tableElem;

    public int exp;
    public int level;

    public List<EquippableTableElem> listDataArmor = new List<EquippableTableElem>();

    // Stats

    public float AD
    {
        get
        {
            var baseAd = tableElem.ad;  // 테이블에 있는 기본 스탯
            var ad = baseAd;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //ad += itemelem.ad;


            // 무기에 의한 추가 수치
            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return ad;
        }
    }

    public float AP
    {
        get
        {
            var baseAp = tableElem.ap;  // 테이블에 있는 기본 스탯
            var ap = baseAp;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //ap += itemelem.ap;


            // 무기에 의한 추가 수치
            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return ap;
        }
    }

    public float DF
    {
        get
        {
            var baseDf = tableElem.df;  // 테이블에 있는 기본 스탯
            var df = baseDf;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //df += itemelem.df;


            // 무기에 의한 추가 수치
            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return df;
        }
    }

    public float Hp
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

    public float Mp
    {
        get
        {
            var baseMp = tableElem.mp;  // 테이블에 있는 기본 스탯
            var mp = baseMp;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //mp += itemelem.mp;


            // 무기에 의한 추가 수치
            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return mp;
        }
    }

    public int STAT_STR
    {
        get
        {
            var baseStat_str = tableElem.statStr;  // 테이블에 있는 기본 스탯
            var statStr = baseStat_str;

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
            return statStr;
        }
    }

    public int STAT_DEX
    {
        get
        {
            var baseStat_dex = tableElem.statDex;  // 테이블에 있는 기본 스탯
            var statDex = baseStat_dex;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //statDex += itemelem.stat_dex;

            // 무기에 의한 추가 수치

            // 방어구들 추가 수치
            // 버프, 디버프 보정
            return statDex;
        }
    }

    public int STAT_INT
    {
        get
        {
            var baseStat_int = tableElem.statInt;  // 테이블에 있는 기본 스탯
            var statInt = baseStat_int;

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
            return statInt;
        }
    }

    public int STAT_LUK
    {
        get
        {
            var baseStat_luk = tableElem.statLuk;  // 테이블에 있는 기본 스탯
            var statLuk = baseStat_luk;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // 레벨에 따른 추가 수치
            //statLuk += itemelem.stat_luk;

            // 무기에 의한 추가 수치
            //statLuk += dataWeapon.tableElem.weapon_luk;

            // 방어구들 추가 수치
            //foreach (var element in listDataArmor)
            //{
            //    statLuk += element.luck;
            //}

            // 버프, 디버프 보정
            return statLuk;
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


using System.Collections.Generic;
using UnityEngine;

public class DataCharacter
{
    public int id; //������ ������ id

    public CharacterTableElem tableElem;

    public int exp;
    public int level;

    public List<EquippableTableElem> listDataArmor = new List<EquippableTableElem>();

    // Stats

    public float AD
    {
        get
        {
            var baseAd = tableElem.ad;  // ���̺� �ִ� �⺻ ����
            var ad = baseAd;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //ad += itemelem.ad;


            // ���⿡ ���� �߰� ��ġ
            // ���� �߰� ��ġ
            // ����, ����� ����
            return ad;
        }
    }

    public float AP
    {
        get
        {
            var baseAp = tableElem.ap;  // ���̺� �ִ� �⺻ ����
            var ap = baseAp;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //ap += itemelem.ap;


            // ���⿡ ���� �߰� ��ġ
            // ���� �߰� ��ġ
            // ����, ����� ����
            return ap;
        }
    }

    public float DF
    {
        get
        {
            var baseDf = tableElem.df;  // ���̺� �ִ� �⺻ ����
            var df = baseDf;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //df += itemelem.df;


            // ���⿡ ���� �߰� ��ġ
            // ���� �߰� ��ġ
            // ����, ����� ����
            return df;
        }
    }

    public float Hp
    {
        get
        {
            var baseHp = tableElem.hp;  // ���̺� �ִ� �⺻ ����
            var hp = baseHp;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //hp += itemelem.hp;


            // ���⿡ ���� �߰� ��ġ
            // ���� �߰� ��ġ
            // ����, ����� ����
            return hp;
        }
    }

    public float Mp
    {
        get
        {
            var baseMp = tableElem.mp;  // ���̺� �ִ� �⺻ ����
            var mp = baseMp;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //mp += itemelem.mp;


            // ���⿡ ���� �߰� ��ġ
            // ���� �߰� ��ġ
            // ����, ����� ����
            return mp;
        }
    }

    public int STAT_STR
    {
        get
        {
            var baseStat_str = tableElem.statStr;  // ���̺� �ִ� �⺻ ����
            var statStr = baseStat_str;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //statStr += itemelem.stat_str;


            // ���⿡ ���� �߰� ��ġ
            //statStr += dataWeapon.tableElem.weapon_str;

            // ���� �߰� ��ġ
            //foreach (var element in listDataArmor)
            //{
            //    statStr += element.def;
            //}

            // ����, ����� ����
            return statStr;
        }
    }

    public int STAT_DEX
    {
        get
        {
            var baseStat_dex = tableElem.statDex;  // ���̺� �ִ� �⺻ ����
            var statDex = baseStat_dex;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //statDex += itemelem.stat_dex;

            // ���⿡ ���� �߰� ��ġ

            // ���� �߰� ��ġ
            // ����, ����� ����
            return statDex;
        }
    }

    public int STAT_INT
    {
        get
        {
            var baseStat_int = tableElem.statInt;  // ���̺� �ִ� �⺻ ����
            var statInt = baseStat_int;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //statInt += itemelem.stat_int;

            // ���⿡ ���� �߰� ��ġ
            //statInt += dataWeapon.tableElem.weapon_int;

            // ���� �߰� ��ġ
            //foreach (var element in listDataArmor)
            //{
            //    statInt += element.intellet;
            //}

            // ����, ����� ����
            return statInt;
        }
    }

    public int STAT_LUK
    {
        get
        {
            var baseStat_luk = tableElem.statLuk;  // ���̺� �ִ� �⺻ ����
            var statLuk = baseStat_luk;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //statLuk += itemelem.stat_luk;

            // ���⿡ ���� �߰� ��ġ
            //statLuk += dataWeapon.tableElem.weapon_luk;

            // ���� �߰� ��ġ
            //foreach (var element in listDataArmor)
            //{
            //    statLuk += element.luck;
            //}

            // ����, ����� ����
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


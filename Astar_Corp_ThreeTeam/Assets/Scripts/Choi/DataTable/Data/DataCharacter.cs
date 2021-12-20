using System.Collections.Generic;
using UnityEngine;

public class DataCharacter
{
    public int id; //������ ������ id

    public CharacterTableElem tableElem;

    // public int exp;
    // public int level;

    public List<EquippableTableElem> listDataArmor = new List<EquippableTableElem>();

    // Stats

    public float LEVEL
    {
        get
        {
            var baseLevel = tableElem.level;  // ���̺� �ִ� �⺻ ����
            var level = baseLevel;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //ad += itemelem.ad;


            // ���⿡ ���� �߰� ��ġ
            // ���� �߰� ��ġ
            // ����, ����� ����
            return level;
        }
    }

    public float DAMAGE
    {
        get
        {
            var baseDamage = tableElem.damage;  // ���̺� �ִ� �⺻ ����
            var damage = baseDamage;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //ap += itemelem.ap;


            // ���⿡ ���� �߰� ��ġ
            // ���� �߰� ��ġ
            // ����, ����� ����
            return damage;
        }
    }

    public float RANGE
    {
        get
        {
            var baseRange = tableElem.range;  // ���̺� �ִ� �⺻ ����
            var range = baseRange;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //df += itemelem.df;


            // ���⿡ ���� �߰� ��ġ
            // ���� �߰� ��ġ
            // ����, ����� ����
            return range;
        }
    }

    public float HP
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

    public float CRIT_RATE
    {
        get
        {
            var baseRate = tableElem.crit_rate;  // ���̺� �ִ� �⺻ ����
            var rate = baseRate;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //mp += itemelem.mp;


            // ���⿡ ���� �߰� ��ġ
            // ���� �߰� ��ġ
            // ����, ����� ����
            return rate;
        }
    }

    public int WILLPOWER
    {
        get
        {
            var baseWillpower = tableElem.willpower;  // ���̺� �ִ� �⺻ ����
            var willpower = baseWillpower;

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
            return willpower;
        }
    }

    public int STAMINA
    {
        get
        {
            var baseStamina = tableElem.stamina;  // ���̺� �ִ� �⺻ ����
            var stamina = baseStamina;

            //var itemTable = DataTableMgr.GetTable<LevelTable>();
            //var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

            // ������ ���� �߰� ��ġ
            //statDex += itemelem.stat_dex;

            // ���⿡ ���� �߰� ��ġ

            // ���� �߰� ��ġ
            // ����, ����� ����
            return stamina;
        }
    }

    public int RESISTANCE
    {
        get
        {
            var baseResist = tableElem.resistance;  // ���̺� �ִ� �⺻ ����
            var resistance = baseResist;

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


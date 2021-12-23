using System.Collections.Generic;
using UnityEngine;

public class DataCharacter
{
    public int id; //������ ������ id

    public CharacterTableElem tableElem;

    public List<EquippableTableElem> listDataArmor = new List<EquippableTableElem>();

    // Stats

    public float DAMAGE
    {
        get
        {
            var baseDamage = tableElem.damage;  // ���̺� �ִ� �⺻ ����
            var damage = baseDamage;

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
            var baseHp = tableElem.max_Hp;  // ���̺� �ִ� �⺻ ����
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
            var baseWillpower = tableElem.min_Willpower;  // ���̺� �ִ� �⺻ ����
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
            var baseStamina = tableElem.min_Stamina;  // ���̺� �ִ� �⺻ ����
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


// ���� ����?
// ������, ������ ���� �߰� ���� �ο��ϱ�

//var itemTable = DataTableMgr.GetTable<LevelTable>();
//var itemelem = itemTable.GetData<LevelTableElem>(level.ToString());

// ������ ���� �߰� ��ġ
//ad += itemelem.ad;


// ���⿡ ���� �߰� ��ġ
// ���� �߰� ��ġ
// ����, ����� ����
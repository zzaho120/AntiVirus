using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquippableStat
{
    Ad,
    StatStr,
    StatInt,
    StatLuk,
}

public class EquippableTableElem : DataTableElemBase
{
    //public string iconId;
    //public string prefabId;
    public string name;
    public string description;
    public int damage;
    public int critRate;
    public int critDamage;
    public int cost;
    public int weight;
    public int str;
    public int con;
    public int intellet;
    public int luck;
    //private Sprite iconSprite;

    //public Sprite IconSprite
    //{
    //    get { return iconSprite; }
    //}
    public EquippableTableElem(Dictionary<string, string> data)
    {
        id = data["ID"];
        //iconId = data["ICON_ID"];
        //prefabId = data["PREFAB_ID"];
        name = data["NAME"];
        description = data["DESC"];
        type = data["TYPE"];
        damage = int.Parse(data["DAMAGE"]);
        critRate = int.Parse(data["CRIT_RATE"]);
        critDamage = int.Parse(data["CRIT_DAMAGE"]);
        cost = int.Parse(data["COST"]);
        weight = int.Parse(data["WEIGHT"]);
        str = int.Parse(data["STR"]);
        con = int.Parse(data["CON"]);
        intellet = int.Parse(data["INT"]);
        luck = int.Parse(data["LUK"]);

        //iconSprite = Resources.Load<Sprite>($"Sprites/Icons/{iconId}");
    }
}

public class EquippableTable : DataTableBase
{
    public int weaponCount;
    public int armorCount;
    public EquippableTable()
    {
        csvFilePath = @"Choi\EquipDataTable";

        weaponCount = 0;
        armorCount = 0;
    }

    public override void Load()
    {
        data.Clear();
        var list = CSVReader.Read(csvFilePath);
        foreach (var line in list)
        {
            var elem = new EquippableTableElem(line);

            var array =elem.id.Split("_");
            if (array[0].Equals("WEA")) weaponCount++;
            else if (array[0].Equals("DEF")) armorCount++;

            data.Add(elem.id, elem);
        }

    }
}
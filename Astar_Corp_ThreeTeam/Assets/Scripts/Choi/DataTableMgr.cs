using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DataTableTypes
{
    Weapon,
    Armor,
    Equippable,
    Consumable,
    Character
}
public static class DataTableMgr
{
    public static Dictionary<DataTableTypes, DataTableBase> tables =
        new Dictionary<DataTableTypes, DataTableBase>();

    public static void Init()
    {
        tables.Clear();
        var weaponTable = new WeaponTable();
        weaponTable.Load();
        tables.Add(DataTableTypes.Weapon, weaponTable);

        var armorTable = new ArmorTable();
        armorTable.Load();
        tables.Add(DataTableTypes.Armor, armorTable);

        var equippableTable = new EquippableTable();
        equippableTable.Load();
        tables.Add(DataTableTypes.Equippable, equippableTable);


        var itemTable = new ItemTable();
        itemTable.Load();
        tables.Add(DataTableTypes.Consumable, itemTable);

        var characterTable = new CharacterTable();
        characterTable.Load();
        tables.Add(DataTableTypes.Character, characterTable);
    }

    public static T GetTable<T>(DataTableTypes dataTableTypes) where T : DataTableBase
    {
        return (T)tables[dataTableTypes];
    }
}

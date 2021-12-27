using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTableMgr
{
    public static Dictionary<System.Type, DataTableBase> tables =
        new Dictionary<System.Type, DataTableBase>();

    private static bool inited = false;

    public static void Init()
    {
        var itemTable = new ItemTable();
        itemTable.Load();
        tables.Add(typeof(ItemTable), itemTable);

        var equippableTable = new EquippableTable();
        equippableTable.Load();
        tables.Add(typeof(EquippableTable), equippableTable);

        CharacterTable charTable = new CharacterTable();
        charTable.Load();
        tables.Add(typeof(CharacterTable), charTable);

        //var levelTable = new LevelTable();
        //levelTable.Load();
        //tables.Add(typeof(LevelTable), levelTable);

        inited = true;
    }

    public static T GetTable<T>() where T : DataTableBase
    {
        if (!inited)
            Init();
        var t = typeof(T);
        if (!tables.ContainsKey(t))
            return null;
        return tables[t] as T;
    }
}

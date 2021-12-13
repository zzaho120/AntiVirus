using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    public 
    void Awake()
    {
        DataTableMgr.Init();
        //WeaponTable weaponTable = DataTableMgr.GetTable<WeaponTable>(DataTableTypes.Weapon);
        //ArmorTable armorTable = DataTableMgr.GetTable<ArmorTable>(DataTableTypes.Armor);
        EquippableTable equippableTable = DataTableMgr.GetTable<EquippableTable>(DataTableTypes.Equippable);
        ItemTable itemTable = DataTableMgr.GetTable<ItemTable>(DataTableTypes.Consumable);
        CharacterTable chracterTable = DataTableMgr.GetTable<CharacterTable>(DataTableTypes.Character);

        for (int i = 0; i < equippableTable.weaponCount; i++)
        {
            var randId = $"WEA_000{i}";
            var element = equippableTable.GetData<EquippableTableElem>(randId);
            if (element != null)
            {
                Debug.Log($"{ element.id}, {element.name}, {element.description}, {element.damage}");
            }
        }

        for (int i = 0; i < equippableTable.armorCount; i++)
        {
            var randId = $"DEF_000{i}";
            var element = equippableTable.GetData<EquippableTableElem>(randId);
            if (element != null)
            {
                Debug.Log($"{ element.id}, {element.name}, {element.description}, {element.damage}");
            }
        }
        Debug.Log($"itemTable.Count : {itemTable.Count}");
        for (int i = 0; i < itemTable.Count; i++)
        {
            var randId = $"CON_000{i+1}";
            var element = itemTable.GetData<ItemTableElem>(randId);
            if (element != null)
            {
                Debug.Log($"{ element.id}, {element.name}, {element.description}");
            }
            else
            {
                Debug.Log($"null");
            }
        }

        //Debug.Log($"chracterTable.Count : {chracterTable.Count}");
        for (int i = 0; i < chracterTable.Count; i++)
        {
            var randId = $"CHAR_000{i+1}";
            var element = chracterTable.GetData<CharacterTableElem>(randId);
            if (element != null)
            {
                Debug.Log($"{ element.id}, {element.name}, {element.description}");
            }
        }
    }
}

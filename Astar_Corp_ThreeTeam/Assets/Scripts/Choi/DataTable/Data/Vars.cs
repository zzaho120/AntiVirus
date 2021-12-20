using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vars
{
    private static UserData userData;

    public static UserData UserData
    {
        get
        {
            if (userData == null)
            {
                userData = new UserData();
                userData.id = 111;
                userData.nickname = "OH";

                userData.consumableNameList.Add("CON_0001");
                userData.consumableNumList.Add(2);
                userData.consumableNameList.Add("CON_0002");
                userData.consumableNumList.Add(3);
                
                //userData.consumableItemList.Add(new ItemCountPair("CON_0001",2));
                //userData.consumableItemList.Add(new ItemCountPair("CON_0002", 3));
                //userData.consumableItemList.Add(new ItemCountPair("CON_0003", 5));
                //userData.consumableItemList.Add(new ItemCountPair("CON_0004", 1));
                //userData.consumableItemList.Add(new ItemCountPair("CON_0005", 2));
                //userData.consumableItemList.Add(new ItemCountPair("CON_0006", 2));
                //userData.consumableItemList.Add(new ItemCountPair("CON_0007", 2));
                //userData.consumableItemList.Add(new ItemCountPair("CON_0008", 1));
                /*
                userData.weaponItemList.Add("WEA_0001");
                userData.weaponItemList.Add("WEA_0002");
                userData.weaponItemList.Add("WEA_0003");
                userData.weaponItemList.Add("WEA_0004");

                userData.armorItemList.Add("DEF_0001");
                userData.armorItemList.Add("DEF_0002");
                userData.armorItemList.Add("DEF_0003");
                userData.armorItemList.Add("DEF_0004");
                userData.armorItemList.Add("DEF_0005");
                userData.armorItemList.Add("DEF_0006");
                userData.armorItemList.Add("DEF_0007");
                userData.armorItemList.Add("DEF_0008");
                userData.armorItemList.Add("DEF_0009");
                userData.armorItemList.Add("DEF_0010");
                userData.armorItemList.Add("DEF_0011");
                userData.armorItemList.Add("DEF_0012");

                userData.characterList.Add(new DataCharacter("CHAR_0001"));
                userData.characterList.Add(new DataCharacter("CHAR_0002"));

                userData.characterList[0].level = 10;
                //公扁.
                var weaponTable = DataTableMgr.GetTable<WeaponTable>();
                userData.characterList[0].dataWeapon = weaponTable.GetData<WeaponTableElem>("WEA_0001");
                //规绢备.
                var armorTable = DataTableMgr.GetTable<ArmorTable>();
                userData.characterList[0].listDataArmor.Add(armorTable.GetData<ArmorTableElem>("DEF_0001"));
                userData.characterList[0].listDataArmor.Add(armorTable.GetData<ArmorTableElem>("DEF_0002"));

                userData.characterList[1].level = 15;
                //公扁.
                userData.characterList[1].dataWeapon = weaponTable.GetData<WeaponTableElem>("WEA_0002");
                //规绢备.
                userData.characterList[1].listDataArmor.Add(armorTable.GetData<ArmorTableElem>("DEF_0003"));
                userData.characterList[1].listDataArmor.Add(armorTable.GetData<ArmorTableElem>("DEF_0004"));
                userData.characterList[1].listDataArmor.Add(armorTable.GetData<ArmorTableElem>("DEF_0005"));
                userData.characterList[1].listDataArmor.Add(armorTable.GetData<ArmorTableElem>("DEF_0006"));
                */
            }

            return userData;
        }
    }
}


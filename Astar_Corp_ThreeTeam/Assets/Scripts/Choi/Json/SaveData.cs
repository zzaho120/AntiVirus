using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public static readonly SaveData DefaultData = new SaveData()
    {
        characterData = Vars.UserData.characterList,
        equippableData = Vars.UserData.equippableList,

        consumableNameData = Vars.UserData.consumableNameList,
        consumableNumData = Vars.UserData.consumableNumList
    };
    public int Version { get; set; }

    //저장할 데이터들.
    public List<string> characterData { get; set; }
    public List<string> equippableData { get; set; }
    public List<string> consumableNameData { get; set; }
    public List<int> consumableNumData { get; set; }
    
}


public class SaveDatabase
{
    public List<string> characterData { get; set; }
    public List<string> equippableData { get; set; }
    public List<string> consumableNameData { get; set; }
    public List<int> consumableNumData { get; set; }

    public int Version { get; set; }

    public virtual SaveDatabase VersionUp()
    {
        return null;
    }
}

public class SaveDataV1 : SaveDatabase
{

    public int IntData { get; set; } = 0;

    public SaveDataV1()
    {
        Version = 1;

        characterData = Vars.UserData.characterList;
        equippableData = Vars.UserData.equippableList;

        consumableNameData = Vars.UserData.consumableNameList;
        consumableNumData = Vars.UserData.consumableNumList;
    }
    public SaveDataV1(SaveDatabase save) : this()
    {
    }

    public override SaveDatabase VersionUp()
    {
        return new SaveDataV2(this);
    }
}

public class SaveDataV2 : SaveDataV1
{
    public string stringData { get; set; } = string.Empty;
    public SaveDataV2()
    {
        Version = 2;

        characterData = Vars.UserData.characterList;
        equippableData = Vars.UserData.equippableList;

        consumableNameData = Vars.UserData.consumableNameList;
        consumableNumData = Vars.UserData.consumableNumList;
    }
    public SaveDataV2(SaveDataV1 save) : this()
    {
    }
    public override SaveDatabase VersionUp()
    {
        return null;
    }
}
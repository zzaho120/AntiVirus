using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTableElem : DataTableElemBase
{
	//public string iconId;
	//public string profileId;
	//public string prefabId;
	public string name;
	public string description;
	public float level;
	public float hp;
	public float damage;
	public float range;
	public float crit_rate;
	public int willpower;
	public int stamina;
	public int resistance;
	//private Sprite iconSprite;
	//private LevelTableElem levelTableElem;

	//public Sprite IconSprite
	//{
	//	get { return iconSprite; }
	//}

	//private Sprite profileSprite;

	//public Sprite ProfileSprite
	//{
	//	get { return profileSprite; }
	//}
	public CharacterTableElem(Dictionary<string, string> data)
	{
		id = data["ID"];
		//iconId = data["ICON_ID"];
		//profileId = data["PROFILE_ID"];
		//prefabId = data["PREFAB_ID"];
		name = data["NAME"];
		description = data["DESC"];
		level = float.Parse(data["LEVEL"]);
		hp = float.Parse(data["HP"]);
		damage = float.Parse(data["DAMAGE"]);
		range = float.Parse(data["RANGE"]);
		crit_rate = float.Parse(data["CRIT_RATE"]);
		willpower = int.Parse(data["WILLPOWER"]);
		stamina = int.Parse(data["STAMINA"]);
		resistance = int.Parse(data["RESISTANCE"]);

		//iconSprite = Resources.Load<Sprite>($"Sprites/Characters/Icons/{iconId}");
		//profileSprite = Resources.Load<Sprite>($"Sprites/Characters/Profiles/{profileId}");
	}
}



public class CharacterTable : DataTableBase
{
    public CharacterTable()
    {
        csvFilePath = @"Choi\CharacterDataTable";
    }
    public override void Load()
    {
        data.Clear();
        var list = CSVReader.Read(csvFilePath);
        foreach (var line in list)
        {
            var elem = new CharacterTableElem(line);
            data.Add(elem.id, elem);
        }
    }
}

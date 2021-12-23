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
	public int min_Hp;
	public int max_Hp;
	public int damage;
	public int range;
	public float crit_rate;
	public int min_Willpower;
	public int max_Willpower;
	public int min_Stamina;
	public int max_Stamina;
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
		name			= data["NAME"];
		description		= data["DESC"];
		min_Hp			= int.Parse(data["MIN_HP"]);
		max_Hp			= int.Parse(data["MAX_HP"]);
		damage			= int.Parse(data["DAMAGE"]);
		range			= int.Parse(data["RANGE"]);
		crit_rate		= float.Parse(data["CRIT_RATE"]);
		min_Willpower	= int.Parse(data["MIN_WILLPOWER"]);
		max_Willpower	= int.Parse(data["MAX_WILLPOWER"]);
		min_Stamina		= int.Parse(data["MIN_STAMINA"]);
		max_Stamina		= int.Parse(data["MAX_STAMINA"]);

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

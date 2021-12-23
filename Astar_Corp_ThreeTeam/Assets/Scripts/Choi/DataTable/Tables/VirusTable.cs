using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusTableElem : DataTableElemBase
{
	//public string iconId;
	//public string profileId;
	//public string prefabId;
	public int level;
	public string name;
	public int damage;
	public int hp;
	public int stamina;
	public int exp;
	public int range;
	public string debuffs;
	public int extraSkills;
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

	public VirusTableElem(Dictionary<string, string> data)
	{
		id = data["ID"];
		//iconId = data["ICON_ID"];
		//profileId = data["PROFILE_ID"];
		//prefabId = data["PREFAB_ID"];
		name		= data["NAME"];
		level		= int.Parse(data["LEVEL"]);
		damage		= int.Parse(data["DAMAGE"]);
		hp			= int.Parse(data["HP"]);
		stamina		= int.Parse(data["STAMINA"]);
		exp			= int.Parse(data["EXP"]);
		range		= int.Parse(data["RANGE"]);
		debuffs		= data["DEBUFFS"];
		//extraSkills = int.Parse(data["EXTRASKILLS"]);

		//iconSprite = Resources.Load<Sprite>($"Sprites/Characters/Icons/{iconId}");
		//profileSprite = Resources.Load<Sprite>($"Sprites/Characters/Profiles/{profileId}");
	}
}



public class VirusTable : DataTableBase
{
	public VirusTable()
	{
		csvFilePath = @"Choi\VirusDataTable";
	}
	public override void Load()
	{
		data.Clear();
		var list = CSVReader.Read(csvFilePath);
		foreach (var line in list)
		{
			var elem = new VirusTableElem(line);
			data.Add(elem.id, elem);
		}
	}
}

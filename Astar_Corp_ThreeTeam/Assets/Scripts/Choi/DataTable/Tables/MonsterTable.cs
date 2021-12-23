using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTableElem : DataTableElemBase
{
	public string name;
	public int damage;
	public int hp;
	public int stamina;
	public int range;

	//public Sprite IconSprite
	//{
	//	get { return iconSprite; }
	//}

	//private Sprite profileSprite;

	//public Sprite ProfileSprite
	//{
	//	get { return profileSprite; }
	//}

	public MonsterTableElem(Dictionary<string, string> data)
	{
		id		= data["ID"];
		name	= data["NAME"];
		damage	= int.Parse(data["DAMAGE"]);
		hp		= int.Parse(data["HP"]);
		stamina = int.Parse(data["STAMINA"]);
		range	= int.Parse(data["RANGE"]);

		//iconSprite = Resources.Load<Sprite>($"Sprites/Characters/Icons/{iconId}");
		//profileSprite = Resources.Load<Sprite>($"Sprites/Characters/Profiles/{profileId}");
	}
}



public class MonsterTable : DataTableBase
{
	public MonsterTable()
	{
		csvFilePath = @"Choi\MonsterDataTable";
	}
	public override void Load()
	{
		data.Clear();
		var list = CSVReader.Read(csvFilePath);
		foreach (var line in list)
		{
			var elem = new MonsterTableElem(line);
			data.Add(elem.id, elem);
		}
	}
}

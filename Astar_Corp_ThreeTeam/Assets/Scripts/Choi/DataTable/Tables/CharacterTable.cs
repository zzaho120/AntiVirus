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

	// ·¾¾÷ ½Ã ½ºÅÈ »ó½Â·ü
	public int min_Hp_Increase;
	public int max_Hp_Increase;
	public float stamina_Increase;
	public int willpower_Increase;
	public int damage_Increase;

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
		// Ä³¸¯ÅÍ ±âº» Á¤º¸
		id = data["ID"];
		//iconId = data["ICON_ID"];
		//profileId = data["PROFILE_ID"];
		//prefabId = data["PREFAB_ID"];
		name			= data["NAME"];
		description		= data["DESC"];

		// ½ºÅÈµé
		min_Hp			= int.Parse(data["MIN_HP"]);
		max_Hp			= int.Parse(data["MAX_HP"]);
		damage			= int.Parse(data["DAMAGE"]);
		range			= int.Parse(data["RANGE"]);
		crit_rate		= float.Parse(data["CRIT_RATE"]);
		min_Willpower	= int.Parse(data["MIN_WILLPOWER"]);
		max_Willpower	= int.Parse(data["MAX_WILLPOWER"]);
		min_Stamina		= int.Parse(data["MIN_STAMINA"]);
		max_Stamina		= int.Parse(data["MAX_STAMINA"]);

		// ·¾¾÷ ½Ã Áõ°¡ ½ºÅÈ
		min_Hp_Increase		= int.Parse(data["HP_MIN_RISE"]);
		max_Hp_Increase		= int.Parse(data["HP_MAX_RISE"]);
		stamina_Increase	= float.Parse(data["STAMINA_RISE"]);
		willpower_Increase	= int.Parse(data["WILLPOWER_RISE"]);
		damage_Increase		= int.Parse(data["DAMAGE_RISE"]);

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

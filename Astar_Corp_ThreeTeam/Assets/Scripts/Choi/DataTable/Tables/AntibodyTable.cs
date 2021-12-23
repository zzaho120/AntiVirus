using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntibodyTableElem : DataTableElemBase
{
	public int level;
	public string name;
	public int demandedExp;			// ���� �ʿ� ����ġ
	public float hitDmgDecRate;		// �ǰ� ���� ������
	public float virusSkillResist;	// Ư���ɷ� ���׷�
	public float virusDmgDecRate;	// Ư���ɷ� ���� ������
	public float suddenDmgDecRate;	// �޽� ���� ������
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

	public AntibodyTableElem(Dictionary<string, string> data)
	{
		id				 = data["ID"];
		name			 = data["NAME"];
		level			 = int.Parse(data["LEVEL"]);
		demandedExp		 = int.Parse(data["EXP"]);
		hitDmgDecRate	 = float.Parse(data["HIT_DMG_DEC_RATE"]);
		virusSkillResist = float.Parse(data["VIRUS_SKILL_RESIST"]);
		virusDmgDecRate  = float.Parse(data["VIRUS_DMG_DEC_RATE"]);
		suddenDmgDecRate = float.Parse(data["SUDDEN_DMG_DEC_RATE"]);

		//iconSprite = Resources.Load<Sprite>($"Sprites/Characters/Icons/{iconId}");
		//profileSprite = Resources.Load<Sprite>($"Sprites/Characters/Profiles/{profileId}");
	}
}



public class AntibodyTable : DataTableBase
{
	public AntibodyTable()
	{
		csvFilePath = @"Choi\AntibodyDataTable";
	}
	public override void Load()
	{
		data.Clear();
		var list = CSVReader.Read(csvFilePath);
		foreach (var line in list)
		{
			var elem = new AntibodyTableElem(line);
			data.Add(elem.id, elem);
		}
	}
}

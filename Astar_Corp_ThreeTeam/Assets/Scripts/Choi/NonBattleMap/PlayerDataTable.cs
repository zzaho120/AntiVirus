using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataTableElem : DataTableElemBase
{
	public string name;
	public int hp;
	public int offensePower;
	public int accuracy;
	public int willPower;
	public int stamina;

	// ���� �� ���� ��·�
	//public int min_Hp_Increase;
	//public int max_Hp_Increase;
	//public float stamina_Increase;
	//public int willpower_Increase;
	//public int damage_Increase;

	public PlayerDataTableElem(Dictionary<string, string> data)
	{
		// ĳ���� �⺻ ����
		id = data["ID"];
		name = data["Name"];
		
		// ���ȵ�
		hp = int.Parse(data["Hp"]);
		offensePower = int.Parse(data["OffensePower"]);
		accuracy = int.Parse(data["Accuracy"]);
		willPower = int.Parse(data["WillPower"]);
		stamina = int.Parse(data["Stamina"]);

		// ���� �� ���� ����
		//min_Hp_Increase = int.Parse(data["HP_MIN_RISE"]);
		//max_Hp_Increase = int.Parse(data["HP_MAX_RISE"]);
		//stamina_Increase = float.Parse(data["STAMINA_RISE"]);
		//willpower_Increase = int.Parse(data["WILLPOWER_RISE"]);
		//damage_Increase = int.Parse(data["DAMAGE_RISE"]);
	}
}


public class PlayerDataTable : DataTableBase
{
	public PlayerDataTable()
	{
		csvFilePath = @"Choi\PlayerData\PlayerData";
	}
	public override void Load()
	{
		data.Clear();
		var list = CSVReader.Read(csvFilePath);
		foreach (var line in list)
		{
			PlayerDataTableElem elem = new PlayerDataTableElem(line);
			data.Add(elem.id, elem);
		}
	}
}

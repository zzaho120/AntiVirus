using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject
{
	public string id;
	public string iconId;
	public string profileId;
	public string prefabId;
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

	public int min_Hp_Increase;
	public int max_Hp_Increase;
	public float stamina_Increase;
	public int willpower_Increase;
	public int damage_Increase;
}

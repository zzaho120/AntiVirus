using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : ScriptableObject
{
	public string id;
	public string name;
	public int level;
	public int damage;
	public int hp;
	public int stamina;
	public int exp;
	public int range;
	public List<string> debuffs = new List<string>();
	//public int extraSkills;
}

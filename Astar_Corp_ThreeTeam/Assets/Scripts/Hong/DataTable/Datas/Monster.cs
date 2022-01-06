using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ScriptableObject
{
	public string id;
	public string name;

	public int min_Hp;
	public int max_Hp;
	public int ap;
	public int closeUpAtk_Ap;
	public int min_Dmg;
	public int max_Dmg;
	public int min_CritRate;
	public int max_CritRate;
	public int critDmg;
	public int exp;
}

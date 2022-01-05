using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : ScriptableObject
{
	// �⺻ ����
	public string id;
	public string name;

	// ĳ���� �г�Ƽ
	public int penaltyType;
	public int hp_Dec;
	public int stat_Dec;

	// ���� ���� ���
	public int hp;
	public int ap;
	public int damage;
	public int crit_Rate;
	public int crit_Dmg;

	public int exp;
	public int resistCharge;
	public int resistDec;
}

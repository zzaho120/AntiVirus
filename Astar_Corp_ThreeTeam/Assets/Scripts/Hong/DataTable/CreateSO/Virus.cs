using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : ScriptableObject
{
	// 기본 정보
	public string id;
	public string name;

	// 캐릭터 패널티
	public int penaltyType;
	public int hp_Dec;
	public int stat_Dec;

	// 몬스터 스탯 상승
	public int hp;
	public int ap;
	public int damage;
	public int crit_Rate;
	public int crit_Dmg;

	public int exp;
	public int resistCharge;
	public int resistDec;
}

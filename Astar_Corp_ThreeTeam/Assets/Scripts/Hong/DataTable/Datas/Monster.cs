using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ScriptableObject
{
	public string id;
	public string name;

	public int min_Hp;			// 최소 HP
	public int max_Hp;          // 최대 HP

	public int atkRange;		// 공격 가능 거리 (타일)
	public int ap;              // 보유 AP
	public int mp;				// 무게에 따른 추가 MP
	public int closeUpAtk_Ap;	// 근접공격 소모 AP

	public int min_Dmg;			// 최소 데미지
	public int max_Dmg;			// 최대 데미지
	public int min_CritRate;	// 최소 크확
	public int max_CritRate;	// 최대 크확
	public int critDmg;			// 크뎀

	public int exp;             // 보유 경험치
	public int sightRange;      // 인식 범위 (칸)
	public int virusGauge;		// 렙당 바이러스 게이지 증가량 (바이러스 레벨당 누적)
}

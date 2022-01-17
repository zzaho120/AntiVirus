using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ScriptableObject
{
	public string id;
	public new string name;

	public int minHp;			// 최소 HP
	public int maxHp;			// 최대 HP

	public int atkRange;		// 공격 가능 거리 (타일)
	public int ap;              // 보유 AP
	public int mp;				// 무게에 따른 추가 MP
	public int closeUpAtkAp;	// 근접공격 소모 AP

	public int minDmg;			// 최소 데미지
	public int maxDmg;			// 최대 데미지
	public int minCritRate;		// 최소 크확
	public int maxCritRate;		// 최대 크확
	public int critDmg;         // 크뎀
	public int critResist;		// 크리티컬 저항력

	public int exp;             // 보유 경험치
	public int sightRange;      // 인식 범위 (칸)
	public int virusGauge;      // 렙당 바이러스 게이지 증가량 (바이러스 레벨당 누적)
	public int escapeHpDec;		// 도망AI시 필요 체력 감소량 ?
}

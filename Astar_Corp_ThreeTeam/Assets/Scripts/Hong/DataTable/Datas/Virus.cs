using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : ScriptableObject
{
	// 기본 정보
	public string id;
	public string name;

	// 캐릭터 패널티
	public int penaltyType;		// 패널티 타입
	public int hp_Dec;			// 체력 감소
	public int stat_Dec;		// 레벨별 패널티 양

	// 몬스터 스탯 상승
	public int hp;				// 레벨당 체력
	public int ap;				// 레벨당 AP
	public int damage;			// 레벨당 데미지
	public int crit_Rate;		// 레벨당 크확
	public int crit_Dmg;		// 레벨당 크뎀

	public int exp;				// 레벨당 캐릭터 경험치량
	public int virusCharge;		// 바이러스 게이지 증가량
}

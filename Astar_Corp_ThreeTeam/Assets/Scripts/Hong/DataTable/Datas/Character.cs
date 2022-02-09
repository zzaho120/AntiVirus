using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject
{
	// 캐릭터 ID
	public string id;
	public string iconId;
	public string profileId;
	public string prefabId;

	// 기본 정보
	public new string name;
	public string type;
	public string description;

	// 건강
	public int minHealth;			// 최소 건강
	public int maxHealth;			// 최대 건강
	public int healthChance;		// 렙업 시 선택될 확률
	public int helathRise;			// 선택될 시 증가하는 건강
	public int critResistRateRise;	// (건강 2포인트당) 크리티컬 저항률 증가

	// HP
	public int minHp;				// 최소 hp
	public int maxHp;               // 최대 hp
	public int hpRise;              // 선택될 시 증가하는 hp

	// 무게 초과 시 패널티
	public int mpPenalty1;			// 100 ~ 150 % 초과
	public int mpPenalty2;			// 150 ~ 200 % 초과
	public int mpPenalty3;			// 200% ~ 초과

	// 예민함
	public int minSensitivity;		// 최소 예민함
	public int maxSensitivity;		// 최대 예민함
	public int senChance;			// 렙업 시 선택될 확률
	public int senRise;				// 선택될 시 증가하는 스탯 수치

	// 회피율
	public int minAvoidRate;		// 최소 회피율
	public int maxAvoidRate;		// 최대 회피율
	public int avoidRateRisePerSen;	// 1포인트당 회피율 증가량 (예민함 1포인트당 증가하는 회피율 스탯)
	// 10, 20 포인트 때 증가하는 MP양 +1

	// 집중력
	public int minConcentration;	// 최소 집중력
	public int maxConcentration;	// 최대 집중력
	public int concentrationChance;	// 렙업 시 선택될 확률
	public int concentrationRise;	// 선택될 시 증가하는 스탯 수치
	public int accurRatePerCon;     // 집중력 1포인트당 명중률 증가량
	// 10, 20 포인트 때 증가하는 시야 범위 +1

	// 정신력
	public int minWillpower;		// 최소 정신력
	public int maxWillpower;		// 최대 정신력
	public int willChance;			// 렙업 시 선택될 확률
	public int willRise;            // 선택될 시 증가하는 수치

	public int firePenaltyDec;		// 2포인트당 연속사격 패널티 감소
	public int critRateRise;        // 3포인트당 크확 증가
	
	// 치명타 확률
	// = 내 크리티컬 확률 - 상대 크리티컬 저항률

	//public int alertAccurRateRise;	// 3포인트당 경계명중률 증가량 --> 없어짐

	// 구매 비용?
	public int minCharCost;			// 용병 최소 구매 비용
	public int maxCharCost;			// 용병 최대 구매 비용

	// 게이지
	//public int resistGauge;        // 레벨당 내성 게이지 바	--> DB 따로 생김
	public int virusDec_Lev0;		// 내성 0렙일 때 바이러스 게이지 감소량
	public int virusDec_Lev1;		// 내성 1렙당 바이러스 게이지 감소량 (렙업 시 누적)

	// 사용가능 무기
	public List<string> weapons = new List<string>();

	// 보유 스킬 리스트
	public List<string> skillA = new List<string>();
	public List<string> skillB = new List<string>();
	public List<string> skillC = new List<string>();
	public List<string> skillD = new List<string>();

	public Sprite fullImg;
	public Sprite halfImg;
	public Sprite icon;
}

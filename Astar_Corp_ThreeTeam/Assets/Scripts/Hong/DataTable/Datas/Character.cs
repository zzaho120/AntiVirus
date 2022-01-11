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
	public string name;
	public string type;
	public string description;

	// HP
	public int min_Hp;				// 최소 HP
	public int max_Hp;				// 최대 HP
	public int min_Hp_Rise;			// 레벨당 최소 성장 HP
	public int max_Hp_Rise;			// 레벨당 최대 성장 HP

	// 무게
	public int weight;				// 들 수 있는 최소 무게
	public int weight_Rise;			// 레벨업당 추가되는 무게

	// 예민성? 민감도?
	public int min_Sensitivity;		// 최소 예민함
	public int max_Sensitivity;		// 최대 예민함
	public int min_Sen_Rise;		// 레벨당 최소 성장 예민함
	public int max_Sen_Rise;		// 레벨당 최대 성장 예민함

	// 회피율
	public int min_Avoid_Rate;		// 최소 회피율
	public int max_Avoid_Rate;		// 최대 회피율

	// 집중력
	public int min_Concentration;	// 최소 집중력
	public int max_Concentration;	// 최대 집중력
	public int min_Con_Rise;		// 레벨당 최소 성장 집중력
	public int max_Con_Rise;		// 레벨당 최대 성장 집중력

	// 정신력
	public int min_Willpower;		// 최소 정신력
	public int max_Willpower;		// 최대 정신력
	public int min_Will_Rise;		// 레벨당 최소 성장 정신력
	public int max_Will_Rise;		// 레벨당 최대 성장 정신력

	// 치명타 확률
	public int min_Crit_Rate;		// 최소 크확
	public int max_Crit_Rate;		// 최대 크확

	// 가격?
	public int min_Char_Cost;		// 용병 최소 구매 비용
	public int max_Char_Cost;		// 용병 최대 구매 비용

	// 게이지
	public int exp;					// 레벨당 경험치 통 (렙업 시 누적)
	public int resistance;          // 레벨당 내성 게이지 바 (렙업 시 누적)
	public int virusDec;			// 내성 1렙당 바이러스 게이지 감소량

	// 사용가능 무기
	public List<string> weapons = new List<string>();
}

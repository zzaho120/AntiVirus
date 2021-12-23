using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antibody : ScriptableObject
{
	public string id;
	public string name;
	public int level;
	public int demandedExp;         // 렙업 필요 경험치
	public float hitDmgDecRate;     // 피격 피해 감소율
	public float virusSkillResist;  // 특수능력 저항력
	public float virusDmgDecRate;   // 특수능력 피해 감소율
	public float suddenDmgDecRate;  // 급습 피해 감소율
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : ScriptableObject
{
	// �⺻ ����
	public string id;
	public string name;

	// ĳ���� �г�Ƽ
	public int penaltyType;		// �г�Ƽ Ÿ��
	public int hp_Dec;			// ü�� ����
	public int stat_Dec;		// ������ �г�Ƽ ��

	// ���� ���� ���
	public int hp;				// ������ ü��
	public int ap;				// ������ AP
	public int damage;			// ������ ������
	public int crit_Rate;		// ������ ũȮ
	public int crit_Dmg;		// ������ ũ��

	public int exp;				// ������ ĳ���� ����ġ��
	public int virusCharge;		// ���̷��� ������ ������
}

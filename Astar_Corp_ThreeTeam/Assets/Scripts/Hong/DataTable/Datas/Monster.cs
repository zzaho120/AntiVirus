using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ScriptableObject
{
	public string id;
	public string name;

	public int min_Hp;			// �ּ� HP
	public int max_Hp;          // �ִ� HP

	public int atkRange;		// ���� ���� �Ÿ� (Ÿ��)
	public int ap;              // ���� AP
	public int mp;				// ���Կ� ���� �߰� MP
	public int closeUpAtk_Ap;	// �������� �Ҹ� AP

	public int min_Dmg;			// �ּ� ������
	public int max_Dmg;			// �ִ� ������
	public int min_CritRate;	// �ּ� ũȮ
	public int max_CritRate;	// �ִ� ũȮ
	public int critDmg;			// ũ��

	public int exp;             // ���� ����ġ
	public int sightRange;      // �ν� ���� (ĭ)
	public int virusGauge;		// ���� ���̷��� ������ ������ (���̷��� ������ ����)
}

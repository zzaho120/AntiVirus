using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ScriptableObject
{
	public string id;
	public new string name;

	public int minHp;			// �ּ� HP
	public int maxHp;			// �ִ� HP

	public int atkRange;		// ���� ���� �Ÿ� (Ÿ��)
	public int ap;              // ���� AP
	public int mp;				// ���Կ� ���� �߰� MP
	public int closeUpAtkAp;	// �������� �Ҹ� AP

	public int minDmg;			// �ּ� ������
	public int maxDmg;			// �ִ� ������
	public int minCritRate;		// �ּ� ũȮ
	public int maxCritRate;		// �ִ� ũȮ
	public int critDmg;         // ũ��
	public int critResist;		// ũ��Ƽ�� ���׷�

	public int exp;             // ���� ����ġ
	public int sightRange;      // �ν� ���� (ĭ)
	public int virusGauge;      // ���� ���̷��� ������ ������ (���̷��� ������ ����)
	public int escapeHpDec;     // ����AI�� �ʿ� ü�� ���ҷ� ?

	/// <summary>
	/// �� ���� �־ string������ ����. ���߿� int.Parse �ؼ� ������ּ���
	/// </summary>
	public string dropItem1;		// ��� ������ 1
	public string item1Rate;		// ������1 ��� Ȯ��
	public string dropItem2;		// ��� ������ 2
	public string item2Rate;       // ������2 ��� Ȯ��
	public string dropItem3;		// ��� ������ 3
	public string item3Rate;		// ������3 ��� Ȯ��
}

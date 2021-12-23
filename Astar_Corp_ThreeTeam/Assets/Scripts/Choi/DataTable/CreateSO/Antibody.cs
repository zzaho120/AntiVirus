using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antibody : ScriptableObject
{
	public string id;
	public string name;
	public int level;
	public int demandedExp;         // ���� �ʿ� ����ġ
	public float hitDmgDecRate;     // �ǰ� ���� ������
	public float virusSkillResist;  // Ư���ɷ� ���׷�
	public float virusDmgDecRate;   // Ư���ɷ� ���� ������
	public float suddenDmgDecRate;  // �޽� ���� ������
}

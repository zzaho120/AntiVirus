using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. ü��
// 2. ���ݷ�
// 3. �а��� ���� ����/��ȿ �Ÿ�
// 4. ������
// 5. ���
// 6. ����

// ���⿡ ���� ���� ���� ���� ����

public class CharacterStats : MonoBehaviour
{
    // 1. ü��
    public int maxHp;
    public int currentHp;

    // 2. ���ݷ�, 3. ���ݹ��� --> AttackStats, AttackDefition Ŭ�������� �߰� ����
    public float baseDamage;
    
    // 4. ������
    public int willpower;

    // 5. ���
    public int stamina;

    // 6. ����
    public float resistance;

    // ����ġ?
    // public float Exp;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        currentHp = maxHp;
    }
}

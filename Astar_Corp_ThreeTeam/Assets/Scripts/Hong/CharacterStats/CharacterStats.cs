using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ĳ���� �⺻ ����
// 1. ü��
// 2. ���ݷ�
// 3. �а��� ���� ����/��ȿ �Ÿ�
// 4. ������
// 5. ���

// ����(��ü) ���� ����
// 6. �ǰ� ���� ������
// 7. Ư���ɷ� ���׷�
// 8. Ư���ɷ� ���� ������
// 9. �޽� ���� ������

public class CharacterStats : MonoBehaviour
{
    public Character characterStat;
    public Antibody antibody;

    // 1. ü��
    public int maxHp;
    public int currentHp;

    // 2. ���ݷ�, 3. ���ݹ��� --> AttackStats, AttackDefition Ŭ�������� �߰� ����
    public float Damage;
    
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack : MonoBehaviour
{
    private CharacterStats stats;     // Old version
    public MonsterStats enemyStats;

    //public Character newStatTest;
    //public AttackDefinition attackStat;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();

        enemyStats = GetComponent<MonsterStats>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Enemy Hp : " + enemyStats.currentHp);
        }

        // ���콺�� Enemy ������Ʈ Ŭ�� �� Attack �ǵ��� Test
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    // Enemy ������Ʈ�� CharacterStats �ӽ÷� ��������
                    enemyStats = hitInfo.collider.gameObject.GetComponent<MonsterStats>();

                    // ����
                    OnAttack(stats);
                }
            }
        }
    }

    //public void OnAttack(GameObject attacker, AttackStats attack)
    public void OnAttack(CharacterStats attack)
    {
        enemyStats.currentHp -= attack.weapon.Damage;

        if (enemyStats.currentHp < 0)
        {
            enemyStats.currentHp = 0;
            Debug.Log("Enemy died");
        }
        else
        {
            Debug.Log("Enemy HP : " + enemyStats.currentHp);
        }

    }
}
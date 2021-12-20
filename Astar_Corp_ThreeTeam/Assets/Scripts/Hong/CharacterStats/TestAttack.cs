using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack : MonoBehaviour
{
    //private CharacterStats stats;     // Old version
    private CharacterStats enemyStats;

    public Character newStatTest;
    public AttackDefinition attackStat;

    private void Awake()
    {
        //stats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        // 마우스로 Enemy 오브젝트 클릭 시 Attack 되도록 Test
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.name == "Enemy")
                {
                    // Enemy 오브젝트의 CharacterStats 임시로 가져오기
                    enemyStats = hitInfo.collider.gameObject.GetComponent<CharacterStats>();
                    
                    //var playerAttackStats = attackStat.CreateAttack(newStatTest.);
                    var playerAttackStats = attackStat.CreateAttack(newStatTest);
                    OnAttack(hitInfo.collider.gameObject, playerAttackStats);
                }
            }
        }
    }

    public void OnAttack(GameObject attacker, AttackStats attack)
    {
        enemyStats.currentHp -= attack.Damage;

        if (enemyStats.currentHp < 0)
        {
            enemyStats.currentHp = 0;
        }

        Debug.Log("Enemy HP : " + enemyStats.currentHp);
    }
}

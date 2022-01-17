using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    [HideInInspector]
    public CharacterStats characterStats;

    private GameObject levelUpSwitch;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();


//    �������������������������������������� ������ ����� ����������������������������������������������

        levelUpSwitch = GameObject.Find("LevelUp Switch");
        
        // UI�Ŵ��� �������� �۵�
        if (SceneManager.GetActiveScene().name == "UIManager")
        {
            if (levelUpSwitch != null)
                levelUpSwitch.SetActive(false);
        }
        else
        {
            Debug.Log("���� ������ ���Ұ�");
        }
//    ����������������������������������������������������������������������������������������������������������������
    }

    private void LevelUp()
    {
        if (characterStats.level < 10)
        {
            characterStats.level += 1;

            int HpIncrease = Random.Range(characterStats.character.maxAvoidRate, characterStats.character.minConcentration + 1);
            //Debug.Log("Hp ��·� : " + HpIncrease);
            
            characterStats.MaxHp += HpIncrease;
            //characterStats.willpower += characterStats.character.min_Con_Rise;
            //characterStats.stamina += characterStats.character.max_Concentration;
        }
        else
        {
            Debug.Log("������");
        }
    }

    public void OnGUI()
    {
        Vector2 screenSize = Camera.main.ViewportToScreenPoint(new Vector3(1, 1));

        if (GUI.Button(new Rect(screenSize.x - 100, screenSize.y - 30, 100, 30), "Level up"))
        {
            if (levelUpSwitch.activeInHierarchy)
            {
                LevelUp();
            }
            else
            {
                Debug.Log("���� ������ ���Ұ�");
            }
        }
    }
}

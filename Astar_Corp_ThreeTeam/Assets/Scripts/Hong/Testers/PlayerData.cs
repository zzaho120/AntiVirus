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


//    ┌────────────────── 레벨업 실험용 ──────────────────────┐

        levelUpSwitch = GameObject.Find("LevelUp Switch");
        
        // UI매니저 씬에서만 작동
        if (SceneManager.GetActiveScene().name == "UIManager")
        {
            if (levelUpSwitch != null)
                levelUpSwitch.SetActive(false);
        }
        else
        {
            Debug.Log("현재 씬에서 사용불가");
        }
//    └──────────────────────────────────────────────────────┘
    }

    private void LevelUp()
    {
        if (characterStats.level < 10)
        {
            characterStats.level += 1;

            int HpIncrease = Random.Range(characterStats.character.maxAvoidRate, characterStats.character.minConcentration + 1);
            //Debug.Log("Hp 상승량 : " + HpIncrease);
            
            characterStats.MaxHp += HpIncrease;
            //characterStats.willpower += characterStats.character.min_Con_Rise;
            //characterStats.stamina += characterStats.character.max_Concentration;
        }
        else
        {
            Debug.Log("만렙임");
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
                Debug.Log("현재 씬에서 사용불가");
            }
        }
    }
}

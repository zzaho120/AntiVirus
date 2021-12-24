using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [HideInInspector]
    public CharacterStats characterInfo;

    private void Start()
    {
        characterInfo = GetComponent<CharacterStats>();
    }

    private void LevelUp()
    {
        //Debug.Log("Level up");

        if (characterInfo.level < 10)
        {
            characterInfo.level += 1;

            //characterInfo.maxHp += Random.Range(characterInfo.characterStat.min_Hp_Increase, characterInfo.characterStat.max_Hp_Increase + 1);
            characterInfo.willpower += characterInfo.characterStat.willpower_Increase;
            characterInfo.stamina += characterInfo.characterStat.stamina_Increase;
        }
        else
        {
            Debug.Log("¸¸·¾ÀÓ");
        }
    }

    public void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 30), "Level up"))
        {
            LevelUp();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public CharacterStats characterInfo;

    private void Start()
    {
        characterInfo = GetComponent<CharacterStats>();
    }
}

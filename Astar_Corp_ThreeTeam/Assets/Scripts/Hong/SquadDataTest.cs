using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquadDataTest : MonoBehaviour
{
    private PlayerDataMgr playerData;
    public TextMeshProUGUI[] text;

    void Start()
    {
        playerData = GameObject.Find("PlayerDataMgr").GetComponent<PlayerDataMgr>();

        for (int i = 0; i < playerData.currentSquad.Count; i++)
        {
            if (playerData.battleSquad.ContainsKey(i))
            {
                if (text[0] != null)
                    text[0].text = playerData.battleSquad[i].Name;

                if (text[1] != null)
                    text[1].text = playerData.battleSquad[i].Name;

                if (text[2] != null)
                    text[2].text = playerData.battleSquad[i].Name;

                if (text[3] != null)
                    text[3].text = playerData.battleSquad[i].Name;
            }
        }
    }
}

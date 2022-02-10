using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMemberExtraInfo : MonoBehaviour
{
    public GameObject virusObj;
    public GameObject alertAttackCountObj;
    public PlayerableChar player;

    public void Init(PlayerableChar player)
    {
        this.player = player;
        UpdateExtraInfo();
    }

    public void UpdateExtraInfo()
    {
        UpdateVirus();
        UpdateAlertAttackCount();
    }
    public void UpdateVirus()
    {
        var virusPenalty = player.characterStats.virusPenalty;

        var childCount = virusObj.transform.childCount;
        for (var idx = 0; idx < childCount; ++idx)
        {
            var child = virusObj.transform.GetChild(idx);
            child.gameObject.SetActive(false);
        }

        foreach (var pair in virusPenalty)
        {
            var virus = pair.Value;
            if (virus.penaltyLevel > 0)
            {
                switch (pair.Key)
                {
                    case "E":
                        virusObj.transform.GetChild(0).gameObject.SetActive(true);
                        break;
                    case "B":
                        virusObj.transform.GetChild(1).gameObject.SetActive(true);
                        break;
                    case "P":
                        virusObj.transform.GetChild(2).gameObject.SetActive(true);
                        break;
                    case "I":
                        virusObj.transform.GetChild(3).gameObject.SetActive(true);
                        break;
                    case "T":
                        virusObj.transform.GetChild(4).gameObject.SetActive(true);
                        break;
                }
            }
        }
    }

    public void UpdateAlertAttackCount()
    {
        var childCount = alertAttackCountObj.transform.childCount;
        for (var idx = 0; idx < childCount; ++idx)
        {
            var child = alertAttackCountObj.transform.GetChild(idx);

            if (player.attackCount > idx)
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);

        }
    }
}

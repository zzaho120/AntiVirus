using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMemberPanel : MonoBehaviour
{
    private BattleBasicWindow parent;
    private PlayerableChar player;
    public Image memberImage;
    public Image classImage;
    public Slider hpBar;
    public GameObject APObj;

    public void Init(PlayerableChar player, BattleBasicWindow basicWindow)
    {
        parent = basicWindow;
        this.player = player;

        var stats = player.characterStats;
        memberImage.sprite = stats.character.halfImg;
        classImage.sprite = stats.character.icon;

        UpdateUI();
    }

    public void OnClickPanel()
    {
        if (player.status != CharacterState.TurnEnd && player.status != CharacterState.Alert && parent.isTurn)
        {
            parent.SetSelectedChar(player);
            CameraController.Instance.SetCameraTrs(player.transform);
            Debug.Log(player.transform.position);
        }
    }

    public void UpdateUI()
    {
        var stats = player.characterStats;
        hpBar.value = stats.currentHp / (float)stats.MaxHp;

        var childCount = APObj.transform.childCount;
        for (var idx = 0; idx < childCount; ++idx)
        {
            var child = APObj.transform.GetChild(idx).gameObject;
            if (idx < player.AP)
                child.SetActive(true);
            else
                child.SetActive(false);
        }
    }
}

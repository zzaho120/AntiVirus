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

    public void Init(PlayerableChar player, List<Sprite> classSprites, BattleBasicWindow basicWindow)
    {
        parent = basicWindow;
        this.player = player;

        // memberImage.sprite
        var stats = player.characterStats;
        switch (stats.character.type)
        {
            case "Tanker":
                classImage.sprite = classSprites[3];
            break;
            case "Healer":
                classImage.sprite = classSprites[1];
            break;
            case "Sniper":
                classImage.sprite = classSprites[2];
            break;
            case "Bombardier":
                classImage.sprite = classSprites[4];
            break;
            case "Scout":
                classImage.sprite = classSprites[0];
            break;
        }

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

    public void OnClickPanel()
    {
        parent.SetSelectedChar(player);
        CameraController.Instance.SetCameraTrs(player.transform);
        Debug.Log(player.transform.position);
    }
}

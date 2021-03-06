using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMonsterPanel : MonoBehaviour
{
    public Image monsterImage;
    public TextMeshProUGUI accuracyText;
    public MonsterChar monster;
    public BattleBasicWindow parent;
    public void Init(PlayerableChar player, MonsterChar monster, List<Sprite> monsterSprite, BattleBasicWindow parent)
    {
        this.parent = parent;
        this.monster = monster;
        
        switch (monster.monsterStats.monster.name)
        {
            case "Bear":
                monsterImage.sprite = monsterSprite[0];
                break;

            case "Boar":
                monsterImage.sprite = monsterSprite[1];
                break;

            case "Wolf":
                monsterImage.sprite = monsterSprite[2];
                break;

            case "Spider":
                monsterImage.sprite = monsterSprite[3];
                break;

            case "Jaguar":
                monsterImage.sprite = monsterSprite[4];
                break;

            case "Tiger":
                monsterImage.sprite = monsterSprite[5];
                break;

            case "Fox":
                monsterImage.sprite = monsterSprite[6];
                break;
        }

        var stats = player.characterStats;
        var weapon = stats.weapon;

        var buffValue = 0;
        if (weapon.curWeapon.kind == "6")
        {
            var moveSRList = stats.buffMgr.GetBuffList(Stat.MoveSRAccuracy);
            foreach (var buff in moveSRList)
            {
                buffValue += (int)buff.GetAmount();
            }
        }

        accuracyText.text = $"{Mathf.Clamp(weapon.GetAttackAccuracy(monster.currentTile.accuracy) + stats.accuracy + buffValue, 0, 100)}%";
    }

    public void OnClickMonsterPanel()
    {
        parent.targetMonster = monster;
        CameraController.Instance.SetCameraTrs(monster.transform);
    }
}

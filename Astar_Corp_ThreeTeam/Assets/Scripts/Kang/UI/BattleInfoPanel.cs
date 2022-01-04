using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class BattleInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;
    public void SetInfoPlayer(PlayerableChar player)
    {
        var stats = player.characterStats;

        nameText.text = stats.Name;

        var sb = new StringBuilder();

        sb.Append($"Level : {stats.level}\n");
        sb.Append($"HP : {stats.currentHp}\n");
        sb.Append($"AP : {player.AP}\n");
        sb.Append($"bullet : {stats.weapon.mainWeaponBullet}");

        infoText.text = sb.ToString();
    }

    public void SetInfoMonster(MonsterChar monster)
    {
        var stats = monster.monsterStats;
        nameText.text = stats.name;

        var sb = new StringBuilder();

        sb.Append($"HP : {stats.currentHp}\n");
    }
}

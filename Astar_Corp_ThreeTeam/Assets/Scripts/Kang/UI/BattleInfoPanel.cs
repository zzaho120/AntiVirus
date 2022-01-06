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
        sb.Append($"AP : {player.characterStats.weapon.curWeapon.name}\n");
        sb.Append($"Bullet : {stats.weapon.MainWeaponBullet}\n");

        foreach (var elem in stats.virusPanalty)
        {
            sb.Append($"virus {elem.Key} : Lv.{elem.Value.penaltyLevel} / {elem.Value.penaltyGauge}\n");
        }

        infoText.text = sb.ToString();
    }

    public void SetInfoMonster(MonsterChar monster, WeaponStats weapon)
    {
        var stats = monster.monsterStats;
        nameText.text = stats.name;

        var sb = new StringBuilder();

        sb.Append($"HP : {stats.currentHp}\n");
        sb.Append($"Virus {stats.virus.name} : Lv.{stats.virusLevel} / {stats.virus}\n");
        sb.Append($"Accuracy : {weapon.CalCulateAccuracy(monster.currentTile.accuracy, weapon.AccurRate_base)}%");


        infoText.text = sb.ToString();
    }
}

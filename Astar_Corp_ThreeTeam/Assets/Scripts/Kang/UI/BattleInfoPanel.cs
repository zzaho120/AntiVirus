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
        sb.Append($"Wp : {player.characterStats.weapon.curWeapon.name}\n");
        sb.Append($"Bullet : {stats.weapon.MainWeaponBullet}\n\n");

        foreach (var elem in stats.virusPenalty)
        {
            sb.Append($"Virus {elem.Key} : Lv.{elem.Value.penaltyLevel} / {elem.Value.penaltyGauge}\n");
            sb.Append($"Reduction {elem.Key} : Lv.{elem.Value.reductionLevel} / {elem.Value.reductionGauge}\n\n");

            if (elem.Key.Equals("B"))
                break;
        }

        sb.Append($"level : {stats.level}\n");
        sb.Append($"Exp : {stats.currentExp} / {stats.totalExp}");

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

    public void SetInfoMonster(MonsterChar monster)
    {
        var stats = monster.monsterStats;
        nameText.text = stats.name;

        var sb = new StringBuilder();

        sb.Append($"HP : {stats.currentHp}\n");
        sb.Append($"Virus {stats.virus.name} : Lv.{stats.virusLevel} / {stats.virus}\n");
        sb.Append($"MinDmg~MaxDmg {stats.virus.name} : {stats.monster.minDmg} / {stats.monster.maxDmg}\n");

        infoText.text = sb.ToString();
    }
}

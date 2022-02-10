using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleFirePanel : MonoBehaviour
{
    public TextMeshProUGUI dmgText;
    public TextMeshProUGUI critDmgText;

    public void SetDmgText(PlayerableChar player)
    {
        var weapon = player.characterStats.weapon.curWeapon;

        dmgText.text = $"{weapon.minDamage}~{weapon.maxDamage}";
        critDmgText.text = $"{weapon.minDamage * weapon.critDamage / 100f}~{weapon.maxDamage * weapon.critDamage / 100f}";
    }
}

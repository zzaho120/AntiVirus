using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Melee,
    SmallGun,
    BigGun
}

public class BattleWeapon : MonoBehaviour
{
    public WeaponName weaponName;
    public WeaponType WeaponType;
    public Transform fireTr;
}

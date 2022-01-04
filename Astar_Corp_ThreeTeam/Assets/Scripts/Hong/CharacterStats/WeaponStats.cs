using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    public enum WeaponType
    {
        Main,
        Sub
    }

    public WeaponType type;

    public Weapon mainWeapon;
    public Weapon subWeapon;

    // 명중률
    [HideInInspector]
    public int accurRate_base;

    [HideInInspector]
    public float accurRate_alert
    {
        get => accurRate_base * 0.7f;
    }

    // 데미지
    [HideInInspector] 
    public int damage { get => SetWeaponDamage(); }

    // 크리티컬 데미지
    [HideInInspector]
    public int critDmg;

    // 주무기 불릿
    [HideInInspector] 
    public int mainWeaponBullet
    {
        get => mainWeapon.bullet;
        set => mainWeapon.bullet = value;
    }
    // 보조무기 불릿
    [HideInInspector] 
    public int subWeaponBullet
    {
        get => subWeapon.bullet;
        set => subWeapon.bullet = value;
    }

    [HideInInspector]
    public int WeaponBullet
    {
        get
        {
            switch (type)
            {
                case WeaponType.Main:
                    return mainWeapon.bullet;
                case WeaponType.Sub:
                    return subWeapon.bullet;
            }
            return 0;
        }

        set
        {
            switch (type)
            {
                case WeaponType.Main:
                    mainWeapon.bullet = value;
                    break;
                case WeaponType.Sub:
                    subWeapon.bullet = value;
                    break;
            }
        }
    }
    // 명중률 감소
    [HideInInspector] 
    public int accurRateDec;

    // 무게
    [HideInInspector] 
    public int weight;

    // 발사 시 Ap 소모량
    [HideInInspector] 
    public int firstShotAp, alertShotAp, aimShotAp, loadAp;

    // 사거리, 패널티
    [HideInInspector] 
    public int range, overRange_penalty, underRange_penalty;

    public int SetWeaponDamage()
    {
        if (mainWeapon != null && subWeapon != null)
        {
            // 무기 잘못 장착 시
            if (mainWeapon.type != "1") Debug.LogError("주무기 잘못 장착");
            if (subWeapon.type != "2") Debug.LogError("보조무기 잘못 장착");

            if (type == WeaponType.Main)
            {
                // 데미지
                var damage = Random.Range(mainWeapon.min_damage, mainWeapon.max_damage + 1);
                //this.damage = damage;
                return damage;
            }
            else if (type == WeaponType.Sub)
            {
                var damage = Random.Range(subWeapon.min_damage, subWeapon.max_damage + 1);
                //this.damage = damage;
                return damage;
            }
            else
            {
                Debug.LogError("무기 타입 오류");
                return 0;
            }
        }
        else
        {
            Debug.LogError("무기 장착해");
            return 0;
        }
    }

    public void Init()
    {
        if (mainWeapon != null && subWeapon != null)
        {
            // 무기 잘못 장착 시
            if (mainWeapon.type != "1") Debug.LogError("주무기 잘못 장착");
            if (subWeapon.type != "2") Debug.LogError("보조무기 잘못 장착");

            // 주무기 장착 시
            if (type == WeaponType.Main)
            {
                // 명중률
                accurRate_base = mainWeapon.accur_Rate_Base;

                // 데미지
                //var damage = Random.Range(mainWeapon.min_damage, mainWeapon.max_damage + 1);
                //this.damage = damage;

                // 크뎀
                critDmg = mainWeapon.crit_Damage;

                // 탄알
                //mainWeaponBullet = mainWeapon.bullet;

                // 명중률 감소
                accurRateDec = mainWeapon.accur_Rate_Dec;

                // 무게
                weight = mainWeapon.weight;

                // 소모AP
                firstShotAp = mainWeapon.firstShot_Ap;
                alertShotAp = mainWeapon.alertShot_Ap;
                aimShotAp = mainWeapon.aimShot_Ap;
                loadAp = mainWeapon.load_Ap;

                // 범위
                range = mainWeapon.range;
                overRange_penalty = mainWeapon.overRange_Penalty;
                underRange_penalty = mainWeapon.underRange_Penalty;
            }
            // 보조무기 장착 시
            else if (type == WeaponType.Sub)
            {
                // 명중률
                accurRate_base = subWeapon.accur_Rate_Base;

                // 데미지
                //var damage = Random.Range(subWeapon.min_damage, subWeapon.max_damage + 1);
                //this.damage = damage;

                // 크뎀
                critDmg = subWeapon.crit_Damage;

                // 탄알
                //subWeaponBullet = subWeapon.bullet;

                // 명중률 감소
                accurRateDec = subWeapon.accur_Rate_Dec;

                // 무게
                weight = subWeapon.weight;

                // 소모AP
                firstShotAp = subWeapon.firstShot_Ap;
                alertShotAp = subWeapon.alertShot_Ap;
                aimShotAp = subWeapon.aimShot_Ap;
                loadAp = subWeapon.load_Ap;

                // 범위
                range = subWeapon.range;
                overRange_penalty = subWeapon.overRange_Penalty;
                underRange_penalty = subWeapon.underRange_Penalty;
            }
            // 탄알 개수

        }
        else
        {
            Debug.LogError("무기 장착해");
            //return 0;
        }
    }


    // 기존
    //private int damage;
    //public int Damage { get => damage; }
    //
    //private bool critical;
    //public bool Critical { get => critical; }
    //
    //public AttackStats(int damage, bool critical)
    //{
    //    this.damage = damage;
    //    this.critical = critical;
    //}
}

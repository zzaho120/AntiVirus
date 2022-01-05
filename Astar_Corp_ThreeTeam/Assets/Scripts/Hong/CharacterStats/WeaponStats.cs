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
    public int AccurRate_base;

    [HideInInspector]
    public float AccurRate_alert
    {
        get => AccurRate_base * 0.7f;
    }

    // 데미지
    [HideInInspector] 
    public int Damage { get => SetWeaponDamage(); }

    // 크리티컬 데미지
    [HideInInspector]
    public int CritDmg;

    // 탄창 클립 수
    [HideInInspector] 
    public int MainWeaponBullet, SubWeaponBullet;

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
    public int AccurRateDec;

    // 무게
    [HideInInspector] 
    public int Weight;

    // 발사 시 Ap 소모량
    [HideInInspector] 
    public int FirstShotAp, AlertShotAp, AimShotAp, LoadAp;

    // 사거리, 패널티
    [HideInInspector] 
    public int Range, OverRange_Penalty, UnderRange_Penalty;

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
                AccurRate_base = mainWeapon.accur_Rate_Base;

                // 데미지
                //var damage = Random.Range(mainWeapon.min_damage, mainWeapon.max_damage + 1);
                //this.damage = damage;

                // 크뎀
                CritDmg = mainWeapon.crit_Damage;

                // 탄알
                MainWeaponBullet = mainWeapon.bullet;

                // 명중률 감소
                AccurRateDec = mainWeapon.accur_Rate_Dec;

                // 무게
                Weight = mainWeapon.weight;

                // 소모AP
                FirstShotAp = mainWeapon.firstShot_Ap;
                AlertShotAp = mainWeapon.alertShot_Ap;
                AimShotAp = mainWeapon.aimShot_Ap;
                LoadAp = mainWeapon.load_Ap;

                // 범위
                Range = mainWeapon.range;
                OverRange_Penalty = mainWeapon.overRange_Penalty;
                UnderRange_Penalty = mainWeapon.underRange_Penalty;
            }
            // 보조무기 장착 시
            else if (type == WeaponType.Sub)
            {
                // 명중률
                AccurRate_base = subWeapon.accur_Rate_Base;

                // 데미지
                //var damage = Random.Range(subWeapon.min_damage, subWeapon.max_damage + 1);
                //this.damage = damage;

                // 크뎀
                CritDmg = subWeapon.crit_Damage;

                // 탄알
                SubWeaponBullet = subWeapon.bullet;

                // 명중률 감소
                AccurRateDec = subWeapon.accur_Rate_Dec;

                // 무게
                Weight = subWeapon.weight;

                // 소모AP
                FirstShotAp = subWeapon.firstShot_Ap;
                AlertShotAp = subWeapon.alertShot_Ap;
                AimShotAp = subWeapon.aimShot_Ap;
                LoadAp = subWeapon.load_Ap;

                // 범위
                Range = subWeapon.range;
                OverRange_Penalty = subWeapon.overRange_Penalty;
                UnderRange_Penalty = subWeapon.underRange_Penalty;
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

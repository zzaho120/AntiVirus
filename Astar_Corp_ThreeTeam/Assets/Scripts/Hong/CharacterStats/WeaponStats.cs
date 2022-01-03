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

    [HideInInspector] public int accurRate_base;
    [HideInInspector] public int accurRate_alert;

    [HideInInspector] public int damage { get => SetWeaponDamage(); } 

    [HideInInspector] public int mainWeaponBullet;
    [HideInInspector] public int subWeaponBullet;

    [HideInInspector] public int recoil;

    [HideInInspector] public int accurRateDec;

    [HideInInspector] public int weight;

    [HideInInspector] public int firstAp;
    [HideInInspector] public int nextAp;
    [HideInInspector] public int loadAp;

    [HideInInspector] public int range;
    [HideInInspector] public int overRange_penalty;
    [HideInInspector] public int underRange_penalty;

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
                accurRate_alert = mainWeapon.accur_Rate_Alert;

                // 데미지
                //var damage = Random.Range(mainWeapon.min_damage, mainWeapon.max_damage + 1);
                //this.damage = damage;

                // 탄알
                mainWeaponBullet = mainWeapon.bullet;

                // 반동
                recoil = mainWeapon.recoil;

                // 명중률 감소
                accurRateDec = mainWeapon.accur_Rate_Dec;

                // 무게
                weight = mainWeapon.weight;

                // 소모AP
                firstAp = mainWeapon.firstAp;
                nextAp = mainWeapon.nextAp;
                loadAp = mainWeapon.loadAp;

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
                accurRate_alert = subWeapon.accur_Rate_Alert;

                // 데미지
                //var damage = Random.Range(subWeapon.min_damage, subWeapon.max_damage + 1);
                //this.damage = damage;

                // 탄알
                subWeaponBullet = subWeapon.bullet;

                // 반동
                recoil = subWeapon.recoil;

                // 명중률 감소
                accurRateDec = subWeapon.accur_Rate_Dec;

                // 무게
                weight = subWeapon.weight;

                // 소모AP
                firstAp = subWeapon.firstAp;
                nextAp = subWeapon.nextAp;
                loadAp = subWeapon.loadAp;

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

        //this.damage = damage;
        //Debug.Log(this.damage);
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

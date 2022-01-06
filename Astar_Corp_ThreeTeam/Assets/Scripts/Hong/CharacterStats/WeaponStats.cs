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
    
    public Weapon curWeapon
    {
        get
        {
            switch (type)
            {
                case WeaponType.Main:
                    return mainWeapon;
                case WeaponType.Sub:
                    return subWeapon;
            }
            return null;
        }
    }

    // 사격 횟수
    [HideInInspector]
    public int fireCount;

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
                    return MainWeaponBullet;
                case WeaponType.Sub:
                    return SubWeaponBullet;
            }
            return 0;
        }
        set
        {
            switch (type)
            {
                case WeaponType.Main:
                    MainWeaponBullet = value;
                    break;
                case WeaponType.Sub:
                    SubWeaponBullet = value;
                    break;
            }
        }
    }

    public bool CheckAvailBullet
    {
        get => WeaponBullet > 0;
    }

    // 명중률 감소
    [HideInInspector] 
    public int AccurRate_dec;

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
                return damage;
            }
            else if (type == WeaponType.Sub)
            {
                var damage = Random.Range(subWeapon.min_damage, subWeapon.max_damage + 1);
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

                // 크뎀
                CritDmg = mainWeapon.crit_Damage;

                // 탄알
                MainWeaponBullet = mainWeapon.bullet;

                // 명중률 감소
                AccurRate_dec = mainWeapon.accur_Rate_Dec;

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
                AccurRate_dec = subWeapon.accur_Rate_Dec;

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

        }
        else
        {
            Debug.LogError("무기 장착해");
        }
    }
    public void StartTurn()
    {
        fireCount = 0;
    }

    public int Reload()
    {
        WeaponBullet = curWeapon.bullet;
        return LoadAp;
    }

    public bool CheckAttackAccuracy(int accuracy)
    {
        var totalAccuracy = CalCulateAccuracy(accuracy, AccurRate_base);
        var result = Random.Range(0, 100) < totalAccuracy;
        fireCount++;
        WeaponBullet--;
        return result;
    }

    public bool CheckAlertAccuracy(int accuracy)
    {
        var totalAccuracy = CalCulateAccuracy(accuracy, (int)(AccurRate_alert - (AccurRate_dec * fireCount)));
        var result = Random.Range(0, 100) < totalAccuracy;
        fireCount++;
        return result;
    }

    public int CalCulateAccuracy(int accuracy, int accurRate)
    {
        var totalAccuracy = 0;
        switch (Range)
        {
            case 1: // 근거리
                if (0 < accuracy && accuracy < 5)
                    totalAccuracy = accurRate;
                else
                    totalAccuracy = accurRate - 30 * (accuracy - 4);
                break;

            case 2: // 중거리
                if (3 < accuracy && accuracy < 8)
                    totalAccuracy = accurRate;
                else if (accuracy < 4)
                    totalAccuracy = accurRate - 10 * (4 - accuracy);
                else if (accuracy > 7)
                    totalAccuracy = accurRate - 15 * (accuracy - 8);
                break;

            case 3: // 원거리
                if (6 < accuracy && accuracy < 11)
                    totalAccuracy = accurRate;
                else if (accuracy < 7)
                    totalAccuracy = accurRate - 15 * (7 - accuracy);
                else if (accuracy > 10)
                    totalAccuracy = accurRate - 10 * (accuracy - 10);
                break;

            case 4: // 근접무기
                if (1 == accuracy)
                    totalAccuracy = accurRate;
                else
                    totalAccuracy = 0;
                break;
        }
        return totalAccuracy;
    }

    public bool CheckAvailShot(int AP, PlayerState state)
    {
        var result = false;

        if (fireCount == 0)
        {
            if ((AP - FirstShotAp) >= 0)
                result = true;
        }
        else
        {
            switch (state)
            {
                case PlayerState.Attack:
                    if ((AP - AimShotAp) >= 0)
                        result = true;
                    break;
                case PlayerState.Alert:
                    if ((AP - AlertShotAp) >= 0)
                        result = true;
                    break;
            }
        }
        return result;
    }

    public int GetWeaponAP(PlayerState state)
    {
        var result = 0;
        if (fireCount == 0)
            result = FirstShotAp;
        else
        {
            switch (state)
            {
                case PlayerState.Attack:
                    result = AimShotAp;
                    break;
                case PlayerState.Alert:
                    result = AlertShotAp;
                    break;
            }
        }
        return result;
    }

    public bool CheckReloadAP(int AP)
    {
        if ((AP - LoadAp) >= 0)
            return true;

        return false;
    }

}
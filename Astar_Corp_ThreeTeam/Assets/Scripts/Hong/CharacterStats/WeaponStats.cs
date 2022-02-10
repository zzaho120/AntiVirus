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

    public Weapon otherWeapon
    {
        get
        {
            switch (type)
            {
                case WeaponType.Main:
                    return subWeapon;
                case WeaponType.Sub:
                    return mainWeapon;
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
    public int CritDmg
    {
        get
        {
            var critDmg = 0;
            switch (type)
            {
                case WeaponType.Main:
                    critDmg = mainWeapon.critDamage;
                    break;
                case WeaponType.Sub:
                    critDmg = subWeapon.critDamage;
                    break;
            }
            return critDmg;
        }
    }

    // 크리티컬 확률
    [HideInInspector]
    public int CritRate
    {
        get
        {
            var critRate = 0;
            switch (type)
            {
                case WeaponType.Main:
                    critRate = mainWeapon.critRate;
                    break;
                case WeaponType.Sub:
                    critRate = subWeapon.critRate;
                    break;
            }
            return critRate;
        }
    }

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

    public int OtherBullet
    {
        get
        {
            switch (type)
            {
                case WeaponType.Main:
                    return SubWeaponBullet;
                case WeaponType.Sub:
                    return MainWeaponBullet;
            }
            return 0;
        }
    }

    public bool CheckAvailBullet
    {
        get => WeaponBullet > 0;
    }

    // 홍수진_스탯삭제
    // 명중률 감소
    //public int AccurRate_dec;

    // 홍수진_스탯...첨언
    // 기존 "무게 (Weight)" -> 현재 "1 AP당 제공 MP" 항목으로 바뀜
    public int MpPerAp;

    // 홍수진_스탯수정
    // 발사 시 Ap 소모량
    [HideInInspector] 
    public int FirstShotAp, OtherShotAp, LoadAp; // AlertShotAp, AimShotAp
    // 무기DB
    // FirstShotAp : 사격 첫발시 소모되는 AP
    // OtherShotAp : 사격 첫발 이후 소모되는 AP

    // 홍수진_스탯수정
    // 사거리, 패널티
    [HideInInspector] 
    public int MinRange, MaxRange, OverRange_Penalty, UnderRange_Penalty;

    // 데미지 구하기
    public int SetWeaponDamage()
    {
        if (mainWeapon != null && subWeapon != null)
        {
            // 무기 잘못 장착 시
            //if (mainWeapon.type != "1") Debug.LogError("주무기 잘못 장착");
            //if (subWeapon.type != "2") Debug.LogError("보조무기 잘못 장착");

            if (type == WeaponType.Main)
            {
                // 데미지
                var damage = Random.Range(mainWeapon.minDamage, mainWeapon.maxDamage + 1);
                return damage;
            }
            else if (type == WeaponType.Sub)
            {
                var damage = Random.Range(subWeapon.minDamage, subWeapon.maxDamage + 1);
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

    // 스탯 초기화
    public void Init()
    {
        if (mainWeapon != null || subWeapon != null)
        {
            // 무기 잘못 장착 시
            //if (mainWeapon.type != "1") Debug.LogError("주무기 잘못 장착");
            //if (subWeapon.type != "2") Debug.LogError("보조무기 잘못 장착");

            // 주무기 장착 시
            if (type == WeaponType.Main)
            {
                // 명중률
                AccurRate_base = mainWeapon.accurRateBase;

                // 탄알
                MainWeaponBullet = mainWeapon.bullet;

                // 홍수진_스탯삭제
                // 명중률 감소
                //AccurRate_dec = mainWeapon.accur_Rate_Dec;
                // 무게
                //Weight = mainWeapon.weight;

                // 1 AP당 제공되는 MP
                MpPerAp = mainWeapon.mpPerAp;

                // 홍수진_스탯수정
                // 소모AP
                FirstShotAp = mainWeapon.firstShotAp;
                OtherShotAp = mainWeapon.otherShotAp;
                //AlertShotAp = mainWeapon.otherShotAp;
                LoadAp = mainWeapon.loadAp;

                // 홍수진_스탯수정
                // 범위
                MinRange = mainWeapon.minRange;
                MaxRange = mainWeapon.maxRange;
                OverRange_Penalty = mainWeapon.overRange_Penalty;
                UnderRange_Penalty = mainWeapon.underRange_Penalty;
            }
            // 보조무기 장착 시
            else if (type == WeaponType.Sub && subWeapon != null)
            {
                // 명중률
                AccurRate_base = subWeapon.accurRateBase;

                // 탄알
                SubWeaponBullet = subWeapon.bullet;

                // 1 AP당 제공되는 MP
                MpPerAp = subWeapon.mpPerAp;

                // 홍수진_스탯수정
                // 소모AP
                FirstShotAp = subWeapon.firstShotAp;
                OtherShotAp = subWeapon.otherShotAp;
                LoadAp = subWeapon.loadAp;

                // 홍수진_스탯수정
                // 범위
                MinRange = subWeapon.minRange;
                MaxRange = subWeapon.maxRange;
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

    public int GetAttackAccuracy(int accuracy)
    {
        var totalAccuracy = CalCulateAccuracy(accuracy, AccurRate_base);
        return totalAccuracy;
    }

    // 홍수진
    // 여기 스탯 수정 됐어요 !!!!!!!!!!!!!!!!!!!!!!!!! -> 좀 다르게 수정
    // \    /\
    //  )  ( ')
    // (  /  )
    //  \(__)|
    // =================
    // 고양이
    // 원래 최소, 최대 사거리가 없고 무기 기본 사거리만 하나 존재하고 있었는데
    // 새로 최소, 최대 사거리가 생겼어요
    // 근데 전투 시스템에 어떻게 적용되는지 잘 모르겠어서 걍 주석 처리할라니까 밑에 다 오류 뜰까봐
    // 임시로 swith문 부분을 Range -> MaxRange로 바꿔놨음 니다
    // 강주수 - 수정완료
    public int CalCulateAccuracy(int accuracy, int accurRate)
    {
        var totalAccuracy = 0;

        if (MinRange <= accuracy && accuracy <= MaxRange)
            totalAccuracy = accurRate;
        else if (accuracy < MinRange)
        {
            var difference = MinRange - accuracy;
            totalAccuracy = accurRate - UnderRange_Penalty * difference;
        }
        else if (MaxRange < accuracy)
        {
            var difference = accuracy - MaxRange;
            totalAccuracy = accurRate - OverRange_Penalty * difference;
        }
        return totalAccuracy;
    }


    // 홍수진
    // 여기 스탯 수정 됐어요 !!!!!!!!!!!!!!!!!!!!!!!!! -> 수정된 친구들 주석처리 함
    // \    /\
    //  )  ( ')
    // (  /  )
    //  \(__)|
    // =================
    // 고양이
    public bool CheckAvailShot(int AP, CharacterState state)
    {
        var result = false;

        if (fireCount == 0)
        {
            if ((AP - FirstShotAp) >= 0)
                result = true;
        }
        else
        {
            if ((AP - OtherShotAp) >= 0)
                result = true;
        }
        return result;
    }

    // 홍수진
    // 여기 스탯 수정 됐어요 !!!!!!!!!!!!!!!!!!!!!!!!! -> 수정된 친구들 주석처리 함
    // \    /\
    //  )  ( ')
    // (  /  )
    //  \(__)|
    // =================
    // 고양이
    public int GetWeaponAP()
    {
        var result = 0;
        if (fireCount == 0)
            result = FirstShotAp;
        else
            result = OtherShotAp;
        return result;
    }

    public bool CheckReloadAP(int AP)
    {
        if ((AP - LoadAp) >= 0)
            return true;

        return false;
    }

    public void ChangeWeapon()
    {
        if (type == WeaponType.Main)
            type = WeaponType.Sub;
        else if (type == WeaponType.Sub)
            type = WeaponType.Main;
    }
}
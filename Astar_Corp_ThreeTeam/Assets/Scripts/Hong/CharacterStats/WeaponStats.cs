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

    // ��� Ƚ��
    [HideInInspector]
    public int fireCount;

    // ���߷�
    [HideInInspector]
    public int AccurRate_base;

    [HideInInspector]
    public float AccurRate_alert
    {
        get => AccurRate_base * 0.7f;
    }

    // ������
    [HideInInspector] 
    public int Damage { get => SetWeaponDamage(); }

    // ũ��Ƽ�� ������
    [HideInInspector]
    public int CritDmg;

    // źâ Ŭ�� ��
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

    // ���߷� ����
    [HideInInspector] 
    public int AccurRate_dec;

    // ����
    [HideInInspector] 
    public int Weight;

    // �߻� �� Ap �Ҹ�
    [HideInInspector] 
    public int FirstShotAp, AlertShotAp, AimShotAp, LoadAp;

    // ��Ÿ�, �г�Ƽ
    [HideInInspector] 
    public int Range, OverRange_Penalty, UnderRange_Penalty;

    public int SetWeaponDamage()
    {
        if (mainWeapon != null && subWeapon != null)
        {
            // ���� �߸� ���� ��
            if (mainWeapon.type != "1") Debug.LogError("�ֹ��� �߸� ����");
            if (subWeapon.type != "2") Debug.LogError("�������� �߸� ����");

            if (type == WeaponType.Main)
            {
                // ������
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
                Debug.LogError("���� Ÿ�� ����");
                return 0;
            }
        }
        else
        {
            Debug.LogError("���� ������");
            return 0;
        }
    }

    public void Init()
    {
        if (mainWeapon != null && subWeapon != null)
        {
            // ���� �߸� ���� ��
            if (mainWeapon.type != "1") Debug.LogError("�ֹ��� �߸� ����");
            if (subWeapon.type != "2") Debug.LogError("�������� �߸� ����");

            // �ֹ��� ���� ��
            if (type == WeaponType.Main)
            {
                // ���߷�
                AccurRate_base = mainWeapon.accur_Rate_Base;

                // ũ��
                CritDmg = mainWeapon.crit_Damage;

                // ź��
                MainWeaponBullet = mainWeapon.bullet;

                // ���߷� ����
                AccurRate_dec = mainWeapon.accur_Rate_Dec;

                // ����
                Weight = mainWeapon.weight;

                // �Ҹ�AP
                FirstShotAp = mainWeapon.firstShot_Ap;
                AlertShotAp = mainWeapon.alertShot_Ap;
                AimShotAp = mainWeapon.aimShot_Ap;
                LoadAp = mainWeapon.load_Ap;

                // ����
                Range = mainWeapon.range;
                OverRange_Penalty = mainWeapon.overRange_Penalty;
                UnderRange_Penalty = mainWeapon.underRange_Penalty;
            }
            // �������� ���� ��
            else if (type == WeaponType.Sub)
            {
                // ���߷�
                AccurRate_base = subWeapon.accur_Rate_Base;

                // ������
                //var damage = Random.Range(subWeapon.min_damage, subWeapon.max_damage + 1);
                //this.damage = damage;

                // ũ��
                CritDmg = subWeapon.crit_Damage;

                // ź��
                SubWeaponBullet = subWeapon.bullet;

                // ���߷� ����
                AccurRate_dec = subWeapon.accur_Rate_Dec;

                // ����
                Weight = subWeapon.weight;

                // �Ҹ�AP
                FirstShotAp = subWeapon.firstShot_Ap;
                AlertShotAp = subWeapon.alertShot_Ap;
                AimShotAp = subWeapon.aimShot_Ap;
                LoadAp = subWeapon.load_Ap;

                // ����
                Range = subWeapon.range;
                OverRange_Penalty = subWeapon.overRange_Penalty;
                UnderRange_Penalty = subWeapon.underRange_Penalty;
            }

        }
        else
        {
            Debug.LogError("���� ������");
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
            case 1: // �ٰŸ�
                if (0 < accuracy && accuracy < 5)
                    totalAccuracy = accurRate;
                else
                    totalAccuracy = accurRate - 30 * (accuracy - 4);
                break;

            case 2: // �߰Ÿ�
                if (3 < accuracy && accuracy < 8)
                    totalAccuracy = accurRate;
                else if (accuracy < 4)
                    totalAccuracy = accurRate - 10 * (4 - accuracy);
                else if (accuracy > 7)
                    totalAccuracy = accurRate - 15 * (accuracy - 8);
                break;

            case 3: // ���Ÿ�
                if (6 < accuracy && accuracy < 11)
                    totalAccuracy = accurRate;
                else if (accuracy < 7)
                    totalAccuracy = accurRate - 15 * (7 - accuracy);
                else if (accuracy > 10)
                    totalAccuracy = accurRate - 10 * (accuracy - 10);
                break;

            case 4: // ��������
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
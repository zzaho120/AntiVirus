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

    // ũ��Ƽ�� Ȯ��
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

    // ȫ����_���Ȼ���
    // ���߷� ����
    //public int AccurRate_dec;

    // ȫ����_����...÷��
    // ���� "���� (Weight)" -> ���� "1 AP�� ���� MP" �׸����� �ٲ�
    public int MpPerAp;

    // ȫ����_���ȼ���
    // �߻� �� Ap �Ҹ�
    [HideInInspector] 
    public int FirstShotAp, OtherShotAp, LoadAp; // AlertShotAp, AimShotAp
    // ����DB
    // FirstShotAp : ��� ù�߽� �Ҹ�Ǵ� AP
    // OtherShotAp : ��� ù�� ���� �Ҹ�Ǵ� AP

    // ȫ����_���ȼ���
    // ��Ÿ�, �г�Ƽ
    [HideInInspector] 
    public int MinRange, MaxRange, OverRange_Penalty, UnderRange_Penalty;

    // ������ ���ϱ�
    public int SetWeaponDamage()
    {
        if (mainWeapon != null && subWeapon != null)
        {
            // ���� �߸� ���� ��
            //if (mainWeapon.type != "1") Debug.LogError("�ֹ��� �߸� ����");
            //if (subWeapon.type != "2") Debug.LogError("�������� �߸� ����");

            if (type == WeaponType.Main)
            {
                // ������
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

    // ���� �ʱ�ȭ
    public void Init()
    {
        if (mainWeapon != null || subWeapon != null)
        {
            // ���� �߸� ���� ��
            //if (mainWeapon.type != "1") Debug.LogError("�ֹ��� �߸� ����");
            //if (subWeapon.type != "2") Debug.LogError("�������� �߸� ����");

            // �ֹ��� ���� ��
            if (type == WeaponType.Main)
            {
                // ���߷�
                AccurRate_base = mainWeapon.accurRateBase;

                // ź��
                MainWeaponBullet = mainWeapon.bullet;

                // ȫ����_���Ȼ���
                // ���߷� ����
                //AccurRate_dec = mainWeapon.accur_Rate_Dec;
                // ����
                //Weight = mainWeapon.weight;

                // 1 AP�� �����Ǵ� MP
                MpPerAp = mainWeapon.mpPerAp;

                // ȫ����_���ȼ���
                // �Ҹ�AP
                FirstShotAp = mainWeapon.firstShotAp;
                OtherShotAp = mainWeapon.otherShotAp;
                //AlertShotAp = mainWeapon.otherShotAp;
                LoadAp = mainWeapon.loadAp;

                // ȫ����_���ȼ���
                // ����
                MinRange = mainWeapon.minRange;
                MaxRange = mainWeapon.maxRange;
                OverRange_Penalty = mainWeapon.overRange_Penalty;
                UnderRange_Penalty = mainWeapon.underRange_Penalty;
            }
            // �������� ���� ��
            else if (type == WeaponType.Sub && subWeapon != null)
            {
                // ���߷�
                AccurRate_base = subWeapon.accurRateBase;

                // ź��
                SubWeaponBullet = subWeapon.bullet;

                // 1 AP�� �����Ǵ� MP
                MpPerAp = subWeapon.mpPerAp;

                // ȫ����_���ȼ���
                // �Ҹ�AP
                FirstShotAp = subWeapon.firstShotAp;
                OtherShotAp = subWeapon.otherShotAp;
                LoadAp = subWeapon.loadAp;

                // ȫ����_���ȼ���
                // ����
                MinRange = subWeapon.minRange;
                MaxRange = subWeapon.maxRange;
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

    public int GetAttackAccuracy(int accuracy)
    {
        var totalAccuracy = CalCulateAccuracy(accuracy, AccurRate_base);
        return totalAccuracy;
    }

    // ȫ����
    // ���� ���� ���� �ƾ�� !!!!!!!!!!!!!!!!!!!!!!!!! -> �� �ٸ��� ����
    // \    /\
    //  )  ( ')
    // (  /  )
    //  \(__)|
    // =================
    // �����
    // ���� �ּ�, �ִ� ��Ÿ��� ���� ���� �⺻ ��Ÿ��� �ϳ� �����ϰ� �־��µ�
    // ���� �ּ�, �ִ� ��Ÿ��� ������
    // �ٵ� ���� �ý��ۿ� ��� ����Ǵ��� �� �𸣰ھ �� �ּ� ó���Ҷ�ϱ� �ؿ� �� ���� ����
    // �ӽ÷� swith�� �κ��� Range -> MaxRange�� �ٲ���� �ϴ�
    // ���ּ� - �����Ϸ�
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


    // ȫ����
    // ���� ���� ���� �ƾ�� !!!!!!!!!!!!!!!!!!!!!!!!! -> ������ ģ���� �ּ�ó�� ��
    // \    /\
    //  )  ( ')
    // (  /  )
    //  \(__)|
    // =================
    // �����
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

    // ȫ����
    // ���� ���� ���� �ƾ�� !!!!!!!!!!!!!!!!!!!!!!!!! -> ������ ģ���� �ּ�ó�� ��
    // \    /\
    //  )  ( ')
    // (  /  )
    //  \(__)|
    // =================
    // �����
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
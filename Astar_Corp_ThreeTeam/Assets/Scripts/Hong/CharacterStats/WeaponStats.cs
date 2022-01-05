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

    // ���߷� ����
    [HideInInspector] 
    public int AccurRateDec;

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

                // ������
                //var damage = Random.Range(mainWeapon.min_damage, mainWeapon.max_damage + 1);
                //this.damage = damage;

                // ũ��
                CritDmg = mainWeapon.crit_Damage;

                // ź��
                MainWeaponBullet = mainWeapon.bullet;

                // ���߷� ����
                AccurRateDec = mainWeapon.accur_Rate_Dec;

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
                AccurRateDec = subWeapon.accur_Rate_Dec;

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
            // ź�� ����

        }
        else
        {
            Debug.LogError("���� ������");
            //return 0;
        }
    }


    // ����
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

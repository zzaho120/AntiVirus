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
    public int accurRate_base;

    [HideInInspector]
    public float accurRate_alert
    {
        get => accurRate_base * 0.7f;
    }

    // ������
    [HideInInspector] 
    public int damage { get => SetWeaponDamage(); }

    // ũ��Ƽ�� ������
    [HideInInspector]
    public int critDmg;

    // �ֹ��� �Ҹ�
    [HideInInspector] 
    public int mainWeaponBullet
    {
        get => mainWeapon.bullet;
        set => mainWeapon.bullet = value;
    }
    // �������� �Ҹ�
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
    // ���߷� ����
    [HideInInspector] 
    public int accurRateDec;

    // ����
    [HideInInspector] 
    public int weight;

    // �߻� �� Ap �Ҹ�
    [HideInInspector] 
    public int firstShotAp, alertShotAp, aimShotAp, loadAp;

    // ��Ÿ�, �г�Ƽ
    [HideInInspector] 
    public int range, overRange_penalty, underRange_penalty;

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
                accurRate_base = mainWeapon.accur_Rate_Base;

                // ������
                //var damage = Random.Range(mainWeapon.min_damage, mainWeapon.max_damage + 1);
                //this.damage = damage;

                // ũ��
                critDmg = mainWeapon.crit_Damage;

                // ź��
                //mainWeaponBullet = mainWeapon.bullet;

                // ���߷� ����
                accurRateDec = mainWeapon.accur_Rate_Dec;

                // ����
                weight = mainWeapon.weight;

                // �Ҹ�AP
                firstShotAp = mainWeapon.firstShot_Ap;
                alertShotAp = mainWeapon.alertShot_Ap;
                aimShotAp = mainWeapon.aimShot_Ap;
                loadAp = mainWeapon.load_Ap;

                // ����
                range = mainWeapon.range;
                overRange_penalty = mainWeapon.overRange_Penalty;
                underRange_penalty = mainWeapon.underRange_Penalty;
            }
            // �������� ���� ��
            else if (type == WeaponType.Sub)
            {
                // ���߷�
                accurRate_base = subWeapon.accur_Rate_Base;

                // ������
                //var damage = Random.Range(subWeapon.min_damage, subWeapon.max_damage + 1);
                //this.damage = damage;

                // ũ��
                critDmg = subWeapon.crit_Damage;

                // ź��
                //subWeaponBullet = subWeapon.bullet;

                // ���߷� ����
                accurRateDec = subWeapon.accur_Rate_Dec;

                // ����
                weight = subWeapon.weight;

                // �Ҹ�AP
                firstShotAp = subWeapon.firstShot_Ap;
                alertShotAp = subWeapon.alertShot_Ap;
                aimShotAp = subWeapon.aimShot_Ap;
                loadAp = subWeapon.load_Ap;

                // ����
                range = subWeapon.range;
                overRange_penalty = subWeapon.overRange_Penalty;
                underRange_penalty = subWeapon.underRange_Penalty;
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

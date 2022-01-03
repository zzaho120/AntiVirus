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
                accurRate_alert = mainWeapon.accur_Rate_Alert;

                // ������
                //var damage = Random.Range(mainWeapon.min_damage, mainWeapon.max_damage + 1);
                //this.damage = damage;

                // ź��
                mainWeaponBullet = mainWeapon.bullet;

                // �ݵ�
                recoil = mainWeapon.recoil;

                // ���߷� ����
                accurRateDec = mainWeapon.accur_Rate_Dec;

                // ����
                weight = mainWeapon.weight;

                // �Ҹ�AP
                firstAp = mainWeapon.firstAp;
                nextAp = mainWeapon.nextAp;
                loadAp = mainWeapon.loadAp;

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
                accurRate_alert = subWeapon.accur_Rate_Alert;

                // ������
                //var damage = Random.Range(subWeapon.min_damage, subWeapon.max_damage + 1);
                //this.damage = damage;

                // ź��
                subWeaponBullet = subWeapon.bullet;

                // �ݵ�
                recoil = subWeapon.recoil;

                // ���߷� ����
                accurRateDec = subWeapon.accur_Rate_Dec;

                // ����
                weight = subWeapon.weight;

                // �Ҹ�AP
                firstAp = subWeapon.firstAp;
                nextAp = subWeapon.nextAp;
                loadAp = subWeapon.loadAp;

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

        //this.damage = damage;
        //Debug.Log(this.damage);
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

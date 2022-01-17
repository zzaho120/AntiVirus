using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusData : MonoBehaviour
{
    public bool None;
    public Dictionary<string, int> currentVirus = new Dictionary<string, int>();

    [HideInInspector] public bool B1;
    [HideInInspector] public bool B2;
    [HideInInspector] public bool B3;

    [HideInInspector] public bool E1;
    [HideInInspector] public bool E2;
    [HideInInspector] public bool E3;

    [HideInInspector] public bool I1;
    [HideInInspector] public bool I2;
    [HideInInspector] public bool I3;

    [HideInInspector] public bool P1;
    [HideInInspector] public bool P2;
    [HideInInspector] public bool P3;

    [HideInInspector] public bool T1;
    [HideInInspector] public bool T2;
    [HideInInspector] public bool T3;

    private void Start()
    {
        None = true;
        currentVirus.Add("B", 0);
        currentVirus.Add("E", 0);
        currentVirus.Add("I", 0);
        currentVirus.Add("P", 0);
        currentVirus.Add("T", 0);
    }

    public void Change()
    {
        foreach (var element in currentVirus)
        {
            if (element.Key.Equals("B"))
            {
                B1 = false;
                B2 = false;
                B3 = false;

                switch (currentVirus["B"])
                {
                    case 1:
                        B1 = true;
                        break;
                    case 2:
                        B2 = true;
                        break;
                    case 3:
                        B3 = true;
                        break;
                }
            }
            if (element.Key.Equals("E"))
            {
                E1 = false;
                E2 = false;
                E3 = false;

                switch (currentVirus["E"])
                {
                    case 1:
                        E1 = true;
                        break;
                    case 2:
                        E2 = true;
                        break;
                    case 3:
                        E3 = true;
                        break;
                }
            }
            if (element.Key.Equals("I"))
            {
                I1 = false;
                I2 = false;
                I3 = false;

                switch (currentVirus["I"])
                {
                    case 1:
                        I1 = true;
                        break;
                    case 2:
                        I2 = true;
                        break;
                    case 3:
                        I3 = true;
                        break;
                }
            }
            if (element.Key.Equals("P"))
            {
                P1 = false;
                P2 = false;
                P3 = false;

                switch (currentVirus["P"])
                {
                    case 1:
                        P1 = true;
                        break;
                    case 2:
                        P2 = true;
                        break;
                    case 3:
                        P3 = true;
                        break;
                }
            }
            if (element.Key.Equals("T"))
            {
                T1 = false;
                T2 = false;
                T3 = false;

                switch (currentVirus["T"])
                {
                    case 1:
                        T1 = true;
                        break;
                    case 2:
                        T2 = true;
                        break;
                    case 3:
                        T3 = true;
                        break;
                }
            }
        }
    }

    public void Init()
    {
        B1 = false;
        B2 = false;
        B3 = false;

        E1 = false;
        E2 = false;
        E3 = false;

        I1 = false;
        I2 = false;
        I3 = false;

        P1 = false;
        P2 = false;
        P3 = false;

        T1 = false;
        T2 = false;
        T3 = false;
    }
}

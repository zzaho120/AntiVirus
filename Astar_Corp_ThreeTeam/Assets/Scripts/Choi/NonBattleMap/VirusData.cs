using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusData : MonoBehaviour
{
    public bool None;
    public Dictionary<string, int> currentVirus = new Dictionary<string, int>();

    public bool B1;
    public bool B2;
    public bool B3;

    public bool E1;
    public bool E2;
    public bool E3;

    public bool I1;
    public bool I2;
    public bool I3;

    public bool P1;
    public bool P2;
    public bool P3;

    public bool T1;
    public bool T2;
    public bool T3;

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
        //if (None)
        //{
        //    B1 = false;
        //    B2 = false;
        //    B3 = false;

        //    E1 = false;
        //    E2 = false;
        //    E3 = false;

        //    I1 = false;
        //    I2 = false;
        //    I3 = false;
            
        //    P1 = false;
        //    P2 = false;
        //    P3 = false;

        //    T1 = false;
        //    T2 = false;
        //    T3 = false;
        //}
     
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
}

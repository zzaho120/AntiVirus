using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarCenterMgr : MonoBehaviour
{
    [HideInInspector]
    public PlayerDataMgr playerDataMgr;

    [Header("Truck Info")]
    public Image truckImg;
    public TextMeshProUGUI text;
    public Button left;
    public Button right;

    private string truckKey;
    private int keyNum;

    public void Init()
    {
        text.text = playerDataMgr.truckList["TRU_0001"].name;
        keyNum = 1;

        NextButton(3);
    }

    public void NextButton(int value)
    {
        ButtonInteractable();
        truckKey = "TRU_000" + (keyNum).ToString();

        if (value == 0)
        {
            keyNum -= 1;
        }
        else if (value == 1)
        {
            keyNum += 1;
        }
        else
        {
           
        }
        
        if (playerDataMgr.truckList.ContainsKey(truckKey))
        {
            text.text = playerDataMgr.truckList[truckKey].name;
        }
        else
        {
        }
        
    }

    private void ButtonInteractable()
    {
        if (keyNum <= 1)
        {
            left.interactable = false;
        }
        else
        {
            left.interactable = true;
            
        }

        if (keyNum >= playerDataMgr.truckList.Count)
        {
            right.interactable = false;
        }
        else
        {
            right.interactable = true;
        }
    }

    //public void Selec
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TruckStat
{ 
    None,
    Speed,
    Trunk,
    FieldOfView
}

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

    //ÁöÀº.
    public List<GameObject> buttonList;
    public List<GameObject> gaugeList;
    public Dictionary<int, Truck> truckList = new Dictionary<int, Truck>();
    Color originColor;
    Color selectedColor;
    TruckStat currentStat;
    int currentKey;

    public void Init()
    {
        int i = 0;
        foreach (var element in playerDataMgr.truckList)
        {
            int num = i;
            truckList.Add(num, element.Value);
            i++;
        }

        originColor = buttonList[0].GetComponent<Image>().color;
        selectedColor = new Color(56/255f, 87/255f, 35/255f);
        currentStat = TruckStat.None;
        
        currentKey = 0;
        text.text = truckList[currentKey].name;
        ButtonInteractable();
    }

    public void PreviousButton()
    {
        if (currentKey-1 < 0) return;

        currentKey--;
        ButtonInteractable();
        text.text = truckList[currentKey].name;
    }

    public void NextButton()
    {
        if (currentKey+1 >= truckList.Count) return;

        currentKey++;
        ButtonInteractable();
        text.text = truckList[currentKey].name;
    }

    private void ButtonInteractable()
    {
        left.interactable = (currentKey <= 0) ? false : true;
        right.interactable = (currentKey >= truckList.Count - 1) ? false : true;
    }

    public void SelectStat(int index)
    {
        if (currentStat != TruckStat.None)
        {
            int previousIndex = -1;
            switch (currentStat)
            {
                case TruckStat.Speed:
                    previousIndex = 0;
                    break;
                case TruckStat.Trunk:
                    previousIndex = 1;
                    break;
                case TruckStat.FieldOfView:
                    previousIndex = 2;
                    break;
            }

            buttonList[previousIndex].GetComponent<Image>().color = originColor;
            for (int i = 0; i < gaugeList[previousIndex].transform.childCount; i++)
            {
                var child = gaugeList[previousIndex].transform.GetChild(i).gameObject;
                child.GetComponent<Image>().color = originColor;
            }
        }

        switch (index)
        {
            case 0:
                currentStat = TruckStat.Speed;
                break;
            case 1:
                currentStat = TruckStat.Trunk;
                break;
            case 2:
                currentStat = TruckStat.FieldOfView;
                break;
        }
        buttonList[index].GetComponent<Image>().color = selectedColor;
        for (int i = 0; i < gaugeList[index].transform.childCount; i++)
        {
            var child = gaugeList[index].transform.GetChild(i).gameObject;
            child.GetComponent<Image>().color = selectedColor;
        }
    }

    public void Upgrage()
    {
        if (currentStat == TruckStat.None) return;


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTruck : MonoBehaviour
{
    private string camper, iceCream, prisionBus, armyTruck, Ambulance;

    private void Awake()
    {
        camper = "TRU_0001";
        iceCream = "TRU_0002";
        prisionBus = "TRU_0003";
        armyTruck = "TRU_0004";
        Ambulance = "TRU_0005";
    }

    private void OnEnable()
    {
        // 벙커 안 들렀다가 시작할때
        if (PlayerDataMgr.Instance.saveData.currentCar == null)
            PlayerDataMgr.Instance.saveData.currentCar = "TRU_0004";

        // Set Truck
        if (PlayerDataMgr.Instance.saveData.currentCar == camper)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (PlayerDataMgr.Instance.saveData.currentCar == iceCream)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (PlayerDataMgr.Instance.saveData.currentCar == prisionBus)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (PlayerDataMgr.Instance.saveData.currentCar == armyTruck)
        {
            transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (PlayerDataMgr.Instance.saveData.currentCar == Ambulance)
        {
            transform.GetChild(4).gameObject.SetActive(true);
        }
    }
}

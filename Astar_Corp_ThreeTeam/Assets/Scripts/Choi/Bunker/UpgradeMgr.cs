using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;

    public GameObject conWindow;
    public List<GameObject> bunkers;
    public GameObject prefab;

    BunkerMgr bunkerMgr;
    [HideInInspector]
    public Dictionary<int, GameObject> prefabs = new Dictionary<int, GameObject>();

    public void Init()
    {
        bunkerMgr = GameObject.Find("BunkerMgr").GetComponent<BunkerMgr>();

        //초기화.
        if (prefabs.Count != 0)
        {
            foreach (var element in prefabs)
            {
                Destroy(element.Value);
            }
            prefabs.Clear();
        }

        int i = 0;
        foreach (var element in bunkerMgr.bunkerObjs)
        {
            var bunkerBase = element.GetComponent<BunkerBase>();
            if (bunkerBase.bunkerName.Equals("None") || bunkerBase.bunkerName.Equals("Locked"))
            {
                i++;
                continue;
            }

            var go = Instantiate(prefab, conWindow.transform);
            go.transform.position = Camera.main.WorldToScreenPoint(element.transform.position);

            var child = go.transform.GetChild(0).gameObject;
            int num = i;

            int currentLevel = 0;
            switch (bunkerBase.bunkerName)
            {
                case "Agit":
                    currentLevel = playerDataMgr.saveData.agitLevel;
                    break;
                case "Storage":
                    currentLevel = playerDataMgr.saveData.storageLevel;
                    break;
                case "Garage":
                    currentLevel = playerDataMgr.saveData.garageLevel;
                    break;
                case "CarCenter":
                    currentLevel = playerDataMgr.saveData.carCenterLevel;
                    break;
                case "Hospital":
                    currentLevel = playerDataMgr.saveData.hospitalLevel;
                    break;
                case "Store":
                    currentLevel = playerDataMgr.saveData.storeLevel;
                    break;
                case "Pub":
                    currentLevel = playerDataMgr.saveData.pubLevel;
                    break;
            }
            child.GetComponent<Text>().text = $"현재레벨 : {currentLevel}";

            child = go.transform.GetChild(1).gameObject;
            int nextLevel = currentLevel + 1;
            child.GetComponent<Text>().text = (nextLevel != 6) ? $"다음레벨 :{nextLevel}" : "-";

            child = go.transform.GetChild(2).gameObject;
            var button = child.GetComponent<Button>();
            button.onClick.AddListener(delegate { Upgrade(bunkerBase.bunkerName, num); });
            if (currentLevel == 5)
            {
                child.transform.GetChild(0).gameObject.GetComponent<Text>().text = "-";
                button.interactable = false;
            }

            prefabs.Add(num, go);
            i++;
        }
    }

    public void Upgrade(string kind, int index)
    {
        //돈이 모자라면 리턴 구현해야됨.

        //제약사항.
        int currentLevel = 0;
        switch (kind)
        {
            case "Agit":
                currentLevel = playerDataMgr.saveData.agitLevel;
                break;
            case "Storage":
                currentLevel = playerDataMgr.saveData.storageLevel;
                break;
            case "Garage":
                currentLevel = playerDataMgr.saveData.garageLevel;
                break;
            case "CarCenter":
                currentLevel = playerDataMgr.saveData.carCenterLevel;
                break;
            case "Hospital":
                currentLevel = playerDataMgr.saveData.hospitalLevel;
                break;
            case "Store":
                currentLevel = playerDataMgr.saveData.storeLevel;
                break;
            case "Pub":
                currentLevel = playerDataMgr.saveData.pubLevel;
                break;
        }
        if (currentLevel == 5) return;

        //레벨업 구현.
        //json. 
        switch (kind)
        {
            case "Agit":
                playerDataMgr.saveData.agitLevel += 1;
                bunkerMgr.agitMgr.Init();
                currentLevel = playerDataMgr.saveData.agitLevel;
                break;
            case "Storage":
                playerDataMgr.saveData.storageLevel += 1;
                //bunkerMgr.sto.Init();
                currentLevel = playerDataMgr.saveData.storageLevel;
                break;
            case "Garage":
                playerDataMgr.saveData.garageLevel += 1;
                currentLevel = playerDataMgr.saveData.garageLevel;
                break;
            case "CarCenter":
                playerDataMgr.saveData.carCenterLevel += 1;
                currentLevel = playerDataMgr.saveData.carCenterLevel;
                break;
            case "Hospital":
                playerDataMgr.saveData.hospitalLevel += 1;
                currentLevel = playerDataMgr.saveData.hospitalLevel;
                break;
            case "Store":
                playerDataMgr.saveData.storeLevel += 1;
                bunkerMgr.storeMgr.Init();
                currentLevel = playerDataMgr.saveData.storeLevel;
                break;
            case "Pub":
                playerDataMgr.saveData.pubLevel += 1;
                currentLevel = playerDataMgr.saveData.pubLevel;
                break;
        }

        var child = prefabs[index].transform.GetChild(0).gameObject;
        child.GetComponent<Text>().text = $"현재레벨 : {currentLevel}";

        child = prefabs[index].transform.GetChild(1).gameObject;
        int nextLevel = currentLevel + 1;
        child.GetComponent<Text>().text = (nextLevel != 6) ? $"다음레벨 :{nextLevel}" : "-";

        if (currentLevel == 5)
        {
            child = prefabs[index].transform.GetChild(2).gameObject;
            child.transform.GetChild(0).gameObject.GetComponent<Text>().text = "-";
            var button = child.GetComponent<Button>();
            button.interactable = false;
        }

        child = bunkerMgr.bunkerObjs[index].transform.GetChild(0).gameObject;
        child.GetComponent<TextMeshPro>().text = $"Lv.{currentLevel}";

        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
    }
}

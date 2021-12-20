using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StoreMgr : MonoBehaviour
{
    public GameObject storeContent;
    public GameObject storeItemPrefab;

    public GameObject itemNumWin;
    public Text itemNum;
    public Slider itemNumSlider;

    public GameObject inventoryContent;
    public GameObject inventoryItemPrefab;

    Dictionary<int, string> storeItemData;
    Dictionary<int, GameObject> storeList;

    Dictionary<string, int> inventoryData;
    Dictionary<string, GameObject> inventoryList;
    int inventoryIndex;

    string currentItem;
    void Start()
    {
        storeItemData = new Dictionary<int, string>();
        storeItemData.Add(0, "빨간포션");
        storeItemData.Add(1, "주황포션");
        storeItemData.Add(2, "파란포션");
        storeItemData.Add(3, "하양포션");
        storeItemData.Add(4, "빨간알약");
        storeItemData.Add(5, "주황알약");
        storeItemData.Add(6, "파란알약");
        storeItemData.Add(7, "하양알약");

        storeList = new Dictionary<int, GameObject>();

        int i = 0;
        foreach (var element in storeItemData)
        {
            var go = Instantiate(storeItemPrefab, storeContent.transform);
            var child = go.transform.GetChild(0);
            var itemName = child.GetComponent<Text>();
            itemName.text = element.Value;

            var button = go.AddComponent<Button>();
            int num = i;
            button.onClick.AddListener(delegate { ShowItemNumWin(num); });
            storeList.Add(i, go);
            i++;
        }

        inventoryData = new Dictionary<string, int>();
        inventoryList = new Dictionary<string, GameObject>();
        inventoryIndex = 0;
    }

    public void ShowItemNumWin(int i)
    {
        itemNumWin.SetActive(true);
        currentItem = storeItemData[i];
    }

    public void BuyItem()
    {
        if (currentItem == null) return;
        if (Mathf.FloorToInt(itemNumSlider.value * 100) == 0) return;

        if (inventoryData.ContainsKey(currentItem))
        {
            int count = Mathf.FloorToInt(itemNumSlider.value * 100);
            inventoryData[currentItem] += count;

            var child = inventoryList[currentItem].transform.GetChild(1);
            child.GetComponent<Text>().text = inventoryData[currentItem].ToString();
        }

        else
        {
            int count = Mathf.FloorToInt(itemNumSlider.value * 100);
            inventoryData.Add(currentItem, count);

            var go = Instantiate(inventoryItemPrefab, inventoryContent.transform);
            var child = go.transform.GetChild(0);
            child.GetComponent<Text>().text = currentItem.Substring(0,1);

            child = go.transform.GetChild(1);
            child.GetComponent<Text>().text = inventoryData[currentItem].ToString();
            inventoryList.Add(currentItem, go);
        }
    }

    public void ChangeItemNum()
    {
        itemNum.text = Mathf.FloorToInt(itemNumSlider.value * 100).ToString();
    }
}
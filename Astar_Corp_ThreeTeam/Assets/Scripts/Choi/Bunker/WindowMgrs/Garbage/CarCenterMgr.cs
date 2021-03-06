using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    public BunkerMgr bunkerMgr;
    public GameObject mainWin;
    public GameObject buyWin;
    public GameObject upgradeWin;

    public Animator menuAnim;
    bool isMenuOpen;
    public GameObject arrowImg;

    [Header("Truck Info")]
    public Image truckImg;
    public TextMeshProUGUI text;
    public Button left;
    public Button right;

    [Header("Popup")]
    public Text popupTitle;
    public Text popupContents;

    private string truckKey;
    private int keyNum;

    //????.
    public Text ownedMoneyTxt;
    public GameObject buyPopup;
    public Text ownedTxt;
    public GameObject carListPopup;
    public GameObject carListContents;
    public GameObject carPrefab;
    public Dictionary<string, GameObject> carObjs = new Dictionary<string, GameObject>();

    public List<GameObject> buttonList;
    public List<GameObject> gaugeList;
    public List<GameObject> costButtons;
    public GameObject lockImg;
    public GameObject popup;
    bool isPopupOpen;
    public Text moneyTxt;
    public Text costTxt;
    public Button buyButton;
    public Button upgradeButton;
    public Text upgradeCostTxt;

    [Header("Upgrade Win")]
    public Text carCenterLevelTxt;
    public Text capacityTxt;
    public Text materialTxt;

    public Dictionary<int, Truck> carOrder = new Dictionary<int, Truck>();

    List<string> owned = new List<string>();
    List<string> notOwned = new List<string>();

    int carCenterLevel;
    int maxCarLevel;
    int nextCarLevel;
    int upgradeCost;

    Color originColor;
    Color selectedColor;
    TruckStat currentStat;
    int currentKey;
    string selectedCar;

    public void Init()
    {
        if (carListPopup.activeSelf) carListPopup.SetActive(false);

        moneyTxt.text = playerDataMgr.saveData.money.ToString();
        carCenterLevel = playerDataMgr.saveData.carCenterLevel;
        Bunker carCenterLevelInfo = playerDataMgr.bunkerList["BUN_0003"];
        switch (carCenterLevel)
        {
            case 1:
                maxCarLevel = carCenterLevelInfo.level1;
                nextCarLevel = carCenterLevelInfo.level2;
                upgradeCost = carCenterLevelInfo.level2Cost;
                break;
            case 2:
                maxCarLevel = carCenterLevelInfo.level2;
                nextCarLevel = carCenterLevelInfo.level3;
                upgradeCost = carCenterLevelInfo.level3Cost;
                break;
            case 3:
                maxCarLevel = carCenterLevelInfo.level3;
                nextCarLevel = carCenterLevelInfo.level4;
                upgradeCost = carCenterLevelInfo.level4Cost;
                break;
            case 4:
                maxCarLevel = carCenterLevelInfo.level4;
                nextCarLevel = carCenterLevelInfo.level5;
                upgradeCost = carCenterLevelInfo.level5Cost;
                break;
            case 5:
                maxCarLevel = carCenterLevelInfo.level5;
                break;
        }

        Refresh();

        originColor = new Color(45 / 255f, 65 / 255f, 85 / 255f);
        selectedColor = new Color(56/255f, 87/255f, 35/255f);
        currentStat = TruckStat.None;
        
        currentKey = 0;
        selectedCar = carOrder[currentKey].id;
        CarDisplay(selectedCar);

        var cost = carOrder[currentKey].price;
        costTxt.text = $"{cost}";
        
        buyButton.interactable = false;
        ButtonInteractable();

        upgradeCostTxt.text = "-";
        upgradeButton.interactable = false;

        isMenuOpen = true;
        arrowImg.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void Refresh()
    {
        if (carObjs.Count != 0)
        {
            foreach (var element in carObjs)
            {
                Destroy(element.Value);
            }
            carObjs.Clear();
            carListContents.transform.DetachChildren();
        }
        if (owned.Count != 0) owned.Clear();
        if (notOwned.Count != 0) notOwned.Clear();
        if (carOrder.Count != 0) carOrder.Clear();

        foreach (var element in playerDataMgr.truckList)
        {
            if (playerDataMgr.saveData.cars.Contains(element.Key))
                owned.Add(element.Key);
            else
                notOwned.Add(element.Key);
        }

        int i = 0;
        foreach (var element in owned)
        {
            var truck = playerDataMgr.truckList[element];
            int num = i;
            carOrder.Add(num, truck);
            i++;
        }
        foreach (var element in notOwned)
        {
            var truck = playerDataMgr.truckList[element];
            int num = i;
            carOrder.Add(num, truck);
            i++;
        }

        //?˾?.
        foreach (var element in carOrder)
        {
            var go = Instantiate(carPrefab, carListContents.transform);
            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;
            if (!owned.Contains(element.Value.id)) go.GetComponent<Image>().color = Color.gray;

            string key = element.Value.id;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCar(key); });

            carObjs.Add(element.Value.id, go);
        }
    }

    public void PreviousButton()
    {
        if (currentKey-1 < 0) return;

        currentKey--;
        selectedCar = carOrder[currentKey].id;
        
        if (popup.activeSelf) popup.SetActive(false);
        CarDisplay(selectedCar);
    }

    public void NextButton()
    {
        if (currentKey+1 >= carOrder.Count) return;

        currentKey++;
        selectedCar = carOrder[currentKey].id;

        if (popup.activeSelf) popup.SetActive(false);
        CarDisplay(selectedCar);
    }

    private void ButtonInteractable()
    {
        left.interactable = (currentKey <= 0) ? false : true;
        right.interactable = (currentKey >= carOrder.Count - 1) ? false : true;
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
            //for (int i = 0; i < gaugeList[previousIndex].transform.childCount; i++)
            //{
            //    var child = gaugeList[previousIndex].transform.GetChild(i).gameObject;
            //    child.GetComponent<Image>().color = originColor;
            //}
        }
      
        upgradeButton.interactable = true;
        switch (index)
        {
            case 0:
                currentStat = TruckStat.Speed;
                var cost = playerDataMgr.truckList[selectedCar].speedUp_Cost;
                upgradeCostTxt.text = $"{cost}";
                break;
            case 1:
                currentStat = TruckStat.Trunk;
                cost = playerDataMgr.truckList[selectedCar].weightUp_Cost;
                upgradeCostTxt.text = $"{cost}";
                break;
            case 2:
                currentStat = TruckStat.FieldOfView;
                cost = playerDataMgr.truckList[selectedCar].sightUp_Cost;
                upgradeCostTxt.text = $"{cost}";
                break;
        }
        buttonList[index].GetComponent<Image>().color = selectedColor;
        //for (int i = 0; i < gaugeList[index].transform.childCount; i++)
        //{
        //    var child = gaugeList[index].transform.GetChild(i).gameObject;
        //    child.GetComponent<Image>().color = selectedColor;
        //}
    }

    public void SelectCar(string key)
    {
        if (selectedCar != null)
        {
            if (owned.Contains(selectedCar))
                carObjs[selectedCar].GetComponent<Image>().color = Color.white;
            else carObjs[selectedCar].GetComponent<Image>().color = Color.gray;
        }

        selectedCar = key;
        carObjs[selectedCar].GetComponent<Image>().color = Color.red;
    }

    public void CarDisplay(string key)
    {
        currentKey = carOrder.FirstOrDefault(x => x.Value.id.Equals(selectedCar)).Key;
        ButtonInteractable();

        int speedLv = 1;
        int trunkLv = 1;
        int sightLv = 1;
        truckImg.sprite = playerDataMgr.truckList[key].img;
        text.text = playerDataMgr.truckList[key].name;
        if (owned.Contains(key))
        {
            int index = playerDataMgr.saveData.cars.IndexOf(key);
            speedLv = playerDataMgr.saveData.speedLv[index];
            trunkLv = playerDataMgr.saveData.weightLv[index];
            sightLv = playerDataMgr.saveData.sightLv[index];

            if (buyButton.interactable == true) buyButton.interactable = false;

            if(!ownedTxt.gameObject.activeSelf) ownedTxt.gameObject.SetActive(true);
            if(buyPopup.activeSelf) buyPopup.SetActive(false);
            if (lockImg.activeSelf) lockImg.SetActive(false);
        }
        else 
        {
            if (buyButton.interactable == false) buyButton.interactable = true;

            if (ownedTxt.gameObject.activeSelf) ownedTxt.gameObject.SetActive(false);
            var cost = playerDataMgr.truckList[key].price;
            buyPopup.transform.GetChild(2).gameObject.GetComponent<Text>().text
                = $"G {cost}";
            if (!buyPopup.activeSelf) buyPopup.SetActive(true);
            if (!lockImg.activeSelf) lockImg.SetActive(true);
        }

        //?ʱ?ȭ.
        foreach (var element in buttonList)
        { 
            element.GetComponent<Image>().color = originColor;
        }
       
        var speedObj = gaugeList[0];
        var trunkObj = gaugeList[1];
        var sightObj = gaugeList[2];
        for (int i=0; i < speedObj.transform.childCount; i++)
        {
            speedObj.transform.GetChild(i).gameObject.GetComponent<Image>().color = originColor;
            trunkObj.transform.GetChild(i).gameObject.GetComponent<Image>().color = originColor;
            sightObj.transform.GetChild(i).gameObject.GetComponent<Image>().color = originColor;
        }

        //??????.
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
            //for (int i = 0; i < gaugeList[previousIndex].transform.childCount; i++)
            //{
            //    var child = gaugeList[previousIndex].transform.GetChild(i).gameObject;
            //    child.GetComponent<Image>().color = originColor;
            //}
            currentStat = TruckStat.None;
        }

        for (int i = 0; i < speedLv; i++)
        {
            speedObj.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.red;
        }
        for (int i = 0; i < trunkLv; i++)
        {
            trunkObj.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.red;
        }
        for (int i = 0; i < sightLv; i++)
        {
            sightObj.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.red;
        }

        //var cost = playerDataMgr.truckList[key].price;
        //costTxt.text = $"{cost}";


        costButtons[0].transform.GetChild(1).GetComponent<Text>().text
            = (speedLv == 5)? "?ִ?ġ": $"{playerDataMgr.truckList[selectedCar].speedUp_Cost}";
        costButtons[1].transform.GetChild(1).GetComponent<Text>().text
           = (trunkLv == 5) ? "?ִ?ġ" : $"{playerDataMgr.truckList[selectedCar].weightUp_Cost}";
        costButtons[2].transform.GetChild(1).GetComponent<Text>().text
           = (sightLv == 5) ? "?ִ?ġ" : $"{playerDataMgr.truckList[selectedCar].sightUp_Cost}";

        upgradeCostTxt.text = "-";
        upgradeButton.interactable = false;
    }

    public void Upgrage()
    {
        if (currentStat == TruckStat.None) return;
        if (!owned.Contains(selectedCar)) return;
        int cost = 0;
        
        switch (currentStat)
        {
            case TruckStat.Speed:
                cost = playerDataMgr.truckList[selectedCar].speedUp_Cost;
                break;
            case TruckStat.Trunk:
                cost = playerDataMgr.truckList[selectedCar].weightUp_Cost;
                break;
            case TruckStat.FieldOfView:
                cost = playerDataMgr.truckList[selectedCar].sightUp_Cost;
                break;
        }
        if (playerDataMgr.saveData.money - cost < 0) return;
        var speedObj = gaugeList[0];
        var trunkObj = gaugeList[1];
        var sightObj = gaugeList[2];
        int index = playerDataMgr.saveData.cars.IndexOf(selectedCar);
        
        if (currentStat == TruckStat.Speed)
        {
            if (playerDataMgr.saveData.speedLv[index] == 5) return;
            if (playerDataMgr.saveData.speedLv[index] == maxCarLevel) return;
            playerDataMgr.saveData.speedLv[index] += 1;
            if (playerDataMgr.saveData.speedLv[index] == 5)
                costButtons[0].transform.GetChild(1).GetComponent<Text>().text = "?ִ?ġ";
            
            var speedLv = playerDataMgr.saveData.speedLv[index];

            for (int i = 0; i < speedLv; i++)
            {
                if(speedObj.transform.GetChild(i).gameObject.GetComponent<Image>().color != Color.red)
                    speedObj.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.red;
            }
        }
        else if (currentStat == TruckStat.Trunk)
        {
            if (playerDataMgr.saveData.weightLv[index] == 5) return;
            if (playerDataMgr.saveData.weightLv[index] == maxCarLevel) return;
            playerDataMgr.saveData.weightLv[index] += 1;
            if (playerDataMgr.saveData.weightLv[index] == 5)
                costButtons[1].transform.GetChild(1).GetComponent<Text>().text = "?ִ?ġ";

            var trunkLv = playerDataMgr.saveData.weightLv[index];

            for (int i = 0; i < trunkLv; i++)
            {
                if (trunkObj.transform.GetChild(i).gameObject.GetComponent<Image>().color != Color.red)
                    trunkObj.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.red;
            }
        }
        else if (currentStat == TruckStat.FieldOfView)
        {
            if (playerDataMgr.saveData.sightLv[index] == 5) return;
            if (playerDataMgr.saveData.sightLv[index] == maxCarLevel) return;
            playerDataMgr.saveData.sightLv[index] += 1;
            if (playerDataMgr.saveData.sightLv[index] == 5)
                costButtons[2].transform.GetChild(1).GetComponent<Text>().text = "?ִ?ġ";

            var sightLv = playerDataMgr.saveData.sightLv[index];

            for (int i = 0; i < sightLv; i++)
            {
                if (sightObj.transform.GetChild(i).gameObject.GetComponent<Image>().color != Color.red)
                    sightObj.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.red;
            }
        }
        playerDataMgr.saveData.money -= cost;
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        bunkerMgr.moneyTxt.text = playerDataMgr.saveData.money.ToString();
        ownedMoneyTxt.text =  $"???? ?ݾ? G {playerDataMgr.saveData.money}";
        //moneyTxt.text = playerDataMgr.saveData.money.ToString();
    }

    public void Buy()
    {
        //???? ?????α?.
        if (playerDataMgr.saveData.money - playerDataMgr.truckList[selectedCar].price < 0) return;

        //json.
        playerDataMgr.saveData.money -= playerDataMgr.truckList[selectedCar].price;
        //moneyTxt.text = playerDataMgr.saveData.money.ToString();

        playerDataMgr.saveData.cars.Add(selectedCar);
        playerDataMgr.saveData.speedLv.Add(1);
        playerDataMgr.saveData.sightLv.Add(1);
        playerDataMgr.saveData.weightLv.Add(1);
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        bunkerMgr.moneyTxt.text = playerDataMgr.saveData.money.ToString();
        ownedMoneyTxt.text = $"???? ?ݾ? G {playerDataMgr.saveData.money}";

        Refresh();
        CarDisplay(selectedCar);
        var key = carOrder.FirstOrDefault(x => x.Value.id.Equals(selectedCar)).Key;
        currentKey = key;
        ButtonInteractable();
    }

    public void RefreshUpgradeWin()
    {
        if (carCenterLevel != 5)
        {
            carCenterLevelTxt.text = $"?ǹ? ???? {carCenterLevel}??{carCenterLevel + 1}";
            capacityTxt.text = $"???? ?ִ? ???? ???? {maxCarLevel}??{nextCarLevel}\n"
                + $"???? ?ִ? ???׷??̵? {maxCarLevel}??{nextCarLevel}";
            materialTxt.text = $"{upgradeCost}";
        }
        else
        {
            carCenterLevelTxt.text = $"?ǹ? ????{carCenterLevel}?? -";
            capacityTxt.text = $"???? ?ִ? ???? ???? {maxCarLevel}?? -\n"
                + $"???? ?ִ? ???׷??̵? {maxCarLevel}?? -";
            materialTxt.text = $"-";
        }
    }

    //â ????.
    public void OpenMainWin()
    {
        if (bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(false);
        if (!mainWin.activeSelf) mainWin.SetActive(true);
        if (buyWin.activeSelf) buyWin.SetActive(false);
        if (popup.activeSelf) popup.SetActive(false);
        isPopupOpen = false;
        if (upgradeWin.activeSelf) upgradeWin.SetActive(false);
    }

    public void CloseMainWin()
    {
        if (!bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(true);
    }

    public void Menu()
    {
        arrowImg.GetComponent<RectTransform>().rotation = (isMenuOpen) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.Euler(0f, 0f, 0f);
        isMenuOpen = !isMenuOpen;
        menuAnim.SetBool("isOpen", isMenuOpen);
    }

    public void OpenBuyWin()
    {
        mainWin.SetActive(false);
        if (popup.activeSelf) popup.SetActive(false);
        
        //?ʱ?ȭ.
        foreach (var element in buttonList)
        {
            element.GetComponent<Image>().color = originColor;
        }
        ownedMoneyTxt.text = $"???? ?ݾ? G {playerDataMgr.saveData.money}";
        buyWin.SetActive(true);
    }

    public void CloseBuyWin()
    {
        buyWin.SetActive(false);
        mainWin.SetActive(true);
    }

    public void OpenPopup(int index)
    {
        isPopupOpen = true;

        switch (index)
        {
            case 0:
                popupTitle.text = $"Ʈ??ũ";
                popupContents.text = $"???? Ʈ??ũ?? ?ִ? ???Կ? ?????? ?ݴϴ?.";
                break;
            case 1:
                popupTitle.text = $"?þ߹???";
                popupContents.text = $"???? ?þ??? ũ?⿡ ?????? ?ݴϴ?.";
                break;
            case 2:
                popupTitle.text = $"????";
                popupContents.text = $"???? ?̵??ӵ??? ?????? ?ݴϴ?.";
                break;
        }

        popup.SetActive(true);
    }

    public void ClosePopup()
    {
        isPopupOpen = false;
        popup.SetActive(false);
    }

    public void OpenUpgradeWin()
    {
        RefreshUpgradeWin();
        mainWin.SetActive(false);
        upgradeWin.SetActive(true);
    }

    public void CloseUpgradeWin()
    {
        upgradeWin.SetActive(false);
        mainWin.SetActive(true);
    }

    public void OpenCarListPopup()
    {
        carListPopup.SetActive(true);
    }

    public void CloseCarListPopup()
    {
        foreach (var element in carObjs)
        {
            if (element.Value.GetComponent<Image>().color == Color.red)
                element.Value.GetComponent<Image>().color = Color.white;
        }
        carListPopup.SetActive(false);
        if (selectedCar != null) CarDisplay(selectedCar);
    }
}

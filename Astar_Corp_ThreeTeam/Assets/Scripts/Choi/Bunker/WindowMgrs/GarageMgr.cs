using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GarageMgr : MonoBehaviour
{
    public PlayerDataMgr playerDataMgr;
    public BunkerMgr bunkerMgr;
    public GameObject mainWin;
    public GameObject popupWin;
    public GameObject carBoardingWin;
    public GameObject trunkWin;

    public Animator menuAnim;
    bool isMenuOpen;
    public GameObject arrowImg;

    [Header("Main Win")]
    public GameObject leftArrow;
    public GameObject rightArrow;
    public List<GameObject> characters;
    public Image carImg;
    public Text weightTxt;
    public Text bullet5Txt;
    public Text bullet7Txt;
    public Text bullet9Txt;
    public Text bullet45Txt;
    public Text bullet12Txt;
    public GameObject boardingButton;
    public GameObject trunkButton;

    public TrunkMgr trunkMgr;
    public BoardingMgr boardingMgr;

    List<string> owned = new List<string>();
    public Dictionary<int, Truck> carOrder = new Dictionary<int, Truck>();
    int carCenterLevel;
    int maxCarCapacity;
    public int currentKey;

    public void Init()
    {
        carCenterLevel = playerDataMgr.saveData.carCenterLevel;
        Bunker carCenterLevelInfo = playerDataMgr.bunkerList["BUN_0003"];
        switch (carCenterLevel)
        {
            case 1:
                maxCarCapacity = carCenterLevelInfo.level1;
                break;
            case 2:
                maxCarCapacity = carCenterLevelInfo.level2;
                break;
            case 3:
                maxCarCapacity = carCenterLevelInfo.level3;
                break;
            case 4:
                maxCarCapacity = carCenterLevelInfo.level4;
                break;
            case 5:
                maxCarCapacity = carCenterLevelInfo.level5;
                break;
        }
        int currentCarNum = playerDataMgr.saveData.cars.Count;

        if (owned.Count != 0) owned.Clear();
        if (carOrder.Count != 0) carOrder.Clear();

        //????????.
        foreach (var element in playerDataMgr.truckList)
        {
            if (playerDataMgr.saveData.cars.Contains(element.Key))
                owned.Add(element.Key);
        }

        int i = 0;
        foreach (var element in owned)
        {
            var truck = playerDataMgr.truckList[element];
            int num = i;
            carOrder.Add(num, truck);
            i++;
        }
        if (playerDataMgr.saveData.currentCar == null)
        {
            currentKey = 0;
        }
        else
        {
            currentKey = carOrder.FirstOrDefault(x => x.Value.id.Equals(playerDataMgr.saveData.currentCar)).Key;
        }

        trunkMgr.playerDataMgr = playerDataMgr;
        trunkMgr.garageMgr = this;
        trunkMgr.Init();

        boardingMgr.playerDataMgr = playerDataMgr;
        boardingMgr.garageMgr = this;
        boardingMgr.Init();

        isMenuOpen = true;
        arrowImg.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void Display()
    {
        var truck = carOrder[currentKey];
        
        carImg.sprite = truck.img;
        var capacity = truck.capacity;

        //?ʱ?ȭ.
        for (int i= 0; i<capacity; i++)
        {
            characters[i].SetActive(true);
            var child = characters[i].transform.GetChild(1).gameObject;
            var color = child.GetComponent<Image>().color;
            child.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
        }
        for (int i = capacity; i < characters.Count; i++)
        {
            characters[i].SetActive(false);
        }

        foreach (var element in playerDataMgr.boardingSquad)
        {
            characters[element.Key].SetActive(true);
            var child = characters[element.Key].transform.GetChild(1).gameObject;
            var color = child.GetComponent<Image>().color;
            child.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
            var index = element.Value;
            child.GetComponent<Image>().sprite =
                playerDataMgr.currentSquad[index].character.halfImg;
        }
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        weightTxt.text = $"{trunkMgr.trunkWeight}";
        bullet5Txt.text = $"{trunkMgr.trunkBullet5}";
        bullet7Txt.text = $"{trunkMgr.trunkBullet7}";
        bullet9Txt.text = $"{trunkMgr.trunkBullet9}";
        bullet45Txt.text = $"{trunkMgr.trunkBullet45}";
        bullet12Txt.text = $"{trunkMgr.trunkBullet12}";
    }

    public void PreviousButton()
    {
        if (currentKey - 1 < 0) return;

        currentKey--;

        boardingMgr.CarReset();
        playerDataMgr.saveData.currentCar = carOrder[currentKey].id;
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        boardingMgr.Init();
        trunkMgr.Init();
        //trunkMgr.DisplayTruckItem(0);
        Display();

        boardingButton.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void NextButton()
    {
        if (currentKey + 1 >= carOrder.Count) return;

        currentKey++;

        boardingMgr.CarReset();
        playerDataMgr.saveData.currentCar = carOrder[currentKey].id;
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        boardingMgr.Init();
        trunkMgr.Init();
        //trunkMgr.DisplayTruckItem(0);
        Display();

        boardingButton.transform.GetChild(1).gameObject.SetActive(true);
    }

    //â ????.
    public void OpenMainWin()
    {
        if (bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(false);
        if (!mainWin.activeSelf) mainWin.SetActive(true);
        if (popupWin.activeSelf) popupWin.SetActive(false);
        if (carBoardingWin.activeSelf) carBoardingWin.SetActive(false);
        if (trunkWin.activeSelf) trunkWin.SetActive(false);
        
        Display();

        if (playerDataMgr.boardingSquad.Count == 0)
            boardingButton.transform.GetChild(1).gameObject.SetActive(true);
        else boardingButton.transform.GetChild(1).gameObject.SetActive(false);

        if (playerDataMgr.truckEquippables.Count == 0
            && playerDataMgr.truckConsumables.Count == 0
            && playerDataMgr.truckOtherItems.Count == 0)
            trunkButton.transform.GetChild(1).gameObject.SetActive(true);
        else trunkButton.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void CloseMainWin()
    {
        if (!bunkerMgr.belowUI.activeSelf) bunkerMgr.belowUI.SetActive(true);
    }

    public void ExitBunker()
    {
        if (playerDataMgr.boardingSquad.Count == 0)
        {
            popupWin.SetActive(true);
        }
        else bunkerMgr.ExitBunker();
    }

    public void ClosePopup()
    {
        popupWin.SetActive(false);
    }

    public void Menu()
    {
        arrowImg.GetComponent<RectTransform>().rotation = (isMenuOpen) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.Euler(0f, 0f, 0f);
        isMenuOpen = !isMenuOpen;
        menuAnim.SetBool("isOpen", isMenuOpen);
    }

    public void OpenCarBoardingWin()
    {
        mainWin.SetActive(false);
        carBoardingWin.SetActive(true);
    }

    public void CloseCarBoardingWin()
    {
        carBoardingWin.SetActive(false);
        OpenMainWin();
    }

    public void OpenTrunkWin()
    {
        mainWin.SetActive(false);
        trunkMgr.Init();
        Debug.Log($"currentCar : {playerDataMgr.saveData.currentCar}");
        trunkWin.SetActive(true);
    }

    public void CloseTrunkWin()
    {
        trunkWin.SetActive(false);
        OpenMainWin();
    }
}

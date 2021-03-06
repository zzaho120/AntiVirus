using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BoardingMgr : MonoBehaviour
{
    public Button left;
    public Button right;
    public Text carNameTxt;
    public Image carImg;

    public GameObject ListContent;
    public GameObject characterPrefab;
    public Dictionary<int, GameObject> characters = new Dictionary<int, GameObject>();
    public List<GameObject> seats;

    public GameObject carListPopup;
    public GameObject carListContents;
    public GameObject carPrefab;
    public Dictionary<string, GameObject> carObjs = new Dictionary<string, GameObject>();
    public Dictionary<int, Truck> carOrder = new Dictionary<int, Truck>();

    List<string> owned = new List<string>();

    int currentIndex;
    int currentSeatNum;
    public int currentKey;
    public string selectedCar;
    Sprite nullImg;

    public PlayerDataMgr playerDataMgr;
    public GarageMgr garageMgr;
    Color originColor;

    public void Init()
    {
        if (carListPopup.activeSelf) carListPopup.SetActive(false);
        var child = seats[0].transform.GetChild(1).gameObject;
        nullImg = child.GetComponent<Image>().sprite;

        foreach (var element in seats)
        {
            element.GetComponent<Image>().color = Color.white;
        }

        //????.
        if (characters.Count != 0)
        {
            foreach (var element in characters)
            {
                Destroy(element.Value);
            }
            characters.Clear();

            ListContent.transform.DetachChildren();
        }

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
        if (carOrder.Count != 0) carOrder.Clear();

        //????.
        int i = 0;
        foreach (var element in playerDataMgr.currentSquad)
        {
            if (playerDataMgr.saveData.boarding[i] != -1)
            {
                i++;
                continue;
            }

            int num = i;

            var go = Instantiate(characterPrefab, ListContent.transform);

            go.GetComponent<DragDrop>().characterNum = num;
            go.GetComponent<DragDrop>().boardingMgr = this;

            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(num); });

            child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Image>().sprite = element.Value.character.halfImg;

            child = go.transform.GetChild(1).gameObject;
            var childObj = child.transform.GetChild(0).gameObject;
            childObj.GetComponent<Text>().text
                = $"{element.Value.characterName}";

            childObj = child.transform.GetChild(1).gameObject;
            string mainWeaponTxt = (element.Value.weapon.mainWeapon == null) ?
                "????????" : element.Value.weapon.mainWeapon.storeName;

            childObj.GetComponent<Text>().text
                = $"{element.Value.character.name}/LV{element.Value.level}/{mainWeaponTxt}";

            childObj = child.transform.GetChild(2).gameObject;
            var slider = childObj.GetComponent<Slider>();
            slider.maxValue = element.Value.MaxHp;
            slider.value = element.Value.currentHp;

            characters.Add(num, go);
            i++;
        }

        foreach (var element in playerDataMgr.boardingSquad)
        {
            currentSeatNum = element.Key;
            currentIndex = element.Value;

            //child = seats[currentSeatNum].transform.GetChild(0).gameObject;
            //child.GetComponent<Text>().text = playerDataMgr.currentSquad[currentIndex].character.name.Substring(0, 3);
            child = seats[currentSeatNum].transform.GetChild(1).gameObject;
            var color = child.GetComponent<Image>().color;
            child.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
            child.GetComponent<Image>().sprite = playerDataMgr.currentSquad[currentIndex].character.halfImg;
            child = seats[currentSeatNum].transform.GetChild(2).gameObject;
            child.SetActive(true);
            child = seats[currentSeatNum].transform.GetChild(3).gameObject;
            var name = playerDataMgr.currentSquad[currentIndex].characterName.Split(' ');
            child.SetActive(true);
            child.GetComponent<Text>().text = $"{name[0]}";
        }

        //????????.
        foreach (var element in playerDataMgr.truckList)
        {
            if (playerDataMgr.saveData.cars.Contains(element.Key))
                owned.Add(element.Key);
        }

        i = 0;
        foreach (var element in owned)
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
            child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = element.Value.name;
            if (!owned.Contains(element.Value.id)) go.GetComponent<Image>().color = Color.gray;

            string key = element.Value.id;
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCar(key); });

            carObjs.Add(element.Value.id, go);
        }

        currentIndex = -1;
        currentSeatNum = -1;

        if (playerDataMgr.saveData.currentCar == null)
        {
            currentKey = 0;
        }
        else
        {
            currentKey = carOrder.FirstOrDefault(x => x.Value.id.Equals(playerDataMgr.saveData.currentCar)).Key;
        }
        selectedCar = carOrder[currentKey].id;
        CarDisplay(selectedCar);

        ButtonInteractable();
        originColor = new Color(45f/255, 65f/255, 85f/255);
    }

    public void PreviousButton()
    {
        if (currentKey - 1 < 0) return;

        currentKey--;
       
        CarReset();
        selectedCar = carOrder[currentKey].id;
        playerDataMgr.saveData.currentCar = selectedCar;
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        CarDisplay(selectedCar);
        garageMgr.PreviousButton();
    }

    public void NextButton()
    {
        if (currentKey + 1 >= carOrder.Count) return;

        currentKey++;

        CarReset();
        selectedCar = carOrder[currentKey].id;
        playerDataMgr.saveData.currentCar = selectedCar;
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);
        CarDisplay(selectedCar);
        garageMgr.NextButton();
    }

    public void CarReset()
    {
        foreach (var element in seats)
        {
            if (element.GetComponent<Image>().color != Color.red) continue;
            element.GetComponent<Image>().color = Color.white;
        }

        foreach (var element in characters)
        {
            if (element.Value.GetComponent<Image>().color != Color.red) continue;
            element.Value.GetComponent<Image>().color = Color.white;
        }

        foreach (var key in playerDataMgr.boardingSquad.Keys.ToList())
        {
            currentSeatNum = key;
            currentIndex = playerDataMgr.boardingSquad[key];
            GetOffTheCar();
        }
        currentIndex = -1;
        currentSeatNum = -1;
    }

    public void CarDisplay(string key)
    {
        currentKey = carOrder.FirstOrDefault(x => x.Value.id.Equals(selectedCar)).Key;
        ButtonInteractable();

        var truck = carOrder[currentKey];
        carNameTxt.text = truck.name;
        carImg.sprite = truck.img;

        var capacity = truck.capacity;
        for (int i = 0; i < capacity; i++)
        {
            if (!seats[i].activeSelf)
                seats[i].SetActive(true);
        }
        for (int i = capacity; i < seats.Count; i++)
        {
            if (seats[i].activeSelf)
                seats[i].SetActive(false);
        }

        garageMgr.Display();
    }

    private void ButtonInteractable()
    {
        left.interactable = (currentKey <= 0) ? false : true;
        right.interactable = (currentKey >= carOrder.Count - 1) ? false : true;
    }

    public void SelectCharacter(int i)
    {
        //if (currentSeatNum == -1) return;
        garageMgr.bunkerMgr.PlayClickSound();

        if (currentIndex != -1 && characters.ContainsKey(currentIndex))
            characters[currentIndex].GetComponent<Image>().color = originColor;

        currentIndex = i;
        characters[currentIndex].GetComponent<Image>().color = Color.red;
    }

    public void SelectSeat(int i)
    {
        garageMgr.bunkerMgr.PlayClickSound();

        if (currentSeatNum != -1)
        {
            seats[currentSeatNum].GetComponent<Image>().color = Color.white;
        }

        currentSeatNum = i;
        seats[currentSeatNum].GetComponent<Image>().color = Color.red;

        if (playerDataMgr.boardingSquad.ContainsKey(currentSeatNum))
            currentIndex = playerDataMgr.boardingSquad[currentSeatNum];
    }

    public void SelectCar(string key)
    {
        if (selectedCar != null)
            carObjs[selectedCar].GetComponent<Image>().color = Color.white;

        selectedCar = key;
        carObjs[selectedCar].GetComponent<Image>().color = Color.red;
    }

    //ž??.
    public void GetInTheCar()
    {
        if (currentIndex == -1 || currentSeatNum == -1) return;
        if (playerDataMgr.saveData.boarding[currentIndex] == currentSeatNum) return;

        //?ٸ? ?????? ž???ϰ? ?????? ????.
        if (playerDataMgr.boardingSquad.ContainsKey(currentSeatNum))
        {
            var current = currentIndex;
            var previous = playerDataMgr.boardingSquad[currentSeatNum];
            currentIndex = previous;
            GetOffTheCar();
            currentIndex = current;
        }

        //json.
        playerDataMgr.saveData.currentCar = selectedCar;
        playerDataMgr.saveData.boarding[currentIndex] = currentSeatNum;
        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        //playerDataMgr.
        playerDataMgr.boardingSquad.Add(currentSeatNum, currentIndex);

        //???? ??????.
        var child = seats[currentSeatNum].transform.GetChild(0).gameObject;
        child.GetComponent<Text>().text = playerDataMgr.currentSquad[currentIndex].character.name.Substring(0, 3);
        child = seats[currentSeatNum].transform.GetChild(1).gameObject;
        var color = child.GetComponent<Image>().color;
        child.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
        child.GetComponent<Image>().sprite = playerDataMgr.currentSquad[currentIndex].character.halfImg;
        child = seats[currentSeatNum].transform.GetChild(2).gameObject;
        child.SetActive(true);
        child = seats[currentSeatNum].transform.GetChild(3).gameObject;
        var name = playerDataMgr.currentSquad[currentIndex].characterName.Split(' ');
        child.SetActive(true);
        child.GetComponent<Text>().text = $"{name[0]}";

        Destroy(characters[currentIndex]);
        characters.Remove(currentIndex);

        seats[currentSeatNum].GetComponent<Image>().color = Color.white;
        currentSeatNum = -1;
        currentIndex = -1;

        garageMgr.Display();
    }

    public void GetOffTheCar()
    {
        if (currentIndex == -1 || currentSeatNum == -1) return;
        if (playerDataMgr.saveData.boarding[currentIndex] != currentSeatNum) return;

        //json.
        playerDataMgr.saveData.boarding[currentIndex] = -1;

        //playerDataMgr.
        playerDataMgr.boardingSquad.Remove(currentSeatNum);
        if (playerDataMgr.boardingSquad.Count == 0) playerDataMgr.saveData.currentCar = null;

        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        //???絥????.
        var child = seats[currentSeatNum].transform.GetChild(0).gameObject;
        child.GetComponent<Text>().text = $"{currentSeatNum + 1}";
        child = seats[currentSeatNum].transform.GetChild(1).gameObject;
        var color = child.GetComponent<Image>().color;
        child.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
        child.GetComponent<Image>().sprite = nullImg;
        child = seats[currentSeatNum].transform.GetChild(2).gameObject;
        child.SetActive(false);
        child = seats[currentSeatNum].transform.GetChild(3).gameObject;
        child.SetActive(false);

        int index = currentIndex;

        var go = Instantiate(characterPrefab, ListContent.transform);
        
        go.GetComponent<DragDrop>().characterNum = index;
        go.GetComponent<DragDrop>().boardingMgr = this;

        var button = go.AddComponent<Button>();
        button.onClick.AddListener(delegate { SelectCharacter(index); });

        child = go.transform.GetChild(0).gameObject;
        child.GetComponent<Image>().sprite = playerDataMgr.currentSquad[currentIndex].character.halfImg;

        child = go.transform.GetChild(1).gameObject;
        var childObj = child.transform.GetChild(0).gameObject;
        childObj.GetComponent<Text>().text
            = $"{playerDataMgr.currentSquad[currentIndex].characterName}";

        childObj = child.transform.GetChild(1).gameObject;
        string mainWeaponTxt = (playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon == null) ?
            "????????" : playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.storeName;

        childObj.GetComponent<Text>().text
            = $"{playerDataMgr.currentSquad[currentIndex].character.name}/LV{playerDataMgr.currentSquad[currentIndex].level}/{mainWeaponTxt}";

        childObj = child.transform.GetChild(2).gameObject;
        var slider = childObj.GetComponent<Slider>();
        slider.maxValue = playerDataMgr.currentSquad[currentIndex].MaxHp;
        slider.value = playerDataMgr.currentSquad[currentIndex].currentHp;

        characters.Add(currentIndex, go);

        seats[currentSeatNum].GetComponent<Image>().color = Color.white;
        currentSeatNum = -1;
        currentIndex = -1;

        garageMgr.Display();
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
        if (selectedCar != null)
        {
            CarReset();
            CarDisplay(selectedCar);
        }
    }
}

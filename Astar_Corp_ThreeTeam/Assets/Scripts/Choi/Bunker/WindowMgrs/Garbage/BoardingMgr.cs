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
    int currentKey;
    string selectedCar;

    public PlayerDataMgr playerDataMgr;
    Color originColor;

    public void Init()
    {
        if (carListPopup.activeSelf) carListPopup.SetActive(false);

        //삭제.
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

        //생성.
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
            var button = go.AddComponent<Button>();
            button.onClick.AddListener(delegate { SelectCharacter(num); });

            var child = go.transform.GetChild(0).gameObject;
            child.GetComponent<Image>().sprite = element.Value.character.halfImg;

            child = go.transform.GetChild(1).gameObject;
            var childObj = child.transform.GetChild(0).gameObject;
            childObj.GetComponent<Text>().text
                = $"{element.Value.characterName}";

            childObj = child.transform.GetChild(1).gameObject;
            string mainWeaponTxt = (element.Value.weapon.mainWeapon == null) ?
                "비어있음" : element.Value.weapon.mainWeapon.name; 

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

            var child = seats[currentSeatNum].transform.GetChild(0).gameObject;
            child.GetComponent<Text>().text = playerDataMgr.currentSquad[currentIndex].character.name.Substring(0, 3);
        }

        //소유차량.
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

        //팝업.
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
        originColor = new Color(255, 192, 0);
    }

    public void PreviousButton()
    {
        if (currentKey - 1 < 0) return;

        currentKey--;
        selectedCar = carOrder[currentKey].id;

        CarReset();
        CarDisplay(selectedCar);
    }

    public void NextButton()
    {
        if (currentKey + 1 >= carOrder.Count) return;

        currentKey++;
        selectedCar = carOrder[currentKey].id;

        CarReset();
        CarDisplay(selectedCar);
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
    }

    private void ButtonInteractable()
    {
        left.interactable = (currentKey <= 0) ? false : true;
        right.interactable = (currentKey >= carOrder.Count - 1) ? false : true;
    }

    public void SelectCharacter(int i)
    {
        if (currentSeatNum == -1) return;

        if(currentIndex!=-1 && characters.ContainsKey(currentIndex))
        characters[currentIndex].GetComponent<Image>().color = originColor;

        currentIndex = i;
        characters[currentIndex].GetComponent<Image>().color = Color.red;
    }

    public void SelectSeat(int i)
    {
        if (currentSeatNum != -1) seats[currentSeatNum].GetComponent<Image>().color = Color.white;

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

    //탑승.
    public void GetInTheCar()
    {
        if (currentIndex == -1 || currentSeatNum == -1) return;

        //다른 사람이 탑승하고 있으면 스왑.
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

        //현재 데이터.
        var child = seats[currentSeatNum].transform.GetChild(0).gameObject;
        child.GetComponent<Text>().text = playerDataMgr.currentSquad[currentIndex].character.name.Substring(0,3);
        
        Destroy(characters[currentIndex]);
        characters.Remove(currentIndex);

        seats[currentSeatNum].GetComponent<Image>().color = Color.white;
        currentSeatNum = -1;
        currentIndex = -1;
    }

    public void GetOffTheCar()
    {
        if (currentIndex == -1 || currentSeatNum == -1) return;

        //json.
        playerDataMgr.saveData.boarding[currentIndex] = -1;

        //playerDataMgr.
        playerDataMgr.boardingSquad.Remove(currentSeatNum);
        if (playerDataMgr.boardingSquad.Count == 0) playerDataMgr.saveData.currentCar = null;

        PlayerSaveLoadSystem.Save(playerDataMgr.saveData);

        //현재데이터.
        var child = seats[currentSeatNum].transform.GetChild(0).gameObject;
        child.GetComponent<Text>().text = $"자리{currentSeatNum+1}";
        
        int index = currentIndex;
       
        var go = Instantiate(characterPrefab, ListContent.transform);
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
            "비어있음" : playerDataMgr.currentSquad[currentIndex].weapon.mainWeapon.name;

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

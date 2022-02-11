using TMPro;
using UnityEngine;

public class UIPrint : MonoBehaviour
{
    private PlayerDataMgr playerDataMgr;
    private ScriptableMgr soMgr;

    private TextMeshProUGUI squadNum;
    private TextMeshProUGUI invenWeight;

    void Start()
    {
        playerDataMgr = PlayerDataMgr.Instance;
        soMgr = ScriptableMgr.Instance;

        squadNum = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        invenWeight = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        // 생존 인원 / 차량 최대 수용 인원
        squadNum.text = $"{playerDataMgr.boardingSquad.Count} / {playerDataMgr.truckList[playerDataMgr.saveData.currentCar].capacity}";

        // 인벤토리 용량
        //invenWeight.text = $"{playerDataMgr.truckList[playerDataMgr.saveData.currentCar].weight} / {soMgr.truckList[playerDataMgr.saveData.currentCar].weight}";
    }
}

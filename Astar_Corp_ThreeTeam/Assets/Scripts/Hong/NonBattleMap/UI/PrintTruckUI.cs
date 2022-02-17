using System.Linq;
using TMPro;
using UnityEngine;

public class PrintTruckUI : MonoBehaviour
{
    private TextMeshProUGUI squadNum;
    private TextMeshProUGUI invenWeight;

    public int truckCurWeight;

    public void Init()
    {
        squadNum = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        invenWeight = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        // 생존 인원 / 차량 최대 수용 인원
        // 인벤토리 용량
        if (PlayerDataMgr.Instance.saveData.currentCar == null)
        {
            Debug.Log("임시 트럭 데이터 사용");
            squadNum.text = $"{PlayerDataMgr.Instance.boardingSquad.Count} / {ScriptableMgr.Instance.GetTruck("TRU_0001").capacity}";
            invenWeight.text = $"{truckCurWeight} / {ScriptableMgr.Instance.GetTruck("TRU_0001").weight}";
        }
        else
        {
            squadNum.text = $"{PlayerDataMgr.Instance.boardingSquad.Count} / {PlayerDataMgr.Instance.truckList[PlayerDataMgr.Instance.saveData.currentCar].capacity}";
            invenWeight.text = $"{truckCurWeight} / {PlayerDataMgr.Instance.truckList[PlayerDataMgr.Instance.saveData.currentCar].weight}";
        }
    }
}

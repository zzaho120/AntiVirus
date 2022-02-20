using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public int seatNum;
    public BoardingMgr boardingMgr;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        boardingMgr.SelectSeat(seatNum - 1);
        boardingMgr.GetInTheCar();

        //if (eventData.pointerDrag != null)
        //{
        //    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
        //        = GetComponent<RectTransform>().anchoredPosition;
        //}
    }
}

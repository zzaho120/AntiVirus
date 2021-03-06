using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EventType
{
    Init, 
    StartPlayer, 
    EndPlayer, 
    GetFrontSight, 
    StartEnemy, 
    EndEnemy, 
    ChangeTurn, 
    EndGame, 
    DetectAlert,
    DestroyChar
}

public static class EventBusMgr
{
    private static readonly
        Dictionary<EventType, UnityEvent<object[]>> eventCollection =
        new Dictionary<EventType, UnityEvent<object[]>>();

    public static void Subscribe
        (EventType eventType, UnityAction<object[]> listener)
    {
        UnityEvent<object[]> thisEvent;

        if (eventCollection.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<object[]>();
            thisEvent.AddListener(listener);
            eventCollection.Add(eventType, thisEvent);
        }
    }

    public static void Unsubscribe
        (EventType eventType, UnityAction<object[]> listener)
    {
        UnityEvent<object[]> thisEvent;

        if (eventCollection.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Publish(EventType eventType, object[] param = null)
    {
        UnityEvent<object[]> thisEvent;

        if (eventCollection.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.Invoke(param);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static Dictionary<int, Action> eventDictionary = new Dictionary<int, Action>();

    public static void StartListening(int eventName, Action listener)
    {
        Action thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(int eventName, Action listener)
    {
        Action thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Remove(eventName);
        }
    }

    public static void TriggerEvent(int eventName)
    {
        Action thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke();
        }
    }

    public static void CheckActionListners()
    {
        foreach(var dict in eventDictionary)
        {
            Debug.Log(dict.Value?.GetInvocationList().Length);
        }
    }
}

public class EventManager<T>
{
    private static Dictionary<int, Action<T>> eventDictionary = new Dictionary<int, Action<T>>();

    public static void StartListening(int eventName, Action<T> listener)
    {
        Action<T> thisEvent;
         
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(int eventName, Action<T> listener)
    {
        Action<T> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Remove(eventName);
        }
    }

    public static void TriggerEvent(int eventName, T param)
    {
        Action<T> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(param);
        }
    }
}

public class EventManager<T, Q>
{
    private static Dictionary<int, Action<T, Q>> eventDictionary = new Dictionary<int, Action<T, Q>>();

    public static void StartListening(int eventName, Action<T, Q> listener)
    {
        Action<T, Q> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(int eventName, Action<T, Q> listener)
    {
        Action<T, Q> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Remove(eventName);
        }
    }

    public static void TriggerEvent(int eventName, T param, Q param2)
    {
        Action<T, Q> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(param, param2);
        }
    }
}
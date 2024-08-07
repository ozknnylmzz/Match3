using System;
using System.Collections.Generic;

public class EventBus
{
    private static EventBus _instance;
    private readonly Dictionary<Enum, List<Delegate>> _eventTable;

    private EventBus()
    {
        _eventTable = new Dictionary<Enum, List<Delegate>>();
    }

    public static EventBus Instance => _instance ?? (_instance = new EventBus());

    public void Subscribe(Enum eventType, Action listener)
    {
        if (!_eventTable.ContainsKey(eventType))
        {
            _eventTable[eventType] = new List<Delegate>();
        }

        _eventTable[eventType].Add(listener);
    }

    public void Subscribe<T>(Enum eventType, Action<T> listener)
    {
        if (!_eventTable.ContainsKey(eventType))
        {
            _eventTable[eventType] = new List<Delegate>();
        }

        _eventTable[eventType].Add(listener);
    }

    public void Unsubscribe(Enum eventType, Action listener)
    {
        if (_eventTable.ContainsKey(eventType))
        {
            _eventTable[eventType].Remove(listener);
        }
    }

    public void Unsubscribe<T>(Enum eventType, Action<T> listener)
    {
        if (_eventTable.ContainsKey(eventType))
        {
            _eventTable[eventType].Remove(listener);
        }
    }

    public void Publish(Enum eventType)
    {
        if (_eventTable.ContainsKey(eventType))
        {
            foreach (var listener in _eventTable[eventType])
            {
                if (listener is Action action)
                {
                    action.Invoke();
                }
            }
        }
    }

    public void Publish<T>(Enum eventType, T eventData)
    {
        if (_eventTable.ContainsKey(eventType))
        {
            foreach (var listener in _eventTable[eventType])
            {
                if (listener is Action<T> action)
                {
                    action.Invoke(eventData);
                }
            }
        }
    }
}
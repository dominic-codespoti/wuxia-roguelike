using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, Dictionary<string, Delegate>> Observers = new();
    private const string GlobalTopic = "";

    public static void Subscribe<T>(Action<T> handler, string topic = null) where T : Event
    {
        topic ??= GlobalTopic;

        Type eventType = typeof(T);
        if (!Observers.ContainsKey(eventType))
        {
            Observers[eventType] = new Dictionary<string, Delegate>();
        }

        if (!Observers[eventType].ContainsKey(topic))
        {
            Observers[eventType][topic] = null;
        }

        Observers[eventType][topic] = Delegate.Combine(Observers[eventType][topic], handler);
    }

    public static void Unsubscribe<T>(string topic = null) where T : Event
    {
        topic ??= GlobalTopic;

        Type eventType = typeof(T);
        if (Observers.ContainsKey(eventType))
        {
            if (topic == GlobalTopic)
            {
                Observers[eventType].Clear();
            }
            else if (Observers[eventType].ContainsKey(topic))
            {
                Observers[eventType][topic] = null;
            }
        }
    }

    public static void Publish<T>(T eventData, string topic = null) where T : Event
    {
        Type eventType = typeof(T);
        if (Observers.ContainsKey(eventType))
        {
            Dictionary<string, Delegate> topicObservers = Observers[eventType];

            if (topic != null && topicObservers.ContainsKey(topic))
            {
                Delegate observers = topicObservers[topic];
                Action<T> handler = observers as Action<T>;
                if (handler != null)
                {
                    handler(eventData);
                    return;
                }
            }

            Delegate globalObservers = topicObservers[GlobalTopic];
            Action<T> globalHandler = globalObservers as Action<T>;
            if (globalHandler != null)
            {
                globalHandler(eventData);
            }
        }
    }
}

public abstract class Event { }

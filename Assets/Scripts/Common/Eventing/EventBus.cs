using System;
using System.Collections.Generic;

namespace Common.Eventing
{
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

            if (!Observers.TryGetValue(eventType, out var topicObservers))
            {
                return;
            }

            if (topic != null && topicObservers.TryGetValue(topic, out var handlerDelegate) && handlerDelegate is Action<T> handler)
            {
                handler(eventData);
                return;
            }

            if (topicObservers.TryGetValue(GlobalTopic, out var globalHandlerDelegate) && globalHandlerDelegate is Action<T> globalHandler)
            {
                globalHandler(eventData);
            }
        }
    }

    public abstract class Event { }
}
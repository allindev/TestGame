using System;
using System.Collections.Generic;

namespace Tool
{

    public static class EventManager
    {


        private static Dictionary<Type, List<IEventListenerBase>> _subscribersList;

        static EventManager()
        {
            _subscribersList = new Dictionary<Type, List<IEventListenerBase>>();
        }


        public static void AddListener<T>(IEventListener<T> listener) where T:struct
        {

            Type eventType = typeof(T);
            if (!_subscribersList.ContainsKey(eventType))
                _subscribersList[eventType] = new List<IEventListenerBase>();

            if (!SubscriptionExists(eventType, listener))
                _subscribersList[eventType].Add(listener);

        }


        public static void RemoveListener<T>(IEventListener<T> listener) where T : struct
        {
            Type eventType = typeof(T);
            if (!_subscribersList.ContainsKey(eventType))
            {
                return;
            }
            List<IEventListenerBase> subscriberList = _subscribersList[eventType];
            for (int i = 0; i < subscriberList.Count; i++)
            {
                if (subscriberList[i] == listener)
                {
                    subscriberList.Remove(subscriberList[i]);

                    if (subscriberList.Count == 0)
                        _subscribersList.Remove(eventType);

                    return;
                }
            }



        }


        public static void TriggerEvent<T>(T newEvent) where T : struct
        {

            List<IEventListenerBase> list;
            if (!_subscribersList.TryGetValue(typeof(T), out list))
                return;
            for (int i = 0; i < list.Count; i++)
            {
                var eventListener = list[i] as IEventListener<T>;
                if (eventListener != null) eventListener.OnEvent(newEvent);
            }
        }


        private static bool SubscriptionExists(Type type, IEventListenerBase receiver)
        {
            List<IEventListenerBase> receivers;
            if (!_subscribersList.TryGetValue(type, out receivers)) return false;
            bool exists = false;
            for (int i = 0; i < receivers.Count; i++)
            {
                if (receivers[i] == receiver)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }


    }


    public static class EventRegister
    {
       
        public static void EventStartListening<T>(this IEventListener<T> caller) where T : struct
        {
            EventManager.AddListener<T>(caller);
        }

        public static void EventStopListening<T>(this IEventListener<T> caller) where T : struct
        {
            EventManager.RemoveListener<T>(caller);
        }

    }





    public interface IEventListenerBase { };

    public interface IEventListener<T> : IEventListenerBase
    {
        void OnEvent(T eventType);
    }



}

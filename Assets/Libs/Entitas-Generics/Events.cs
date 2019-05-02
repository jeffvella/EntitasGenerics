using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Events
{
    public class TestEventComponent : GameEventBase<(string, string)>
    {

    }

    public static class Events
    {
        public static TestEventComponent TestEvent { get; } = new TestEventComponent();
    }


    public interface IEventObserver { }

    public interface IEventObserver<in T, in TArg>
    {
        void OnRaised(T eventInfo, TArg value);
    }

    public interface IEventObserver<in TArg>
    {
        void OnRaised(TArg value);
    }

    public class ActionEventDelegator<T, TArg> : IEventObserver<T, TArg>, IEventObserver
    {
        public ActionEventDelegator(Action<T, TArg> action)
        {
            _action = action;
        }

        private readonly Action<T, TArg> _action;

        public void OnRaised(T eventInfo, TArg value)
        {
            _action.Invoke(eventInfo, value);
        }
    }

    public class ActionEventDelegator<TArg> : IEventObserver<TArg>, IEventObserver
    {
        public ActionEventDelegator(Action<TArg> action)
        {
            _action = action;
        }

        private readonly Action<TArg> _action;

        public void OnRaised(TArg value)
        {
            _action.Invoke(value);
        }
    }

    /// <summary>
    /// A <see cref="ScriptableObject"/> based event that can notify event listeners, with an argument and fixed event info.
    /// </summary>
    /// <typeparam name="TInfo">Static info unique to the event instance; configured in the property inspector for the <see cref="ScriptableObject"/></typeparam>
    /// <typeparam name="TArgs">Dynamic info specific to the each occurence of the event</typeparam>
    public class GameEventBase<TInfo, TArgs>// : GameEventBaseScriptableObject where TInfo : struct where TArgs : struct
    {
        private TInfo _eventInfo;

        public void SetEventInfo(TInfo info)
        {
            _eventInfo = info;
        }

        private readonly List<IEventObserver<TInfo, TArgs>> _observers = new List<IEventObserver<TInfo, TArgs>>();

        public void Register(Action<TInfo, TArgs> action)
        {
            Register(new ActionEventDelegator<TInfo, TArgs>(action));
        }

        public void Register(IEventObserver<TInfo, TArgs> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Deregister(IEventObserver<TInfo, TArgs> observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        public void Raise(TArgs args)
        {
            //OnBeforeRaised(EventInfo, args);

            for (int i = _observers.Count - 1; i >= 0; i--)
            {
                // todo automatically remove null/destroyed observers
                _observers[i]?.OnRaised(_eventInfo, args);
            }

            //OnAfterRaised(EventInfo, args);
        }

        ///// <summary>
        ///// Called before observers are notified of the event.
        ///// </summary>
        //public virtual void OnBeforeRaised(TInfo info, TArgs args) { }

        ///// <summary>
        ///// Called after observers are notified of the event.
        ///// </summary>
        //public virtual void OnAfterRaised(TInfo info, TArgs args) { }

    }

    /// <summary>
    /// A <see cref="ScriptableObject"/> based event that can notify event listeners, with one argument.
    /// </summary>
    /// <typeparam name="TArgs">Dynamic info specific to the each occurence of the event</typeparam>
    public class GameEventBase<TArgs> : IListenerComponent<TArgs> //: GameEventBaseScriptableObject //where TArgs : struct
    {
        private readonly List<IEventObserver<TArgs>> _observers = new List<IEventObserver<TArgs>>();

        public void Register(Action<TArgs> action)
        {
            Register(new ActionEventDelegator<TArgs>(action));
        }

        public void Register(IEventObserver<TArgs> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Deregister(IEventObserver<TArgs> observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        public void Raise(TArgs arg)
        {
            //OnBeforeRaised(arg);

            for (int i = _observers.Count - 1; i >= 0; i--)
            {
                // todo automatically remove null/destroyed observers
                _observers[i]?.OnRaised(arg);
            }

            //OnAfterRaised(arg);
        }

        ///// <summary>
        ///// Called before observers are notified of the event. Useful for debugging/logging,
        ///// e.g setting breakpoints in derived class lets you filter to that specific type.
        ///// </summary>
        //protected virtual void OnBeforeRaised(TArgs args) { }

        ///// <summary>
        ///// Called after observers are notified of the event. Useful for debugging/logging,
        ///// e.g setting breakpoints in derived class lets you filter to that specific type.
        ///// </summary>
        //protected virtual void OnAfterRaised(TArgs args) { }
    }

    //public static class EventHelper
    //{
    //    private static readonly Func<int, Object> FindObjectFromInstanceId;

    //    static EventHelper()
    //    {
    //        var methodInfo = typeof(Object).GetMethod("FindObjectFromInstanceID", BindingFlags.NonPublic | BindingFlags.Static);
    //        if (methodInfo == null)
    //            Debug.LogError("FindObjectFromInstanceID was not found in UnityEngine.Object");
    //        else
    //            FindObjectFromInstanceId = (Func<int, Object>)Delegate.CreateDelegate(typeof(Func<int, Object>), methodInfo);
    //    }

    //    public static T FindObjectById<T>(int instanceId) where T : Object
    //    {
    //        return (T)FindObjectFromInstanceId.Invoke(instanceId);
    //    }
    //}

    //public class GameEventBaseScriptableObject : ScriptableObject
    //{

    //}

}



using System;
using System.Collections.Generic;
using System.Reflection;
using Entitas;
using Entitas.Generics;
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

    //public interface IEventObserver<in T, in TArg>
    //{
    //    void OnEvent(T eventInfo, TArg value);
    //}

    public interface IEventObserver<TContext, TEntity, TComponent> 
        where TContext : IGenericContext<TEntity> where TEntity : class, IEntity, new()
    {
        void OnEvent((TContext Context, TEntity Entity, TComponent Component) args);
    }

    public interface IEventObserver<TEntity, TComponent>
    {
        void OnEvent((TEntity Entity, TComponent Component) args);
    }

    public interface IEventObserver<in TArg>
    {
        void OnEvent(TArg value);

        //void OnEvent(IEntity entity, TArg value);
    }

    public class ActionEventDelegator<T, TArg> : IEventObserver<T, TArg>, IEventObserver
    {
        public ActionEventDelegator(Action<T, TArg> action)
        {
            _action = action;
        }

        private readonly Action<T, TArg> _action;

        public void OnEvent(T eventInfo, TArg value)
        {
            _action.Invoke(eventInfo, value);
        }

        public void OnEvent((T Entity, TArg Component) args)
        {
            _action.Invoke(args.Entity, args.Component);
        }
    }

    public class ActionEventDelegator<TArg> : IEventObserver<TArg>, IEventObserver
    {
        public ActionEventDelegator(Action<TArg> action)
        {
            _action = action;
        }

        private readonly Action<TArg> _action;

        public void OnEvent(TArg value)
        {
            _action.Invoke(value);
        }

        //public void OnEvent(IEntity entity, TArg value)
        //{
        //    _action.Invoke(entity, value);
        //}
    }



    /// <summary>
    /// A <see cref="ScriptableObject"/> based event that can notify event listeners, with an argument and fixed event info.
    /// </summary>
    /// <typeparam name="TEntity">Static info unique to the event instance; configured in the property inspector for the <see cref="ScriptableObject"/></typeparam>
    /// <typeparam name="TArgs">Dynamic info specific to the each occurence of the event</typeparam>
    public class GameEventBase<TEntity, TArgs> : IListenerComponent<TEntity, TArgs> where TEntity : IEntity
        // : GameEventBaseScriptableObject where TInfo : struct where TArgs : struct
    {
        private TEntity _entity;

        public void SetEntity(TEntity entity)
        {
            _entity = entity;
        }

        private readonly List<IEventObserver<TEntity, TArgs>> _observers = new List<IEventObserver<TEntity, TArgs>>();

        public void Register(Action<TEntity, TArgs> action)
        {
            Register(new ActionEventDelegator<TEntity, TArgs>(action));
        }

        public void Register(IEventObserver<TEntity, TArgs> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Deregister(IEventObserver<TEntity, TArgs> observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }


        public void Register(Action<TArgs> action)
        {
            throw new NotImplementedException();
        }

        public void Register(IEventObserver<TArgs> observer)
        {
            throw new NotImplementedException();
        }

        public void Deregister(IEventObserver<TArgs> observer)
        {
            throw new NotImplementedException();
        }

        public void Raise(TEntity entity, TArgs arg)
        {
            throw new NotImplementedException();
        }

        //public void Register(Action<TArgs> action)
        //{
        //    Register((info, args) => );
        //}

        //public void Register(IEventObserver<TArgs> observer)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Deregister(IEventObserver<TArgs> observer)
        //{
        //    throw new NotImplementedException();
        //}

        public void Raise(TArgs args)
        {
            //OnBeforeRaised(EventInfo, args);

            for (int i = _observers.Count - 1; i >= 0; i--)
            {
                // todo automatically remove null/destroyed observers
                _observers[i]?.OnEvent((_entity, args));
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
                _observers[i]?.OnEvent(arg);
            }

            //OnAfterRaised(arg);
        }

        //public void Raise(IEntity entity, TArgs arg)
        //{
        //    for (int i = _observers.Count - 1; i >= 0; i--)
        //    {
        //        // todo automatically remove null/destroyed observers
        //        _observers[i]?.OnEvent(entity, arg);
        //    }
        //}

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



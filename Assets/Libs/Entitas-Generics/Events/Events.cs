using Entitas.VisualDebugging.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entitas.Generics
{
    public interface IEventListener { }

    //public interface IAddedComponentListener<in TEntity, in TComponent> : IEventListener
    //{
    //    void OnComponentAdded(TEntity entity, TComponent component);
    //}

    public interface IAddedComponentListener<in TEntity> : IEventListener where TEntity : IGenericEntity
    {
        void OnComponentAdded(TEntity entity);
    }

    public interface IAddedComponentListener<in TEntity, TComponent> : IAddedComponentListener<TEntity> where TEntity : IGenericEntity
    {

    }

    public interface IRemovedComponentListener<in TEntity> : IEventListener where TEntity : IGenericEntity
    {
        void OnComponentRemoved(TEntity entity);
    }

    public interface IRemovedComponentListener<in TEntity, TComponent> : IRemovedComponentListener<TEntity> where TEntity : IGenericEntity
    {

    }

    //public interface IRemovedComponentListener<in TEntity> : IEventListener
    //{
    //    void OnComponentRemoved(TEntity entity);
    //}

    //public interface IEventObserver<in TArg>
    //{
    //    void OnEvent(TEntity entity);
    //}

    ///// <summary>
    ///// A wrapper to allow event registrations from an Action.
    ///// </summary>
    ///// <typeparam name="TArg">An arguyment to be passed to event listeners</typeparam>
    //public class AddedActionEventDelegator<TArg> : ActionEventDelegator
    //{        public void OnEvent(TArg value)
    //    {
    //        _invocations++;
    //        _action.Invoke(value);
    //    }

    //}

    //public class AddedActionEventDelegator<TEntity> : IAddedComponentListener<TEntity>, IEventListener, ICustomDisplayName where TEntity : IEntity, IGenericEntity
    //{
    //    private int _invocations;

    //    public AddedActionEventDelegator(Action<TEntity> action)
    //    {
    //        _action = action;
    //    }

    //    private readonly Action<TEntity> _action;

    //    public void OnComponentAdded(TEntity entity)
    //    {
    //        _invocations++;
    //        _action.Invoke(entity);
    //    }

    //    public string DisplayName => $"{_action.Target} InvokeCount={_invocations}";
    //}

    //public class RemovedActionEventDelegator<TEntity> : IRemovedComponentListener<TEntity>, IEventListener, ICustomDisplayName where TEntity : IEntity, IGenericEntity
    //{
    //    private int _invocations;

    //    public RemovedActionEventDelegator(Action<TEntity> action)
    //    {
    //        _action = action;
    //    }

    //    private readonly Action<TEntity> _action;

    //    public void OnComponentRemoved(TEntity entity)
    //    {
    //        _invocations++;
    //        _action.Invoke(entity);
    //    }

    //    public string DisplayName => $"{_action.Target} InvokeCount={_invocations}";
    //}

    ///// <summary>
    ///// A wrapper to allow event registrations from an Action.
    ///// </summary>
    ///// <typeparam name="TArg">An arguyment to be passed to event listeners</typeparam>
    //public class ActionEventDelegator<TArg> : IEventObserver<TArgs>, IEventListener, ICustomDisplayName
    //{
    //    private int _invocations;

    //    public ActionEventDelegator(Action<TArg> action)
    //    {
    //        _action = action;
    //    }

    //    private readonly Action<TArg> _action;

    //    public void OnEvent(TArg value)
    //    {
    //        _invocations++;
    //        _action.Invoke(value);
    //    }

    //    public string DisplayName => $"{_action.Target} InvokeCount={_invocations}";
    //}

    /// <summary>
    /// A base for an event that can store listeners/observers and foward data to them.
    /// </summary>
    /// <typeparam name="TArgs">Dynamic info specific to the each occurence of the event</typeparam>
    public abstract class AbstractGameEvent<TArgs> : IListenerComponent<TArgs>
    {
        public int ListenerCount => Listeners.Count;

        // Must be public for debug drawer display within Entitas (it copies only public members)
        public List<IEventListener> Listeners = new List<IEventListener>();

        public void Register(IEventListener listener)
        {
            if (!Listeners.Contains(listener))
            {
                Listeners.Add(listener);
            }
        }

        public void Deregister(IEventListener observer)
        {
            if (Listeners.Contains(observer))
            {
                Listeners.Remove(observer);
            }
        }

        public abstract void Register(Action<TArgs> action);

        public abstract void Deregister(Action<TArgs> action);

        public abstract void Raise(TArgs arg);

        public void ClearListeners()
        {
            Listeners.Clear();
        }

        public string[] GetListenersNames() => Listeners.Select(observer =>
        {
            if (observer is ICustomDisplayName delegator)
            {
                return delegator.DisplayName;
            }
            return observer.GetType().PrettyPrintGenericTypeName();

        }).ToArray();


    }

}



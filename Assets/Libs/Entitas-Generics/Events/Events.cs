using Entitas.VisualDebugging.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entitas.Generics
{
    public interface IEventListener { }

    public interface IAddedComponentListener<in TEntity, in TComponent> : IEventListener
    {
        void OnComponentAdded(TEntity entity, TComponent component);
    }

    public interface IRemovedComponentListener<in TEntity> : IEventListener
    {
        void OnComponentRemoved(TEntity entity);
    }

    public interface IEventObserver<in TArg>
    {
        void OnEvent(TArg value);
    }

    /// <summary>
    /// A wrapper to allow event registrations from an Action.
    /// (Event observers must implement the IEventObserver<typeparamref name="TArg"/> interface).
    /// It also tracks some debug information to help with understanding events in the inspector.
    /// </summary>
    /// <typeparam name="TArg">An arguyment to be passed to event listeners</typeparam>
    public class ActionEventDelegator<TArg> : IEventObserver<TArg>, IEventListener, ICustomDisplayName
    {
        private int _invocations;

        public ActionEventDelegator(Action<TArg> action)
        {
            _action = action;
        }

        private readonly Action<TArg> _action;

        public void OnEvent(TArg value)
        {
            _invocations++;
            _action.Invoke(value);
        }

        public string DisplayName => $"{_action.Target} InvokeCount={_invocations}";
    }

    /// <summary>
    /// A base for an event that can store listeners/observers and foward data to them.
    /// </summary>
    /// <typeparam name="TArgs">Dynamic info specific to the each occurence of the event</typeparam>
    public class GameEventBase<TArgs> : IListenerComponent<TArgs>
    {
        public int ListenerCount => Observers.Count;

        // Must be public for debug drawer display within Entitas (it copies only public members)
        public List<IEventObserver<TArgs>> Observers = new List<IEventObserver<TArgs>>();

        public void Register(Action<TArgs> action)
        {
            Register(new ActionEventDelegator<TArgs>(action));
        }

        public void Register(IEventObserver<TArgs> observer)
        {
            if (!Observers.Contains(observer))
            {
                Observers.Add(observer);
            }
        }

        public void Deregister(IEventObserver<TArgs> observer)
        {
            if (Observers.Contains(observer))
            {
                Observers.Remove(observer);
            }
        }

        public void Raise(TArgs arg)
        {
            for (int i = Observers.Count - 1; i >= 0; i--)
            {
                Observers[i]?.OnEvent(arg);
            }
        }

        public void ClearListeners()
        {
            Observers.Clear();
        }

        public string[] GetListenersNames() => Observers.Select(observer =>
        {
            if (observer is ICustomDisplayName delegator)
            {
                return delegator.DisplayName;
            }
            return observer.GetType().PrettyPrintGenericTypeName();

        }).ToArray();
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entitas;
using Entitas.Generics;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Events
{

    public interface IEventListener { }

    public interface IEventObserver<TContext, TEntity, TComponent> 
        where TContext : IGenericContext<TEntity> where TEntity : class, IEntity, new()
    {
        void OnEvent((TContext Context, TEntity Entity, TComponent Component) args);
    }

    public interface IEventObserver<TEntity, TComponent>
    {
        void OnEvent((TEntity Entity, TComponent Component) args);
    }

    public interface IAddedComponentListener<in TEntity, in TComponent> : IEventListener
    {
        void OnComponentAdded(TEntity entity, TComponent component);
    }

    public interface IAddedEventObserver<in TEntity> : IEventListener
    {
        void OnComponentAdded(TEntity entity);
    }

    public interface IRemovedComponentListener<in TEntity> : IEventListener
    {
        void OnComponentRemoved(TEntity entity);
    }

    public interface IEventObserver<in TArg>
    {
        void OnEvent(TArg value);
    }

    public class AddedActionEventDelegator<TEntity, TComponent> : IAddedComponentListener<TEntity, TComponent>, IEventListener, ICustomDebugInfo
    {
        private int _invocations;

        public AddedActionEventDelegator(Action<TEntity, TComponent> action)
        {
            _action = action;
        }

        private readonly Action<TEntity, TComponent> _action;

        public void OnComponentAdded(TEntity entity, TComponent component)
        {
            _invocations++;
            _action.Invoke(entity, component);
        }

        public string DisplayName => $"{_action.Target} InvokeCount={_invocations}";
    }

    public class AddedActionEventDelegator<TEntity> : IAddedEventObserver<TEntity>, IEventListener, ICustomDebugInfo
    {
        private int _invocations;

        public AddedActionEventDelegator(Action<TEntity> action)
        {
            _action = action;
        }

        private readonly Action<TEntity> _action;

        public void OnComponentAdded(TEntity entity)
        {
            _invocations++;
            _action.Invoke(entity);
        }

        public string DisplayName => $"{_action.Target} InvokeCount={_invocations}";
    }

    public class RemovedActionEventDelegator<TEntity> : IRemovedComponentListener<TEntity>, IEventListener, ICustomDebugInfo
    {
        private int _invocations;

        public RemovedActionEventDelegator(Action<TEntity> action)
        {
            _action = action;
        }

        private readonly Action<TEntity> _action;

        public void OnComponentRemoved(TEntity entity)
        {
            _invocations++;
            _action.Invoke(entity);
        }

        public string DisplayName => $"{_action.Target} InvokeCount={_invocations}";
    }

    public class ActionEventDelegator<TArg> : IEventObserver<TArg>, IEventListener, ICustomDebugInfo
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
    /// A <see cref="ScriptableObject"/> based event that can notify event listeners, with one argument.
    /// </summary>
    /// <typeparam name="TArgs">Dynamic info specific to the each occurence of the event</typeparam>
    public class GameEventBase<TArgs> : IListenerComponent<TArgs>
    {
        public int ListenerCount => Observers.Count;

        // Must be public for debug drawer display within Entitas
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
            if (observer is ICustomDebugInfo delegator)
            {
                return delegator.DisplayName;
            }
            return observer.GetType().Name;

        }).ToArray();
    }
}



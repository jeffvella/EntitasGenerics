using System;
using System.Collections.Generic;
using Entitas.VisualDebugging.Unity;

namespace Entitas.Generics
{
    public class RemovedListenersComponent<TEntity, TComponent> : AbstractGameEvent<TEntity>, ICustomDisplayName
        where TEntity : IEntity, IGenericEntity
        where TComponent : IComponent
    {
        public string DisplayName => $"Removed Event Listener ({typeof(TComponent).Name})";

        public override void Raise(TEntity arg)
        {
            for (int i = Listeners.Count - 1; i >= 0; i--)
            {
                ((IRemovedComponentListener<TEntity>)Listeners[i])?.OnComponentRemoved(arg);
            }
        }

        public override void Register(Action<TEntity> action)
        {
            var listener = new RemovedActionEventDelegator<TEntity>(action);
            if (!Listeners.Contains(listener))
            {
                Listeners.Add(listener);
            }
        }

        public override void Deregister(Action<TEntity> action)
        {
            var listener = new RemovedActionEventDelegator<TEntity>(action);
            if (!Listeners.Contains(listener))
            {
                Listeners.Remove(listener);
            }
        }
    }

    public class RemovedActionEventDelegator<TEntity> : IRemovedComponentListener<TEntity>, IEqualityComparer<RemovedActionEventDelegator<TEntity>>, IEventListener, ICustomDisplayName 
        where TEntity : IEntity, IGenericEntity
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

        public bool Equals(RemovedActionEventDelegator<TEntity> x, RemovedActionEventDelegator<TEntity> y)
        {
            return x._action == y._action; 
        }

        public int GetHashCode(RemovedActionEventDelegator<TEntity> obj)
        {
            return _action.GetHashCode();
        }

        public string DisplayName => $"{_action.Target} InvokeCount={_invocations}";
    }
}
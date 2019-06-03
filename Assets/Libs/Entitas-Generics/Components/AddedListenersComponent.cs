using Entitas.VisualDebugging.Unity;
using System;
using System.Collections.Generic;

namespace Entitas.Generics
{
  
    public class AddedListenersComponent<TEntity, TComponent> : AbstractGameEvent<TEntity>, ICustomDisplayName
        where TEntity : IEntity, IGenericEntity 
        where TComponent : IComponent
    {
        public override void Raise(TEntity arg)
        {          
            for (int i = Listeners.Count - 1; i >= 0; i--)
            {
                ((IAddedComponentListener<TEntity>)Listeners[i])?.OnComponentAdded(arg);
            }           
        }

        public override void Register(Action<TEntity> action)
        {
            var listener = new AddedActionEventDelegator<TEntity>(action);
            if (!Listeners.Contains(listener))
            {
                Listeners.Add(listener);
            }
        }

        public override void Deregister(Action<TEntity> action)
        {
            var listener = new AddedActionEventDelegator<TEntity>(action);
            if (!Listeners.Contains(listener))
            {
                Listeners.Remove(listener);
            }
        }

        public string DisplayName => $"Added Event Listener ({typeof(TComponent).Name})";
    }

    /// <summary>
    /// A wrapper to allow registering event listeners with an Action.
    /// </summary>
    public class AddedActionEventDelegator<TEntity> : IAddedComponentListener<TEntity>, IEqualityComparer<AddedActionEventDelegator<TEntity>>, IEventListener, ICustomDisplayName 
        where TEntity : IEntity, IGenericEntity
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

        public bool Equals(AddedActionEventDelegator<TEntity> x, AddedActionEventDelegator<TEntity> y)
        {
            return x._action == y._action;
        }

        public int GetHashCode(AddedActionEventDelegator<TEntity> obj)
        {
            return _action.GetHashCode();
        }

        public string DisplayName => $"{_action.Target} InvokeCount={_invocations}";
    }

}


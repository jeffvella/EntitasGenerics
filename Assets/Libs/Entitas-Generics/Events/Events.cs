using Entitas.VisualDebugging.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entitas.Generics
{
    public interface IEventListener
    {

    }

    public interface IAddedComponentListener<in TEntity, TComponent> : IAddedComponentListener<TEntity> where TEntity : IGenericEntity
    {

    }

    public interface IRemovedComponentListener<in TEntity, TComponent> : IRemovedComponentListener<TEntity> where TEntity : IGenericEntity
    {

    }

    public interface IAddedComponentListener<in TEntity> : IEventListener where TEntity : IGenericEntity
    {
        void OnComponentAdded(TEntity entity);
    }

    public interface IRemovedComponentListener<in TEntity> : IEventListener where TEntity : IGenericEntity
    {
        void OnComponentRemoved(TEntity entity);
    }

    public abstract class AbstractGameEvent<TArgs> : IListenerComponent<TArgs>
    {
        public int ListenerCount => Listeners.Count;

        // Must be public for debug drawer display within Entitas (which only copies public members)
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



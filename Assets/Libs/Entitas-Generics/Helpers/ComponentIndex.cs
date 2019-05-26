using Entitas;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Entitas.Generics
{
    public interface IIndexedComponent : IComponent { }

    public interface IIndexedComponent<in TComponent> : IEqualityComparer<TComponent>, IIndexedComponent
    {

    }    

    public class ComponentIndex<TContext, TEntity, TComponent> : IComponentSearchIndex<TEntity>
        where TContext : IContext
        where TEntity : class, IEntity, new() 
        where TComponent : IComponent, new()
    {
        private readonly ConcurrentDictionary<TComponent, TEntity> _index;
        private readonly ConcurrentStack<TComponent> _pool;
        private IEqualityComparer<TComponent> _comparer;

        public ComponentIndex(IEqualityComparer<TComponent> comparer)
        {
            _comparer = comparer;
            _index = new ConcurrentDictionary<TComponent, TEntity>(comparer);
            _pool = new ConcurrentStack<TComponent>();
        }

        public bool TryFindEntity<T>(Action<T> componentValueProducer, out TEntity entity) where T : IIndexedComponent, new()
        {
            if (!_pool.TryPop(out TComponent component))
            {
                component = new TComponent();
            }
            if (!(component is T tComponent))
            {
                throw new InvalidCastException($"{component.GetType()} is not valid");
            }

            componentValueProducer?.Invoke(tComponent);

            if (_index.TryGetValue(component, out TEntity e))
            {
                entity = e;
                _pool.Push(component);
                return true;
            }

            _pool.Push(component);
            entity = default;
            return false;
        }

        public bool Contains<T>(Action<T> componentValueProducer) where T : IIndexedComponent, new()
        {
            if (!_pool.TryPop(out TComponent component))
            {
                component = new TComponent();
            }
            if (!(component is T tComponent))
            {
                throw new InvalidCastException($"{component.GetType()} is not valid");
            }

            componentValueProducer?.Invoke(tComponent);

            if (_index.ContainsKey(component))
            {
                _pool.Push(component);
                return true;
            }

            _pool.Push(component);
            return false;
        }

        public void Update(TEntity entity, IComponent previouscomponent, IComponent newcomponent)
        {
            if (!(previouscomponent is TComponent tPreviousComponent))
            {
                throw new InvalidCastException($"{previouscomponent.GetType()} is not valid for for ComponentIndex {typeof(TComponent)}");
            }
            if (!(newcomponent is TComponent tNewComponent))
            {
                throw new InvalidCastException($"{newcomponent.GetType()} is not valid for for ComponentIndex {typeof(TComponent)}");
            }
            _index.TryRemove(tPreviousComponent, out var removed);
            _index.AddOrUpdate(tNewComponent, entity, UpdateValue);
        }

        public void Add(TEntity entity, IIndexedComponent component)
        {
            if (!(component is TComponent tComponent))
            {
                throw new InvalidCastException($"{component.GetType()} is not valid for for ComponentIndex {typeof(TComponent)}");
            }
            _index.AddOrUpdate(tComponent, entity, UpdateValue);
        }

        private static TEntity UpdateValue(TComponent component, TEntity entity)
        {            
            return entity;
        }

        public void Remove(IIndexedComponent component)
        {
            if (!(component is TComponent tComponent))
            {
                throw new InvalidCastException($"{component.GetType()} is not valid for for ComponentIndex {typeof(TComponent)}");
            }
            _index.TryRemove(tComponent, out var removedEntity);
        }

        public void Clear(TEntity entity = null)
        {
            if (entity == null)
            {
                _index.Clear();
                return;
            }
            foreach (var pair in _index)
            {
                if (pair.Value == entity)
                {
                    _index.TryRemove(pair.Key, out var removed);
                }
            }
        }

    }

    public interface IComponentSearchIndex<TEntity> where TEntity : class, IEntity, new()
    {
        void Add(TEntity entity, IIndexedComponent component);

        void Remove(IIndexedComponent component);

        bool TryFindEntity<TComponent>(Action<TComponent> componentValueProducer, out TEntity entity) where TComponent : IIndexedComponent, new();

        bool Contains<TComponent>(Action<TComponent> componentValueProducer) where TComponent : IIndexedComponent, new();

        void Update(TEntity entity, IComponent previouscomponent, IComponent newcomponent);

        void Clear(TEntity entity = null);
    }


}

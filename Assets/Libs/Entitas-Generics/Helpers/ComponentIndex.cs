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

    public interface ILinkedComponent : IComponent
    {
        int Index { get; set; }

        IEntity Entity { get; set; }
    }

    public interface IValueComponent<TValue> : ILinkedComponent
    {
        TValue Value { get; set; }
    }

    //public interface IValueController<TValue> : IEntityLinkedComponent, IComponent
    //{
    //    void InternalSet(TValue value);

    //    TValue InternalGet();
    //}

    public class ComponentIndex<TEntity, TComponent> : IComponentSearchIndex<TEntity>
        where TEntity : class, IEntity, new() 
        where TComponent : IIndexedComponent, new()
    {
        private readonly ConcurrentDictionary<TComponent, TEntity> _index;
        private readonly Stack<TComponent> _pool;

        public ComponentIndex(IEqualityComparer<TComponent> comparer)
        {
            _index = new ConcurrentDictionary<TComponent, TEntity>(comparer);
            _pool = new Stack<TComponent>();
        }

        public bool TryFindEntity(Action<TComponent> componentValueProducer, out TEntity entity) //where TCom : IIndexedComponent, TComponent, new()
        {
            TComponent component = _pool.Count != 0 ? _pool.Pop() : new TComponent();

            componentValueProducer?.Invoke(component);

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

        public bool TryFindEntity<TValue>(TValue value, out TEntity entity) //where TComponent : IValueComponent<TValue> //where TCom : IIndexedComponent, TComponent, new()
        {
            TComponent component = _pool.Count != 0 ? _pool.Pop() : new TComponent();

            ((IValueComponent<TValue>)component).Value = value;

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

        public void Update(TEntity entity, IComponent previouscomponent, IComponent newcomponent)
        {
            //if (!(previouscomponent is TComponent tPreviousComponent))
            //{
            //    throw new InvalidCastException($"{previouscomponent.GetType()} is not valid for for ComponentIndex {typeof(TComponent)}");
            //}
            //if (!(newcomponent is TComponent tNewComponent))
            //{
            //    throw new InvalidCastException($"{newcomponent.GetType()} is not valid for for ComponentIndex {typeof(TComponent)}");
            //}
            _index.TryRemove((TComponent)previouscomponent, out var removed);
            _index.AddOrUpdate((TComponent)newcomponent, entity, UpdateValue);
        }

        public void Add(TEntity entity, IIndexedComponent component)
        {
            //if (!(component is TComponent tComponent))
            //{
            //    throw new InvalidCastException($"{component.GetType()} is not valid for for ComponentIndex {typeof(TComponent)}");
            //}
            _index.AddOrUpdate((TComponent)component, entity, UpdateValue);
        }

        private static TEntity UpdateValue(TComponent component, TEntity entity)
        {            
            return entity;
        }

        public void Remove(IIndexedComponent component)
        {
            //if (!(component is TComponent tComponent))
            //{
            //    throw new InvalidCastException($"{component.GetType()} is not valid for for ComponentIndex {typeof(TComponent)}");
            //}
            _index.TryRemove((TComponent)component, out var removedEntity);
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

    public interface IComponentSearchIndex<in TEntity> 
        where TEntity : class, IEntity, new()
    {
        void Add(TEntity entity, IIndexedComponent component);

        void Remove(IIndexedComponent component);

        void Update(TEntity entity, IComponent previouscomponent, IComponent newcomponent);

        void Clear(TEntity entity = null);
    }

    public interface IComponentSearchIndex<TEntity, out TComponent> : IComponentSearchIndex<TEntity> 
        where TEntity : class, IEntity, new() 
        where TComponent : IIndexedComponent, new()
    {
        bool TryFindEntity(Action<TComponent> componentValueProducer, out TEntity entity);
    }

}

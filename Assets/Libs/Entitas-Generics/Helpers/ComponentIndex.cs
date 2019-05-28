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
    public interface ISearchableComponent : IComponent
    {

    }

    public interface ISearchableComponent<TComponent> : IEqualityComparer<TComponent>, ISearchableComponent
    {

    }

    /// <summary>
    /// A component that is aware of its associated entity and context-specific component index
    /// </summary>    
    public interface ILinkedComponent : IComponent
    {
        int Index { get; set; }

        IEntity Entity { get; set; }
    }

    public interface IValueComponent : IComponent
    {

    }

    /// <summary>
    /// A component that contains a value.
    /// </summary>
    public interface IValueComponent<TValue> : IValueComponent
    {
        TValue Value { get; set; }
    }

    public interface ISearchKeyProvider<TValue> : IValueComponent
    {
        //Func<IComponent, TValue> IndexKeyRetriever { get; }

        Func<TValue> ValueProducer { get; }
    }

    //public static class SearcherExtensions
    //{ 

    //    public static void FindEntity<TComponent, TValue>(this TComponent component, TValue value) where TComponent : IIndexedComponent, IValueComponent<TValue>, ILinkedComponent
    //    {
    //        ((ILinkedEntity)component.Entity).Context
    //    }
    //}

    public class EntityByComponentSearchIndex<TEntity, TComponent> : IComponentSearchIndex<TEntity, TComponent>
        where TEntity : class, IEntity, new() 
        where TComponent : ISearchableComponent<TComponent>, IEqualityComparer<TComponent>, new()
    {
        private readonly ConcurrentDictionary<TComponent, TEntity> _index;
        private readonly Stack<TComponent> _pool;

        public EntityByComponentSearchIndex(IEqualityComparer<TComponent> comparer)
        {
            _index = new ConcurrentDictionary<TComponent, TEntity>(comparer);
            _pool = new Stack<TComponent>();
        }

        public bool TryFindEntity<TValue>(TValue value, out TEntity entity) 
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

        public TEntity FindEntity<TValue>(TValue value)
        {
            TComponent component = _pool.Count != 0 ? _pool.Pop() : new TComponent();
            ((IValueComponent<TValue>)component).Value = value;
            if (_index.TryGetValue(component, out TEntity e))
            {  
                _pool.Push(component);
                return e;
            }
            _pool.Push(component);
            return null;
        }

        public void Update(TEntity entity, IComponent previouscomponent, IComponent newcomponent)
        {
            _index.AddOrUpdate((TComponent)newcomponent, entity, UpdateValue);
        }

        public void Add(TEntity entity, IComponent component)
        {
            _index.AddOrUpdate((TComponent)component, entity, UpdateValue);
        }

        private static TEntity UpdateValue(TComponent component, TEntity entity)
        {            
            return entity;
        }

        public void Remove(IComponent component)
        {
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

    public interface IComponentSearchIndex<in TEntity> where TEntity : class, IEntity, new()
    {
        void Add(TEntity entity, IComponent component);

        void Remove(IComponent component);

        void Update(TEntity entity, IComponent previouscomponent, IComponent newcomponent);

        void Clear(TEntity entity = null);
    }

    public interface IComponentSearchIndex<TEntity, out TComponent> : IComponentSearchIndex<TEntity> 
        where TEntity : class, IEntity, new() 
        where TComponent : IComponent, new()
    {
        bool TryFindEntity<TValue>(TValue value, out TEntity entity);

        TEntity FindEntity<TValue>(TValue value);
    }



}

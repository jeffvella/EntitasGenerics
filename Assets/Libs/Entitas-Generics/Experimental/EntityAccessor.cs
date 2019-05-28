using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using Entitas.CodeGeneration.Attributes;
using TreeEditor;
using UnityEditorInternal;
using UnityEngine.Experimental.PlayerLoop;

namespace Entitas.Generics
{

    //// An alternative to storing the context within each entity would be using a wrapper
    //// like this. Tests show that stack-only allocation and a couple of assignments are
    //// so fast that it's inconsequential compared to everything else going on;
    //// but i'm not yet convinced it feels nice to use.

    //public readonly ref struct EntityAccessor<TEntity> where TEntity : class, IEntity, new()
    //{
    //    public readonly IGenericContext<TEntity> Context;
    //    public readonly TEntity Entity;

    //    public EntityAccessor(IGenericContext<TEntity> context, TEntity entity)
    //    {
    //        Context = context;
    //        Entity = entity;
    //    }

    //    public TComponent Get<TComponent>() where TComponent : IComponent, new()
    //    {
    //        var index = Context.GetComponentIndex<TComponent>();
    //        return (TComponent)Entity.GetComponent(index);
    //    }

    //    public void Set<TComponent>(Action<TComponent> componentUpdater) where TComponent : IComponent, new()
    //    {
    //        var index = Context.GetComponentIndex<TComponent>();
    //        var newComponent = Entity.CreateComponent<TComponent>(index);
    //        componentUpdater(newComponent);
    //        Entity.ReplaceComponent(index, newComponent);
    //    }
    //}



    //public class IndexedValueComponent<TComponent, TValue> : ValueComponent<TValue>, ISearchableComponent<TComponent> where TComponent : IValueComponent<TValue>
    //{
    //    public bool Equals(TComponent x, TComponent y) => x != null && y != null && x.Equals(y.Value);

    //    public int GetHashCode(TComponent obj) => obj.Value.GetHashCode();
    //}

    //public class ValueComponent<TValue> : LinkedComponent, IValueComponent<TValue>
    //{
    //    public TValue Value { get; set; }
    //}

    //public class FlagComponent : LinkedComponent, IFlagComponent
    //{

    //}

    //public class LinkedComponent : ILinkedComponent
    //{
    //    int ILinkedComponent.Index { get; set; }

    //    IEntity ILinkedComponent.Entity { get; set; }
    //}

    //public static class ComponentExtensions
    //{
    //    /// <summary>
    //    /// Saves any changes to the value of a component by executing ReplaceComponent, 
    //    /// which ensures that listening events are properly informed of the changes.
    //    /// </summary>
    //    /// <typeparam name="TComponent">A component that is capable of updating itself; <see cref="ILinkedComponent"/></typeparam>
    //    /// <param name="component">The component to be changed</param>
    //    public static void Apply<TComponent>(this TComponent component) where TComponent : ILinkedComponent, new()
    //    {
    //        component.Entity.ReplaceComponent(component.Index, component);
    //    }

    //    /// <summary>
    //    /// Update
    //    /// </summary>
    //    /// <typeparam name="TComponent">A component that is capable of updating itself; <see cref="ILinkedComponent"/></typeparam>
    //    /// <typeparam name="TValue">The value type; determined by <typeparamref name="TComponent"/>; see <see cref="IValueComponent{TValue}"/></typeparam>
    //    /// <param name="component">The component to be changed</param>
    //    /// <param name="value">The new value</param>
    //    public static void Set<TComponent, TValue>(this TComponent component, TValue value) where TComponent : IValueComponent<TValue>, ILinkedComponent, new()
    //    {
    //        component.Value = value;
    //        component.Entity.ReplaceComponent(component.Index, component);
    //    }
    //}

    public readonly ref struct ComponentAccessor<TComponent> where TComponent : IComponent
    {
        internal readonly int Index;
        internal readonly ILinkedEntity Entity;
        public readonly TComponent Component;

        public ComponentAccessor(ILinkedEntity entity, int index, TComponent component)
        {
            Index = index;
            Entity = entity;
            Component = component;
        }
    }

    public static class ComponentAccessorExtensions
    {
        public static void ApplyChanges<TComponent>(this ComponentAccessor<TComponent> accessor) where TComponent : class, IValueComponent, new()
        {            
            var newcomponent = accessor.Entity.CreateComponent<TComponent>(accessor.Index);
            accessor.Entity.ReplaceComponent(accessor.Index, newcomponent);
        }

        public static void UpdateValue<TComponent, TValue>(this ComponentAccessor<TComponent> accessor, TValue value) where TComponent : class, IValueComponent<TValue>, new()
        {
            // note: the component indexes update on Add/Remove (not replace/group-changed/value changed)
            // in order to get change events to fire the component needs to be added/removed.

            var newcomponent = accessor.Entity.CreateComponent<TComponent>(accessor.Index);
            newcomponent.Value = value;
            accessor.Entity.ReplaceComponent(accessor.Index, newcomponent);
        }

        public static void Remove<TComponent>(this ComponentAccessor<TComponent> accessor) where TComponent : class, IValueComponent, new()
        {
            accessor.Entity.RemoveComponent(accessor.Index);
        }

        public static void AddEventListener<TComponent>(this ComponentAccessor<TComponent> accessor, Action<(IEntity, TComponent)> action) where TComponent : class, IEventComponent, new()
        {
            accessor.Entity.Context.RegisterAddedComponentListener(accessor.Entity, action);
        }

        public static void RemoveEventListener<TComponent>(this ComponentAccessor<TComponent> accessor, Action<(IEntity, TComponent)> action) where TComponent : class, IEventComponent, new()
        {
            accessor.Entity.Context.RegisterAddedComponentListener(accessor.Entity, action);
        }


        //public static void CreateIndex<TComponent>(this ComponentAccessor<TComponent> accessor) where TComponent : class, IValueComponent<TValue>, ISearchKeyProvider<TComponent, TValue>, new()
        //{
        //    //_game.CreateIndex<PositionComponent>((e, c) => c.Value);

        //    accessor.Entity.Context.CreateIndex<TComponent,TValue>(accessor.Component);

        //    //var context = accessor.Entity.Context.CreateIndex<TComponent,TValue>();
        //    //var indexKeyRetriever = ((IIndexedValueProvider<TComponent, TValue>)accessor.Component).IndexKeyRetriever;
        //    //var index = new PrimaryEntityIndex<>("", accessor.Entity.Context.GetGroup<TComponent>(), indexKeyRetriever);

        //    //context.AddEntityIndex(;
        //}


        //public static void CreateIndex<TComponent, TValue>(this ComponentAccessor<TComponent> accessor, TValue value) where TComponent : class, ISearchKeyProvider<TComponent, TValue>, new()
        //{
        //    accessor.Entity.Context.CreateIndex<TComponent, TValue>(accessor.Component, valueProvider());

        //    //var context = accessor.Entity.Context.CreateIndex<TComponent,TValue>();
        //    //var indexKeyRetriever = ((IIndexedValueProvider<TComponent, TValue>)accessor.Component).IndexKeyRetriever;
        //    //var index = new PrimaryEntityIndex<>("", accessor.Entity.Context.GetGroup<TComponent>(), indexKeyRetriever);

        //    //context.AddEntityIndex(;
        //}


    }

    public static class LinkedEntityExtensions
    {
        public static ComponentAccessor<TComponent> Find<TComponent>(this ILinkedEntity entity) where TComponent : class, IValueComponent, new()
        {
            var index = entity.Context.GetComponentIndex<TComponent>();
            TComponent component;
            if (!entity.HasComponent(index))
            {
                component = entity.CreateComponent<TComponent>(index);
                //entity.AddComponent(index, component);
            }
            else
            {
                component = (TComponent)entity.GetComponent(index);
            }
            return new ComponentAccessor<TComponent>(entity, entity.Context.GetComponentIndex<TComponent>(), component);
        }

        //private void CreateIndex()


        //public static TComponent Get2<TComponent>(this ILinkedEntity entity) where TComponent : class, ILinkedComponent, new()
        //{
        //    //return entity.Context.GetOrCreateComponent<TComponent>(entity);

        //    var index = entity.Context.GetComponentIndex<TComponent>();
        //    TComponent component;
        //    if (!entity.HasComponent(index))
        //    {
        //        component = entity.CreateComponent<TComponent>(index);
        //        //entity.AddComponent(index, newComponent);
        //    }
        //    else
        //    {
        //        component = (TComponent)entity.GetComponent(index);
        //    }
        //    component.Index = index;
        //    component.Entity = entity;
        //    return component;
        //}

        //public static void SetDefault<TComponent>(this ILinkedEntity entity) where TComponent : class, ILinkedComponent, new()
        //{
        //    entity.Get2<TComponent>().SetDefault();
        //}

        //public static TComponent SetDefault<TComponent>(this ILinkedEntity entity) where TComponent : class, ILinkedComponent, new()

        //{
        //    var index = entity.Context.GetComponentIndex<TComponent>();
        //    TComponent newComponent;
        //    if (!entity.HasComponent(index))
        //    {
        //        newComponent = entity.CreateComponent<TComponent>(index);
        //        entity.AddComponent(index, newComponent);
        //    }
        //    else
        //    {
        //        newComponent = (TComponent)entity.GetComponent(index);
        //    }
        //    newComponent.Apply();
        //    newComponent.Index = index;
        //    newComponent.Entity = entity;
        //    return newComponent;
        //}

        public static void RegisterAddedComponentListener<TComponent>(this ILinkedEntity entity, Action<(IEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new()
        {
            entity.Context.RegisterAddedComponentListener<TComponent>(entity, action);
        }

        public static void RegisterRemovedComponentListener<TComponent>(this ILinkedEntity entity, Action<IEntity> action) where TComponent : IEventComponent, new()
        {
            entity.Context.RegisterRemovedComponentListener<TComponent>(entity, action);
        }

        public static bool Has<TComponent>(this ILinkedEntity entity) where TComponent : IComponent, new()
        {
            return entity.Context.Has<TComponent>(entity);
        }

        public static bool IsFlagged<TComponent>(this ILinkedEntity entity) where TComponent : IFlagComponent, new()
        {
            return entity.Context.IsFlagged<TComponent>(entity);
        }

        public static void SetFlag<TComponent>(this ILinkedEntity entity, bool value = true) where TComponent : IFlagComponent, new()
        {
            entity.Context.SetFlag<TComponent>(entity, value);
        }

        //public static bool Test<TComponent>(this ILinkedEntity entity) where TComponent : ILinkedComponent, new()
        //{
        //    return entity.Context.Test(entity);
        //}
    }

    //public static class ContextExtensions
    //{
    //    public static void Test<TContext, TEntity>(this TContext context, TEntity entity) where TContext : IGenericContext<TEntity> where TEntity : class, IEntity, new()
    //    {

    //        return entity.Context.GetOrCreateComponent<TComponent>(entity);

    //        //var index = entity.Context.GetComponentIndex<TComponent>();
    //        //TComponent newComponent;
    //        //if (!entity.HasComponent(index))
    //        //{
    //        //    newComponent = entity.CreateComponent<TComponent>(index);
    //        //    entity.AddComponent(index, newComponent);
    //        //}
    //        //else
    //        //{
    //        //    newComponent = (TComponent)entity.GetComponent(index);
    //        //}
    //        //newComponent.Index = index;
    //        //newComponent.Entity = entity;
    //        //return newComponent;
    //    }

    //    public static void RegisterAddedComponentListener<TComponent>(this ILinkedEntity entity, Action<(IEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new()
    //    {
    //        entity.Context.RegisterAddedComponentListener<TComponent>(entity, action);
    //    }

    //    public static void RegisterRemovedComponentListener<TComponent>(this ILinkedEntity entity, Action<IEntity> action) where TComponent : IEventComponent, new()
    //    {
    //        entity.Context.RegisterRemovedComponentListener<TComponent>(entity, action);
    //    }

    //    public static bool Has<TComponent>(this ILinkedEntity entity) where TComponent : ILinkedComponent, new()
    //    {
    //        return entity.Context.Has<TComponent>(entity);
    //    }

    //    public static bool IsFlagged<TComponent>(this ILinkedEntity entity) where TComponent : ILinkedComponent, IFlagComponent, new()
    //    {
    //        return entity.Context.IsFlagged<TComponent>(entity);
    //    }

    //    public static void SetFlag<TComponent>(this ILinkedEntity entity, bool value = true) where TComponent : ILinkedComponent, IFlagComponent, new()
    //    {
    //        entity.Context.SetFlag<TComponent>(entity, value);
    //    }
    //}

    public class TestIndex<TEntity, TKey> : AbstractEntityIndex<TEntity, TKey> where TEntity : class, IEntity
    {

        readonly Dictionary<TKey, TEntity> _index;

        public TestIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey> getKey) : base(name, group, getKey)
        {
            _index = new Dictionary<TKey, TEntity>();
            Activate();
        }

        public TestIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey[]> getKeys) : base(name, group, getKeys)
        {
            _index = new Dictionary<TKey, TEntity>();
            Activate();
        }

        public TestIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey> getKey, IEqualityComparer<TKey> comparer) : base(name, group, getKey)
        {
            _index = new Dictionary<TKey, TEntity>(comparer);
            Activate();
        }

        public TestIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey[]> getKeys, IEqualityComparer<TKey> comparer) : base(name, group, getKeys)
        {
            _index = new Dictionary<TKey, TEntity>(comparer);
            Activate();
        }

        public override void Activate()
        {
            base.Activate();
            indexEntities(_group);
        }

        public TEntity GetEntity(TKey key)
        {
            TEntity entity;
            _index.TryGetValue(key, out entity);
            return entity;
        }

        public override string ToString()
        {
            return "PrimaryEntityIndex(" + name + ")";
        }

        protected override void clear()
        {
            foreach (var entity in _index.Values)
            {
                var safeAerc = entity.aerc as SafeAERC;
                if (safeAerc != null)
                {
                    if (safeAerc.owners.Contains(this))
                    {
                        entity.Release(this);
                    }
                }
                else
                {
                    entity.Release(this);
                }
            }

            _index.Clear();
        }

        protected override void addEntity(TKey key, TEntity entity)
        {
            if (_index.ContainsKey(key))
            {
                throw new EntityIndexException(
                    "Entity for key '" + key + "' already exists!",
                    "Only one entity for a primary key is allowed.");
            }

            _index.Add(key, entity);

            var safeAerc = entity.aerc as SafeAERC;
            if (safeAerc != null)
            {
                if (!safeAerc.owners.Contains(this))
                {
                    entity.Retain(this);
                }
            }
            else
            {
                entity.Retain(this);
            }
        }

        protected override void removeEntity(TKey key, TEntity entity)
        {
            _index.Remove(key);

            var safeAerc = entity.aerc as SafeAERC;
            if (safeAerc != null)
            {
                if (safeAerc.owners.Contains(this))
                {
                    entity.Release(this);
                }
            }
            else
            {
                entity.Release(this);
            }
        }
    }

}
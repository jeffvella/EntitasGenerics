using System;

namespace Entitas.Generics
{    
    public readonly ref struct ComponentAccessor<TComponent> where TComponent : IComponent, new()
    {
        internal readonly IGenericEntity Entity;
        internal readonly int Index;
        public readonly TComponent Component;

        public ComponentAccessor(IGenericEntity entity)
        {
            Entity = entity;
            Index = entity.GetIndex<TComponent>();
            Component = entity.GetComponent<TComponent>();
        }

        public ComponentAccessor(IGenericEntity entity, int index, TComponent component)
        {
            Entity = entity;
            Index = index;
            Component = component;
        }

        public void Apply() => Entity.ReplaceComponent(Component);

        //public PersistentComponentAccessor<TComponent> ToPersistant()
        //    => new PersistentComponentAccessor<TComponent>(Entity, Index, Component);

        public static implicit operator TComponent(ComponentAccessor<TComponent> accessor) => accessor.Component;
    }

    public static class ComponentAccessorConditionalExtensions
    {
        public static void Apply<TComponent, TValue>(this ComponentAccessor<TComponent> accessor, TValue value) where TComponent : class, IValueComponent<TValue>, new()
        {
            var newComponent = accessor.Entity.CreateComponent<TComponent>();
            //accessor.Component.Value = default;
            newComponent.Value = value;
            accessor.Entity.ReplaceComponent(newComponent);
        }

        public static void Apply<TComponent, TValue>(this ComponentAccessor<TComponent> accessor, Func<TComponent, TValue> valueProducer) where TComponent : class, IValueComponent<TValue>, new()
        {
            var newcomponent = accessor.Entity.CreateComponent<TComponent>();
            var newValue = valueProducer(accessor.Component);
            //accessor.Component.Value = newValue;
            newcomponent.Value = newValue;
            accessor.Entity.ReplaceComponent(newcomponent);
        }


        public static void Set<TComponent>(this ComponentAccessor<TComponent> accessor, Action<TComponent> componentModifier) where TComponent : class, IComponent, new()
        {
            var newComponent = accessor.Entity.CreateComponent<TComponent>();
            componentModifier(newComponent);
            accessor.Entity.ReplaceComponent(newComponent);
        }


        //public static void SetFlag<TEntity, TComponent>(this ComponentAccessor<TEntity, TComponent> component, bool value = true) 
        //    where TEntity : IContextLinkedEntity<TEntity> where TComponent : class, IFlagComponent, new()
        //{
        //    if (component.Exists())
        //    {
        //        if (!value)
        //        {
        //            component.Remove();
        //        }
        //    }
        //    else if (value)
        //    {
        //        component.Add();
        //    }
        //}

        //public static bool IsFlagged<TComponent>(this ComponentAccessor<TComponent> component, bool value = true) where TComponent : class, IFlagComponent, new()
        //{
        //    return component.();
        //}

        //public static TComponent Create<TComponent>(this ComponentAccessor<TComponent> accessor) where TComponent : class, IComponent, new()
        //{
        //    return accessor.Entity.CreateComponent<TComponent>(accessor.Index);
        //}

        //public static bool IsUnique<TEntity, TComponent>(this ComponentAccessor<TComponent> accessor) where TComponent : class, IComponent, new()
        //{
        //    return ComponentHelper<TComponent>.IsUnique;
        //}

        //public static bool IsEvent<TEntity, TComponent>(this ComponentAccessor<TEntity, TComponent> accessor) 
        //    where TEntity : IContextLinkedEntity<TEntity> where TComponent : class, IComponent, new()
        //{
        //    return ComponentHelper<TComponent>.IsEvent;
        //}

        //public static bool IsFlag<TEntity, TComponent>(this ComponentAccessor<TEntity, TComponent> accessor) 
        //    where TEntity : IContextLinkedEntity<TEntity> where TComponent : class, IComponent, new()
        //{
        //    return ComponentHelper<TComponent>.IsFlag;
        //}

        //public static void Add<TEntity, TComponent>(this ComponentAccessor<TEntity, TComponent> accessor) 
        //    where TEntity : IContextLinkedEntity<TEntity> where TComponent : class, IComponent, new()
        //{
        //    accessor.Entity.AddComponent(accessor.Index, accessor.Create());
        //}

        //public static void Add<TEntity, TComponent>(this ComponentAccessor<TEntity, TComponent> accessor, TComponent component) 
        //    where TEntity : IContextLinkedEntity<TEntity> where TComponent : class, IComponent, new()
        //{
        //    accessor.Entity.AddComponent(accessor.Index, component);
        //}

        //public static void Remove<TEntity, TComponent>(this ComponentAccessor<TEntity, TComponent> accessor) 
        //    where TEntity : IContextLinkedEntity<TEntity> where TComponent : class, IComponent, new()
        //{
        //    accessor.Entity.RemoveComponent(accessor.Index);
        //}

        //public static bool Exists<TEntity, TComponent>(this ComponentAccessor<TEntity, TComponent> accessor) 
        //    where TEntity : IContextLinkedEntity<TEntity> where TComponent : class, IComponent, new()
        //{
        //    return accessor.Entity.HasComponent(accessor.Index);
        //}

        //public static void AddEventListener<TEntity, TComponent>(this ComponentAccessor<TEntity, TComponent> accessor, Action<(IEntity, TComponent)> action)
        //    where TEntity : IContextLinkedEntity<TEntity> where TComponent : class, IEventComponent, new()
        //{
        //    accessor.Context.RegisterAddedComponentListener(accessor.Entity, action);
        //}

        //public static void RemoveEventListener<TEntity, TComponent>(this ComponentAccessor<TEntity, TComponent> accessor, Action<(IEntity, TComponent)> action) 
        //    where TEntity : IContextLinkedEntity<TEntity> where TComponent : class, IEventComponent, new()
        //{
        //    accessor.Context.RegisterAddedComponentListener(accessor.Entity, action);
        //}
    }

    //public interface IComponentAccessor
    //{
    //    void CreateComponent();

    //    bool Exists();

    //    void Remove();
    //}

    //public sealed class PersistentComponentAccessor<TComponent> where TComponent : IComponent, new()
    //{
    //    internal readonly int Index;
    //    internal readonly IEntity Entity;
    //    internal readonly IEntityContext Context;
    //    private TComponent _component;

    //    public TComponent Component
    //    {
    //        get
    //        {
    //            if (_component == null)
    //            {
    //                Refresh();
    //            }
    //            return _component;
    //        }
    //    }

    //    public PersistentComponentAccessor(IEntity entity, IEntityContext context)
    //    {
    //        Entity = entity;
    //        Context = context;
    //        Index = context.GetComponentIndex<TComponent>();
    //        Refresh();
    //    }

    //    public PersistentComponentAccessor(IEntity entity, IEntityContext context, int index)
    //    {
    //        Index = index;
    //        Entity = entity;
    //        Context = context;
    //        Refresh();
    //    }

    //    public PersistentComponentAccessor(IEntity entity, IEntityContext context, int index, TComponent component)
    //    {
    //        Index = index;
    //        Entity = entity;
    //        Context = context;
    //        _component = component;
    //    }

    //    public TComponent Create() => Entity.CreateComponent<TComponent>(Index);

    //    public void Apply() => Entity.ReplaceComponent(Index, Component);

    //    public bool Exists() => Entity.HasComponent(Index);

    //    public void Remove() => Entity.RemoveComponent(Index);

    //    public void Add() => Entity.AddComponent(Index, Create());

    //    public void Refresh()
    //    {
    //        if (!Entity.HasComponent(Index))
    //        {
    //            Add();
    //        }
    //        _component = (TComponent)Entity.GetComponent(Index);
    //    }
    //}

    //public static class PersistantComponentAccessorExtensions
    //{
    //    public static void Apply<TComponent, TValue>(this PersistentComponentAccessor<TComponent> accessor, TValue value) where TComponent : class, IValueComponent<TValue>, new()
    //    {
    //        var newcomponent = accessor.Entity.CreateComponent<TComponent>(accessor.Index);
    //        newcomponent.Value = value;
    //        accessor.Entity.ReplaceComponent(accessor.Index, newcomponent);
    //    }

    //    public static void AddEventListener<TComponent>(this PersistentComponentAccessor<TComponent> accessor, Action<(IEntity, TComponent)> action) where TComponent : class, IEventComponent, new()
    //    {
    //        accessor.Context.RegisterAddedComponentListener(accessor.Entity, action);
    //    }

    //    public static void RemoveEventListener<TComponent>(this PersistentComponentAccessor<TComponent> accessor, Action<(IEntity, TComponent)> action) where TComponent : class, IEventComponent, new()
    //    {
    //        accessor.Context.RegisterAddedComponentListener(accessor.Entity, action);
    //    }

    //    public static void SetFlag<TComponent>(this PersistentComponentAccessor<TComponent> component, bool value = true) where TComponent : class, IFlagComponent, new()
    //    {
    //        if (component.Exists())
    //        {
    //            if (!value)
    //            {
    //                component.Remove();
    //            }
    //        }
    //        else if (value)
    //        {
    //            component.Add();
    //        }
    //    }
    //}

}
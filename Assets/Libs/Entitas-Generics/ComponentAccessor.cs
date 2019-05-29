using System;

namespace Entitas.Generics
{
    public readonly ref struct ComponentAccessor<TComponent> where TComponent : IComponent, new()
    {
        internal readonly int Index;
        internal readonly IEntityContext Context;
        internal readonly IEntity Entity;

        public readonly TComponent Component;

        public ComponentAccessor(IEntity entity, IEntityContext context, int index, TComponent component)
        {
            Index = index;
            Entity = entity;
            Context = context;
            Component = component;
        }

        //public ComponentAccessor(IEntity entity, IEntityContext context, TComponent component)
        //{
        //    Entity = entity;
        //    Context = context;
        //    Index = context.GetComponentIndex<TComponent>();
        //    Component = component;
        //}

        public ComponentAccessor(IEntity entity, IEntityContext context)
        {
            Entity = entity;
            Context = context;
            Index = context.GetComponentIndex<TComponent>();
            Component = entity.GetComponent<TComponent>(context);

            //Component = !entity.HasComponent(Index)
            //    ? entity.CreateComponent<TComponent>(Index)
            //    : (TComponent)entity.GetComponent(Index);
        }

        public PersistentComponentAccessor<TComponent> ToPersistant()
            => new PersistentComponentAccessor<TComponent>(Entity, Context, Index);

        public static implicit operator TComponent(ComponentAccessor<TComponent> accessor) => accessor.Component;
    }

    public static class ComponentAccessorExtensions
    {
        public static void Apply<TComponent>(this ComponentAccessor<TComponent> accessor) where TComponent : class, IComponent, new()
        {
            var newcomponent = accessor.Entity.CreateComponent<TComponent>(accessor.Index);
            accessor.Entity.ReplaceComponent(accessor.Index, newcomponent);
        }

        public static void Apply<TComponent, TValue>(this ComponentAccessor<TComponent> accessor, TValue value) where TComponent : class, IValueComponent<TValue>, new()
        {
            var newcomponent = accessor.Entity.CreateComponent<TComponent>(accessor.Index);
            newcomponent.Value = value;
            accessor.Entity.ReplaceComponent(accessor.Index, newcomponent);
        }

        public static void SetFlag<TComponent>(this ComponentAccessor<TComponent> component, bool value = true) where TComponent : class, IFlagComponent, new()
        {
            if (component.Exists())
            {
                if (!value)
                {
                    component.Remove();
                }
            }
            else if (value)
            {
                component.Add();
            }
        }

        public static bool IsFlagged<TComponent>(this ComponentAccessor<TComponent> component, bool value = true) where TComponent : class, IFlagComponent, new()
        {
            return component.Exists();
        }

        public static TComponent Create<TComponent>(this ComponentAccessor<TComponent> accessor) where TComponent : class, IComponent, new()
        {
            return accessor.Entity.CreateComponent<TComponent>(accessor.Index);
        }

        public static void Add<TComponent>(this ComponentAccessor<TComponent> accessor) where TComponent : class, IComponent, new()
        {
            accessor.Entity.AddComponent(accessor.Index, accessor.Create());
        }

        public static void Add<TComponent>(this ComponentAccessor<TComponent> accessor, TComponent component) where TComponent : class, IComponent, new()
        {
            accessor.Entity.AddComponent(accessor.Index, component);
        }

        public static void Remove<TComponent>(this ComponentAccessor<TComponent> accessor) where TComponent : class, IComponent, new()
        {
            accessor.Entity.RemoveComponent(accessor.Index);
        }

        public static bool Exists<TComponent>(this ComponentAccessor<TComponent> accessor) where TComponent : class, IComponent, new()
        {
            return accessor.Entity.HasComponent(accessor.Index);
        }

        public static void AddEventListener<TComponent>(this ComponentAccessor<TComponent> accessor, Action<(IEntity, TComponent)> action) where TComponent : class, IEventComponent, new()
        {
            accessor.Context.RegisterAddedComponentListener(accessor.Entity, action);
        }

        public static void RemoveEventListener<TComponent>(this ComponentAccessor<TComponent> accessor, Action<(IEntity, TComponent)> action) where TComponent : class, IEventComponent, new()
        {
            accessor.Context.RegisterAddedComponentListener(accessor.Entity, action);
        }
    }

    //public interface IComponentAccessor
    //{
    //    void CreateComponent();

    //    bool Exists();

    //    void Remove();
    //}

    public sealed class PersistentComponentAccessor<TComponent> where TComponent : IComponent, new()
    {
        internal readonly int Index;
        internal readonly IEntity Entity;
        internal readonly IEntityContext Context;

        public TComponent Component => (TComponent)Entity.GetComponent(Index);

        public TComponent Create() => Entity.CreateComponent<TComponent>(Index);

        public bool Exists() => Entity.HasComponent(Index);

        public void Remove() => Entity.RemoveComponent(Index);

        public void Add() => Entity.AddComponent(Index, Create());

        public PersistentComponentAccessor(IEntity entity, IEntityContext context)
        {
            Entity = entity;
            Context = context;
            Index = context.GetComponentIndex<TComponent>();
        }

        public PersistentComponentAccessor(IEntity entity, IEntityContext context, int index)
        {
            Index = index;
            Entity = entity;
            Context = context;
        }
    }

    public static class PersistantComponentAccessorExtensions
    {
        public static void Apply<TComponent>(this PersistentComponentAccessor<TComponent> accessor) where TComponent : class, IComponent, new()
        {
            var newcomponent = accessor.Entity.CreateComponent<TComponent>(accessor.Index);
            accessor.Entity.ReplaceComponent(accessor.Index, newcomponent);
        }

        public static void Apply<TComponent, TValue>(this PersistentComponentAccessor<TComponent> accessor, TValue value) where TComponent : class, IValueComponent<TValue>, new()
        {
            var newcomponent = accessor.Entity.CreateComponent<TComponent>(accessor.Index);
            newcomponent.Value = value;
            accessor.Entity.ReplaceComponent(accessor.Index, newcomponent);
        }

        public static void AddEventListener<TComponent>(this PersistentComponentAccessor<TComponent> accessor, Action<(IEntity, TComponent)> action) where TComponent : class, IEventComponent, new()
        {
            accessor.Context.RegisterAddedComponentListener(accessor.Entity, action);
        }

        public static void RemoveEventListener<TComponent>(this PersistentComponentAccessor<TComponent> accessor, Action<(IEntity, TComponent)> action) where TComponent : class, IEventComponent, new()
        {
            accessor.Context.RegisterAddedComponentListener(accessor.Entity, action);
        }


        public static void SetFlag<TComponent>(this PersistentComponentAccessor<TComponent> component, bool value = true) where TComponent : class, IFlagComponent, new()
        {
            if (component.Exists())
            {
                if (!value)
                {
                    component.Remove();
                }
            }
            else if (value)
            {
                component.Add();
            }
        }
    }

}
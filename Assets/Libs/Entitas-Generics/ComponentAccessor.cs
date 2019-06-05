using System;

namespace Entitas.Generics
{    
    public readonly ref struct ComponentAccessor<TComponent> where TComponent : class, IComponent, new()
    {
        internal readonly IGenericEntity Entity;
        internal readonly int Index;
        public readonly TComponent Component;

        public ComponentAccessor(IGenericEntity entity)
        {
            Entity = entity;
            Index = entity.GetComponentIndex<TComponent>();
            Component = entity.Get<TComponent>();
        }

        public ComponentAccessor(IGenericEntity entity, int index, TComponent component)
        {
            Entity = entity;
            Index = index;
            Component = component;
        }

        public void Apply() => Entity.Replace(Component);

        public PersistentComponentAccessor<TComponent> ToPersistant()
            => new PersistentComponentAccessor<TComponent>(Entity, Index, Component);

        public static implicit operator TComponent(ComponentAccessor<TComponent> accessor) => accessor.Component;
    }

    public static class ComponentAccessorConditionalExtensions
    {
        public static void Apply<TComponent, TValue>(this ComponentAccessor<TComponent> accessor, TValue value) where TComponent : class, IValueComponent<TValue>, new()
        {
            var newComponent = accessor.Entity.Create<TComponent>();
            newComponent.Value = value;
            accessor.Entity.Replace(newComponent);
        }

        public static void Apply<TComponent, TValue>(this ComponentAccessor<TComponent> accessor, Func<TComponent, TValue> valueProducer) where TComponent : class, IValueComponent<TValue>, new()
        {
            var newcomponent = accessor.Entity.Create<TComponent>();
            var newValue = valueProducer(accessor.Component);
            newcomponent.Value = newValue;
            accessor.Entity.Replace(newcomponent);
        }


        public static void Set<TComponent>(this ComponentAccessor<TComponent> accessor, Action<TComponent> componentModifier) where TComponent : class, IComponent, new()
        {
            var newComponent = accessor.Entity.Create<TComponent>();
            componentModifier(newComponent);
            accessor.Entity.Replace(newComponent);
        }
    }

    public sealed class PersistentComponentAccessor<TComponent> where TComponent : class, IComponent, new()
    {
        internal readonly int Index;
        internal readonly IGenericEntity Entity;
        private TComponent _component;

        public TComponent Component
        {
            get
            {
                if (_component == null)
                {
                    Refresh();
                }
                return _component;
            }
        }

        public PersistentComponentAccessor(IGenericEntity entity)
        {
            Entity = entity; 
            Index = entity.GetComponentIndex<TComponent>();
            Refresh();
        }

        public PersistentComponentAccessor(IGenericEntity entity, int index)
        {
            Index = index;
            Entity = entity;
            Refresh();
        }

        public PersistentComponentAccessor(IGenericEntity entity, int index, TComponent component)
        {
            Index = index;
            Entity = entity;
            _component = component;
        }

        public TComponent Create() => Entity.Create<TComponent>();

        public void Apply() => Entity.Replace(Component);

        public bool Exists() => Entity.Has<TComponent>();

        public void Remove() => Entity.Remove<TComponent>();

        public void Add() => Entity.Add(Create());

        public void Refresh()
        {
            if (!Entity.Has<TComponent>())
                Add();
            
            _component = Entity.Get<TComponent>();
        }
    }

    public static class PersistantComponentAccessorExtensions
    {
        public static void Set<TComponent, TValue>(this PersistentComponentAccessor<TComponent> accessor, TValue value) where TComponent : class, IValueComponent<TValue>, new()
        {
            var newcomponent = accessor.Entity.Create<TComponent>();
            newcomponent.Value = value;
            accessor.Entity.Replace(newcomponent);
        }

        public static void SetFlag<TComponent>(this PersistentComponentAccessor<TComponent> accessor, bool value = true) where TComponent : class, IFlagComponent, new()
        {
            accessor.Entity.SetFlag<TComponent>(value);
        }

        public static void IsFlagged<TComponent>(this PersistentComponentAccessor<TComponent> accessor, bool value = true) where TComponent : class, IFlagComponent, new()
        {
            accessor.Entity.IsFlagged<TComponent>();
        }

    }

}
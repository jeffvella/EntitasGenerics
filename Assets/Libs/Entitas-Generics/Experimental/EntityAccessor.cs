using System;
using System.CodeDom;
using Entitas.CodeGeneration.Attributes;
using TreeEditor;
using UnityEditorInternal;
using UnityEngine.Experimental.PlayerLoop;

namespace Entitas.Generics
{

    // An alternative to storing the context within each entity would be using a wrapper
    // like this. Tests show that stack-only allocation and a couple of assignments are
    // so fast that it's inconsequential compared to everything else going on;
    // but i'm not yet convinced it feels nice to use.

    public readonly ref struct EntityAccessor<TEntity> where TEntity : class, IEntity
    {
        public readonly IGenericContext<TEntity> Context;
        public readonly TEntity Entity;

        public EntityAccessor(IGenericContext<TEntity> context, TEntity entity)
        {
            Context = context;
            Entity = entity;
        }

        public TComponent Get<TComponent>() where TComponent : IComponent, new()
        {
            var index = Context.GetComponentIndex<TComponent>();
            return (TComponent)Entity.GetComponent(index);
        }

        public void Set<TComponent>(Action<TComponent> componentUpdater) where TComponent : IComponent, new()
        {
            var index = Context.GetComponentIndex<TComponent>();
            var newComponent = Entity.CreateComponent<TComponent>(index);
            componentUpdater(newComponent);
            Entity.ReplaceComponent(index, newComponent);
        }
    }

    public interface IEntityLinkedComponent
    {
        void Link(IEntity entity, int index);

        IEntity GetEntity();

        int GetIndex();
    }

    public class ValueComponent<TValue> : IValueComponent<TValue>
    {
        public TValue Value { get; set; }

        public int Index { get; set; }

        public IEntity Entity { get; set; }
    }

    public static class ValueExtensions
    {
        public static void Apply<TComponent>(this TComponent component) where TComponent : ILinkedComponent, new()
        {
            component.Entity.ReplaceComponent(component.Index, component);
        }

        public static void Update<TComponent, TValue>(this TComponent component, TValue value) where TComponent : IValueComponent<TValue>, new()
        {            
            IEntity entity = component.Entity;
            int index = component.Index;
            var newComponent = (IValueComponent<TValue>)entity.CreateComponent<TComponent>(index);
            newComponent.Value = value;
            entity.ReplaceComponent(index, newComponent);
        }

        public static TComponent Get2<TComponent>(this ILinkedEntity entity) where TComponent : ILinkedComponent, new()
        {
            var index = entity.Context.GetComponentIndex<TComponent>();
            TComponent newComponent;
            if (!entity.HasComponent(index))
            {
                newComponent = entity.CreateComponent<TComponent>(index);
            }
            else
            {
                newComponent = (TComponent)entity.GetComponent(index);
            }
            newComponent.Index = index;
            newComponent.Entity = entity;
            return newComponent;
        }

        //public static TComponent Update<TEntity, TComponent>(this TEntity entity, TComponent component) where TEntity : IContextLinkedEntity where TComponent : IComponent, new()
        //{
        //    return entity.Context.Get<TComponent>(entity);
        //}

    }

    //public static class ComponentAccessExtensions
    //{
    //    public static void Test(this IComponent component)
    //    {
    //        if (!(component is IAccessorComponent accessor))
    //        {
    //            throw new InvalidCastException();
    //        }

    //        var x = ComponentTest.GetTest(accessor);
    //    }
    //}

    //public interface IAccessorComponent : IComponent
    //{

    //}

    //public static class ComponentTest
    //{
    //    public static bool GetTest<T>(T instance) where T : IComponent
    //    {
    //        var x = ComponentTest<T>.value;
    //    }
    //}

    //public static class ComponentTest<TComponent>
    //{
    //    public static bool value;
    //}


}
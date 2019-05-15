using System;
using Entitas;
using Entitas.Generics;

//public interface IEntityAccessor2
//{

//}



//public interface IValueAccessor<TValue>
//{

//}

//public class ComponentAccessor<TEntity, TComponent, TValue> : IValueAccessor<TValue>
//    where TEntity : class, IEntity
//    where TComponent : IValueComponent<TValue>, IComponent, new()
//    where TValue : unmanaged
//{
//    public IGenericContext<TEntity> Context { get; }

//    public TEntity Entity { get; }

//    public ComponentAccessor(IGenericContext<TEntity> context, TEntity entity)
//    {
//        Context = context;
//        Entity = entity;
//    }

//    public TValue Value
//    {
//        get => Context.Get<TComponent>(Entity).value;
//        set
//        {
//            var index = Context.GetComponentIndex<TComponent>();
//            if (Entity.HasComponent(index))
//            {
//                // By default with generated Entitas objects all data updates are atomic with a component
//                // replace which ensures a clear added/removed event chain.

//                // Its important that mew allocations are avoided by using CreateComponent
//                // which under the hood quickly grabs a new component off the pool stack.

//                // This also means holding onto component references is dangerous because
//                // the active component switches constantly with every change.

//                var component = Entity.CreateComponent<TComponent>(index);
//                component.value = value;
//                Entity.ReplaceComponent(index, component);
//            }
//        }
//    }
//}

//public readonly ref struct ComponentAccessor<TEntity, TComponent, TValue>
//    where TEntity : class, IEntity
//    where TComponent : IValueComponent<TValue>, IComponent, new()
//    where TValue : unmanaged
//{
//    public IGenericContext<TEntity> Context { get; }

//    public TEntity Entity { get; }

//    public ComponentAccessor(IGenericContext<TEntity> context, TEntity entity)
//    {
//        Context = context;
//        Entity = entity;
//    }

//    public TValue Value
//    {
//        get => Context.Get<TComponent>(Entity).value;
//        set
//        {
//            var index = Context.GetComponentIndex<TComponent>();
//            if (Entity.HasComponent(index))
//            {
//                // By default with generated Entitas objects all data updates are atomic with a component
//                // replace which ensures a clear added/removed event chain.

//                // Its important that mew allocations are avoided by using CreateComponent
//                // which under the hood quickly grabs a new component off the pool stack.

//                // This also means holding onto component references is dangerous because
//                // the active component switches constantly with every change.

//                var component = Entity.CreateComponent<TComponent>(index);
//                component.value = value;
//                Entity.ReplaceComponent(index, component);
//            }
//        }
//    }
//}

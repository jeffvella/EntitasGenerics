using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.VisualDebugging.Unity;
using Events;
using UnityEngine;
using Debug = UnityEngine.Debug;
using EventType = Entitas.CodeGeneration.Attributes.EventType;

namespace Entitas.Generics
{
    /// <summary>
    /// Exposes GenericContext functionality externally in situations
    /// where the context type is not known.
    /// </summary>
    public interface IGenericContext<TEntity> : IContext<TEntity> where TEntity : class, IEntity
    {
        IContextDefinition Definition { get; }

        IMatcher<TEntity> GetMatcher<T>() where T : IComponent, new();

        ICollector<TEntity> GetCollector<T>() where T : IComponent, new();

        IGroup<TEntity> GetGroup<T>() where T : IComponent, new();

        TComponent Get<TComponent>(TEntity entity) where TComponent : IComponent, new();

        TComponent GetUnique<TComponent>() where TComponent : IComponent, new();

        TriggerOnEvent<TEntity> GetTrigger<T>(GroupEvent eventType = default) where T : IComponent, new();

        ICollector<TEntity> GetTriggerCollector<T>(GroupEvent eventType = default) where T : IComponent, new();

        bool IsTagged<TComponent>() where TComponent : ITagComponent, new();

        bool HasComponent<TComponent>(TEntity entity) where TComponent : IComponent, new();

        void Set<TComponent>(TEntity entity, TComponent component = default) where TComponent : IComponent, new();

        void SetUnique<TComponent>(TComponent component) where TComponent : IComponent, new();

        //void SetUnique2<TComponent, TValue>(TValue value) where TComponent : IComponent, IValueComponent<TValue>, new();

        //void SetUnique3<TComponent, TValue>(TComponent component, TValue value) where TComponent : IComponent, IValueComponent<TValue>, new();

        //void AddEventListener<TEvent>(TEntity entity, IEventListener listener);
    }

    public interface IGenericEventSystem : ISystem
    {

    }

    public interface IGenericEventSystem<TComponent> : IGenericEventSystem
    {

    }

    public class EventSystemFactory
    {
        public static IGenericEventSystem<TComponent> Create<TEntity, TComponent>(IGenericContext<TEntity> context, GroupEvent type = GroupEvent.Added) 
            where TEntity : class, IEntity
            where TComponent : IComponent, new()
        {
            return new GenericEventSystem<TEntity, TComponent, 
                ListenerStorageComponent<TEntity, TComponent>>(context, type);
        }
    }

    public sealed class GenericEventSystem<TEntity, TComponent, TListenerComponent> : GenericReactiveSystem<TEntity>, IGenericEventSystem<TComponent>
        where TEntity : class, IEntity
        where TComponent : IComponent, new()
        where TListenerComponent : IListenerComponent<(TEntity Entity, TComponent Component)>, new()
    {
        // Notes:
        // * This system is responsible for dispatching events when TComponent is added/removed.
        // * Each entity houses its own subscriber list in a listener component.
        // * When specific Components TComponent (which seem to be tags) are added/removed, it triggers the event for that entity's subscriber list.          

        private readonly IGenericContext<TEntity> _context;
        private readonly GroupEvent _type;

        public GenericEventSystem(IGenericContext<TEntity> context, GroupEvent type = GroupEvent.Added) : base(context,  
            ctx => context.GetTriggerCollector<TComponent>(type), Filter)
        {
            _context = context;
            _type = type;
        }

        private static bool Filter(IGenericContext<TEntity> context, TEntity entity)
        {
            return context.HasComponent<TComponent>(entity) && context.HasComponent<TListenerComponent>(entity);
        }

        protected override void Execute(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var component = _context.Get<TComponent>(entity);
                var listenerComponent = _context.Get<TListenerComponent>(entity);  
                
                listenerComponent.Raise((entity, component));
                //listenerComponent.Raise(_context, entity, component));
            }
        }
    }

    //public sealed class GenericEventSystemBase<TEntity, TComponent, TListenerComponent> : GenericReactiveSystem<TEntity>, IGenericEventSystem<TComponent>
    //    where TEntity : class, IEntity
    //    where TComponent : IComponent, new()
    //    where TListenerComponent : IListenerComponent<(TEntity Entity, TComponent Component)>, new()
    //{
    //    // Notes:
    //    // * This system is responsible for dispatching events when TComponent is added/removed.
    //    // * Each entity houses its own subscriber list in a listener component.
    //    // * When specific Components TComponent (which seem to be tags) are added/removed, it triggers the event for that entity's subscriber list.          

    //    private readonly IGenericContext<TEntity> _context;
    //    private readonly GroupEvent _type;

    //    public GenericEventSystem(IGenericContext<TEntity> context, GroupEvent type = GroupEvent.Added) : base(context,
    //        ctx => context.GetTriggerCollector<TComponent>(type), Filter)
    //    {
    //        _context = context;
    //        _type = type;
    //    }

    //    private static bool Filter(IGenericContext<TEntity> context, TEntity entity)
    //    {
    //        return context.HasComponent<TComponent>(entity) && context.HasComponent<TListenerComponent>(entity);
    //    }

    //    protected override void Execute(List<TEntity> entities)
    //    {
    //        foreach (var entity in entities)
    //        {
    //            var component = _context.Get<TComponent>(entity);
    //            var listenerComponent = _context.Get<TListenerComponent>(entity);

    //            listenerComponent.Raise((entity, component));
    //        }
    //    }
    //}

    /// <summary>
    /// This wrapper for context is aware of its own type which lets it be used to
    /// effectively hard-code efficient data access specific to this context
    /// </summary>
    public class GenericContext<TContext, TEntity> : Context<TEntity>, IGenericContext<TEntity> where TContext : IGenericContext<TEntity> where TEntity : class, IEntity, new()
    {
        public IContextDefinition Definition { get; }

        //public List<IGenericEventSystem> EventSystems { get; } 

        public GenericContext(IContextDefinition contextDefinition) 
            : base(contextDefinition.ComponentCount, 0, contextDefinition.ContextInfo, AercFactory, EntityFactory)
        {
            SetupVisualDebugging();    

            //EventSystems = new List<IGenericEventSystem>(count);
        }

        private static TEntity EntityFactory()
        {
            return new TEntity();
        }

        private static IAERC AercFactory(IEntity entity)
        {
            return new UnsafeAERC();
        }

        public void AddEventListener<TComponent>(IEventObserver<(TEntity Entity, TComponent Component)> listener) where TComponent : IComponent, new()
        {
            Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            //var entitiesWithComponent = GetGroup<TComponent>();
            //if (entitiesWithComponent.count == 0)
            //{

            //}

            //var system = EventSystems<TContext, TEntity, TComponent, ListenerHolderComponent<TEntity, TComponent>>.GetOrCreateInstance(this);            

            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<ListenerStorageComponent<TEntity, TComponent>>(entity);
            component.Register(listener);
        }

        public void AddEventListener<TComponent>(Action<(TEntity Entity, TComponent Component)> action) where TComponent : IComponent, new()
        {
            Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<ListenerStorageComponent<TEntity, TComponent>>(entity);
            component.Register(action);
        }

        public void AddEventListener<TComponent>(TEntity entity, IEventObserver<(TEntity Entity, TComponent Component)> listener) where TComponent : IComponent
        {
            Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");
            
            var component = GetOrCreateComponent<ListenerStorageComponent<TEntity, TComponent>>(entity);
            component.Register(listener);
        }

        public void AddEventListener<TComponent>(IEventObserver<TComponent> listener) where TComponent : IComponent, new()
        {
            Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<ListenerStorageComponent<TEntity, TComponent>>(entity);
            component.Register(t => listener.OnEvent(t.Component));
        }

        public void AddEventListener<TComponent>(TEntity entity, IEventObserver<TEntity, TComponent> listener) where TComponent : IComponent
        {
            Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var component = GetOrCreateComponent<ListenerStorageComponent<TEntity, TComponent>>(entity);
            component.Register(t => listener.OnEvent((t.Entity, t.Component)));
        }

        public void AddEventListener<TComponent>(IEventObserver<TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<ListenerStorageComponent<TEntity, TComponent>>(entity);
            component.Register(t => listener.OnEvent((t.Entity, t.Component)));
        }

        //public void AddEventListener<TComponent>(TEntity entity, IEventObserver<GenericContext<TContext, TEntity>, TEntity, TComponent> listener) where TComponent : IComponent 
        //{
        //    Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

        //    var component = GetOrCreateComponent<ListenerHolderComponent<TEntity, TComponent>>(entity);
        //    component.Register(t => listener.OnEvent((this, t.Entity, t.Component)));
        //}

        public void AddEventListener<TComponent>(IEventObserver<IGenericContext<TEntity>, TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<ListenerStorageComponent<TEntity, TComponent>>(entity);
            component.Register(t => listener.OnEvent((this, t.Entity, t.Component)));
        }

        //public void AddEventListener<TComponent>(TEntity entity, IEventObserver<TComponent> listener) where TComponent : IComponent
        //{
        //    Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

        //    var component = GetOrCreateComponent<ListenerHolderComponent<TComponent>>(entity);
        //    component.Register(listener);
        //}

        //public void AddEventListener<TComponent>(TEntity entity, IEventObserver<TEntity, TComponent > listener) where TComponent : IComponent
        //{
        //    Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

        //    var component = GetOrCreateComponent<EntityComponentEvent<TEntity,TComponent>>(entity);
        //    component.SetEntity(entity);
        //    component.Register(listener);
        //}

        //private void OnRaised<TEvent>((TEntity entity, IEventObserver<TEntity> Listener, TEvent Event) args)
        //{
        //    args.Listener.OnRaised(entity);
        //}


        //private void EventRouter<TEvent>(TEntity entity, TEvent e)
        //{
        //    (IEventObserver<TEntity>)e.
        //}

        //public ListenerHolderComponent<TEntity,TEvent> GetOrCreateEventComponent<TEvent>(TEntity entity) //where TEvent : IEventObserver<TEntity>
        //{
        //    if (!TryGetComponent(entity, out ListenerHolderComponent<TEntity, TEvent> component))
        //    {
        //        component = CreateAndAddComponent<ListenerHolderComponent<TEntity, TEvent>>(entity);
        //        component.SetEventInfo(entity);
        //    }
        //    return component;
        //}

        public TComponent GetOrCreateComponent<TComponent>(TEntity entity) where TComponent : IComponent, new()
        {
            if (!TryGetComponent(entity, out TComponent component))
            {
                component = CreateAndAddComponent<TComponent>(entity);
            }
            return component;
        }

        private void SetupVisualDebugging()
        {
#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
            if (!UnityEngine.Application.isPlaying)
                return;

            var observer = new ContextObserver(this);
            UnityEngine.Object.DontDestroyOnLoad(observer.gameObject);
#endif
        }

        public IMatcher<TEntity> GetMatcher<T>() where T : IComponent, new()
        {
            return Matcher<TContext, TEntity, T>.AnyOf;
        }

        public ICollector<TEntity> GetCollector<T>() where T : IComponent, new()
        {
            return this.CreateCollector(Matcher<TContext, TEntity, T>.AnyOf);
        }

        private ICollector<TEntity> GetCollector<T>(TriggerOnEvent<TEntity> triggerOnEvent) where T : IComponent, new()
        {
            return this.CreateCollector(triggerOnEvent);
        }

        public IGroup<TEntity> GetGroup<T>() where T : IComponent, new()
        {
            return GetGroup(Matcher<TContext, TEntity, T>.AllOf);
        }

        public TriggerOnEvent<TEntity> GetTrigger<T>(GroupEvent eventType = default) where T : IComponent, new()
        {
            return new TriggerOnEvent<TEntity>(GetMatcher<T>(), eventType);
        }

        public ICollector<TEntity> GetTriggerCollector<T>(GroupEvent eventType = default) where T : IComponent, new()
        {
            return GetCollector<T>(new TriggerOnEvent<TEntity>(GetMatcher<T>(), eventType));
        }

        //public bool HasListener<T>(TEntity entity) where T : IEventListener<T>
        //{
            
        //}

        public bool HasComponent<T>(TEntity entity) where T : IComponent, new()
        {
            var index = ComponentHelper<TContext, T>.ComponentIndex;
            return entity.HasComponent(index);
        }

        public bool HasComponent<T>() where T : IComponent, new()
        {
            var index = ComponentHelper<TContext, T>.ComponentIndex;
            return GetEntityWith<T>().HasComponent(index);
        }

        public TComponent Get<TComponent>(TEntity entity) where TComponent : IComponent, new()
        {
            return (TComponent)entity.GetComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);
        }

        public TComponent GetUnique<TComponent>() where TComponent : IComponent, new()
        {
            if (TryGetEntityWith<TComponent>(out var entity))
            {
                return GetOrCreateComponent<TComponent>(entity);
            }
            if (ComponentHelper<TContext, TComponent>.IsUnique)
            {
                Debug.Log($"Creating new Entity for unique component {typeof(TContext).Name}.{typeof(TComponent).Name}");
                var component = new TComponent();
                CreateEntityWith(component);
                return component;
            }
            return default;
        }

        public (TEntity Entity, TComponent Component) GetUniqueEntityAndComponent<TComponent>() where TComponent : IComponent, new()
        {
            if (TryGetEntityWith<TComponent>(out var entity))
            {
                return (entity, GetOrCreateComponent<TComponent>(entity));
            }
            if (ComponentHelper<TContext, TComponent>.IsUnique)
            {
                Debug.Log($"Creating new Entity for unique component {typeof(TContext).Name}.{typeof(TComponent).Name}");
                var component = new TComponent();
                entity = CreateEntityWith(component);
                return (entity, component);
            }
            return default;
        }

        public static TComponent CreateAndAddComponent<TComponent>(TEntity entity) where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            return CreateAndAddComponent<TComponent>(entity, index);
        }

        public static TComponent CreateAndAddComponent<TComponent>(TEntity entity, int index) where TComponent : IComponent, new()
        {
            var newComponent = entity.CreateComponent<TComponent>(index);
            entity.AddComponent(index, newComponent);
            return newComponent;
        }

        //public T Get<TComponent, T>() where TComponent : IValueComponent<T>, IComponent, new()
        //{
        //    if (TryGetEntityWith<TComponent>(out var entity))
        //    {
        //        var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
        //        return ((IValueComponent<T>)entity.GetComponent(index)).Value;
        //    }
        //    if (ComponentHelper<TContext, TComponent>.IsUnique)
        //    {
        //        Debug.Log($"Creating new Entity for unique component {typeof(TContext).Name}.{typeof(TComponent).Name}");
        //        var component = new TComponent();
        //        CreateEntityWith(component);
        //        return component.Value;
        //    }
        //    return default;
        //}

        public bool TryGetComponent<T>(TEntity entity, out T component) where T : IComponent, new()
        {
            if (!HasComponent<T>(entity))
            {
                component = default;
                return false;
            }

            var index = ComponentHelper<TContext, T>.ComponentIndex;
            component = (T)entity.GetComponent(index);
            return true;
        }

        public void Set<TComponent>(TEntity entity, TComponent component = default) where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            if (component == null)
            {
                CreateAndAddComponent<TComponent>(entity, index);
            }
            else
            {
                entity.ReplaceComponent(index, component);
            }            
        }

        public void Set<TComponent>(TEntity entity, bool state) where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            if (state)
            {
                var component = entity.CreateComponent<TComponent>(index);
                entity.AddComponent(index, component);
            }
            else
            {
                entity.RemoveComponent(index);
            }
        }

        //public void SetUnique2<TComponent, TValue>(TValue value) where TComponent : IComponent, IValueComponent<TValue>, new()
        //{
        //    if (TryGetEntityWith<TComponent>(out var entity))
        //    {
        //        var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
        //        entity.ReplaceComponent(index, new TComponent { value = value });
        //    }
        //    else if (ComponentHelper<TContext, TComponent>.IsUnique)
        //    {
        //        CreateEntityWith(new TComponent { value = value });
        //    }
        //}

        //public void SetUnique3<TComponent, TValue>(TComponent component, TValue value) where TComponent : IComponent, IValueComponent<TValue>, new()
        //{
        //    if (TryGetEntityWith<TComponent>(out var entity))
        //    {
        //        var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
        //        entity.ReplaceComponent(index, new TComponent { value = value });
        //    }
        //    else if (ComponentHelper<TContext, TComponent>.IsUnique)
        //    {
        //        CreateEntityWith(new TComponent { value = value });
        //    }
        //}

        public void SetUnique<TComponent>(TComponent component) where TComponent : IComponent, new()
        {
            if (TryGetEntityWith<TComponent>(out var entity))
            {
                var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
                entity.ReplaceComponent(index, component);
            }
            else if (ComponentHelper<TContext, TComponent>.IsUnique)
            {
                CreateEntityWith(component);
            }
        }



        #region Tags

        // Todo: Thoughts, Currently going back and forth on how best to implement tag components
        // For unique tag components it makes sense to avoid adding/removing entities
        // by designating a special entity to hold them; there might be something im missing
        // such as a requirement because of the the events system triggering on entity add/remove.

        // Also, by default in entitas [Unique] components are each given their own entity.
        // They could also// be housed on their own hardcoded UniqueComponents entity.
        // Is there a benefit to viewing them on different game objects in the inspector
        // versus together in the same inspector?

        // Is there value is having empty components intended to be used as tags explicitly
        // designated as such with a [Tag] or ITagComponent interface? Does it matter if the
        // user marks non-empty components as tags? is there value to be being able to have
        // tag-style boolean assignment for any component?

        public TEntity TagEntity => _tagEntity ?? (_tagEntity = CreateEntityWith(new TagHolderComponent()));
        private TEntity _tagEntity;

        public void SetTag<TComponent>(bool toggle) where TComponent : ITagComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            var hasComponent = TagEntity.HasComponent(index);

            if (toggle && !hasComponent)
            {
                //Debug.Log($"Added Tag {typeof(TContext).Name}.{typeof(TComponent).Name}");
                if (ComponentHelper<TContext, TComponent>.IsUnique)
                {
                    TagEntity.AddComponent(index, ComponentHelper<TContext, TComponent>.Default);
                }
                else
                {
                    TagEntity.AddComponent(index, TagEntity.CreateComponent<TComponent>(index));
                }
            }
            else if (!toggle && hasComponent)
            {
                //Debug.Log($"Removed Tag {typeof(TContext).Name}.{typeof(TComponent).Name}");
                TagEntity.RemoveComponent(index);
            }
        }

        public bool IsTagged<TComponent>() where TComponent : ITagComponent, new() 
            => TagEntity.HasComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);

        #endregion



        //public void Set<TComponent>() where TComponent : IComponent, new()
        //{
        //    var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
        //    var entity = GetFirstEntity();
        //    entity.CreateComponent<TComponent>(index);
        //}

        //public void Set<TComponent>(Action<TComponent> setter) where TComponent : IComponent, new()
        //{
        //    // todo replace with direct ref update to entity table / cache GetSingle/Group.

        //    var component = new TComponent();
        //    setter.Invoke(component);

        //    var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
        //    GetOrCreateSingle<TComponent>().ReplaceComponent(index, component);
        //}

        public void ReplaceComponent<T>(TEntity entity, T component) where T : IComponent, new()
        {
            var index = ComponentHelper<TContext, T>.ComponentIndex;
            entity.ReplaceComponent(index, component);
        }

        public void ReplaceComponent<TComponent>(TComponent component) where TComponent : IComponent, new()
        {
            if (TryGetEntityWith<TComponent>(out var entity))
            {
                var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
                entity.ReplaceComponent(index, component);
            }
        }

        public void Remove<T>(TEntity entity) where T : IComponent, new()
        {
            var index = ComponentHelper<TContext, T>.ComponentIndex;
            entity.RemoveComponent(index);
        }

        public void Remove<TComponent>() where TComponent : IComponent, new()
        {
            if (TryGetEntityWith<TComponent>(out var entity))
            {
                entity.RemoveComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);
            }
        }

        //public TEntity GetFirstEntity()
        //{
        //    return count == 0 ? CreateEntity() : GetEntities().First();
        //}

        //private TComponent GetFirstEntityComponent<TComponent>() where TComponent : IComponent, new()
        //{
        //    return Get<TComponent>(GetGroup<TComponent>().GetSingleEntity());
        //}


        public bool TryGetEntityWith<TComponent>(out TEntity entity) where TComponent : IComponent, new()
        {
            if (count == 0)
            {
                entity = default;
                return false;
            }
            var group = GetGroup<TComponent>();
            if(group.count > 0)
            {
                entity = group.GetSingleEntity();
                return true;
            }
            entity = default;
            return false;
        }

        public TEntity GetOrCreateEntityWith<TComponent>() where TComponent : IComponent, new()
        {
            return count == 0 || !HasComponent<TComponent>()
                ? CreateEntityWith<TComponent>()
                : GetEntityWith<TComponent>();
        }

        public TEntity GetEntityWith<TComponent>() where TComponent : IComponent, new()
        {
            return GetGroup<TComponent>().GetSingleEntity();
        }

        public bool EntityExistsWithComponent<T>() where T : IComponent, new()
        {
            return GetEntityWith<T>() != null;
        }

        public TEntity CreateEntityWith<T>(T component = default) where T : IComponent, new()
        {
            if (ComponentHelper<TContext, T>.IsUnique && EntityExistsWithComponent<T>())
                throw new Exception($"Entity already has component of type: '{typeof(T)}'");

            TEntity entity = CreateEntity();
            Set(entity, component);
            return entity;
        }

    }
}
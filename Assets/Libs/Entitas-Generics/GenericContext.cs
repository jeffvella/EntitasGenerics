using System;
using System.Linq;
using Entitas.VisualDebugging.Unity;
using Entitas.VisualDebugging;
using Events;
using Debug = UnityEngine.Debug;


namespace Entitas.Generics
{
    /// <summary>
    /// Provides component access for a specific IEntity
    /// </summary>
    public interface IEntityContext
    {
        bool Has<TComponent>(IEntity entity) where TComponent : IComponent, new();

        TComponent Get<TComponent>(IEntity entity) where TComponent : IComponent, new();

        bool TryGet<T>(IEntity entity, out T component) where T : IComponent, new();

        TComponent GetOrCreateComponent<TComponent>(IEntity entity) where TComponent : class, IComponent, new();

        void Set<TComponent>(IEntity entity, Action<TComponent> componentUpdater) where TComponent : class, IComponent, new();

        void Remove<TComponent>(IEntity entity) where TComponent : IComponent, new();

        // Flags

        bool IsFlagged<TComponent>(IEntity entity) where TComponent : IFlagComponent, new();

        void SetFlag<TComponent>(IEntity entity, bool toggle = true) where TComponent : IFlagComponent, new();

        // Events

        void RegisterAddedComponentListener<TComponent>(IEntity entity, Action<(IEntity Entity, TComponent Component)> action) where TComponent : IComponent, new();

        void RegisterRemovedComponentListener<TComponent>(IEntity entity, Action<IEntity> action) where TComponent : IComponent, new();

        // Helpers

        int GetComponentIndex<TComponent>() where TComponent : IComponent, new();

        void NotifyChanged<TComponent>(IEntity entity) where TComponent : class, IComponent, new();
    }

    /// <summary>
    /// Provides component access for a specific TEntity
    /// </summary>
    public interface IGenericContext<TEntity> : IContext<TEntity>, IEntityContext<TEntity>, IEntityContext where TEntity : class, IEntity
    {
        bool Has<TComponent>(TEntity entity) where TComponent : IComponent, new();

        TComponent Get<TComponent>(TEntity entity) where TComponent : IComponent, new();

        bool TryGet<T>(TEntity entity, out T component) where T : IComponent, new();

        TComponent GetOrCreateComponent<TComponent>(TEntity entity) where TComponent : IComponent, new();

        void Set<TComponent>(TEntity entity, TComponent component = default) where TComponent : IComponent, new();

        void Remove<TComponent>(TEntity entity) where TComponent : IComponent, new();

        // Flags

        bool IsFlagged<TComponent>(TEntity entity) where TComponent : IFlagComponent, new();

        void SetFlag<TComponent>(TEntity entity, bool toggle = true) where TComponent : IFlagComponent, new();

        // Events

        void RegisterAddedComponentListener<TComponent>(TEntity entity, Action<(TEntity Entity, TComponent Component)> action) where TComponent : IComponent, new();

        void RegisterRemovedComponentListener<TComponent>(TEntity entity, Action<TEntity> action) where TComponent : IComponent, new();
    }

    /// <summary>
    /// Provides entity search functionality and special entity component access (unique and flag components)
    /// </summary>
    public interface IEntityContext<TEntity> where TEntity : class, IEntity
    {
        IMatcher<TEntity> GetMatcher<T>() where T : IComponent, new();

        ICollector<TEntity> GetCollector<T>() where T : IComponent, new();

        IGroup<TEntity> GetGroup<T>() where T : IComponent, new();

        TriggerOnEvent<TEntity> GetTrigger<T>(GroupEvent eventType = default) where T : IComponent, new();

        ICollector<TEntity> GetTriggerCollector<T>(GroupEvent eventType = default) where T : IComponent, new();

        // Unique

        TComponent GetUnique<TComponent>() where TComponent : IUniqueComponent, new();

        void SetUnique<TComponent>(Action<TComponent> component) where TComponent : IUniqueComponent, new();

        // Flags
 
        bool IsFlagged<TComponent>() where TComponent : IFlagComponent, new();

        void SetFlag<TComponent>(bool toggle = true) where TComponent : IFlagComponent, IUniqueComponent, new();

        // Events

        void RegisterAddedComponentListener<TComponent>(Action<(TEntity Entity, TComponent Component)> action) where TComponent : IComponent, new();

        void RegisterAddedComponentListener<TComponent>(IEventObserver<TEntity, TComponent> listener) where TComponent : IComponent, new();

        void RegisterRemovedComponentListener<TComponent>(Action<TEntity> action) where TComponent : IComponent, new();

        // Entity Searches

        TEntity FindEntity<TComponent, TValue>(TValue searchValue) where TComponent : IComponent, IEquatable<TValue>, new();

        bool TryFindEntity<TComponent, TValue>(TValue searchValue, out TEntity entity) where TComponent : IComponent, IEquatable<TValue>, new();

        // Entity/Component Searches

        TEntity GetOrCreateEntityWith<TComponent>() where TComponent : IComponent, new();

        bool EntityExistsWithComponent<TComponent>() where TComponent : IComponent, new();

    }



    //public interface IValueAccessor<TValue>
    //{
    //    TValue Value { get; set; }
    //}



    //public sealed class EventSystem<TEntity, TComponent, TListenerComponent> : GenericReactiveSystem<TEntity>, IGenericEventSystem<TComponent>, ICustomDebugInfo
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

    //    public EventSystem(IGenericContext<TEntity> context, GroupEvent type = GroupEvent.Added) : base(context,  
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
    //            //listenerComponent.Raise(_context, entity, component));
    //        }
    //    }

    //    public string DisplayName => $"{typeof(TComponent).Name} {_type}";
    //}

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

    //    public EventSystem(IGenericContext<TEntity> context, GroupEvent type = GroupEvent.Added) : base(context,
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


    //public interface IEntityAccessor2<TEntity>
    //{

    //}

    //public interface IComponentAccessor<TComponent, TValue>
    //{

    //}

    //public interface IValueAccessor<TValue>
    //{

    //}

    //public readonly ref struct EntityAccessor2<TComponent> 
    //    where TComponent : IComponent, new()
    //{
    //    private readonly IEntityContext _context;
    //    private readonly IEntity _entity;
    //    private readonly int _index;

    //    public EntityAccessor2(IEntityContext context, IEntity entity)
    //    {
    //        _index = context.GetComponentIndex<TComponent>();
    //        _context = context;
    //        _entity = entity;
    //    }

    //    public TComponent Get()
    //    {
    //        return (TComponent)_entity.GetComponent(_index);
    //    }

    //    public void Set(Action<TComponent> componentUpdater) 
    //    {            
    //        var newComponent = _entity.CreateComponent<TComponent>(_index);
    //        componentUpdater(newComponent);
    //        _entity.ReplaceComponent(_index, newComponent);
    //    }
    //}

    //public readonly ref struct EntityAccessor
    //{
    //    public readonly IGenericContext<IEntity> Context;
    //    public readonly IEntity Entity;

    //    public EntityAccessor(IGenericContext<IEntity> context, IEntity entity)
    //    {
    //        Context = context;
    //        Entity = entity;
    //    }

    //    public void Update<TComponent>(Action<TComponent> componentUpdater) where TComponent : IComponent, new()
    //    {
    //        var index = Context.GetComponentIndex<TComponent>();
    //        var newComponent = Entity.CreateComponent<TComponent>(index);
    //        componentUpdater(newComponent);
    //        Entity.ReplaceComponent(index, newComponent);
    //    }       
    //}

    //public class ComponentAccessor<TComponent, TValue> : IValueAccessor<TValue>
    //    where TComponent : IValueComponent<TValue>, IComponent, new()
    //{
    //    private readonly int _index;
    //    public IEntityContext Context { get; }
    //    public IEntity Entity { get; }

    //    public ComponentAccessor(IEntityContext context, IEntity entity)
    //    {
    //        _index = context.GetComponentIndex<TComponent>();
    //        Context = context;
    //        Entity = entity;
    //    }

    //    public TValue Value
    //    {
    //        get => Context.Get<TComponent>(Entity).value;
    //        set
    //        {
    //            // Setting a value needs to be done through create/replace because it ensures
    //            // the pool is used and all the right events are fired.

    //            var component = Entity.CreateComponent<TComponent>(_index);     
    //            component.DirectSetValue(value);
    //            Entity.ReplaceComponent(_index, component);
               
    //        }
    //    }
    //}

    public class GenericContext<TContext, TEntity> : Context<TEntity>, IGenericContext<TEntity>, IEntityContext
        where TContext : IGenericContext<TEntity>
        where TEntity : class, IEntity, new()
    {

        public EntityAccessor<TEntity> ForEntity(TEntity entity) => new EntityAccessor<TEntity>(this, entity);

        public IContextDefinition Definition { get; }
 
        public GenericContext(IContextDefinition contextDefinition) : base(contextDefinition.ComponentCount, 0, contextDefinition.ContextInfo, AercFactory, EntityFactory)
        {
            SetupVisualDebugging();

            Definition = contextDefinition;

            if (contextDefinition.EventListenerIndices.Count > 0)
            {
                OnEntityWillBeDestroyed += ClearEventListenersOnDestroyed;
            }

            OnEntityCreated += LinkContextToEntity;
            //OnEntityWillBeDestroyed += UnlinkContextToEnity;
        }

        //private void UnlinkContextToEnity(IContext context, IEntity entity)
        //{
        //    if (entity is IContextLinkedEntity contextEntity)
        //    {
        //        contextEntity.Context = null;
        //        //entity.OnComponentAdded -= EntityOnOnComponentAdded;
        //    }
        //}

        private void LinkContextToEntity(IContext context, IEntity entity)
        {
            if (entity is IContextLinkedEntity contextEntity)
            {
                contextEntity.Context = this;
                //entity.OnComponentAdded += EntityOnOnComponentAdded;
            }
        }

        //private void EntityOnOnComponentAdded(IEntity entity, int index, IComponent component)
        //{            
        //    if (component is IEntityLinkedComponent linkedComponent)
        //    {
        //        linkedComponent.Link(this, entity);
        //    }
        //}

        private void ClearEventListenersOnDestroyed(IContext context, IEntity entity)
        {
            for (int i = 0; i < Definition.EventListenerIndices.Count; i++)
            {
                var index = Definition.EventListenerIndices[i];
                if (entity.HasComponent(index))
                {
                    var component = (IListenerComponent)entity.GetComponent(index);
                    component.ClearListeners();
                }
            }  
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
            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(listener);
        }

        public void RegisterAddedComponentListener<TComponent>(Action<(TEntity Entity, TComponent Component)> action) where TComponent : IComponent, new()
        {
            if (ComponentHelper<TContext, TComponent>.IsUnique)
            {
                RegisterAddedComponentListener(UniqueEntity, action);
            }
            else
            {
                RegisterAddedComponentListener(GetOrCreateEntityWith<TComponent>(), action);
            }
        }

        public void RegisterAddedTagListener<TComponent>(Action<(TEntity Entity, TComponent Component)> action) where TComponent : IFlagComponent, new()
        {
            RegisterAddedComponentListener(UniqueEntity, action);
        }

        public void RegisterAddedComponentListener<TComponent>(TEntity entity, Action<(TEntity Entity, TComponent Component)> action) where TComponent : IComponent, new()
        {
            var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(action);
        }

        public void RegisterRemovedTagListener<TComponent>(Action<TEntity> action) where TComponent : IFlagComponent, new()
        {
            RegisterRemovedComponentListener<TComponent>(UniqueEntity, action);
        }

        public void RegisterRemovedComponentListener<TComponent>(Action<TEntity> action) where TComponent : IComponent, new()
        {
            if(ComponentHelper<TContext,TComponent>.IsUnique)
            {
                RegisterRemovedComponentListener<TComponent>(UniqueEntity, action);
            }
            else
            {
                RegisterRemovedComponentListener<TComponent>(GetOrCreateEntityWith<TComponent>(), action);
            }            
        }

        public void RegisterRemovedComponentListener<TComponent>(TEntity entity, Action<TEntity> action) where TComponent : IComponent, new()
        {
            var component = GetOrCreateComponent<RemovedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(action);
        }

        public void RegisterRemovedComponentListener<TComponent>(TEntity entity, IEventObserver<TEntity> listener) where TComponent : IComponent
        {
            var component = GetOrCreateComponent<RemovedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(listener.OnEvent);
        }

        public void RegisterAddedComponentListener<TComponent>(IEventObserver<TComponent> listener) where TComponent : IComponent, new()
        {
            Debug.Log($"RegisterAddedComponentListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(t => listener.OnEvent(t.Component));
        }

        public void RegisterAddedComponentListener<TComponent>(TEntity entity, IEventObserver<TEntity, TComponent> listener) where TComponent : IComponent
        {
            Debug.Log($"RegisterAddedComponentListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(t => listener.OnEvent((t.Entity, t.Component)));
        }

        //public EntityAccessor<TEntity> GetAccessor(TEntity entity)
        //{
        //    return new EntityAccessor<TEntity>(this, entity);
        //}

        public void RegisterAddedComponentListener<TComponent>(IEventObserver<TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(t => listener.OnEvent((t.Entity, t.Component)));
        }

        //public void RegisterAddedComponentListener<TComponent>(TEntity entity, IEventObserver<GenericContext<TContext, TEntity>, TEntity, TComponent> listener) where TComponent : IComponent 
        //{
        //    Debug.Log($"RegisterAddedComponentListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

        //    var component = GetOrCreateComponent<ListenerHolderComponent<TEntity, TComponent>>(entity);
        //    component.Register(t => listener.OnEvent((this, t.Entity, t.Component)));
        //}

        public void AddEventListener<TComponent>(IEventObserver<IGenericContext<TEntity>, TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            Debug.Log($"RegisterAddedComponentListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(t => listener.OnEvent((this, t.Entity, t.Component)));
        }

        //public void RegisterAddedComponentListener<TComponent>(TEntity entity, IEventObserver<TComponent> listener) where TComponent : IComponent
        //{
        //    Debug.Log($"RegisterAddedComponentListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

        //    var component = GetOrCreateComponent<ListenerHolderComponent<TComponent>>(entity);
        //    component.Register(listener);
        //}

        //public void RegisterAddedComponentListener<TComponent>(TEntity entity, IEventObserver<TEntity, TComponent > listener) where TComponent : IComponent
        //{
        //    Debug.Log($"RegisterAddedComponentListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

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
            if (!TryGet(entity, out TComponent component))
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

        public void Set<TComponent>(TComponent component = default) where TComponent : IComponent, new()
        {         
            if (TryGetEntityWith<TComponent>(out var entity))
            {
                Set(entity, component);
            }
        }

        public IMatcher<TEntity> GetMatcher<T>() where T : IComponent, new()
        {
            return GenericMatcher<TContext, TEntity, T>.AllOf;
        }

        public ICollector<TEntity> GetCollector<T>() where T : IComponent, new()
        {
            return this.CreateCollector(GenericMatcher<TContext, TEntity, T>.AllOf);
        }

        private ICollector<TEntity> GetCollector<T>(TriggerOnEvent<TEntity> triggerOnEvent) where T : IComponent, new()
        {
            return this.CreateCollector(triggerOnEvent);
        }

        public IGroup<TEntity> GetGroup<T>() where T : IComponent, new()
        {
            return GetGroup(GenericMatcher<TContext, TEntity, T>.AllOf);
        }

        public TriggerOnEvent<TEntity> GetTrigger<T>(GroupEvent eventType = default) where T : IComponent, new()
        {
            return new TriggerOnEvent<TEntity>(GetMatcher<T>(), eventType);
        }

        public ICollector<TEntity> GetTriggerCollector<T>(GroupEvent eventType = default) where T : IComponent, new()
        {
            return GetCollector<T>(new TriggerOnEvent<TEntity>(GetMatcher<T>(), eventType));
        }

        public bool Has<T>(TEntity entity) where T : IComponent, new()
        {
            var index = ComponentHelper<TContext, T>.ComponentIndex;
            return entity.HasComponent(index);
        }

        public TComponent Get<TComponent>() where TComponent : IComponent, new()
        {
            var entity = GetOrCreateEntityWith<TComponent>();
            return Get<TComponent>(entity);
        }

        public bool HasComponent<T>() where T : IComponent, new()
        {
            return EntityExistsWithComponent<T>();
        }

        public TEntity FindEntity<TComponent, TValue>(TValue searchValue) where TComponent : IComponent, IEquatable<TValue>, new()
        {
            // todo review index builders
            var entities = GetGroup<TComponent>().GetEntities();
            for (var i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var component = Get<TComponent>(entity);
                if (component.Equals(searchValue))
                    return entity;
            }
            throw new InvalidOperationException($"Search for an '{typeof(TEntity).Name}' entity with the specific ''{typeof(TComponent).Name}'' value of type '{searchValue}' found no matches");
        }

        public bool TryFindEntity<TComponent, TValue>(TValue searchValue, out TEntity entity) where TComponent : IComponent, IEquatable<TValue>, new()
        {
            // todo review index builders
            foreach (var e in GetGroup<TComponent>().GetEntities())
            {
                var component = Get<TComponent>(e);
                if (component.Equals(searchValue))
                {
                    entity = e;
                    return true;
                }
            }
            entity = default;
            return false;
        }

        //public void NotifyChanged<TComponent>(TEntity entity) where TComponent : class, IComponent, new()
        //{
        //    var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
        //    entity.ReplaceComponent(index, component);
        //}

        public TComponent Get<TComponent>(TEntity entity) where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            return (TComponent)entity.GetComponent(index);
            //return (TComponent)entity.Components[index];
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

        public bool TryGet<T>(TEntity entity, out T component) where T : IComponent, new()
        {
            if (!Has<T>(entity))
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

        //private int GetComponentIndex<TComponent>()
        //{
        //    return ComponentHelper<TContext, TComponent>.ComponentIndex;
        //}

 

        int IEntityContext.GetComponentIndex<TComponent>()
        {
            return ComponentHelper<TContext, TComponent>.ComponentIndex;
        }

        //int IGenericContext<TEntity>.GetComponentIndex<TComponent>()
        //{
        //    return ComponentHelper<TContext, TComponent>.ComponentIndex;
        //}

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

        //public void Set<TComponent>(TComponent component = default) where TComponent : IComponent, new()
        //{
        //    throw new NotImplementedException();
        //}



        #region Unique

        public TEntity UniqueEntity => _uniqueEntity ?? (_uniqueEntity = CreateEntityWith(new UniqueComponents()));
        private TEntity _uniqueEntity;


        public void SetUnique<TComponent>(Action<TComponent> componentUpdater) where TComponent : IUniqueComponent, new()
        {            
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            var component = UniqueEntity.CreateComponent<TComponent>(index);
            componentUpdater(component);
            UniqueEntity.ReplaceComponent(index, component);
        }

        public void SetFlag<TComponent>(bool toggle = true) where TComponent : IFlagComponent, IUniqueComponent, new()
        {
            SetFlag<TComponent>(UniqueEntity, toggle);
        }

        public TComponent GetUnique<TComponent>() where TComponent : IUniqueComponent, new()
        {
            return GetOrCreateComponent<TComponent>(UniqueEntity);
        }

        public (TEntity Entity, TComponent Component) GetUniqueEntityAndComponent<TComponent>() where TComponent : IUniqueComponent, new()
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

        #endregion

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
        // designated as such with a [Tag] or IFlagComponent interface? Does it matter if the
        // user marks non-empty components as tags? is there value to be being able to have
        // tag-style boolean assignment for any component?

        //public TEntity UniqueFlagEntity => _flagEntity ?? (_flagEntity = CreateEntityWith(new UniqueFlags()));
        //private TEntity _flagEntity;

        public void SetFlag<TComponent>(TEntity entity, bool toggle = true) where TComponent : IFlagComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            var hasComponent = entity.HasComponent(index);

            if (toggle && !hasComponent)
            {
                entity.AddComponent(index, entity.CreateComponent<TComponent>(index));                
            }
            else if (!toggle && hasComponent)
            {
                entity.RemoveComponent(index);
            }
        }

        public bool IsFlagged<TComponent>() where TComponent : IFlagComponent, new() 
            => UniqueEntity.HasComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);

        public bool IsFlagged<TComponent>(TEntity entity) where TComponent : IFlagComponent, new()
            => entity.HasComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);

        #endregion


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

        public void Remove<TComponent>(TEntity entity) where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            entity.RemoveComponent(index);
        }

        public void Remove<TComponent>() where TComponent : IComponent, new()
        {
            if (TryGetEntityWith<TComponent>(out var entity))
            {
                entity.RemoveComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);
            }
        }

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
                entity = group.AsEnumerable().FirstOrDefault();
                return true;
            }
            entity = default;
            return false;
        }

        public TEntity GetOrCreateEntityWith<TComponent>() where TComponent : IComponent, new()
        {
            if(count == 0 || !TryGetEntityWith<TComponent>(out var entity))
            {
                return CreateEntityWith<TComponent>();
            }
            return entity;
        }

        public bool EntityExistsWithComponent<T>() where T : IComponent, new()
        {
            return GetGroup<T>().count > 0;
        }

        public TEntity CreateEntityWith<T>(T component = default) where T : IComponent, new()
        {
            TEntity entity = CreateEntity();
            Set(entity, component);
            return entity;
        }

        #region IEntityContext

        TComponent IEntityContext.Get<TComponent>(IEntity entity)
        {
            return (TComponent)entity.GetComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);
        }

        TComponent IEntityContext.GetOrCreateComponent<TComponent>(IEntity entity) 
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            if (entity.HasComponent(index))
            {
                return (TComponent)entity.GetComponent(index);
            }

            var component = entity.CreateComponent<TComponent>(index);
            UniqueEntity.AddComponent(index, component);
            return component;
        }

        bool IEntityContext.TryGet<TComponent>(IEntity entity, out TComponent component)
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            if (entity.HasComponent(index))
            {
                component = (TComponent)entity.GetComponent(index);
                return true;
            }
            component = default;
            return false;
        }

        void IEntityContext.SetFlag<TComponent>(IEntity entity, bool toggle)
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            if (toggle)
            {
                if (entity.HasComponent(index))
                    return;

                var component = entity.CreateComponent<TComponent>(index);
                UniqueEntity.AddComponent(index, component);
            }
            else
            {
                if (entity.HasComponent(index))
                {
                    UniqueEntity.RemoveComponent(index);
                }
            }
        }

        public void RegisterAddedComponentListener<TComponent>(IEntity entity, Action<(IEntity Entity, TComponent Component)> action) where TComponent : IComponent, new()
        {
            ((IGenericContext<TEntity>)this).RegisterAddedComponentListener(entity, action);
        }

        public void RegisterRemovedComponentListener<TComponent>(IEntity entity, Action<IEntity> action) where TComponent : IComponent, new()
        {
            ((IGenericContext<TEntity>)this).RegisterRemovedComponentListener<TComponent>(entity, action);
        }

        public bool IsFlagged<TComponent>(IEntity entity) where TComponent : IFlagComponent, new()
        {
            return ((IGenericContext<TEntity>)this).IsFlagged<TComponent>(entity);
        }

        bool IEntityContext.Has<TComponent>(IEntity entity)
        {
            return entity.HasComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);
        }

        //void IEntityContext.Set<TComponent>(IEntity entity, TComponent component)
        //{
        //    var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
        //    entity.ReplaceComponent(index, component);
        //}

        //public void RegisterAddedComponentListener<TComponent>(Action<(IEntity Entity, TComponent Component)> action) where TComponent : IComponent, new()
        //{
        //    ((IGenericContext<TEntity>)this).RegisterAddedComponentListener(action);
        //}

        //public void RegisterRemovedComponentListener<TComponent>(Action<IEntity> action) where TComponent : IComponent, new()
        //{
        //    ((IGenericContext<TEntity>)this).RegisterRemovedComponentListener<TComponent>(action);
        //}

        void IEntityContext.Set<TComponent>(IEntity entity, Action<TComponent> componentUpdater)
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            var newComponent = entity.CreateComponent<TComponent>(index);
            componentUpdater(newComponent);
            entity.ReplaceComponent(index, newComponent);
        }

        void IEntityContext.NotifyChanged<TComponent>(IEntity entity)
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            entity.ReplaceComponent(index, entity.GetComponent(index));
        }

        void IEntityContext.Remove<TComponent>(IEntity entity)
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            UniqueEntity.RemoveComponent(index);   
        }

        #endregion
    }



}
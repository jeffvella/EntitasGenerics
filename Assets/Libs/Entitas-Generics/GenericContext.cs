using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Instrumentation;
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
using System;
using Entitas.VisualDebugging.Unity.Editor;
using UnityEditor;
using UnityEditorInternal;
using SystemInfo = Entitas.VisualDebugging.Unity.SystemInfo;


namespace Entitas.Generics
{
    public interface IGenericContext<TEntity> : ISpecificEntityContext<TEntity> where TEntity : class, IEntity
    {
        IMatcher<TEntity> GetMatcher<T>() where T : IComponent, new();

        ICollector<TEntity> GetCollector<T>() where T : IComponent, new();

        IGroup<TEntity> GetGroup<T>() where T : IComponent, new();

        TEntity GetEntityWith<TComponent>() where TComponent : IComponent, new();

        TEntity FindEntity<TComponent, TValue>(TValue searchValue) where TComponent : IComponent, IEquatable<TValue>, new();

        bool TryFindEntity<TComponent, TValue>(TValue searchValue, out TEntity entity) where TComponent : IComponent, IEquatable<TValue>, new();

        TComponent Get<TComponent>(TEntity entity) where TComponent : IComponent, new();

        TriggerOnEvent<TEntity> GetTrigger<T>(GroupEvent eventType = default) where T : IComponent, new();

        void SetTag<TComponent>(TEntity entity, bool toggle = true) where TComponent : ITagComponent, new();

        bool IsTagged<TComponent>(TEntity entity) where TComponent : ITagComponent, new();

        bool HasComponent<TComponent>(TEntity entity) where TComponent : IComponent, new();

        void Set<TComponent>(TEntity entity, TComponent component = default) where TComponent : IComponent, new();

        void AddEventListener<TComponent>(Action<(TEntity Entity, TComponent Component)> action) where TComponent : IComponent, new();
        void AddEventListener<TComponent>(TEntity entity, Action<(TEntity Entity, TComponent Component)> action) where TComponent : IComponent, new();
        void RegisterComponentRemovedListener<TComponent>(Action<TEntity> action) where TComponent : IComponent, new();
        void RegisterComponentRemovedListener<TComponent>(TEntity entity, Action<TEntity> action) where TComponent : IComponent, new();
        void AddEventListener<TComponent>(TEntity entity, IEventObserver<(TEntity Entity, TComponent Component)> listener) where TComponent : IComponent;
        void AddEventListener<TComponent>(TEntity entity, IEventObserver<TEntity, TComponent> listener) where TComponent : IComponent;
        void Remove<TComponent>(TEntity entity) where TComponent : IComponent, new();
        EntityAccessor<TEntity> GetAccessor(TEntity entity);


        (TEntity Entity, TComponent Component) GetUniqueEntityAndComponent<TComponent>() where TComponent : IComponent, new();

        bool TryGetComponent<T>(TEntity entity, out T component) where T : IComponent, new();
    }

    public interface ISpecificEntityContext<TEntity> : IContext<TEntity> where TEntity : class, IEntity
    {
        TComponent GetUnique<TComponent>() where TComponent : IComponent, new();

        ICollector<TEntity> GetTriggerCollector<T>(GroupEvent eventType = default) where T : IComponent, new();

        void SetTag<TComponent>(bool toggle = true) where TComponent : ITagComponent, new();

        bool IsTagged<TComponent>() where TComponent : ITagComponent, new();

        void SetUnique<TComponent>(TComponent component) where TComponent : IComponent, new();

        void AddEventListener<TComponent>(IEventObserver<TComponent> listener) where TComponent : IComponent, new();

        void AddEventListener<TComponent>(IEventObserver<TEntity, TComponent> listener) where TComponent : IComponent, new();
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

                //return new GenericEventSystem<TEntity, TComponent, ListenerStorageComponent<TEntity, TComponent>>(context, type);

            //if (type == GroupEvent.Added)
            //{
                return new GenericEventSystem<TEntity, TComponent>(context, type);
            //}
            //if (type == GroupEvent.Removed)
            //{
            //    return new GenericEventSystem<TEntity, TComponent, RemovedListenerStorageComponent<TEntity, TComponent>>(context, GroupEvent.re);
            //}
            //if (type == GroupEvent.AddedOrRemoved)
            //{
            //    return new GenericEventSystem<TEntity, TComponent, RemovedListenerStorageComponent<TEntity, TComponent>>(context, GroupEvent.AddedOrRemoved);
            //}
        }
    }


    public class EventSystemListenerComponentDrawer : IComponentDrawer
    { 
        public bool HandlesType(Type type)
        {
            return typeof(IListenerComponent).IsAssignableFrom(type);
        }

        public IComponent DrawComponent(IComponent component)
        {
            var listenerComponent = (IListenerComponent)component;
            var listenerNames = listenerComponent.GetListenersNames();

            if(listenerNames.Length == 0)
            {
                EditorGUILayout.LabelField($"No Subscribers");
            }
            else
            {
                EditorGUILayout.LabelField($"{listenerNames.Length} Subscribers");

                for (int i = 0; i < listenerNames.Length; i++)
                {
                    EditorGUILayout.LabelField($"- {listenerNames[i]}");
                }
            }

            //person.name = EditorGUILayout.TextField("Name", person.name);
            //var gender = (PersonGender)Enum.Parse(typeof(PersonGender), person.gender);
            //gender = (PersonGender)EditorGUILayout.EnumPopup("Gender", gender);
            //person.gender = gender.ToString();
          
            return listenerComponent;
        }
    }

    public static class TypeExtensions
    {
        public static string PrettyPrintGenericTypeName(this Type type)
        {
            if (type.IsGenericType)
            {
                var simpleName = type.Name.Substring(0, type.Name.IndexOf('`'));
                var genericTypeParams = type.GenericTypeArguments.Select(PrettyPrintGenericTypeName).ToList();
                return string.Format("{0}<{1}>", simpleName, string.Join(", ", genericTypeParams));
            }
            return type.Name;
        }
    }

    public sealed class GenericEventSystem<TEntity, TComponent> : IReactiveSystem, IGenericEventSystem<TComponent>, ICustomDebugInfo
        where TEntity : class, IEntity
        where TComponent : IComponent, new()
    {
        private readonly GroupEvent _type;
        private readonly IGenericContext<TEntity> _context;
        private readonly ICollector<TEntity> _addedCollector;
        private readonly ICollector<TEntity> _removedCollector;
        private readonly bool _isRemoveType;
        private readonly bool _isAddType;


        public GenericEventSystem(IGenericContext<TEntity> context, GroupEvent type = GroupEvent.Added)
        {         
            _context = context;
            _type = type;

            _isAddType = _type == GroupEvent.Added || _type == GroupEvent.AddedOrRemoved;
            _isRemoveType = _type == GroupEvent.Removed || _type == GroupEvent.AddedOrRemoved;

            if (_isAddType)
            {
                _addedCollector = context.GetTriggerCollector<TComponent>(GroupEvent.Added);
            }

            if(_isRemoveType)
            {
                _removedCollector = context.GetTriggerCollector<TComponent>(GroupEvent.Removed);
            }
        }

        public string DisplayName => $"{typeof(TComponent).Name} {_type}";

        public void Execute()
        {
            if (_isAddType && _addedCollector.count > 0)
            {
                foreach (var entity in _addedCollector.collectedEntities)
                {
                    if (_context.HasComponent<ComponentAddedListenersComponent<TEntity, TComponent>>(entity))
                    {
                        var addedListenerComponent = _context.Get<ComponentAddedListenersComponent<TEntity, TComponent>>(entity);
                        if (addedListenerComponent.ListenerCount > 0)
                        {
                            if (_context.HasComponent<TComponent>(entity))
                            {
                                var changedComponent = _context.Get<TComponent>(entity);
                                addedListenerComponent.Raise((entity, changedComponent));
                            }
                        }
                    }
                }
                _addedCollector.ClearCollectedEntities();
            }

            if (_isRemoveType && _removedCollector.count > 0)
            {
                foreach (var entity in _removedCollector.collectedEntities)
                {
                    if (_context.HasComponent<ComponentRemovedListenersComponent<TEntity, TComponent>>(entity))
                    {
                        var removedListenersComponent = _context.Get<ComponentRemovedListenersComponent<TEntity, TComponent>>(entity);
                        if (removedListenersComponent.ListenerCount > 0)
                        {
                            removedListenersComponent.Raise(entity);
                        }
                    }
                }            
                _removedCollector.ClearCollectedEntities();
            }            
        }

        public void Activate()
        {
            _addedCollector?.Activate();
            _removedCollector?.Activate();
        }

        public void Deactivate()
        {
            _addedCollector?.Deactivate();
            _removedCollector?.Deactivate();
        }

        public void Clear()
        {
            _addedCollector?.ClearCollectedEntities();
            _removedCollector?.ClearCollectedEntities();
        }
    }

    //public sealed class GenericEventSystem<TEntity, TComponent, TListenerComponent> : GenericReactiveSystem<TEntity>, IGenericEventSystem<TComponent>, ICustomDebugInfo
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
 
        public GenericContext(IContextDefinition contextDefinition) 
            : base(contextDefinition.ComponentCount, 0, contextDefinition.ContextInfo, AercFactory, EntityFactory)
        {
            SetupVisualDebugging();

            Definition = contextDefinition;

            if (contextDefinition.EventListenerIndices.Count > 0)
            {
                OnEntityWillBeDestroyed += ClearEventListenersOnDestroyed;
            }
        }

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
            var component = GetOrCreateComponent<ComponentAddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(listener);
        }

        public void AddEventListener<TComponent>(Action<(TEntity Entity, TComponent Component)> action) where TComponent : IComponent, new()
        {
            var entity = GetOrCreateEntityWith<TComponent>();
            AddEventListener(entity, action);
        }

        public void AddEventListener<TComponent>(TEntity entity, Action<(TEntity Entity, TComponent Component)> action) where TComponent : IComponent, new()
        {
            var component = GetOrCreateComponent<ComponentAddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(action);
        }

        public void RegisterComponentRemovedListener<TComponent>(Action<TEntity> action) where TComponent : IComponent, new()
        {
            var entity = GetOrCreateEntityWith<TComponent>();
            RegisterComponentRemovedListener<TComponent>(entity, action);
        }

        public void RegisterComponentRemovedListener<TComponent>(TEntity entity, Action<TEntity> action) where TComponent : IComponent, new()
        {
            var component = GetOrCreateComponent<ComponentRemovedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(action);
        }

        public void AddEventListener<TComponent>(TEntity entity, IEventObserver<(TEntity Entity, TComponent Component)> listener) where TComponent : IComponent
        {
            var component = GetOrCreateComponent<ComponentAddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(listener);
        }

        public void AddEventListener<TComponent>(IEventObserver<TComponent> listener) where TComponent : IComponent, new()
        {
            Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<ComponentAddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(t => listener.OnEvent(t.Component));
        }

        public void AddEventListener<TComponent>(TEntity entity, IEventObserver<TEntity, TComponent> listener) where TComponent : IComponent
        {
            Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var component = GetOrCreateComponent<ComponentAddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(t => listener.OnEvent((t.Entity, t.Component)));
        }

        public EntityAccessor<TEntity> GetAccessor(TEntity entity)
        {
            return new EntityAccessor<TEntity>(this, entity);
        }

        public void AddEventListener<TComponent>(IEventObserver<TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            Debug.Log($"AddEventListener called for {typeof(TComponent).Name} / {typeof(TEntity).Name}");

            var entity = GetOrCreateEntityWith<TComponent>();
            var component = GetOrCreateComponent<ComponentAddedListenersComponent<TEntity, TComponent>>(entity);
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
            var component = GetOrCreateComponent<ComponentAddedListenersComponent<TEntity, TComponent>>(entity);
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
            return Matcher<TContext, TEntity, T>.AllOf;
        }

        public ICollector<TEntity> GetCollector<T>() where T : IComponent, new()
        {
            return this.CreateCollector(Matcher<TContext, TEntity, T>.AllOf);
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
            return GetEntityWith<T>()?.HasComponent(index) ?? false;
        }

        public TEntity FindEntity<TComponent, TValue>(TValue searchValue) where TComponent : IComponent, IEquatable<TValue>, new()
        {
            // todo review index builders
            foreach(var entity in GetGroup<TComponent>().GetEntities())
            {
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
            //if (newComponent is IListenerComponent listenerComponent)
            //{
            //    listenerComponent.ClearListeners();
            //}
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

        //public void Set<TComponent>(TComponent component = default) where TComponent : IComponent, new()
        //{
        //    throw new NotImplementedException();
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

        public TEntity UniqueTagEntity => _tagEntity ?? (_tagEntity = CreateEntityWith(new TagHolderComponent()));
        private TEntity _tagEntity;

        public void SetTag<TComponent>(TEntity entity, bool toggle = true) where TComponent : ITagComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            var hasComponent = entity.HasComponent(index);

            if (toggle && !hasComponent)
            {
                //Debug.Log($"Added Tag {typeof(TContext).Name}.{typeof(TComponent).Name}");
                if (ComponentHelper<TContext, TComponent>.IsUnique)
                {
                    entity.AddComponent(index, ComponentHelper<TContext, TComponent>.Default);
                }
                else
                {
                    entity.AddComponent(index, entity.CreateComponent<TComponent>(index));
                }
            }
            else if (!toggle && hasComponent)
            {
                //Debug.Log($"Removed Tag {typeof(TContext).Name}.{typeof(TComponent).Name}");
                entity.RemoveComponent(index);
            }
        }

        public void SetTag<TComponent>(bool toggle = true) where TComponent : ITagComponent, new()
        {
            SetTag<TComponent>(UniqueTagEntity, toggle);
        }

        public bool IsTagged<TComponent>() where TComponent : ITagComponent, new() 
            => UniqueTagEntity.HasComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);

        public bool IsTagged<TComponent>(TEntity entity) where TComponent : ITagComponent, new()
            => entity.HasComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);

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

        //public new EntityAccessor CreateEntity()
        //{
        //    Debug.Log("Entity created through custom Context");
        //    return new EntityAccessor(this, base.CreateEntity());
        //}

        //public class EntityAccessor
        //{
        //    public TEntity Entity;
        //    public IGenericContext<TEntity> Context;

        //    public EntityAccessor(IGenericContext<TEntity> context, TEntity entity)
        //    {
        //        Entity = entity;
        //        Context = context;
        //    }
        //}

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
                //  todo get first or throw?
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Entitas.VisualDebugging.Unity;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Entitas.Generics
{
    /// <summary>
    /// Provides component access for a specific TEntity
    /// </summary>
    public interface IGenericContext<TEntity> : IContext<TEntity> where TEntity : class, IEntity, IGenericEntity
    {
        //bool Has<TComponent>(TEntity entity) where TComponent : IComponent, new();

        //ComponentAccessor<TComponent> Get<TComponent>(TEntity entity) where TComponent : IComponent, new();

        //bool TryGet<T>(TEntity entity, out T component) where T : IComponent, new();

        //TComponent GetOrCreateComponent<TComponent>(TEntity entity) where TComponent : IComponent, new();

        //void Set<TComponent>(TEntity entity, TComponent component = default) where TComponent : IComponent, new();

        //void Remove<TComponent>(TEntity entity) where TComponent : IComponent, new();

        //// Flags:

        //bool IsFlagged<TComponent>(TEntity entity) where TComponent : IFlagComponent, new();

        //void SetFlag<TComponent>(TEntity entity, bool toggle = true) where TComponent : IFlagComponent, new();

        //// Events:

        //void RegisterAddedComponentListener<TComponent>(TEntity entity, Action<(TEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new();


        //void RegisterRemovedComponentListener<TComponent>(TEntity entity, Action<TEntity> action) where TComponent : IEventComponent, new();

        //TEntity UniqueEntity { get; }

        //IComponentSearchIndex<TEntity, TComponent> GetSearchIndex<TComponent>() where TComponent : class, ISearchableComponent<TComponent>, new();

        //TEntity FindEntity<TComponent, TValue>(TValue searchValue) where TComponent : IComponent, IEquatable<TValue>, new();

        //bool TryFindEntityLoop<TComponent, TValue>(TValue searchValue, out TEntity entity) where TComponent : IComponent, IEquatable<TValue>, new()

        TEntity Unique { get; }

        IMatcher<TEntity> GetMatcher<T>() where T : IComponent, new();

        ICollector<TEntity> GetCollector<T>() where T : IComponent, new();

        IGroup<TEntity> GetGroup<T>() where T : IComponent, new();

        TriggerOnEvent<TEntity> GetTrigger<T>(GroupEvent eventType = default) where T : IComponent, new();

        ICollector<TEntity> GetTriggerCollector<T>(GroupEvent eventType = default) where T : IComponent, new();

        bool TryFindEntity<TComponent, TValue>(TValue value, out TEntity entity) where TComponent : IEqualityComparer<TComponent>, IValueComponent<TValue>, new();

        //void SetUniqueFlag<T>() where T : IUniqueComponent, IFlagComponent new();
    }

    //public static class GenericContextConditionalExtensions
    //{
    //    public static void SetUniqueFlag<TEntity,T>(this IGenericContext<TEntity> context, bool value = true)
    //        where TEntity : class, IEntity, IGenericEntity
    //        where T : IUniqueComponent, IFlagComponent, new()
    //    {
    //        context.Unique.SetFlag<>
    //    }
    //}

    //public interface IEntityContext<TEntity> : IEntityContext where TEntity : class, IEntity, IGenericEntity
    //{


    //    //// Unique:

    //    //ComponentAccessor<TComponent> GetUnique<TComponent>() where TComponent : IUniqueComponent, new();

    //    ////ComponentAccessor<TComponent> WithUnique<TComponent>() where TComponent : IUniqueComponent, new();

    //    //void SetUnique<TComponent>(Action<TComponent> component) where TComponent : IUniqueComponent, new();

    //    //// Flags:

    //    //bool IsFlagged<TComponent>() where TComponent : IFlagComponent, new();

    //    //void SetFlag<TComponent>(bool toggle = true) where TComponent : IFlagComponent, IUniqueComponent, new();

    //    //// Events:

    //    //void RegisterAddedComponentListener<TComponent>(Action<(TEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new();

    //    //void RegisterAddedComponentListener<TComponent>(IAddedComponentListener<TEntity, TComponent> listener) where TComponent : IEventComponent, new();

    //    //void RegisterRemovedComponentListener<TComponent>(Action<TEntity> action) where TComponent : IEventComponent, new();

    //    //void RegisterRemovedComponentListener<TComponent>(IRemovedComponentListener<TEntity> listener) where TComponent : IEventComponent, new();

    //    //TEntity GetOrCreateEntityWith<TComponent>() where TComponent : IComponent, new();

    //    //bool EntityExistsWithComponent<TComponent>() where TComponent : IComponent, new();
    //}

    //public interface IEntityContext : IContext
    //{
    //    //bool Has<TComponent>(IEntity entity) where TComponent : IComponent, new();

    //    //TComponent Get<TComponent>(IEntity entity) where TComponent : IComponent, new();

    //    //TComponent Create<TComponent>(IEntity entity) where TComponent : IComponent, new();

    //    //bool TryGet<T>(IEntity entity, out T component) where T : IComponent, new();

    //    //TComponent GetOrCreate<TComponent>(IEntity entity) where TComponent : class, IComponent, new();

    //    //void Set<TComponent>(IEntity entity, TComponent component) where TComponent : class, IComponent, new();

    //    //void Set<TComponent>(IEntity entity, Action<TComponent> componentUpdater) where TComponent : class, IComponent, new();

    //    //void Remove<TComponent>(IEntity entity) where TComponent : IComponent, new();

    //    //// Flags

    //    //bool IsFlagged<TComponent>(IEntity entity) where TComponent : IFlagComponent, new();

    //    //void SetFlag<TComponent>(IEntity entity, bool toggle = true) where TComponent : IFlagComponent, new();

    //    //// Events

    //    //void RegisterAddedComponentListener<TComponent>(IEntity entity, Action<IEntity> action) where TComponent : IEventComponent, new();

    //    //void RegisterRemovedComponentListener<TComponent>(IEntity entity, Action<IEntity> action) where TComponent : IEventComponent, new();

    //    //// Helpers

    //    //int GetComponentIndex<TComponent>() where TComponent : IComponent, new();

    //    //void NotifyChanged<TComponent>(IEntity entity) where TComponent : class, IComponent, new();
    //}

    public class GenericContext<TContext, TEntity> : Context<TEntity>, IGenericContext<TEntity> 
        where TContext : IGenericContext<TEntity> 
        where TEntity : class, IEntity, IGenericEntity
    {
        public IContextDefinition<TEntity> Definition { get; }

        public TEntity Unique => _unique ?? (_unique = CreateUniqueEntity());

        private TEntity _unique;
        private IEntityIndex[] _primaryIndexes;


        public GenericContext(IContextDefinition<TEntity> contextDefinition) 
            : base(contextDefinition.ComponentCount, 0, contextDefinition.ContextInfo, AercFactory, contextDefinition.EntityFactory)
        {
            Definition = contextDefinition;

            if (contextDefinition.EventListenerIndices.Count > 0)
            {
                OnEntityWillBeDestroyed += ClearEventListenersOnDestroyed;
            }

            _primaryIndexes = new IEntityIndex[contextDefinition.ComponentCount];
        }

        private static IAERC AercFactory(IEntity entity)
        {
            return new UnsafeAERC();
        }

        private TEntity CreateUniqueEntity()
        {
            var entity = CreateEntity();
            entity.AddComponent(entity.CreateComponent<UniqueComponentsHolder>());
            return entity;
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

        public bool TryFindEntity<TComponent, TValue>(TValue value, out TEntity entity) where TComponent : IEqualityComparer<TComponent>, IValueComponent<TValue>, new()
        {
            var componentIndex = ComponentHelper<TEntity, TComponent>.Index;
            var searchIndex = (PrimaryEntityIndex<TEntity, TComponent>)_primaryIndexes[componentIndex];
            var pool = componentPools[componentIndex] ?? new Stack<IComponent>();
            var testComponent = pool.Count > 0 ? (TComponent)pool.Pop() : new TComponent();
            testComponent.Value = value;
            entity = searchIndex.GetEntity(testComponent);
            pool.Push(testComponent);
            return entity != null;
        }

        public void AddIndex<TComponent>() where TComponent : class, IEqualityComparer<TComponent>, IComponent, new()
        {
            var componentIndex = ComponentHelper<TEntity, TComponent>.Index;
            _primaryIndexes[componentIndex] = CreateIndex<TComponent>();
        }

        private IEntityIndex CreateIndex<TComponent>() where TComponent : class, IEqualityComparer<TComponent>, IComponent, new()
        {
            string name = nameof(TComponent);
            IGroup<TEntity> group = GetGroup<TComponent>();
            Func<TEntity, IComponent, TComponent> getKey = (e, c) => (TComponent)c;
            var index = new PrimaryEntityIndex<TEntity, TComponent>(name, group, getKey, ComponentHelper<TComponent>.Default);
            return index;
        }

        //public new EntityAccessor<TEntity> CreateEntity()
        //{
        //    return new EntityAccessor<TEntity>(this, base.CreateEntity());
        //}

        //private void ClearIndexedComponentsOnDestroyed(IContext context, IEntity entity)
        //{
        //    for (int i = 0; i < Definition.SearchableComponentIndices.Count; i++)
        //    {
        //        _searchIndexes[Definition.SearchableComponentIndices[i]].Clear((TEntity)entity);          
        //    }            
        //}

        //private void LinkContextToEntity(IContext context, IEntity entity)
        //{
        //    ((ILinkedEntity)IEntity).Context = this;
        //}

        //private void OnComponentRemoved(IEntity entity, int index, IComponent component)
        //{
        //    if (component is ISearchableComponent indexedComponent)
        //    {
        //        //_searchIndexes[index].Remove(component);
        //    }
        //}

        //private void OnComponentReplaced(IEntity entity, int index, IComponent previouscomponent, IComponent newcomponent)
        //{
        //    if (newcomponent is ISearchableComponent indexedComponent)
        //    {
        //        //_searchIndexes[index].Update((TEntity)entity, previouscomponent, indexedComponent);
        //    }
        //}

        //private void OnComponentAdded(IEntity entity, int index, IComponent component)
        //{
        //    if (component is ISearchableComponent indexedComponent)
        //    {
        //        //_searchIndexes[index].Add((TEntity)entity, indexedComponent);
        //    }
        //}

        //public bool TryFindEntity<TComponent>(Action<TComponent> componentValueProducer, out TEntity entity) where TComponent : IIndexedComponent, new()
        //{  
        //    if(((ComponentIndex<TEntity,TComponent>)_searchIndexes[ComponentHelper<TEntity, TComponent>.ComponentIndex]).TryFindEntity(componentValueProducer, out entity))
        //    {
        //        return true;
        //    }
        //    entity = default;
        //    return false;         
        //}



        //public bool TryFindEntity2<TComponent, TValue>(TValue value, out TEntity entity) where TComponent : IIndexedComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    var searcher = (EntityByComponentSearcher<TEntity, TComponent>)_searchIndexes[index];
        //    if (searcher.TryFindEntity(value, out entity))
        //    {
        //        return true;
        //    }
        //    entity = default;
        //    return false;
        //}

        //public bool TryFindEntity2<TComponent, TValue>(TValue value, out TEntity entity) where TComponent : IIndexedComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    if (((ComponentIndex<TEntity, TComponent>)_searchIndexes[index]).TryFindEntity(value, out entity))
        //    {
        //        return true;
        //    }
        //    entity = default;
        //    return false;
        //}

        //public EntityByComponentSearchIndex<TEntity, TComponent> GetEntitySearcher<TComponent>() 
        //    where TComponent : ISearchableComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    return (EntityByComponentSearchIndex<TEntity, TComponent>)_searchIndexes[index];
        //}

        //public bool TryFindEntity2<TComponent>(ValueHolder<TComponent> valueHolder, out TEntity entity) where TComponent : IIndexedComponent, new()
        //{
        //    var component = valueHolder.Get;
        //    return true;
        //}

        //public bool EntityWithComponentValueExists<TComponent>(Action<TComponent> componentValueProducer) where TComponent : IIndexedComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    return ((ComponentIndex<TEntity,TComponent>)_searchIndexes[index]).Contains(componentValueProducer);
        //}

        //public void RegisterAddedComponentListener<TComponent>(Action<TEntity> action) where TComponent : IEventComponent, new()
        //{
        //    RegisterAddedComponentListener(UniqueEntity, action);
        //}

        //public void RegisterAddedComponentListener<TComponent>(TEntity entity, Action<TEntity> action) where TComponent : IEventComponent, new()
        //{
        //    var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(entity);
        //    component.Register(action);
        //}

        //public void RegisterAddedComponentListener2<TComponent>(TEntity entity) where TComponent : IEventComponent, new()
        //{
        //    var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(entity);
        //    component.Register(action);
        //}

        //public void RegisterAddedComponentListener<TComponent>(IAddedComponentListener<TEntity, TComponent> listener) where TComponent : IEventComponent, new()
        //{
        //    RegisterAddedComponentListener(UniqueEntity, listener);
        //}

        //public void RegisterAddedComponentListener<TComponent>(TEntity entity, IAddedComponentListener<TEntity, TComponent> listener) where TComponent : IEventComponent, new()
        //{
        //    var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(entity);
        //    component.Register(arg => listener.OnComponentAdded(arg.Entity, arg.Component));
        //}

        //public void AddEventListener<TComponent>(IEventObserver<(TEntity Entity, TComponent Component)> listener) where TComponent : IComponent, new()
        //{
        //    var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(UniqueEntity);
        //    component.Register(listener);
        //}

        //public void RegisterRemovedComponentListener<TComponent>(Action<TEntity> action) where TComponent : IEventComponent, new()
        //{
        //    RegisterRemovedComponentListener<TComponent>(UniqueEntity, action);                     
        //}
        //public void RegisterRemovedComponentListener<TComponent>(TEntity entity, Action<TEntity> action) where TComponent : IEventComponent, new()
        //{
        //    var component = GetOrCreateComponent<RemovedListenersComponent<TEntity, TComponent>>(entity);
        //    component.Register(action);
        //}

        //public void RegisterRemovedComponentListener<TComponent>(IRemovedComponentListener<TEntity> listener) where TComponent : IEventComponent, new()
        //{
        //    RegisterRemovedComponentListener<TComponent>(UniqueEntity, listener);
        //}

        //public void RegisterRemovedComponentListener<TComponent>(TEntity entity, IRemovedComponentListener<TEntity> listener) where TComponent : IEventComponent, new()
        //{
        //    var component = GetOrCreateComponent<RemovedListenersComponent<TEntity, TComponent>>(entity);
        //    component.Register(listener.OnComponentRemoved);
        //}

        //public void RegisterRemovedComponentListener<TComponent>(TEntity entity, IEventObserver<TEntity> listener) where TComponent : IEventComponent
        //{
        //    var component = GetOrCreateComponent<RemovedListenersComponent<TEntity, TComponent>>(entity);
        //    component.Register(listener.OnEvent);
        //}

        //public TComponent GetOrCreateComponent<TComponent>(TEntity entity) where TComponent : IComponent, new()
        //{
        //    if (!TryGet(entity, out TComponent component))
        //    {
        //        component = CreateAndAddComponent<TComponent>(entity);
        //    }
        //    return component;
        //}

        //public void Set<TComponent>(TComponent component = default) where TComponent : IComponent, new()
        //{         
        //    if (TryGetEntityWith<TComponent>(out var entity))
        //    {
        //        Set(entity, component);
        //    }
        //}

        public IMatcher<TEntity> GetMatcher<T>() where T : IComponent, new()
        {
            return GenericMatcher<TContext, TEntity, T>.AllOf;
        }

        public ICollector<TEntity> GetCollector<T>() where T : IComponent, new()
        {
            return this.CreateCollector(GenericMatcher<TContext, TEntity, T>.AllOf);
        }

        public ICollector<TEntity> GetCollector<T>(params TriggerOnEvent<TEntity>[] triggers) where T : IComponent, new()
        {
            return this.CreateCollector(triggers);
        }

        private ICollector<TEntity> GetCollector<T>(TriggerOnEvent<TEntity> trigger) where T : IComponent, new()
        {
            return this.CreateCollector(trigger);
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

        //public bool Has<T>(TEntity entity) where T : IComponent, new()
        //{
        //    var index = ComponentHelper<TContext, T>.ComponentIndex;
        //    return entity.HasComponent(index);
        //}

        //public ComponentAccessor<TComponent> Get<TComponent>(TEntity entity) where TComponent : IComponent, new()
        //{
        //    return new ComponentAccessor<TComponent>(entity, this);

        //    //var entity = GetOrCreateEntityWith<TComponent>();
        //    //return Get<TComponent>(entity);
        //}

        //public EntityAccessor<TEntity> CreateAccessor(TEntity entity)
        //{
        //    return new EntityAccessor<TEntity>(this, entity);
        //}

        //public bool HasComponent<T>() where T : IComponent, new()
        //{
        //    return EntityExistsWithComponent<T>();
        //}

        //public TEntity FindEntity<TComponent, TValue>(TValue searchValue) where TComponent : ISearchableComponent<TComponent>, IValueComponent<TValue>, IEquatable<TValue>, new()
        //{
        //    var sw = new Stopwatch();
        //    TEntity result = default;
        //    var entities = GetGroup<TComponent>().GetEntities();
        //    for (var i = 0; i < entities.Length; i++)
        //    {
        //        var entity = entities[i];
        //        var component = Get<TComponent>(entity);
        //        if (component.Component.Equals(searchValue))
        //        {
        //            result = entity;
        //        }
        //    }
        //    return result;
        //    //throw new InvalidOperationException($"Search for an '{typeof(TEntity).Name}' entity with the specific ''{typeof(TComponent).Name}'' value of type '{searchValue}' found no matches");
        //}

        //public bool TryFindEntityLoop<TComponent, TValue>(TValue searchValue, out TEntity entity) where TComponent : ISearchableComponent<TComponent>, IValueComponent<TValue>, new()
        //{
        //    foreach (var e in GetGroup<TComponent>().GetEntities())
        //    {
        //        var component = Get<TComponent>(e);
        //        if (component.Equals(searchValue))
        //        {
        //            entity = e;
        //            return true;
        //        }
        //    }
        //    entity = default;
        //    return false;
        //}

        //public TComponent Get<TComponent>(TEntity entity) where TComponent : IComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    return (TComponent)entity.GetComponent(index);
        //}

        //public static TComponent CreateAndAddComponent<TComponent>(TEntity entity) where TComponent : IComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    return CreateAndAddComponent<TComponent>(entity, index);
        //}

        //public static TComponent CreateAndAddComponent<TComponent>(TEntity entity, int index) where TComponent : IComponent, new()
        //{
        //    var newComponent = entity.CreateComponent<TComponent>(index);
        //    entity.AddComponent(index, newComponent);
        //    return newComponent;
        //}

        //public bool TryGet<T>(TEntity entity, out T component) where T : IComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, T>.ComponentIndex;
        //    if (!entity.HasComponent(index))
        //    {
        //        component = default;
        //        return false;
        //    }
        //    component = (T)entity.GetComponent(index);
        //    return true;
        //}

        //public void Set<TComponent>(TEntity entity, TComponent component = default) where TComponent : IComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    if (component == null)
        //    {
        //        CreateAndAddComponent<TComponent>(entity, index);
        //    }
        //    else
        //    {
        //        entity.ReplaceComponent(index, component);
        //    }            
        //}

        //int IEntityContext.GetComponentIndex<TComponent>()
        //{
        //    return ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //}

        //public void Set<TComponent>(TEntity entity, bool state) where TComponent : IComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    if (state)
        //    {
        //        var component = entity.CreateComponent<TComponent>(index);
        //        entity.AddComponent(index, component);
        //    }
        //    else
        //    {
        //        entity.RemoveComponent(index);
        //    }
        //}


        //public void SetUnique<TComponent>(Action<TComponent> componentUpdater) where TComponent : IUniqueComponent, new()
        //{            
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    var component = UniqueEntity.CreateComponent<TComponent>(index);
        //    componentUpdater(component);
        //    UniqueEntity.ReplaceComponent(index, component);
        //}

        //public void SetFlag<TComponent>(bool value = true) where TComponent : IFlagComponent, IUniqueComponent, new()
        //{
        //    //SetFlag<TComponent>(UniqueEntity, toggle);

        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    if(UniqueEntity.HasComponent(index))
        //    {
        //        if (!value)
        //        {
        //            UniqueEntity.RemoveComponent(index);          
        //        }
        //    }
        //    else if (value)
        //    {
        //        UniqueEntity.AddComponent(index, UniqueEntity.CreateComponent<TComponent>(index));
        //    }
        //}

        //public ComponentAccessor<TComponent> GetUnique<TComponent>() where TComponent : IUniqueComponent, new()
        //{            
        //    return new ComponentAccessor<TComponent>(UniqueEntity, this);

        //    //return GetOrCreateComponent<TComponent>(UniqueEntity);
        //}

        //public TComponent GetUnique<TComponent>() where TComponent : IUniqueComponent, new()
        //{
        //    return UniqueEntity.Get<TComponent>(this).Component;
        //}

        //public ComponentAccessor<TComponent> WithUnique<TComponent>() where TComponent : IUniqueComponent, new()
        //{
        //    return UniqueEntity.Get<TComponent>(this);
        //}

        //public void SetFlag<TComponent>(TEntity entity, bool toggle = true) where TComponent : IFlagComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    var hasComponent = entity.HasComponent(index);

        //    if (toggle && !hasComponent)
        //    {
        //        entity.AddComponent(index, entity.CreateComponent<TComponent>(index));                
        //    }
        //    else if (!toggle && hasComponent)
        //    {
        //        entity.RemoveComponent(index);
        //    }
        //}

        //public bool IsFlagged<TComponent>() where TComponent : IFlagComponent, new() 
        //    => UniqueEntity.HasComponent(ComponentHelper<TEntity, TComponent>.ComponentIndex);

        //public bool IsFlagged<TComponent>(TEntity entity) where TComponent : IFlagComponent, new()
        //    => entity.HasComponent(ComponentHelper<TEntity, TComponent>.ComponentIndex);


        //public void ReplaceComponent<T>(TEntity entity, T component) where T : IComponent, new()
        //{
        //    var index = ComponentHelper<TContext, T>.ComponentIndex;
        //    entity.ReplaceComponent(index, component);
        //}

        //public void ReplaceComponent<TComponent>(TComponent component) where TComponent : IComponent, new()
        //{
        //    if (TryGetEntityWith<TComponent>(out var entity))
        //    {
        //        var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //        entity.ReplaceComponent(index, component);
        //    }
        //}

        //public void Remove<TComponent>(TEntity entity) where TComponent : IComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    entity.RemoveComponent(index);
        //}

        //public void Remove<TComponent>() where TComponent : IComponent, new()
        //{
        //    if (TryGetEntityWith<TComponent>(out var entity))
        //    {
        //        entity.RemoveComponent(ComponentHelper<TEntity, TComponent>.ComponentIndex);
        //    }
        //}

        //public bool TryGetEntityWith<TComponent>(out TEntity entity) where TComponent : IComponent, new()
        //{
        //    if (count == 0)
        //    {
        //        entity = default;
        //        return false;
        //    }
        //    var group = GetGroup<TComponent>();
        //    if(group.count > 0)
        //    { 
        //        entity = group.AsEnumerable().FirstOrDefault();
        //        return true;
        //    }
        //    entity = default;
        //    return false;
        //}

        //public TEntity GetOrCreateEntityWith<TComponent>() where TComponent : IComponent, new()
        //{
        //    if (ComponentHelper<TEntity, TComponent>.IsUnique)
        //    {
        //        return UniqueEntity;
        //    }
        //    if (count == 0)
        //    {
        //        return CreateEntityWith<TComponent>();
        //    }
        //    if (!TryGetEntityWith<TComponent>(out var entity))
        //    {
        //        return CreateEntityWith<TComponent>();
        //    }
        //    return entity;
        //}

        //public bool EntityExistsWithComponent<T>() where T : IComponent, new()
        //{
        //    return GetGroup<T>().count > 0;
        //}

        //public TEntity CreateEntityWith<T>(T component = default) where T : IComponent, new()
        //{
        //    TEntity entity = CreateEntity();
        //    Set(entity, component);
        //    return entity;
        //}

        //#region IEntityContext

        //TComponent IEntityContext.Get<TComponent>(IEntity entity)
        //{
        //    return (TComponent)entity.GetComponent(ComponentHelper<TEntity, TComponent>.ComponentIndex);
        //}

        //TComponent IEntityContext.GetOrCreate<TComponent>(IEntity entity) 
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;

        //    TComponent component = !entity.HasComponent(index)
        //        ? entity.CreateComponent<TComponent>(index)
        //        : (TComponent)entity.GetComponent(index);

        //    return component;
        //}

        //TComponent IEntityContext.Create<TComponent>(IEntity entity)
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    return entity.CreateComponent<TComponent>(index);
        //}

        //bool IEntityContext.TryGet<TComponent>(IEntity entity, out TComponent component)
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    if (entity.HasComponent(index))
        //    {
        //        component = (TComponent)entity.GetComponent(index);
        //        return true;
        //    }
        //    component = default;
        //    return false;
        //}

        //void IEntityContext.SetFlag<TComponent>(IEntity entity, bool toggle)
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    if (toggle)
        //    {
        //        if (entity.HasComponent(index))
        //            return;

        //        var component = entity.CreateComponent<TComponent>(index);
        //        entity.AddComponent(index, component);
        //    }
        //    else
        //    {
        //        if (entity.HasComponent(index))
        //        {
        //            entity.RemoveComponent(index);
        //        }
        //    }
        //}

        //public void RegisterAddedComponentListener<TComponent>(IEntity entity, Action<(IEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new()
        //{
        //    var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>((TEntity)entity);
        //    component.Register(args => action((args.Entity, args.Component)));
        //}

        //public void RegisterRemovedComponentListener<TComponent>(IEntity entity, Action<IEntity> action) where TComponent : IEventComponent, new()
        //{
        //    var component = GetOrCreateComponent<RemovedListenersComponent<TEntity, TComponent>>((TEntity)entity);
        //    component.Register(action);
        //}

        //public bool IsFlagged<TComponent>(IEntity entity) where TComponent : IFlagComponent, new()
        //{
        //    return entity.HasComponent(ComponentHelper<TEntity, TComponent>.ComponentIndex);
        //}

        //bool IEntityContext.Has<TComponent>(IEntity entity)
        //{
        //    return entity.HasComponent(ComponentHelper<TEntity, TComponent>.ComponentIndex);
        //}

        //void IEntityContext.Set<TComponent>(IEntity entity, Action<TComponent> componentUpdater)
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    var newComponent = entity.CreateComponent<TComponent>(index);
        //    componentUpdater(newComponent);            
        //    entity.ReplaceComponent(index, newComponent);
        //}

        //void IEntityContext.Set<TComponent>(IEntity entity, TComponent component)
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    entity.ReplaceComponent(index, component);
        //}

        ////public IEntityIndex CreateIndex<TComponent, TValue>(TComponent component) where TComponent : class, ISearchKeyProvider<TComponent, TValue>, new()
        ////{
        ////    var name = nameof(TComponent) + nameof(PrimaryEntityIndex<TEntity, TValue>);  
        ////    var index = new PrimaryEntityIndex<TEntity, TValue>(name, GetGroup<TComponent>(), (e,c) => component.IndexKeyRetriever(c));
        ////    return index;
        ////}



        ////public static class AccessorFactory
        ////{

        ////}

        ////private ComponentAccessor<TComponent, TValue>[] _accessors;

        ////public ValueComponent<TComponent, TValue> Get<TComponent, TValue>(IEntity entity) where TComponent : class, IValueComponent<TValue>, new()
        ////{
        ////    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex; 
        ////    return new ValueComponent<TComponent, TValue>(entity, index);
        ////    //return _accessors[index];
        ////}

        //void IEntityContext.NotifyChanged<TComponent>(IEntity entity)
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    entity.ReplaceComponent(index, entity.GetComponent(index));
        //}

        //void IEntityContext.Remove<TComponent>(IEntity entity)
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        //    UniqueEntity.RemoveComponent(index);   
        //}

        ////IComponentSearchIndex<TEntity, TComponent> IGenericContext<TEntity>.GetSearchIndex<TComponent>()
        ////{
        ////    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        ////    return (IComponentSearchIndex<TEntity, TComponent>)_searchIndexes[index];
        ////}

        ////public IComponentSearchIndex<TEntity, TComponent> CreateCustomIndex<TComponent>(string name, IGroup<TComponent> group, IEqualityComparer<TComponent> comparer) 
        ////    where TComponent : class, IComponent, new()
        ////{
        ////    var group = GetGroup<TComponent>
        ////    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;
        ////    var comparer = ComponentHelper<TEntity, TComponent>.Default;
        ////    var searchIndex = new EntityByComponentSearchIndex<TEntity, TComponent>(comparer);
        ////    if (_searchIndexes[index] != null)
        ////        throw new IndexAlreadyExistsException($"A search index for '{typeof(TComponent).Name}' already exists in the context '{typeof(TContext).Name}'");
        ////    _searchIndexes[index] = searchIndex;
        ////    return searchIndex;
        ////}

        ////private Dictionary<string,comparer>

        //#endregion
    }
}

//public class IndexAlreadyExistsException : Exception
//{
//    public IndexAlreadyExistsException() : base() { }
//    public IndexAlreadyExistsException(string msg) : base(msg) { }
//}
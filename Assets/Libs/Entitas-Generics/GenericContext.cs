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
    public interface IGenericContext<TEntity> : IContext<TEntity>, IEntityContext<TEntity>, IEntityContext where TEntity : class, IEntity
    {
        bool Has<TComponent>(TEntity entity) where TComponent : IComponent, new();

        TComponent Get<TComponent>(TEntity entity) where TComponent : IComponent, new();

        bool TryGet<T>(TEntity entity, out T component) where T : IComponent, new();

        TComponent GetOrCreateComponent<TComponent>(TEntity entity) where TComponent : IComponent, new();

        void Set<TComponent>(TEntity entity, TComponent component = default) where TComponent : IComponent, new();

        void Remove<TComponent>(TEntity entity) where TComponent : IComponent, new();

        // Flags:

        bool IsFlagged<TComponent>(TEntity entity) where TComponent : IFlagComponent, new();

        void SetFlag<TComponent>(TEntity entity, bool toggle = true) where TComponent : IFlagComponent, new();

        // Events:

        void RegisterAddedComponentListener<TComponent>(TEntity entity, Action<(TEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new();

        void RegisterRemovedComponentListener<TComponent>(TEntity entity, Action<TEntity> action) where TComponent : IEventComponent, new();

        TEntity UniqueEntity { get; }
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

        // Unique:

        TComponent GetUnique<TComponent>() where TComponent : IUniqueComponent, new();

        void SetUnique<TComponent>(Action<TComponent> component) where TComponent : IUniqueComponent, new();

        // Flags:
 
        bool IsFlagged<TComponent>() where TComponent : IFlagComponent, new();

        void SetFlag<TComponent>(bool toggle = true) where TComponent : IFlagComponent, IUniqueComponent, new();

        // Events:

        void RegisterAddedComponentListener<TComponent>(Action<(TEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new();

        void RegisterAddedComponentListener<TComponent>(IAddedComponentListener<TEntity, TComponent> listener) where TComponent : IEventComponent, new();

        void RegisterRemovedComponentListener<TComponent>(Action<TEntity> action) where TComponent : IEventComponent, new();

        void RegisterRemovedComponentListener<TComponent>(IRemovedComponentListener<TEntity> listener) where TComponent : IEventComponent, new();

        // Entity Searches:

        TEntity FindEntity<TComponent, TValue>(TValue searchValue) where TComponent : IComponent, IEquatable<TValue>, new();

        bool TryFindEntity<TComponent, TValue>(TValue searchValue, out TEntity entity) where TComponent : IComponent, IEquatable<TValue>, new();

        //bool TryFindEntity2<TComponent>(Action<TComponent> componentValueProducer, out TEntity entity) where TComponent : IIndexedComponent<TComponent>, new();
        //bool TryFindEntity2<TComponent>(Action<TComponent> componentValueProducer, out TEntity entity) where TComponent : IComponent, new();

        bool TryFindEntity<TComponent>(Action<TComponent> componentValueProducer, out TEntity entity) where TComponent : IIndexedComponent, new();

        bool EntityWithComponentValueExists<TComponent>(Action<TComponent> componentValueProducer) where TComponent : IIndexedComponent, new();

        // Entity/Component Searches:

        TEntity GetOrCreateEntityWith<TComponent>() where TComponent : IComponent, new();

        bool EntityExistsWithComponent<TComponent>() where TComponent : IComponent, new();

    }

    /// <summary>
    /// Provides component access for a specific IEntity.
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

        void RegisterAddedComponentListener<TComponent>(IEntity entity, Action<(IEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new();

        void RegisterRemovedComponentListener<TComponent>(IEntity entity, Action<IEntity> action) where TComponent : IEventComponent, new();

        // Helpers

        int GetComponentIndex<TComponent>() where TComponent : IComponent, new();

        void NotifyChanged<TComponent>(IEntity entity) where TComponent : class, IComponent, new();
    }

    public class GenericContext<TContext, TEntity> : Context<TEntity>, IGenericContext<TEntity>
        where TContext : IGenericContext<TEntity>
        where TEntity : class, IEntity, new()
    {
        public IContextDefinition<TEntity> Definition { get; }

        private IComponentSearchIndex<TEntity>[] _searchIndexes;

        public GenericContext(IContextDefinition<TEntity> contextDefinition) 
            : base(contextDefinition.ComponentCount, 0, contextDefinition.ContextInfo, AercFactory, EntityFactory)
        {
            Definition = contextDefinition;

            if (contextDefinition.EventListenerIndices.Count > 0)
            {
                OnEntityWillBeDestroyed += ClearEventListenersOnDestroyed;
            }

            if (contextDefinition.SearchableComponentIndices.Count > 0)
            {
                OnEntityWillBeDestroyed += ClearIndexedComponentsOnDestroyed;
            }

            OnEntityCreated += LinkContextToEntity;            

            _searchIndexes = contextDefinition.SearchIndexes.ToArray();

            //_indices = new ComponentIndex<TEntity>[contextDefinition.ComponentCount];

        }

        private void RemoveEntityIndexedComponents(IContext context, IEntity entity)
        {
            
        }

        private static TEntity EntityFactory()
        {
            return new TEntity();
        }

        private static IAERC AercFactory(IEntity entity)
        {
            return new UnsafeAERC();
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

        private void ClearIndexedComponentsOnDestroyed(IContext context, IEntity entity)
        {
            for (int i = 0; i < Definition.SearchableComponentIndices.Count; i++)
            {
                _searchIndexes[Definition.SearchableComponentIndices[i]].Clear((TEntity)entity);          
            }            
        }

        private void LinkContextToEntity(IContext context, IEntity entity)
        {
            if (entity is IContextLinkedEntity contextEntity)
            {
                contextEntity.Context = this;                
            }       
            entity.OnComponentAdded += OnComponentAdded;
            entity.OnComponentReplaced += OnComponentReplaced;
        }

        private void OnComponentReplaced(IEntity entity, int index, IComponent previouscomponent, IComponent newcomponent)
        {
            if (newcomponent is IIndexedComponent indexedComponent)
            {
                //Debug.Log($"Replaced: {previouscomponent.GetType().Name} ({previouscomponent}) for {newcomponent} on Entity={entity}");

                _searchIndexes[index].Update((TEntity)entity, previouscomponent, newcomponent);
            }
        }

        private void OnComponentAdded(IEntity entity, int index, IComponent component)
        {
            if (component is IIndexedComponent indexedComponent)
            {
                //Debug.Log($"Added: {component.GetType().Name} for {entity.GetType().Name} Entity={entity} Component={component}");

                _searchIndexes[index].Add((TEntity)entity, indexedComponent);
            }
        }

        public bool TryFindEntity<TComponent>(Action<TComponent> componentValueProducer, out TEntity entity) where TComponent : IIndexedComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            if(_searchIndexes[index].TryFindEntity(componentValueProducer, out entity))
            {
                return true;
            }
            entity = default;
            return false;         
        }

        public bool EntityWithComponentValueExists<TComponent>(Action<TComponent> componentValueProducer) where TComponent : IIndexedComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            return _searchIndexes[index].Contains(componentValueProducer);
        }

        public void RegisterAddedComponentListener<TComponent>(Action<(TEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new()
        {
            RegisterAddedComponentListener(UniqueEntity, action);
        }

        public void RegisterAddedComponentListener<TComponent>(TEntity entity, Action<(TEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new()
        {
            var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(action);
        }

        public void RegisterAddedComponentListener<TComponent>(IAddedComponentListener<TEntity, TComponent> listener) where TComponent : IEventComponent, new()
        {
            RegisterAddedComponentListener(UniqueEntity, listener);
        }

        public void RegisterAddedComponentListener<TComponent>(TEntity entity, IAddedComponentListener<TEntity, TComponent> listener) where TComponent : IEventComponent, new()
        {
            var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(UniqueEntity);
            component.Register(arg => listener.OnComponentAdded(arg.Entity, arg.Component));
        }

        public void AddEventListener<TComponent>(IEventObserver<(TEntity Entity, TComponent Component)> listener) where TComponent : IComponent, new()
        {
            var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>(UniqueEntity);
            component.Register(listener);
        }

        public void RegisterRemovedComponentListener<TComponent>(Action<TEntity> action) where TComponent : IEventComponent, new()
        {
            RegisterRemovedComponentListener<TComponent>(UniqueEntity, action);                     
        }
        public void RegisterRemovedComponentListener<TComponent>(TEntity entity, Action<TEntity> action) where TComponent : IEventComponent, new()
        {
            var component = GetOrCreateComponent<RemovedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(action);
        }

        public void RegisterRemovedComponentListener<TComponent>(IRemovedComponentListener<TEntity> listener) where TComponent : IEventComponent, new()
        {
            RegisterRemovedComponentListener<TComponent>(UniqueEntity, listener);
        }

        public void RegisterRemovedComponentListener<TComponent>(TEntity entity, IRemovedComponentListener<TEntity> listener) where TComponent : IEventComponent, new()
        {
            var component = GetOrCreateComponent<RemovedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(listener.OnComponentRemoved);
        }

        public void RegisterRemovedComponentListener<TComponent>(TEntity entity, IEventObserver<TEntity> listener) where TComponent : IEventComponent
        {
            var component = GetOrCreateComponent<RemovedListenersComponent<TEntity, TComponent>>(entity);
            component.Register(listener.OnEvent);
        }

        public TComponent GetOrCreateComponent<TComponent>(TEntity entity) where TComponent : IComponent, new()
        {
            if (!TryGet(entity, out TComponent component))
            {
                component = CreateAndAddComponent<TComponent>(entity);
            }
            return component;
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
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            return (TComponent)entity.GetComponent(index);
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

        int IEntityContext.GetComponentIndex<TComponent>()
        {
            return ComponentHelper<TContext, TComponent>.ComponentIndex;
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
            if (ComponentHelper<TContext, TComponent>.IsUnique)
            {
                return UniqueEntity;
            }
            if (count == 0)
            {
                return CreateEntityWith<TComponent>();
            }
            if (!TryGetEntityWith<TComponent>(out var entity))
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
                entity.AddComponent(index, component);
            }
            else
            {
                if (entity.HasComponent(index))
                {
                    entity.RemoveComponent(index);
                }
            }
        }

        public void RegisterAddedComponentListener<TComponent>(IEntity entity, Action<(IEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new()
        {
            var component = GetOrCreateComponent<AddedListenersComponent<TEntity, TComponent>>((TEntity)entity);
            component.Register(args => action((args.Entity, args.Component)));
        }

        public void RegisterRemovedComponentListener<TComponent>(IEntity entity, Action<IEntity> action) where TComponent : IEventComponent, new()
        {
            var component = GetOrCreateComponent<RemovedListenersComponent<TEntity, TComponent>>((TEntity)entity);
            component.Register(action);
        }

        public bool IsFlagged<TComponent>(IEntity entity) where TComponent : IFlagComponent, new()
        {
            return entity.HasComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);
        }

        bool IEntityContext.Has<TComponent>(IEntity entity)
        {
            return entity.HasComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);
        }

        void IEntityContext.Set<TComponent>(IEntity entity, Action<TComponent> componentUpdater)
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            var newComponent = entity.CreateComponent<TComponent>(index);
            componentUpdater(newComponent);            
            entity.ReplaceComponent(index, newComponent);

            //Debug.Log($"{typeof(TComponent).Name} updated to {newComponent}");
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
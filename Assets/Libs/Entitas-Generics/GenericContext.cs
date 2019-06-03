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
    /// Provides context specific functions, such as creating entities and searching for entities.
    /// </summary>
    public interface IGenericContext<TEntity> : IContext<TEntity> where TEntity : class, IEntity, IGenericEntity
    {
        /// <summary>
        /// A hardcoded Entity for storing <see cref="IUniqueComponent"/>s
        /// </summary>
        TEntity Unique { get; }

        IMatcher<TEntity> GetMatcher<T>() where T : IComponent, new();

        ICollector<TEntity> GetCollector<T>() where T : IComponent, new();

        IGroup<TEntity> GetGroup<T>() where T : IComponent, new();

        TriggerOnEvent<TEntity> GetTrigger<T>(GroupEvent eventType = default) where T : IComponent, new();

        ICollector<TEntity> GetTriggerCollector<T>(GroupEvent eventType = default) where T : IComponent, new();

        bool TryFindEntity<TComponent, TValue>(TValue value, out TEntity entity) where TComponent : IEqualityComparer<TComponent>, IValueComponent<TValue>, new();

    }

    public class GenericContext<TContext, TEntity> : Context<TEntity>, IGenericContext<TEntity> 
        where TContext : IGenericContext<TEntity> 
        where TEntity : class, IEntity, IGenericEntity
    {
        private TEntity _unique;
        private IEntityIndex[] _primaryIndexes;

        public IContextDefinition<TEntity> Definition { get; }

        public TEntity Unique => _unique ?? (_unique = CreateUniqueEntity());

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

    }
}
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.VisualDebugging.Unity;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

        TriggerOnEvent<TEntity> GetTrigger<T>(GroupEvent eventType = default) where T : IComponent, new();

        ICollector<TEntity> GetTriggerCollector<T>(GroupEvent eventType = default) where T : IComponent, new();
    }

    /// <summary>
    /// This wrapper for context is aware of its own type which lets it be used to
    /// effectively hard-code efficient data access specific to this context
    /// </summary>
    public class GenericContext<TContext, TEntity> : Context<TEntity>, IGenericContext<TEntity> where TContext : IGenericContext<TEntity> where TEntity : class, IEntity, new()
    {
        public IContextDefinition Definition { get; }

        public GenericContext(IContextDefinition contextDefinition) : base(contextDefinition.ComponentCount, 0, contextDefinition.GetContextInfo(), AercFactory, EntityFactory)
        {
            ContextHelper<TContext>.Initialize(contextInfo);
            SetupVisualDebugging();
            Definition = contextDefinition;
        }

        private static TEntity EntityFactory()
        {
            return new TEntity();
        }

        private static IAERC AercFactory(IEntity arg1)
        {
            return new UnsafeAERC();
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("ENTITAS_DISABLE_VISUAL_DEBUGGING")]
        private void SetupVisualDebugging()
        {
            if (!UnityEngine.Application.isPlaying)
                return;

            var observer = new ContextObserver(this);
            UnityEngine.Object.DontDestroyOnLoad(observer.gameObject);
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

        public bool HasComponent<T>(TEntity entity) where T : IComponent, new()
        {
            return entity.HasComponent(ComponentHelper<TContext, T>.ComponentIndex);
        }

        public bool HasComponent<T>() where T : IComponent, new()
        {
            return GetEntityWith<T>().HasComponent(ComponentHelper<TContext, T>.ComponentIndex);
        }

        public TComponent Get<TComponent>(TEntity entity) where TComponent : IComponent, new()
        {
            return (TComponent)entity.GetComponent(ComponentHelper<TContext, TComponent>.ComponentIndex);
        }

        public TComponent Get<TComponent>() where TComponent : IComponent, new()
        {
            if (TryGetEntityWith<TComponent>(out var entity))
            {
                var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
                return (TComponent)entity.GetComponent(index);
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
                var newComponent = entity.CreateComponent<TComponent>(index);
                entity.AddComponent(index, newComponent);
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

        public TEntity TagEntity => _tagEntity ?? (_tagEntity = CreateEntityWith(new UniqueTagHolderComponent()));
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

        public void Set<TComponent>(TComponent component) where TComponent : IComponent, new()
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

            //var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            //var entity = GetFirstEntity();
            //entity.ReplaceComponent(index, component);
        }

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
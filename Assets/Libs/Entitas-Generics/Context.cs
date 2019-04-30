using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using Entitas;
using Entitas.VisualDebugging.Unity;

namespace EntitasGenerics
{
    /// <summary>
    /// This wrapper for context is aware of its own type which lets it be used to
    /// effectively hard-code efficient data access specific to this context
    /// </summary>
    public class Context<TContext, TEntity> : Context<TEntity> where TEntity : class, IEntity, new() where TContext : IContext
    {   
        public Context(IContextDefinition contextDefinition) : base(contextDefinition.ComponentCount, 0, contextDefinition.GetContextInfo(), AercFactory, EntityFactory)
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

        public IContextDefinition Definition { get; }

        public IMatcher<TEntity> CreateMatcher<T>() where T : IComponent
        {
            return Matcher<TContext, TEntity, T>.AnyOf;
        }

        public bool HasComponent<T>(TEntity entity) where T : IComponent
        {
            return entity.HasComponent(ComponentHelper<TContext, T>.ComponentIndex);
        }

        public bool HasComponent<T>() where T : IComponent, new()
        {
            return GetEntityWith<T>().HasComponent(ComponentHelper<TContext, T>.ComponentIndex);
        }

        public TComponent Get<TComponent>(TEntity entity) where TComponent : IComponent
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
                var component = new TComponent();
                CreateEntityWith(component);
                return component;
            }
            return default;
        }

        public bool TryGetComponent<T>(TEntity entity, out T component) where T : IComponent
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
                entity.CreateComponent<TComponent>(index);
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
                entity.CreateComponent<TComponent>(index);
            }
            else
            {
                entity.RemoveComponent(index);
            }
        }

        public void Set<TComponent>(bool tagState) where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            var entity = GetFirstEntity();

            if (tagState)
            {
                entity.CreateComponent<TComponent>(index);
            }
            else if (entity.HasComponent(index))
            {
                entity.RemoveComponent(index);
            }
        }

        public void Set<TComponent>(TComponent component) where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            var entity = GetFirstEntity();
            entity.ReplaceComponent(index, component);
        }

        public void Set<TComponent>() where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            var entity = GetFirstEntity();
            entity.CreateComponent<TComponent>(index);
        }

        //public void Set<TComponent>(Action<TComponent> setter) where TComponent : IComponent, new()
        //{
        //    // todo replace with direct ref update to entity table / cache GetSingle/Group.

        //    var component = new TComponent();
        //    setter.Invoke(component);

        //    var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
        //    GetOrCreateSingle<TComponent>().ReplaceComponent(index, component);
        //}

        public void ReplaceComponent<T>(TEntity entity, T component) where T : IComponent
        {
            var index = ComponentHelper<TContext, T>.ComponentIndex;
            entity.ReplaceComponent(index, component);
        }

        public void ReplaceComponent<TComponent>(TComponent component) where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TContext, TComponent>.ComponentIndex;
            GetEntityWith<TComponent>().ReplaceComponent(index, component);
        }

        public void Remove<T>(TEntity entity) where T : IComponent
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

        public IGroup<TEntity> GetGroup<T>() where T : IComponent
        {
            return GetGroup(Matcher<TContext, TEntity, T>.AllOf);
        }

        public TEntity GetFirstEntity()
        {
            return count == 0 ? CreateEntity() : GetEntities().First();
        }

        private TComponent GetFirstEntityComponent<TComponent>() where TComponent : IComponent
        {
            return Get<TComponent>(GetGroup<TComponent>().GetSingleEntity());
        }

        public TEntity GetOrCreateEntityWith<TComponent>() where TComponent : IComponent, new()
        {
            return count == 0 || !HasComponent<TComponent>()
                ? CreateEntityWith<TComponent>()
                : GetEntityWith<TComponent>();

            //return GetGroup<TComponent>().GetSingleEntity();
        }

        //public bool EntityExistsWithComponent<T>() where T : IComponent
        //{
        //    return GetEntityWith<T>() != null;
        //}

        public TEntity GetEntityWith<TComponent>() where TComponent : IComponent
        {
            return GetGroup<TComponent>().GetSingleEntity();
        }

        public bool TryGetEntityWith<TComponent>(out TEntity entity) where TComponent : IComponent
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

        //public bool TryGetGroupWith<TComponent>(out IGroup<TEntity> group) where TComponent : IComponent
        //{
        //    group = GetGroup<TComponent>();
        //    if (group.count > 0)
        //    {           
        //        return true;
        //    }
        //    return false;
        //}

        public TEntity CreateEntityWith<T>(T component = default) where T : IComponent, new()
        {
            //if (EntityExistsWithComponent<T>())
            //    throw new Exception($"Entity already has component of type: '{typeof(T)}'");

            TEntity entity = CreateEntity();
            Set(entity, component);
            return entity;
        }

    }
}
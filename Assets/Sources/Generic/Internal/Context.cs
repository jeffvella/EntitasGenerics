using System;
using System.Diagnostics;
using Entitas;
using Entitas.VisualDebugging.Unity;

namespace EntitasGeneric
{
    /// <summary>
    /// This wrapper for context is aware of its own type which lets it be used to
    /// effectively hard-code efficient data access specific to this context
    /// </summary>
    public class Context<TContext, TEntity> : Context<TEntity> where TEntity : class, IEntity, new() where TContext : IContext
    {

        private int _typeIndex;

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

        public bool HasComponent<TComponent>(TEntity entity) where TComponent : IComponent
        {
            return entity.HasComponent(TypeHelper<TContext, TComponent>.ComponentIndex);
        }

        public TComponent GetComponent<TComponent>(TEntity entity) where TComponent : IComponent
        {
            return (TComponent)entity.GetComponent(TypeHelper<TContext, TComponent>.ComponentIndex);
        }

        public bool TryGetComponent<TComponent>(TEntity entity, out TComponent component) where TComponent : IComponent
        {
            if (!HasComponent<TComponent>(entity))
            {
                component = default;
                return false;
            }
            component = (TComponent)entity.GetComponent(TypeHelper<TContext, TComponent>.ComponentIndex);
            return true;
        }

        public void ReplaceComponent<TComponent>(TEntity entity, TComponent component) where TComponent : IComponent
        {
            entity.ReplaceComponent(TypeHelper<TContext, TComponent>.ComponentIndex, component);
        }

        public void RemoveComponent<TComponent>(TEntity entity) where TComponent : IComponent
        {
            entity.RemoveComponent(TypeHelper<TContext, TComponent>.ComponentIndex);
        }

        public IGroup<TEntity> GetGroup<T>() where T : IComponent
        {
            return GetGroup(Matcher<TContext, TEntity, T>.AllOf);
        }

        public TEntity GetSingle<T>() where T : IComponent
        {
            return GetGroup<T>().GetSingleEntity();
        }

        public bool EntityExistsWithComponent<T>() where T : IComponent
        {
            return GetSingle<T>() != null;
        }

        public TEntity CreateEntityWithComponent<T>(T component = default) where T : IComponent, new()
        {
            if (EntityExistsWithComponent<T>())
                throw new Exception($"Entity already has component of type: '{typeof(T)}'");

            TEntity entity = CreateEntity();
            if (component == null)
            {
                entity.CreateComponent<T>(TypeHelper<TContext, T>.ComponentIndex);
            }
            else
            {
                entity.AddComponent(TypeHelper<TContext, T>.ComponentIndex, component);
            }
            return entity;
        }

    }
}
using System;
using Entitas.VisualDebugging.Unity;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.UIElements;


namespace Entitas.Generics
{
    public interface IContextDefinition : ICustomDisplayName
    {
        ContextInfo ContextInfo { get; }

        int ComponentCount { get; }

        List<int> EventListenerIndices { get; }
    }

    public interface IContextDefinition<TEntity> : IContextDefinition where TEntity : class, IEntity, IGenericEntity
    {
        Func<TEntity> EntityFactory { get; }
    }

    /// <summary>
    /// The <see cref="ContextDefinition{TContext,TEntity}"/> defines the component types for a context,
    /// and produces the contextInfo required by Entitas' Context base constructor.
    /// </summary>
    public abstract class ContextDefinition<TContext, TEntity> : IContextDefinition<TEntity> 
        where TContext : IContext 
        where TEntity : class, IEntity, IGenericEntity
    {
        private ContextInfo _contextInfo;

        public ContextDefinition()
        {
            AddDefaultComponents();
        }

        public void AddDefaultComponents()
        {            
            AddComponent<UniqueComponentsHolder>();     
        }

        public abstract Func<TEntity> EntityFactory { get; }

        private List<string> ComponentNames { get; } = new List<string>();

        private List<Type> ComponentTypes { get; } = new List<Type>();

        public List<int> EventListenerIndices { get; } = new List<int>();

        public int ComponentCount { get; private set; }

        public ContextInfo ContextInfo
        {
            get
            {
                if (!IsInitialized)
                {
                    Initialize();
                }
                return _contextInfo;
            }
        }

        public void AddComponent<TComponent>() where TComponent : class, IComponent, new()
        {
            var componentIndex = ComponentTypes.Count;

            AddComponentType<TComponent>(componentIndex);

            if (ComponentHelper.IsEventComponent<TComponent>())
            {
                AddEventComponentType<AddedListenersComponent<TEntity, TComponent>>();
                AddEventComponentType<RemovedListenersComponent<TEntity, TComponent>>();
            }
        }

        private void AddEventComponentType<T>() where T : IComponent, new()
        {
            int index = ComponentTypes.Count;
            EventListenerIndices.Add(index);
            AddComponentType<T>(index);
        }

        private void AddComponentType<T>(int index) where T : IComponent, new()
        {
            ComponentHelper<TEntity, T>.Initialize(index);
            ComponentNames.Add(typeof(T).PrettyPrintGenericTypeName());
            ComponentTypes.Add(typeof(T));
            ComponentCount++;
        }

        public bool IsInitialized { get; private set; }

        private void Initialize()
        {
            _contextInfo = new ContextInfo(DisplayName,
                ComponentNames.ToArray(),
                ComponentTypes.ToArray());
         
            ContextHelper<TContext>.Initialize(_contextInfo);
            IsInitialized = true;
        }

        public string DisplayName => typeof(TContext).Name.Replace("Context", "");
    }
}

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

    public interface IContextDefinition<TEntity> : IContextDefinition where TEntity : class, IEntity
    {
        Func<TEntity> EntityFactory { get; }

        List<IComponentSearchIndex<TEntity>> SearchIndexes { get; }

        List<int> SearchableComponentIndices { get; }
    }

    /// <summary>
    /// The <see cref="ContextDefinition{TContext,TEntity}"/> defines the component types for a context,
    /// and produces the contextInfo required by Entitas' Context base constructor.
    /// </summary>
    public abstract class ContextDefinition<TContext, TEntity> : IContextDefinition<TEntity> where TContext : IContext where TEntity : class, IEntity
    {
        private ContextInfo _contextInfo;

        public ContextDefinition()
        {
            AddDefaultComponents();
        }

        public void AddDefaultComponents()
        {            
            Add<UniqueComponents>();     
        }

        public abstract Func<TEntity> EntityFactory { get; }

        private List<string> ComponentNames { get; } = new List<string>();

        private List<Type> ComponentTypes { get; } = new List<Type>();

        public List<int> EventListenerIndices { get; } = new List<int>();

        public List<int> IndexedComponentIndices { get; } = new List<int>();

        public List<int> SearchableComponentIndices { get; } = new List<int>();

        public List<IComponentSearchIndex<TEntity>> SearchIndexes { get; } = new List<IComponentSearchIndex<TEntity>>();

 
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

        public void AddIndexed<TComponent>() where TComponent : class, ISearchableComponent<TComponent>, new()
        {
            var componentIndex = ComponentTypes.Count;

            AddComponentType<TComponent>(componentIndex);

            //if (ComponentHelper.IsIndexedComponent<TComponent>())
            //{
                //IIndexedComponent instance = ComponentHelper.Cast<IIndexedComponent>(ComponentHelper<TComponent>.Default);

                //if (!(ComponentHelper<TComponent>.Default is IIndexedComponent))
                //{
                //    throw new InvalidCastException();
                //}
                CreateComponentSearchIndex<TComponent>(componentIndex);
            //}

            if (ComponentHelper.IsEventComponent<TComponent>())
            {
                AddEventComponentType<AddedListenersComponent<TEntity, TComponent>>();
                AddEventComponentType<RemovedListenersComponent<TEntity, TComponent>>();
            }
        }

        public void Add<TComponent>() where TComponent : class, IComponent, new()
        {
            var componentIndex = ComponentTypes.Count;

            AddComponentType<TComponent>(componentIndex);

            //if (ComponentHelper.IsIndexedComponent<TComponent>())
            //{
            //    //IIndexedComponent instance = ComponentHelper.Cast<IIndexedComponent>(ComponentHelper<TComponent>.Default);

            //    if (!(ComponentHelper<TComponent>.Default is IIndexedComponent indexed))
            //    {
            //        throw new InvalidCastException();
            //    }
            //    CreateComponentSearchIndex(indexed, componentIndex);
            //}

            SearchIndexes.Add(null);

            if (ComponentHelper.IsEventComponent<TComponent>())
            {
                AddEventComponentType<AddedListenersComponent<TEntity, TComponent>>();
                AddEventComponentType<RemovedListenersComponent<TEntity, TComponent>>();
            }
        }

        private void CreateComponentSearchIndex<TComponent>(int componentIndex) where TComponent : class, ISearchableComponent<TComponent>, new()
        {
            var debugTest = typeof(TComponent);

            if (ComponentHelper.IsIndexedComponent<TComponent>() && ComponentHelper<TComponent>.Default is IEqualityComparer<TComponent> comparer)
            {
                SearchIndexes.Add(new EntityByComponentSearchIndex<TEntity, TComponent>(comparer));

                SearchableComponentIndices.Add(componentIndex);
            }
            else
            {
                SearchIndexes.Add(null);
            }
        }

        private void AddEventComponentType<T>() where T : IComponent, new()
        {
            int index = ComponentTypes.Count;
            EventListenerIndices.Add(index);
            SearchIndexes.Add(null);
            AddComponentType<T>(index);
        }

        private void AddComponentType<T>(int index) where T : IComponent, new()
        {
            ComponentHelper<TContext, T>.Initialize(index);
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

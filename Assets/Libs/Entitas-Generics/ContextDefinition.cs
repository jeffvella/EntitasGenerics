using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Entitas.Generics
{
    public interface IContextDefinition : ICustomDebugInfo
    {
        ContextInfo ContextInfo { get; }

        int ComponentCount { get; }

        List<int> EventListenerIndices { get; }
    }

    /// <summary>
    /// The <see cref="ContextDefinition{TContext,TEntity}"/> defines the component types for a context,
    /// and produces the contextInfo required by Entitas' Context base constructor.
    /// </summary>
    public class ContextDefinition<TContext, TEntity> : IContextDefinition where TContext : IContext where TEntity : class, IEntity, new()
    {
        private ContextInfo _contextInfo;

        public ContextDefinition()
        {
            AddDefaultComponents();
        }

        public void AddDefaultComponents()
        {            
            Add<UniqueComponents>();
            Add<UniqueFlags>();
        }

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

        public void Add<T>() where T : class, IComponent, new()
        {            
            AddComponentType<T>(ComponentTypes.Count);

            if (ComponentHelper.IsEventComponent<T>())
            {
                AddEventComponentType<AddedListenersComponent<TEntity, T>>();
                AddEventComponentType<RemovedListenersComponent<TEntity, T>>();
            }
        }

        private void AddEventComponentType<T>() where T : new()
        {
            int index = ComponentTypes.Count;
            EventListenerIndices.Add(index);
            AddComponentType<T>(index);
        }

        private void AddComponentType<T>(int index) where T : new()
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

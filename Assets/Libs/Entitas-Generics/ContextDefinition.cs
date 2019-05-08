using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Entitas.Generics
{
    public interface IContextDefinition
    {
        ContextInfo ContextInfo { get; }

        int ComponentCount { get; }

        List<int> EventListenerIndices { get; }

        //List<IComponentDefinition> Components { get; }

        //List<IListenerComponent> Listeners { get; }
    }

    /// <summary>
    /// The <see cref="ContextDefinition{TContext,TEntity}"/> defines the component types that will be in the context,
    /// and produces a contextInfo object for the Context base constructor.
    /// todo: evaluate if this should be replaced by a simple collection initializer of types  
    /// </summary>
    public class ContextDefinition<TContext, TEntity> : IContextDefinition where TContext : IContext where TEntity : class, IEntity, new()
    {
        private ContextInfo _contextInfo;

        //private List<IListenerComponent> _listeners { get; } = new List<IListenerComponent>();

        //public List<IComponentDefinition> Components { get; } = new List<IComponentDefinition>();

        public ContextDefinition()
        {
            AddDefaultComponents();
        }

        public void AddDefaultComponents()
        {            
            Add<TagHolderComponent>();
        }

        private List<string> _componentNames { get; } = new List<string>();

        private List<Type> _componentTypes { get; } = new List<Type>();

        public List<int> EventListenerIndices { get; } = new List<int>();

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

        public int ComponentCount
        {
            get
            {
                if (!IsInitialized)
                {
                    Initialize();
                }
                return _contextInfo.componentTypes.Length;
            }
        }

        public void Add<T>() where T : class, IComponent, new()
        {
            // Note: Methods on component definition that rely on ContextHelper
            // can't be used until ContextDefinition.Initialize();

            //var def = new ComponentDefinition<TContext, TEntity, T>();

            // todo: switch from attributes to implementing an interface eg. IEventComponent
            // fits better with the established approach for systems, IReactiveSystem, IExecuteSystem etc?

            if (AttributeHelper.HasAttribute<EventAttribute>(typeof(T)))
            {
                // cache it for doing something with later?
                //var listenerComponent = new ListenerHolderComponent<TEntity, T>(); 

                EventListenerIndices.Add(_componentTypes.Count);                
                AddComponentType(typeof(AddedListenerStorageComponent<TEntity, T>));
                EventListenerIndices.Add(_componentTypes.Count);
                AddComponentType(typeof(RemovedListenerStorageComponent<TEntity, T>));

                
            }

            // Is it useful to be able to access these IComponentDefinitions later via the context?
            //Components.Add(def);
            AddComponentType(typeof(T));
            //return def;
        }

        //private void AddComponentTypeInfo(IListenerComponent component)
        //{
        //    var t = component.GetType();
        //    _componentNames.Add(t.Name);
        //    _componentTypes.Add(t);        
        //}

        private void AddComponentType(Type type)
        {                   
            _componentNames.Add(type.PrettyPrintGenericTypeName());
            _componentTypes.Add(type);            
        }

        public bool IsInitialized { get; private set; }

        private void Initialize()
        {
            // ContextHelper needs to be created after the final list of components is established.
            // which happens after the derived constructor, so Initialize is called when ContextInfo
            // is first requested, which will happen on the Entitas base Context constructor.

            //foreach (var component in _listeners)
            //{
            //    AddListener(component);
            //}

            _contextInfo = new ContextInfo(typeof(TContext).Name,
                _componentNames.ToArray(),
                _componentTypes.ToArray());

            // It is important that ContextHelper holds the ContextInfo as soon as possible.            
            ContextHelper<TContext>.Initialize(_contextInfo);
            IsInitialized = true;
        }
    }
}

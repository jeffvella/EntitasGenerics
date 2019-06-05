using Entitas;
using Entitas.Generics;
using Entitas.VisualDebugging.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitas.Generics
{
    public interface IGenericEntity
    {
        int GetIndex<TComponent>() where TComponent : IComponent, new();


        // Classes

        TComponent GetComponent<TComponent>() where TComponent : class, IComponent, new();

        bool TryGetComponent<TComponent>(out TComponent component) where TComponent : class, IComponent, new();

        TComponent CreateComponent<TComponent>() where TComponent : class, IComponent, new();

        IGenericEntity ReplaceComponent<TComponent>(TComponent component) where TComponent : class, IComponent, new();

        IGenericEntity AddComponent<TComponent>(TComponent component) where TComponent : class, IComponent, new();

        IGenericEntity RemoveComponent<TComponent>() where TComponent : class, IComponent, new();

        bool HasComponent<TComponent>() where TComponent : class, IComponent, new();

        IGenericEntity SetFlag<TComponent>(bool value = true) where TComponent : class, IFlagComponent, new();

        bool IsFlagged<TComponent>() where TComponent : class, IFlagComponent, new();

        
        // Structs

        IGenericEntity Add<TComponentData>(TComponentData data) where TComponentData : struct, IComponent;

        IGenericEntity Replace<TComponentData>(TComponentData data) where TComponentData : struct, IComponent;

        StructComponentWrapper<TComponentData> Create<TComponentData>() where TComponentData : struct, IComponent;

        IGenericEntity Remove<TComponentData>() where TComponentData : struct, IComponent;

        TComponentData Get2<TComponentData>() where TComponentData : struct, IComponent;

        IGenericEntity Set<TComponentData>(TComponentData data) where TComponentData : struct, IComponent;

        bool TryGet<TComponent>(out TComponent component) where TComponent : struct, IComponent;

        bool Has<TComponentData>() where TComponentData : struct, IComponent;

        IGenericEntity SetFlag2<TComponent>(bool value = true) where TComponent : struct, IFlagComponent;

        bool Is<TComponent>() where TComponent : struct, IFlagComponent;

    }

    public class StructComponentWrapper<TData> : IComponent, ICustomDisplayName //IEqualityComparer<StructComponentWrapper<TData>>, IEquatable<StructComponentWrapper<TData>>, 
    {
        public TData Data;

        public string DisplayName => typeof(TData).Name;

        //public bool Equals(StructComponentWrapper<TData> other)
        //{
        //    return other.Data.Equals(Data);
        //}

        //public bool Equals(StructComponentWrapper<TData> x, StructComponentWrapper<TData> y)
        //{
        //    return x.Data.Equals(y.Data);
        //}

        //public int GetHashCode(StructComponentWrapper<TData> obj)
        //{
        //    return obj.Data.GetHashCode();
        //}
    }


    public class GenericEntity<TEntity> : NativeEntity, IGenericEntity where TEntity : IEntity, IGenericEntity
    {
        public IGenericEntity Add<TComponentData>(TComponentData data) where TComponentData : struct, IComponent
        {
            var index = ComponentHelper<TEntity,StructComponentWrapper<TComponentData>>.Index;
            var component = CreateComponent<StructComponentWrapper<TComponentData>>(index);
            component.Data = data;
            AddComponent(index, component);
            return this;
        }

        public IGenericEntity Replace<TComponentData>(TComponentData data) where TComponentData : struct, IComponent
        {
            var index = ComponentHelper<TEntity, StructComponentWrapper<TComponentData>>.Index;
            var component = CreateComponent<StructComponentWrapper<TComponentData>>(index);
            component.Data = data;
            ReplaceComponent(index, component);
            return this;
        }

        public StructComponentWrapper<TComponentData> Create<TComponentData>() where TComponentData : struct, IComponent
        {
            var index = ComponentHelper<TEntity, StructComponentWrapper<TComponentData>>.Index;
            var component = CreateComponent<StructComponentWrapper<TComponentData>>(index);
            return component;
        }

        public IGenericEntity Remove<TComponentData>() where TComponentData : struct, IComponent
        {
            RemoveComponent(ComponentHelper<TEntity, StructComponentWrapper<TComponentData>>.Index);
            return this;
        }

        public TComponentData Get2<TComponentData>() where TComponentData : struct, IComponent
        {
            var index = ComponentHelper<TEntity, StructComponentWrapper<TComponentData>>.Index;
            return ((StructComponentWrapper<TComponentData>)GetComponent(index)).Data;
        }

        public IGenericEntity Set<TComponentData>(TComponentData data) where TComponentData : struct, IComponent
        {
            var index = ComponentHelper<TEntity, StructComponentWrapper<TComponentData>>.Index;
            var newComponent = CreateComponent<StructComponentWrapper<TComponentData>>();
            newComponent.Data = data;
            ReplaceComponent(index, newComponent);
            return this;
        }


        public bool TryGet<TComponentData>(out TComponentData component) where TComponentData : struct, IComponent
        {
            var index = ComponentHelper<TEntity, StructComponentWrapper<TComponentData>>.Index;
            if(HasComponent(index))
            {
                component = ((StructComponentWrapper<TComponentData>)GetComponent(index)).Data;
                return true;
            }
            component = default;
            return false;
        }

        public bool Has<TComponentData>() where TComponentData : struct, IComponent
        {
            var index = ComponentHelper<TEntity, StructComponentWrapper<TComponentData>>.Index;
            return HasComponent(index);
        }

        public IGenericEntity SetFlag2<TComponentData>(bool value = true) where TComponentData : struct, IFlagComponent
        {
            var index = ComponentHelper<TEntity, TComponentData>.Index;
            if (value)
            {
                if (!HasComponent(index))
                {
                    AddComponent(index, CreateComponent<StructComponentWrapper<TComponentData>>(index));

                }
            }
            else
            {
                if (HasComponent(index))
                {
                    RemoveComponent(index);
                }
            }
            return this;
        }

        public bool Is<TComponentData>() where TComponentData : struct, IFlagComponent
        {
            var index = ComponentHelper<TEntity, StructComponentWrapper<TComponentData>>.Index;
            return HasComponent(index);
        }






        public TComponent GetComponent<TComponent>() where TComponent : class, IComponent, new()
        {
            return (TComponent)GetComponent(ComponentHelper<TEntity, TComponent>.Index);
        }

        public bool TryGetComponent<TComponent>(out TComponent component) where TComponent : class, IComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            if (!HasComponent(index))
            {
                component = default;
                return false;
            }
            component = (TComponent)GetComponent(index);
            return true;
        }

        public int GetIndex<TComponent>() where TComponent : IComponent, new()
        {
            return ComponentHelper<TEntity, TComponent>.Index;
        }

        public ComponentAccessor<TComponent> Get<TComponent>() where TComponent : class, IComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            var component = !HasComponent(index)
                ? CreateComponent<TComponent>(index)
                : (TComponent)GetComponent(index);

            return new ComponentAccessor<TComponent>(this, index, component);
        }

        public TComponent CreateComponent<TComponent>() where TComponent : class, IComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            return CreateComponent<TComponent>(index);
        }

        public IGenericEntity ReplaceComponent<TComponent>(TComponent component) where TComponent : class, IComponent, new()
        {
            ReplaceComponent(ComponentHelper<TEntity, TComponent>.Index, component);
            return this;
        }

        public IGenericEntity AddComponent<TComponent>(TComponent component) where TComponent : class, IComponent, new()
        {
            AddComponent(ComponentHelper<TEntity, TComponent>.Index, component);
            return this;
        }

        public IGenericEntity RemoveComponent<TComponent>() where TComponent : class, IComponent, new()
        {
            RemoveComponent(ComponentHelper<TEntity, TComponent>.Index);
            return this;
        }

        public bool HasComponent<TComponent>() where TComponent : class, IComponent, new()
        {
            return HasComponent(ComponentHelper<TEntity, TComponent>.Index);
        }

        public IGenericEntity SetFlag<TComponent>(bool value = true) where TComponent : class, IFlagComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            if (value)
            {
                if(!HasComponent(index))
                {
                    AddComponent(index, CreateComponent<TComponent>());
                    
                }
            }
            else             
            {
                if (HasComponent(index))
                {
                    RemoveComponent(index);
                }
            }
            return this;
        }

        public void AddFlag<TComponent>() where TComponent : class, IFlagComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            AddComponent(index, CreateComponent<TComponent>());
        }

        public void Apply<TComponent, TValue>(TValue value) where TComponent : class, IValueComponent<TValue>, new()
        {
            Get<TComponent>().Apply(value);
        }

        public void RemoveFlag<TComponent>() where TComponent : class, IFlagComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            RemoveComponent(index);
        }

        public bool IsFlagged<TComponent>() where TComponent : class, IFlagComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            return HasComponent(index);
        }

        public void RegisterComponentListener<TComponent>(IAddedComponentListener<TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            ModifyComponent<AddedListenersComponent<TEntity, TComponent>>(c => c.Register(listener));
        }

        public void DeregisterComponentListener<TComponent>(IAddedComponentListener<TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            ModifyComponent<AddedListenersComponent<TEntity, TComponent>>(c => c.Deregister(listener));
        }

        public void RegisterComponentListener<TComponent>(IRemovedComponentListener<TEntity,TComponent> listener) where TComponent : IComponent, new()
        {
            ModifyComponent<RemovedListenersComponent<TEntity, TComponent>>(c => c.Register(listener));
        }

        public void DeregisterComponentListener<TComponent>(IRemovedComponentListener<TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            ModifyComponent<RemovedListenersComponent<TEntity, TComponent>>(c => c.Deregister(listener));
        }

        private void ModifyComponent<T>(Action<T> action) where T : class, IComponent, new()
        {
            var acc = Get<T>();
            action(acc.Component);
            acc.Apply();
        }

        public void RegisterComponentListener<TComponent>(Action<TEntity> listener, GroupEvent type) where TComponent : class, IComponent, new()
        {
            switch (type)
            {
                case GroupEvent.Added:
                    ModifyComponent<AddedListenersComponent<TEntity, TComponent>>(c => c.Register(listener));
                    break;
                case GroupEvent.Removed:
                    ModifyComponent<RemovedListenersComponent<TEntity, TComponent>>(c => c.Register(listener));

                    break;
                case GroupEvent.AddedOrRemoved:
                    ModifyComponent<AddedListenersComponent<TEntity, TComponent>>(c => c.Register(listener));
                    ModifyComponent<RemovedListenersComponent<TEntity, TComponent>>(c => c.Register(listener));
                    break;
            }
        }

        public void DeregisterComponentListener<TComponent>(Action<TEntity> listener, GroupEvent type) where TComponent : class, IComponent, new()
        {
            switch (type)
            {
                case GroupEvent.Added:
                    ModifyComponent<AddedListenersComponent<TEntity, TComponent>>(c => c.Deregister(listener));
                    break;
                case GroupEvent.Removed:
                    ModifyComponent<RemovedListenersComponent<TEntity, TComponent>>(c => c.Deregister(listener));
                    break;
                case GroupEvent.AddedOrRemoved:
                    ModifyComponent<AddedListenersComponent<TEntity, TComponent>>(c => c.Deregister(listener));
                    ModifyComponent<RemovedListenersComponent<TEntity, TComponent>>(c => c.Deregister(listener));
                    break;
            }
        }


        //------------

        public void RegisterComponentListener2<TComponentData>(Action<TEntity> listener, GroupEvent type) where TComponentData : struct, IComponent
        {
            switch (type)
            {
                case GroupEvent.Added:
                    ModifyComponent<AddedListenersComponent<TEntity, StructComponentWrapper<TComponentData>>>(c => c.Register(listener));
                    break;
                case GroupEvent.Removed:
                    ModifyComponent<RemovedListenersComponent<TEntity, StructComponentWrapper<TComponentData>>>(c => c.Register(listener));

                    break;
                case GroupEvent.AddedOrRemoved:
                    ModifyComponent<AddedListenersComponent<TEntity, StructComponentWrapper<TComponentData>>>(c => c.Register(listener));
                    ModifyComponent<RemovedListenersComponent<TEntity, StructComponentWrapper<TComponentData>>>(c => c.Register(listener));
                    break;
            }
        }

        public void DeregisterComponentListener2<TComponentData>(Action<TEntity> listener, GroupEvent type) where TComponentData : struct, IComponent
        {
            switch (type)
            {
                case GroupEvent.Added:
                    ModifyComponent<AddedListenersComponent<TEntity, StructComponentWrapper<TComponentData>>>(c => c.Deregister(listener));
                    break;
                case GroupEvent.Removed:
                    ModifyComponent<RemovedListenersComponent<TEntity, StructComponentWrapper<TComponentData>>>(c => c.Deregister(listener));
                    break;
                case GroupEvent.AddedOrRemoved:
                    ModifyComponent<AddedListenersComponent<TEntity, StructComponentWrapper<TComponentData>>>(c => c.Deregister(listener));
                    ModifyComponent<RemovedListenersComponent<TEntity, StructComponentWrapper<TComponentData>>>(c => c.Deregister(listener));
                    break;
            }
        }
    }


    public unsafe class NativeEntity : IEntity
    {
        /// Occurs when a component gets added.
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        public event EntityComponentChanged OnComponentAdded;

        /// Occurs when a component gets removed.
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        public event EntityComponentChanged OnComponentRemoved;

        /// Occurs when a component gets replaced.
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        public event EntityComponentReplaced OnComponentReplaced;

        /// Occurs when an entity gets released and is not retained anymore.
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        public event EntityEvent OnEntityReleased;

        /// Occurs when calling entity.Destroy().
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        public event EntityEvent OnDestroyEntity;

        /// The total amount of components an entity can possibly have.
        public int totalComponents { get { return _totalComponents; } }

        /// Each entity has its own unique creationIndex which will be set by
        /// the context when you create the entity.
        public int creationIndex { get { return _creationIndex; } }

        /// The context manages the state of an entity.
        /// Active entities are enabled, destroyed entities are not.
        public bool isEnabled { get { return _isEnabled; } }

        /// componentPools is set by the context which created the entity and
        /// is used to reuse removed components.
        /// Removed components will be pushed to the componentPool.
        /// Use entity.CreateComponent(index, type) to get a new or
        /// reusable component from the componentPool.
        /// Use entity.GetComponentPool(index) to get a componentPool for
        /// a specific component index.
        public Stack<IComponent>[] componentPools { get { return _componentPools; } }

        /// The contextInfo is set by the context which created the entity and
        /// contains information about the context.
        /// It's used to provide better error messages.
        public ContextInfo contextInfo { get { return _contextInfo; } }

        /// Automatic Entity Reference Counting (AERC)
        /// is used internally to prevent pooling retained entities.
        /// If you use retain manually you also have to
        /// release it manually at some point.
        public IAERC aerc { get { return _aerc; } }

        readonly List<IComponent> _componentBuffer;
        readonly List<int> _indexBuffer;

        int _creationIndex;
        bool _isEnabled;

        int _totalComponents;
        IComponent[] _components;
        Stack<IComponent>[] _componentPools;
        ContextInfo _contextInfo;
        IAERC _aerc;

        IComponent[] _componentsCache;
        int[] _componentIndicesCache;
        string _toStringCache;
        StringBuilder _toStringBuilder;

        public NativeEntity()
        {
            _componentBuffer = new List<IComponent>();
            _indexBuffer = new List<int>();
        }

        public void Initialize(int creationIndex, int totalComponents, Stack<IComponent>[] componentPools, ContextInfo contextInfo = null, IAERC aerc = null)
        {
            Reactivate(creationIndex);

            _totalComponents = totalComponents;
            _components = new IComponent[totalComponents];
            _componentPools = componentPools;
            _contextInfo = contextInfo ?? createDefaultContextInfo();
            _aerc = aerc ?? new SafeAERC(this);
        }

        ContextInfo createDefaultContextInfo()
        {
            var componentNames = new string[totalComponents];
            for (int i = 0; i < componentNames.Length; i++)
            {
                componentNames[i] = i.ToString();
            }

            return new ContextInfo("No Context", componentNames, null);
        }

        public void Reactivate(int creationIndex)
        {
            _creationIndex = creationIndex;
            _isEnabled = true;
        }

        /// Adds a component at the specified index.
        /// You can only have one component at an index.
        /// Each component type must have its own constant index.
        /// The prefered way is to use the
        /// generated methods from the code generator.
        public void AddComponent(int index, IComponent component)
        {
            if (!_isEnabled)
            {
                throw new EntityIsNotEnabledException(
                    "Cannot add component '" +
                    _contextInfo.componentNames[index] + "' to " + this + "!"
                );
            }

            if (HasComponent(index))
            {
                throw new EntityAlreadyHasComponentException(
                    index, "Cannot add component '" +
                            _contextInfo.componentNames[index] + "' to " + this + "!",
                    "You should check if an entity already has the component " +
                    "before adding it or use entity.ReplaceComponent()."
                );
            }

            _components[index] = component;
            _componentsCache = null;
            _componentIndicesCache = null;
            _toStringCache = null;
            if (OnComponentAdded != null)
            {
                OnComponentAdded(this, index, component);
            }
        }

        /// Removes a component at the specified index.
        /// You can only remove a component at an index if it exists.
        /// The prefered way is to use the
        /// generated methods from the code generator.
        public void RemoveComponent(int index)
        {
            if (!_isEnabled)
            {
                throw new EntityIsNotEnabledException(
                    "Cannot remove component '" +
                    _contextInfo.componentNames[index] + "' from " + this + "!"
                );
            }

            if (!HasComponent(index))
            {
                throw new EntityDoesNotHaveComponentException(
                    index, "Cannot remove component '" +
                            _contextInfo.componentNames[index] + "' from " + this + "!",
                    "You should check if an entity has the component " +
                    "before removing it."
                );
            }

            replaceComponent(index, null);
        }

        /// Replaces an existing component at the specified index
        /// or adds it if it doesn't exist yet.
        /// The prefered way is to use the
        /// generated methods from the code generator.
        public void ReplaceComponent(int index, IComponent component)
        {
            if (!_isEnabled)
            {
                throw new EntityIsNotEnabledException(
                    "Cannot replace component '" +
                    _contextInfo.componentNames[index] + "' on " + this + "!"
                );
            }

            if (HasComponent(index))
            {
                replaceComponent(index, component);
            }
            else if (component != null)
            {
                AddComponent(index, component);
            }
        }

        void replaceComponent(int index, IComponent replacement)
        {
            // TODO VD PERFORMANCE
            // _toStringCache = null;

            var previousComponent = _components[index];
            if (replacement != previousComponent)
            {
                _components[index] = replacement;
                _componentsCache = null;
                if (replacement != null)
                {
                    if (OnComponentReplaced != null)
                    {
                        OnComponentReplaced(
                            this, index, previousComponent, replacement
                        );
                    }
                }
                else
                {
                    _componentIndicesCache = null;

                    // TODO VD PERFORMANCE
                    _toStringCache = null;

                    if (OnComponentRemoved != null)
                    {
                        OnComponentRemoved(this, index, previousComponent);
                    }
                }

                GetComponentPool(index).Push(previousComponent);

            }
            else
            {
                if (OnComponentReplaced != null)
                {
                    OnComponentReplaced(
                        this, index, previousComponent, replacement
                    );
                }
            }
        }

        /// Returns a component at the specified index.
        /// You can only get a component at an index if it exists.
        /// The prefered way is to use the
        /// generated methods from the code generator.
        public IComponent GetComponent(int index)
        {
            if (!HasComponent(index))
            {
                throw new EntityDoesNotHaveComponentException(
                    index, "Cannot get component '" +
                            _contextInfo.componentNames[index] + "' from " + this + "!",
                    "You should check if an entity has the component " +
                    "before getting it."
                );
            }

            return _components[index];
        }

        /// Returns all added components.
        public IComponent[] GetComponents()
        {
            if (_componentsCache == null)
            {
                for (int i = 0; i < _components.Length; i++)
                {
                    var component = _components[i];
                    if (component != null)
                    {
                        _componentBuffer.Add(component);
                    }
                }

                _componentsCache = _componentBuffer.ToArray();
                _componentBuffer.Clear();
            }

            return _componentsCache;
        }

        /// Returns all indices of added components.
        public int[] GetComponentIndices()
        {
            if (_componentIndicesCache == null)
            {
                for (int i = 0; i < _components.Length; i++)
                {
                    if (_components[i] != null)
                    {
                        _indexBuffer.Add(i);
                    }
                }

                _componentIndicesCache = _indexBuffer.ToArray();
                _indexBuffer.Clear();
            }

            return _componentIndicesCache;
        }

        /// Determines whether this entity has a component
        /// at the specified index.
        public bool HasComponent(int index)
        {
            var test = this;
            var c = _components;
            if (_components == null)
            {

            }
            return _components[index] != null;
        }

        /// Determines whether this entity has components
        /// at all the specified indices.
        public bool HasComponents(int[] indices)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                if (_components[indices[i]] == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// Determines whether this entity has a component
        /// at any of the specified indices.
        public bool HasAnyComponent(int[] indices)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                if (_components[indices[i]] != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// Removes all components.
        public void RemoveAllComponents()
        {
            _toStringCache = null;
            for (int i = 0; i < _components.Length; i++)
            {
                if (_components[i] != null)
                {
                    replaceComponent(i, null);
                }
            }
        }

        /// Returns the componentPool for the specified component index.
        /// componentPools is set by the context which created the entity and
        /// is used to reuse removed components.
        /// Removed components will be pushed to the componentPool.
        /// Use entity.CreateComponent(index, type) to get a new or
        /// reusable component from the componentPool.
        public Stack<IComponent> GetComponentPool(int index)
        {
            var componentPool = _componentPools[index];
            if (componentPool == null)
            {
                componentPool = new Stack<IComponent>();
                _componentPools[index] = componentPool;
            }

            return componentPool;
        }

        /// Returns a new or reusable component from the componentPool
        /// for the specified component index.
        public IComponent CreateComponent(int index, Type type)
        {
            var componentPool = GetComponentPool(index);
            return componentPool.Count > 0
                ? componentPool.Pop()
                : (IComponent)Activator.CreateInstance(type);
        }

        /// Returns a new or reusable component from the componentPool
        /// for the specified component index.
        public T CreateComponent<T>(int index) where T : new()
        {
            var componentPool = GetComponentPool(index);
            return componentPool.Count > 0 ? (T)componentPool.Pop() : new T();
        }

        /// Returns the number of objects that retain this entity.
        public int retainCount { get { return _aerc.retainCount; } }

        /// Retains the entity. An owner can only retain the same entity once.
        /// Retain/Release is part of AERC (Automatic Entity Reference Counting)
        /// and is used internally to prevent pooling retained entities.
        /// If you use retain manually you also have to
        /// release it manually at some point.
        public void Retain(object owner)
        {
            _aerc.Retain(owner);

            // TODO VD PERFORMANCE
            // _toStringCache = null;
        }

        /// Releases the entity. An owner can only release an entity
        /// if it retains it.
        /// Retain/Release is part of AERC (Automatic Entity Reference Counting)
        /// and is used internally to prevent pooling retained entities.
        /// If you use retain manually you also have to
        /// release it manually at some point.
        public void Release(object owner)
        {
            _aerc.Release(owner);

            // TODO VD PERFORMANCE
            // _toStringCache = null;

            if (_aerc.retainCount == 0)
            {
                if (OnEntityReleased != null)
                {
                    OnEntityReleased(this);
                }
            }
        }

        // Dispatches OnDestroyEntity which will start the destroy process.
        public void Destroy()
        {
            if (!_isEnabled)
            {
                throw new EntityIsNotEnabledException("Cannot destroy " + this + "!");
            }

            if (OnDestroyEntity != null)
            {
                OnDestroyEntity(this);
            }
        }

        // This method is used internally. Don't call it yourself.
        // Use entity.Destroy();
        public void InternalDestroy()
        {
            _isEnabled = false;
            RemoveAllComponents();
            OnComponentAdded = null;
            OnComponentReplaced = null;
            OnComponentRemoved = null;
            OnDestroyEntity = null;
        }

        // Do not call this method manually. This method is called by the context.
        public void RemoveAllOnEntityReleasedHandlers()
        {
            OnEntityReleased = null;
        }

        /// Returns a cached string to describe the entity
        /// with the following format:
        /// Entity_{creationIndex}(*{retainCount})({list of components})
        public override string ToString()
        {
            if (_toStringCache == null)
            {
                if (_toStringBuilder == null)
                {
                    _toStringBuilder = new StringBuilder();
                }
                _toStringBuilder.Length = 0;
                _toStringBuilder
                    .Append("Entity_")
                    .Append(_creationIndex)

                    // TODO VD PERFORMANCE
                    //                    .Append("(*")
                    //                    .Append(retainCount)
                    //                    .Append(")")

                    .Append("(");

                const string separator = ", ";
                var components = GetComponents();
                var lastSeparator = components.Length - 1;
                for (int i = 0; i < components.Length; i++)
                {
                    var component = components[i];
                    var type = component.GetType();

                    // TODO VD PERFORMANCE
                    _toStringCache = null;

                    //                    var implementsToString = type.GetMethod("ToString")
                    //                        .DeclaringType.ImplementsInterface<IComponent>();
                    //                    _toStringBuilder.Append(
                    //                        implementsToString
                    //                            ? component.ToString()
                    //                            : type.ToCompilableString().RemoveComponentSuffix()
                    //                    );

                    _toStringBuilder.Append(component.ToString());

                    if (i < lastSeparator)
                    {
                        _toStringBuilder.Append(separator);
                    }
                }

                _toStringBuilder.Append(")");
                _toStringCache = _toStringBuilder.ToString();
            }

            return _toStringCache;
        }
    }
   


}

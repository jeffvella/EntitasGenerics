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


    public class GenericEntity<TEntity> : Entity, IGenericEntity where TEntity : IEntity, IGenericEntity
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




}

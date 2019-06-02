using Entitas;
using Entitas.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitas.Generics
{
    public interface IGenericEntity
    {
        TComponent GetComponent<TComponent>() where TComponent : IComponent, new();

        bool TryGetComponent<TComponent>(out TComponent component) where TComponent : IComponent, new();

        TComponent CreateComponent<TComponent>() where TComponent : IComponent, new();

        int GetIndex<TComponent>() where TComponent : IComponent, new();

        void ReplaceComponent<TComponent>(TComponent component) where TComponent : IComponent, new();

        void AddComponent<TComponent>(TComponent component) where TComponent : IComponent, new();

        void RemoveComponent<TComponent>() where TComponent : IComponent, new();

        bool HasComponent<TComponent>() where TComponent : IComponent, new();

        void SetFlag<TComponent>(bool value = true) where TComponent : IFlagComponent, new();

        bool IsFlagged<TComponent>() where TComponent : IFlagComponent, new();
    }

    public class GenericEntity<TEntity> : Entity, IGenericEntity where TEntity : IEntity, IGenericEntity
    {
        public TComponent GetComponent<TComponent>() where TComponent : IComponent, new()
        {
            return (TComponent)GetComponent(ComponentHelper<TEntity, TComponent>.Index);
        }

        public bool TryGetComponent<TComponent>(out TComponent component) where TComponent : IComponent, new()
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

        public ComponentAccessor<TComponent> Get<TComponent>() where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            var component = !HasComponent(index)
                ? CreateComponent<TComponent>(index)
                : (TComponent)GetComponent(index);

            return new ComponentAccessor<TComponent>(this, index, component);
        }

        public TComponent CreateComponent<TComponent>() where TComponent : IComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            return CreateComponent<TComponent>(index);
        }

        public void ReplaceComponent<TComponent>(TComponent component) where TComponent : IComponent, new()
        {
            ReplaceComponent(ComponentHelper<TEntity, TComponent>.Index, component);
        }

        public void AddComponent<TComponent>(TComponent component) where TComponent : IComponent, new()
        {
            AddComponent(ComponentHelper<TEntity, TComponent>.Index, component);
        }

        public void RemoveComponent<TComponent>() where TComponent : IComponent, new()
        {
            RemoveComponent(ComponentHelper<TEntity, TComponent>.Index);
        }

        public bool HasComponent<TComponent>() where TComponent : IComponent, new()
        {
            return HasComponent(ComponentHelper<TEntity, TComponent>.Index);
        }

        public void SetFlag<TComponent>(bool value = true) where TComponent : IFlagComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            if (HasComponent(index))
            {
                if(!value)
                {
                    RemoveComponent(index);
                }
            }
            else if(value)
            {
                AddComponent(index, CreateComponent<TComponent>());
            }
        }

        public void AddFlag<TComponent>() where TComponent : IFlagComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            AddComponent(index, CreateComponent<TComponent>());
        }

        public void Apply<TComponent, TValue>(TValue value) where TComponent : class, IValueComponent<TValue>, new()
        {
            Get<TComponent>().Apply(value);
        }

        public void RemoveFlag<TComponent>() where TComponent : IFlagComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            RemoveComponent(index);
        }

        public bool IsFlagged<TComponent>() where TComponent : IFlagComponent, new()
        {
            var index = ComponentHelper<TEntity, TComponent>.Index;
            return HasComponent(index);
        }

        public void RegisterComponentListener<TComponent>(IAddedComponentListener<TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            //var acc = Get<AddedListenersComponent<TEntity, TComponent>>();
            //acc.Component.Register(listener);
            //acc.Apply();

            ModifyComponent<AddedListenersComponent<TEntity, TComponent>>(c => c.Register(listener));
        }

        public void DeregisterComponentListener<TComponent>(IAddedComponentListener<TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            //var acc = Get<AddedListenersComponent<TEntity, TComponent>>();
            //acc.Component.Deregister(listener);
            //acc.Apply();

            ModifyComponent<AddedListenersComponent<TEntity, TComponent>>(c => c.Deregister(listener));
        }

        public void RegisterComponentListener<TComponent>(IRemovedComponentListener<TEntity,TComponent> listener) where TComponent : IComponent, new()
        {
            //var acc = Get<RemovedListenersComponent<TEntity, TComponent>>();
            //acc.Component.Register(listener);
            //acc.Apply();

            ModifyComponent<RemovedListenersComponent<TEntity, TComponent>>(c => c.Register(listener));
        }

        //public void DeregisterComponentListener<TComponent>(IRemovedComponentListener<TEntity, TComponent> listener) where TComponent : IComponent, new()
        //{
        //    var acc = Get<RemovedListenersComponent<TEntity, TComponent>>();
        //    acc.Component.Deregister(listener);
        //    acc.Apply();
        //}

        public void DeregisterComponentListener<TComponent>(IRemovedComponentListener<TEntity, TComponent> listener) where TComponent : IComponent, new()
        {
            ModifyComponent<RemovedListenersComponent<TEntity, TComponent>>(c => c.Deregister(listener));
        }

        private void ModifyComponent<T>(Action<T> action) where T : IComponent, new()
        {
            var acc = Get<T>();
            action(acc.Component);
            acc.Apply();
        }

        public void RegisterComponentListener<TComponent>(Action<TEntity> listener, GroupEvent type) where TComponent : IComponent, new()
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

        public void DeregisterComponentListener<TComponent>(Action<TEntity> listener, GroupEvent type) where TComponent : IComponent, new()
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



    }




}

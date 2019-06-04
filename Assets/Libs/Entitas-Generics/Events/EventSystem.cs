using Entitas.VisualDebugging.Unity;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.Generics
{
    public sealed class EventSystem<TEntity, TComponent> : IReactiveSystem, ICustomDisplayName
        where TEntity : class, IEntity, IGenericEntity, new()
        where TComponent : class, IEventComponent, new()
    {
        private readonly GroupEvent _type;
        private readonly IGenericContext<TEntity> _context;
        private readonly ICollector<TEntity> _addedCollector;
        private readonly ICollector<TEntity> _removedCollector;
        private readonly bool _isRemoveType;
        private readonly bool _isAddType;
        private List<TEntity> _buffer;

        public EventSystem(IGenericContext<TEntity> context, GroupEvent type)
        {         
            _context = context;
            _type = type;

            _isAddType = _type == GroupEvent.Added || _type == GroupEvent.AddedOrRemoved;
            _isRemoveType = _type == GroupEvent.Removed || _type == GroupEvent.AddedOrRemoved;
            _buffer = new List<TEntity>();

            if (_isAddType)
            {
                _addedCollector = context.GetTriggerCollector<TComponent>(GroupEvent.Added);

            }
            if(_isRemoveType)
            {
                _removedCollector = context.GetTriggerCollector<TComponent>(GroupEvent.Removed); 
            }
        }

        public void Execute()
        {
            if (_isAddType)
            {
                ProcessEvents<AddedListenersComponent<TEntity, TComponent>>(_addedCollector);
            }
            if (_isRemoveType)
            {
                ProcessEvents<RemovedListenersComponent<TEntity, TComponent>>(_removedCollector);
            }
        }

        private void ProcessEvents<TListenerComponent>(ICollector<TEntity> collector) where TListenerComponent : class, IListenerComponent<TEntity>, new()
        {
            if(collector.count == 0)            
                return;            

            var uniqueExists = _context.Unique.TryGetComponent<TListenerComponent>(out var uniqueComponent);
            var notifyUniqueListeners = uniqueExists && uniqueComponent != null && uniqueComponent.ListenerCount > 0;

            LoadBuffer(collector);

            for (int i = 0; i < _buffer.Count; i++)
            {
                TEntity entity = _buffer[i];

                if (notifyUniqueListeners && entity.Equals(_context.Unique))
                {
                    notifyUniqueListeners = false;
                }

                if (entity.HasComponent<TListenerComponent>())
                {
                    var listenerComponent = entity.GetComponent<TListenerComponent>();
                    if (listenerComponent.ListenerCount > 0)
                    {
                        listenerComponent.Raise(entity);
                    }
                }
            }

            if (notifyUniqueListeners)
            {
                for (int i = 0; i < _buffer.Count; i++)
                {
                    TEntity entity = _buffer[i];
                    uniqueComponent.Raise(entity);
                }
            }

            ClearBuffer();
        }

        private void LoadBuffer(ICollector<TEntity> collector)
        {
            foreach (var entity in collector.collectedEntities)
            {
                entity.Retain(this);
                _buffer.Add(entity);
            }
            collector.ClearCollectedEntities();
        }

        private void ClearBuffer()
        {
            for (int i = 0; i < _buffer.Count; i++)
            {
                TEntity entity = _buffer[i];
                entity.Release(this);
            }
            _buffer.Clear();
        }

        public void Activate()
        {
            _addedCollector?.Activate();
            _removedCollector?.Activate();
        }

        public void Deactivate()
        {
            _addedCollector?.Deactivate();
            _removedCollector?.Deactivate();
        }

        public void Clear()
        {
            _addedCollector?.ClearCollectedEntities();
            _removedCollector?.ClearCollectedEntities();
        }

        public string DisplayName => $"EventSystem ({typeof(TComponent).Name}, {_type})";
    }

    public sealed class EventSystemStruct<TEntity, TComponentData> : IReactiveSystem, ICustomDisplayName
    where TEntity : class, IEntity, IGenericEntity, new()
    where TComponentData : struct, IEventComponent
    {
        private readonly GroupEvent _type;
        private readonly IGenericContext<TEntity> _context;
        private readonly ICollector<TEntity> _addedCollector;
        private readonly ICollector<TEntity> _removedCollector;
        private readonly bool _isRemoveType;
        private readonly bool _isAddType;
        private List<TEntity> _buffer;

        public EventSystemStruct(IGenericContext<TEntity> context, GroupEvent type)
        {
            _context = context;
            _type = type;

            _isAddType = _type == GroupEvent.Added || _type == GroupEvent.AddedOrRemoved;
            _isRemoveType = _type == GroupEvent.Removed || _type == GroupEvent.AddedOrRemoved;
            _buffer = new List<TEntity>();

            if (_isAddType)
            {
                _addedCollector = context.GetTriggerCollector2<TComponentData>(GroupEvent.Added);

            }
            if (_isRemoveType)
            {
                _removedCollector = context.GetTriggerCollector2<TComponentData>(GroupEvent.Removed);
            }
        }

        public void Execute()
        {
            if (_isAddType)
            {
                ProcessEvents<AddedListenersComponent<TEntity, StructComponentWrapper<TComponentData>>>(_addedCollector);
            }
            if (_isRemoveType)
            {
                ProcessEvents<RemovedListenersComponent<TEntity, StructComponentWrapper<TComponentData>>>(_removedCollector);
            }
        }

        private void ProcessEvents<TListenerComponent>(ICollector<TEntity> collector) where TListenerComponent : class, IListenerComponent<TEntity>, new()
        {
            if (collector.count == 0)
                return;

            var uniqueExists = _context.Unique.TryGetComponent<TListenerComponent>(out var uniqueComponent);
            var notifyUniqueListeners = uniqueExists && uniqueComponent != null && uniqueComponent.ListenerCount > 0;

            LoadBuffer(collector);

            for (int i = 0; i < _buffer.Count; i++)
            {
                TEntity entity = _buffer[i];

                if (notifyUniqueListeners && entity.Equals(_context.Unique))
                {
                    notifyUniqueListeners = false;
                }

                if (entity.HasComponent<TListenerComponent>())
                {
                    var listenerComponent = entity.GetComponent<TListenerComponent>();
                    if (listenerComponent.ListenerCount > 0)
                    {
                        listenerComponent.Raise(entity);
                    }
                }
            }

            if (notifyUniqueListeners)
            {
                for (int i = 0; i < _buffer.Count; i++)
                {
                    TEntity entity = _buffer[i];
                    uniqueComponent.Raise(entity);
                }
            }

            ClearBuffer();
        }

        private void LoadBuffer(ICollector<TEntity> collector)
        {
            foreach (var entity in collector.collectedEntities)
            {
                entity.Retain(this);
                _buffer.Add(entity);
            }
            collector.ClearCollectedEntities();
        }

        private void ClearBuffer()
        {
            for (int i = 0; i < _buffer.Count; i++)
            {
                TEntity entity = _buffer[i];
                entity.Release(this);
            }
            _buffer.Clear();
        }

        public void Activate()
        {
            _addedCollector?.Activate();
            _removedCollector?.Activate();
        }

        public void Deactivate()
        {
            _addedCollector?.Deactivate();
            _removedCollector?.Deactivate();
        }

        public void Clear()
        {
            _addedCollector?.ClearCollectedEntities();
            _removedCollector?.ClearCollectedEntities();
        }

        public string DisplayName => $"EventSystem ({typeof(TComponentData).Name}, {_type})";
    }
}
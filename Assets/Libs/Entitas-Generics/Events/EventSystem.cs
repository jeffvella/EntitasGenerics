using Entitas.VisualDebugging.Unity;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.Generics
{
    public sealed class EventSystem<TEntity, TComponent> : IReactiveSystem, ICustomDisplayName
        where TEntity : class, IContextLinkedEntity, new()
        where TComponent : IEventComponent, new()
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
            if (_isAddType && _addedCollector.count > 0)
            {
                var notifyUniqueListeners = _context.TryGet<AddedListenersComponent<TEntity, TComponent>>(_context.UniqueEntity, out var unqiueListener) && unqiueListener.ListenerCount > 0;

                LoadBuffer(_addedCollector);

                for (int i = 0; i < _buffer.Count; i++)
                {
                    TEntity entity = _buffer[i];

                    if (notifyUniqueListeners && entity.Equals(_context.UniqueEntity))
                    {
                        notifyUniqueListeners = false;
                    }

                    if (_context.Has<AddedListenersComponent<TEntity, TComponent>>(entity))
                    {
                        var listenerComponent = _context.Get<AddedListenersComponent<TEntity, TComponent>>(entity);
                        if (listenerComponent.Component.ListenerCount > 0)
                        {
                            if (_context.Has<TComponent>(entity))
                            {
                                var component = _context.Get<TComponent>(entity);
                                listenerComponent.Component.Raise((entity, component.Component));
                            }
                        }
                    }
                }

                if (notifyUniqueListeners)
                {
                    for (int i = 0; i < _buffer.Count; i++)
                    {
                        TEntity entity = _buffer[i];
                        var component = _context.Get<TComponent>(entity);
                        unqiueListener.Raise((entity, component.Component));
                    }
                }

                ClearBuffer();
            }

            if (_isRemoveType && _removedCollector.count > 0)
            {
                var notifyUniqueListeners = _context.TryGet<RemovedListenersComponent<TEntity, TComponent>>(_context.UniqueEntity, out var unqiueListener) && unqiueListener.ListenerCount > 0;

                LoadBuffer(_removedCollector);

                for (int i = 0; i < _buffer.Count; i++)
                {
                    TEntity entity = _buffer[i];

                    if (notifyUniqueListeners && entity.Equals(_context.UniqueEntity))
                    {
                        notifyUniqueListeners = false;
                    }

                    if (_context.Has<RemovedListenersComponent<TEntity, TComponent>>(entity))
                    {
                        var listenerComponent = _context.Get<RemovedListenersComponent<TEntity, TComponent>>(entity);
                        if (listenerComponent.Component.ListenerCount > 0)
                        {
                            listenerComponent.Component.Raise(entity);
                        }
                    }
                }

                if (notifyUniqueListeners)
                {
                    for (int i = 0; i < _buffer.Count; i++)
                    {
                        TEntity entity = _buffer[i];
                        unqiueListener.Raise(entity);
                    }
                }

                ClearBuffer();
            }
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
}
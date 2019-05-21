using System.Collections.Generic;
using System.Linq;

namespace Entitas.Generics
{
    public sealed class EventSystem<TEntity, TComponent> : IReactiveSystem, ICustomDebugInfo
        where TEntity : class, IEntity
        where TComponent : IComponent, new()
    {
        private readonly GroupEvent _type;
        private readonly IGenericContext<TEntity> _context;
        private readonly ICollector<TEntity> _addedCollector;
        private readonly ICollector<TEntity> _removedCollector;
        private readonly bool _isRemoveType;
        private readonly bool _isAddType;

        public EventSystem(IGenericContext<TEntity> context, GroupEvent type)
        {         
            _context = context;
            _type = type;

            _isAddType = _type == GroupEvent.Added || _type == GroupEvent.AddedOrRemoved;
            _isRemoveType = _type == GroupEvent.Removed || _type == GroupEvent.AddedOrRemoved;

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
                var entities = _addedCollector.collectedEntities.ToList(); // todo buffer like reactive system

                foreach (var entity in entities)
                {
                    if (_context.Has<AddedListenersComponent<TEntity, TComponent>>(entity))
                    {
                        var addedListenerComponent = _context.Get<AddedListenersComponent<TEntity, TComponent>>(entity);
                        if (addedListenerComponent.ListenerCount > 0)
                        {
                            if (_context.Has<TComponent>(entity))
                            {
                                var changedComponent = _context.Get<TComponent>(entity);
                                addedListenerComponent.Raise((entity, changedComponent));
                            }
                        }
                    }
                }
                _addedCollector?.ClearCollectedEntities();
            }

            if (_isRemoveType && _removedCollector.count > 0)
            {
                var entities = _removedCollector.collectedEntities.ToList(); // todo buffer like reactive system

                foreach (var entity in entities)
                {
                    if (_context.Has<RemovedListenersComponent<TEntity, TComponent>>(entity))
                    {
                        var removedListenersComponent = _context.Get<RemovedListenersComponent<TEntity, TComponent>>(entity);
                        if (removedListenersComponent.ListenerCount > 0)
                        {
                            removedListenersComponent.Raise(entity);
                        }
                    }
                }            
                _removedCollector?.ClearCollectedEntities();
            }            
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

        public string DisplayName => $"{typeof(TComponent).Name} {_type}";
    }
}
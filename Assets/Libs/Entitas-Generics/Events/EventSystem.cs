using System.Collections.Generic;
using System.Linq;

namespace Entitas.Generics
{
    public sealed class EventSystem<TEntity, TComponent> : IReactiveSystem, ICustomDebugInfo
        where TEntity : class, IEntity
        where TComponent : IEventComponent, new()
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

                TEntity[] array = _addedCollector.collectedEntities.ToArray(); // todo: dont allocate, why is collectedEntities a HashSet?

                bool uniqueEntityProcessed = false;

                for (int i = 0; i < array.Length; i++)
                {
                    TEntity entity = array[i];

                    if (!uniqueEntityProcessed && entity.Equals(_context.UniqueEntity))
                        uniqueEntityProcessed = true;

                    if (_context.Has<AddedListenersComponent<TEntity, TComponent>>(entity))
                    {
                        var addedListenerComponent = _context.Get<AddedListenersComponent<TEntity, TComponent>>(entity);
                        if (addedListenerComponent.ListenerCount > 0)
                        {
                            if (_context.Has<TComponent>(entity))
                            {
                                var component = _context.Get<TComponent>(entity);      
                                addedListenerComponent.Raise((entity, component));
                            }
                        }
                    }
                }

                if(!uniqueEntityProcessed)
                {                    
                    var uniqueListenerComponentFound = _context.TryGet<AddedListenersComponent<TEntity, TComponent>>(_context.UniqueEntity, out var unqiueListener);
                    if (uniqueListenerComponentFound && unqiueListener.ListenerCount > 0)
                    {
                        for (int i = 0; i < array.Length; i++)
                        {
                            TEntity entity = array[i];
                            var component = _context.Get<TComponent>(entity);                          
                            unqiueListener.Raise((entity, component));
                        }
                    }
                }
          
                if (_addedCollector.count > 0)
                {
                    _addedCollector.ClearCollectedEntities();
                }
            }

            if (_isRemoveType && _removedCollector.count > 0)
            {
                var notifyUniqueListeners = _context.TryGet<RemovedListenersComponent<TEntity, TComponent>>(_context.UniqueEntity, out var unqiueListener) && unqiueListener.ListenerCount > 0;

                foreach (var entity in _removedCollector.collectedEntities)
                {
                    // Handle listeners stored on the entity that owns the event.

                    if (_context.Has<RemovedListenersComponent<TEntity, TComponent>>(entity))
                    {
                        var addedListenerComponent = _context.Get<RemovedListenersComponent<TEntity, TComponent>>(entity);
                        if (addedListenerComponent.ListenerCount > 0)
                        {        
                            addedListenerComponent.Raise(entity);                         
                        }
                    }

                    // Handle listeners that should be notified whenever the event occurs on ANY entity.
                    // These are currently stored on the unique entity.

                    //if (notifyUniqueListeners)
                    //{
                    //    unqiueListener.Raise(entity);
                    //}
                }

                if (_removedCollector.count > 0)
                {
                    _removedCollector.ClearCollectedEntities();
                }
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
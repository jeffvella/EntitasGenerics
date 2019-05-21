using System;
using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas
{
    /// A ReactiveSystem calls Execute(entities) if there were changes based on
    /// the specified Collector and will only pass in changed entities.
    /// A common use-case is to react to changes, e.g. a change of the position
    /// of an entity to update the gameObject.transform.position
    /// of the related gameObject.
   /// 
    public abstract class GenericReactiveSystem2<TEntity> : IReactiveSystem where TEntity : class, IEntity
    {
        private string _toStringCache;
        private ICollector<TEntity> _collector;
        private List<TEntity> _buffer;
        private Func<IGenericContext<TEntity>, TEntity, bool> _filter;
        private IGenericContext<TEntity> _context;
        private ReactiveSystemConfig _config;

        protected class ReactiveSystemConfig
        {
            public Func<IGenericContext<TEntity>, ICollector<TEntity>> CollectionTrigger { get; set; }

            public Func<IGenericContext<TEntity>, TEntity, bool> EntityFilter { get; set; }

            public Action<IGenericContext<TEntity>, List<TEntity>> OnEntitiesFound { get; set; }
        }

        protected void Initialize(IGenericContext<TEntity> context, Action<ReactiveSystemConfig> configurator)
        {
            _context = context;
            _config = new ReactiveSystemConfig();
            configurator?.Invoke(_config);
            _collector = _config.CollectionTrigger(_context);
            _buffer = new List<TEntity>();
        }

        //protected abstract void Execute(IGenericContext<TEntity> context, List<TEntity> entities);

        /// Activates the ReactiveSystem and starts observing changes
        /// based on the specified Collector.
        /// ReactiveSystem are activated by default.
        public void Activate()
        {
            _collector.Activate();
        }

        /// Deactivates the ReactiveSystem.
        /// No changes will be tracked while deactivated.
        /// This will also clear the ReactiveSystem.
        /// ReactiveSystem are activated by default.
        public void Deactivate()
        {
            _collector.Deactivate();
        }

        /// Clears all accumulated changes.
        public void Clear()
        {
            _collector.ClearCollectedEntities();
        }

        /// Will call Execute(entities) with changed entities
        /// if there are any. Otherwise it will not call Execute(entities).
        public void Execute()
        {
            if (_collector.count != 0)
            {
                if (_filter != null)
                {                
                    foreach (var e in _collector.collectedEntities)
                    {
                        if (_filter(_context, e))
                        {
                            e.Retain(this);
                            _buffer.Add(e);
                        }
                    }
                }
                else
                {
                    foreach (var e in _collector.collectedEntities)
                    {
                        e.Retain(this);
                        _buffer.Add(e);
                    }
                }

                _collector.ClearCollectedEntities();

                if (_buffer.Count != 0)
                {
                    try
                    {
                        _config.OnEntitiesFound(_context, _buffer);
                    }
                    finally
                    {
                        for (int i = 0; i < _buffer.Count; i++)
                        {
                            _buffer[i].Release(this);
                        }
                        _buffer.Clear();
                    }
                }
            }
        }

        public override string ToString()
        {
            if (_toStringCache == null)
            {
                _toStringCache = "ReactiveSystem(" + GetType().Name + ")";
            }

            return _toStringCache;
        }

        ~GenericReactiveSystem2()
        {
            Deactivate();
        }
    }

    /// A ReactiveSystem calls Execute(entities) if there were changes based on
    /// the specified Collector and will only pass in changed entities.
    /// A common use-case is to react to changes, e.g. a change of the position
    /// of an entity to update the gameObject.transform.position
    /// of the related gameObject.
    public abstract class GenericReactiveSystem<TEntity> : IReactiveSystem where TEntity : class, IEntity
    {
        string _toStringCache;
        readonly ICollector<TEntity> _collector;
        readonly List<TEntity> _buffer = new List<TEntity>();
        private Func<IGenericContext<TEntity>, TEntity, bool> _filter;

        protected readonly IGenericContext<TEntity> Context;

        protected GenericReactiveSystem(IGenericContext<TEntity> context, 
            Func<IGenericContext<TEntity>, ICollector<TEntity>> triggerProducer)
        {
            _collector = triggerProducer(context);
            //_buffer = new List<TEntity>();
        }


        protected GenericReactiveSystem(IGenericContext<TEntity> context,
            Func<IGenericContext<TEntity>, ICollector<TEntity>> triggerProducer,
            Func<IGenericContext<TEntity>, TEntity, bool> filter)
        {
            Context = context;
            _filter = filter;
            _collector = triggerProducer(context);
            //_buffer = new List<TEntity>();
        }

        protected GenericReactiveSystem(ICollector<TEntity> collector)
        {
            _collector = collector;
            //_buffer = new List<TEntity>();
        }

        protected abstract void Execute(List<TEntity> entities);

        /// Activates the ReactiveSystem and starts observing changes
        /// based on the specified Collector.
        /// ReactiveSystem are activated by default.
        public void Activate()
        {
            _collector.Activate();
        }

        /// Deactivates the ReactiveSystem.
        /// No changes will be tracked while deactivated.
        /// This will also clear the ReactiveSystem.
        /// ReactiveSystem are activated by default.
        public void Deactivate()
        {
            _collector.Deactivate();
        }

        /// Clears all accumulated changes.
        public void Clear()
        {
            _collector.ClearCollectedEntities();
        }

        /// Will call Execute(entities) with changed entities
        /// if there are any. Otherwise it will not call Execute(entities).
        public void Execute()
        {
            if (_buffer == null)
                throw new InvalidOperationException("_buffer is null, probably a thread access ownership issue");

            if (_collector.count != 0)
            {
                if (_filter != null)
                {
                    foreach (var e in _collector.collectedEntities)  
                    {
                        if (_filter(Context, e))
                        {
                            e.Retain(this);
                            _buffer.Add(e);
                        }
                    }
                }
                else
                {
                    foreach (var e in _collector.collectedEntities)
                    {         
                        e.Retain(this);
                        _buffer.Add(e);                      
                    }
                }

                _collector.ClearCollectedEntities();

                if (_buffer.Count != 0)
                {
                    try
                    {
                        Execute(_buffer);
                    }
                    finally
                    {
                        for (int i = 0; i < _buffer.Count; i++)
                        {
                            _buffer[i].Release(this);
                        }
                        _buffer.Clear();
                    }
                }
            }
        }

        public override string ToString()
        {
            if (_toStringCache == null)
            {
                _toStringCache = "ReactiveSystem(" + GetType().Name + ")";
            }

            return _toStringCache;
        }

        ~GenericReactiveSystem()
        {
            Deactivate();
        }
    }
}


using Entitas;
using UnityEngine;


namespace EntitasGenerics
{
    public class DebugMessageSystem : IReactiveSystem
    {
        private bool _isActive;
        private readonly LoggingContext _context;
        private ICollector<Entity> _collector;
        private IMatcher<Entity> _matcher;
        private TriggerOnEvent<Entity> _trigger;

        public DebugMessageSystem(GenericContexts contexts, Services services)
        {
            _context = contexts.Logging;
            _matcher = _context.CreateMatcher<DebugMessageComponent>();
            _trigger = new TriggerOnEvent<Entity>(_matcher, GroupEvent.AddedOrRemoved);
            _collector = _context.CreateCollector(_trigger);  
        } 

        public void Execute()
        {
            if (_collector.count == 0)
                return;            

            foreach (var entity in _collector.collectedEntities)
            {            
                if(_context.TryGetComponent(entity, out DebugMessageComponent debugMessage))
                {
                    Debug.Log($"DebugMessage component changed - Message='{debugMessage.Message}'");                   
                }
                else
                {
                    Debug.Log($"DebugMessage component was removed'");
                }           
            }

            _collector.ClearCollectedEntities();
        }

        public void Activate()
        {
            _isActive = true;
            _collector.Activate();
        }

        public void Deactivate()
        {
            _isActive = false;
            _collector.Deactivate();
        }

        public bool IsActive => _isActive;

        public void Clear()
        {
            _collector.ClearCollectedEntities();
        }
    }

}
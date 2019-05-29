using Entitas.Generics;
using System.Diagnostics;

namespace Entitas.MatchLine
{
    public sealed class UpdateInputSystem : IExecuteSystem
    {
        private readonly Contexts _contexts;
        private readonly IInputService _inputService;
      
        private readonly PointerHoldingPositionComponent _pointerHoldingPositionComponent;
        private readonly PointerHoldingTimeComponent _pointerHoldingTimeComponent;

        //private readonly PersistentComponentAccessor<PointerHoldingPositionComponent> _pointerHoldingPosition;
        //private readonly PersistentComponentAccessor<PointerHoldingTimeComponent> _pointerHolderingTime;
        //private readonly PersistentComponentAccessor<GameOverComponent> _gameOver;
        //private readonly PersistentEntityAccessor<InputEntity> _flagsEntity;

        //private PersistentComponentAccessor<PointerHoldingComponent> _PointerHoldingComponent;
        //private PersistentComponentAccessor<PointerStartedHoldingComponent> _PointerStartedHoldingComponent;
        //private PersistentComponentAccessor<PointerReleasedComponent> _PointerReleasedComponent;

        public UpdateInputSystem(Contexts contexts, IServices services)
        {
            _contexts = contexts;
            _inputService = services.InputService;

            //_pointerHoldingPosition = _contexts.Input.GetUnique<PointerHoldingPositionComponent>().ToPersistant();
            //_pointerHolderingTime = _contexts.Input.GetUnique<PointerHoldingTimeComponent>().ToPersistant();
            //_gameOver = _contexts.GameState.GetUnique<GameOverComponent>().ToPersistant();

            //_flagsEntity = new EntityAccessor<InputEntity>(_contexts.Input, _contexts.Input.UniqueEntity);

            //_PointerHoldingComponent = flagsEntity.Get<PointerHoldingComponent>().ToPersistant();
            //_PointerStartedHoldingComponent = flagsEntity.Get<PointerStartedHoldingComponent>().ToPersistant();
            //_PointerReleasedComponent = flagsEntity.Get<PointerReleasedComponent>().ToPersistant();

            _pointerHoldingPositionComponent = _contexts.Input.GetUnique<PointerHoldingPositionComponent>().Component;
            _pointerHoldingTimeComponent = _contexts.Input.GetUnique<PointerHoldingTimeComponent>().Component;
        }

        public void Execute()
        {
            if (_contexts.GameState.IsFlagged<GameOverComponent>())    
            {
                _contexts.Input.SetFlag<PointerHoldingComponent>(false);
                _contexts.Input.SetFlag<PointerStartedHoldingComponent>(false);
                _contexts.Input.SetFlag<PointerReleasedComponent>(true);
            }
            else
            {
                var delta = _contexts.Input.GetUnique<DeltaTimeComponent>().Component.Value;
                _inputService.Update(delta);

                _contexts.Input.SetFlag<PointerHoldingComponent>(_inputService.IsHolding());
                _contexts.Input.SetFlag<PointerStartedHoldingComponent>(_inputService.IsStartedHolding());
                _contexts.Input.SetFlag<PointerReleasedComponent>(_inputService.IsReleased());

                _pointerHoldingPositionComponent.Value = _inputService.HoldingPosition();
                _pointerHoldingTimeComponent.Value = _inputService.HoldingTime();
            }

            //var entity = _contexts.Input.UniqueEntity;
            //var context = _contexts.Input;
            ////var a11 = new ComponentAccessor<PointerStartedHoldingComponent>(entity, context);

            //var sw = new Stopwatch();
            //sw.Start();
            //var a1 = new ComponentAccessor<PointerStartedHoldingComponent>(entity, context);
            //sw.Stop();
            //var sw2 = new Stopwatch();
            //var index = context.GetComponentIndex<PointerStartedHoldingComponent>();
            //sw2.Start();

            //var c = !entity.HasComponent(index)
            //    ? entity.CreateComponent<PointerStartedHoldingComponent>(index)
            //    : (PointerStartedHoldingComponent)entity.GetComponent(index);

            ////entity.CreateComponent<PointerStartedHoldingComponent>(index);
            ////var c = entity.GetComponent(index);
            //sw2.Stop();  
            //UnityEngine.Debug.Log($"{sw.Elapsed.TotalMilliseconds:N6} / {sw2.Elapsed.TotalMilliseconds:N6}");
        }
    }

}
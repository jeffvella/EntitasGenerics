using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class UpdateInputSystem : IExecuteSystem
    {
        private readonly Contexts _contexts;
        private readonly IInputService _inputService;

        public UpdateInputSystem(Contexts contexts, IServices services)
        {
            _contexts = contexts;
            _inputService = services.InputService;
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
                _inputService.Update(_contexts.Input.GetUnique<DeltaTimeComponent>().Value);
                _contexts.Input.SetFlag<PointerHoldingComponent>(_inputService.IsHolding());
                _contexts.Input.SetFlag<PointerStartedHoldingComponent>(_inputService.IsStartedHolding());
                _contexts.Input.SetFlag<PointerReleasedComponent>(_inputService.IsReleased());

                _contexts.Input.UniqueEntity.Get2<PointerHoldingPositionComponent>().Update(_inputService.HoldingPosition());
                _contexts.Input.UniqueEntity.Get2<PointerHoldingTimeComponent>().Update(_inputService.HoldingTime());
            }
        }
    }

}
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
                var deltaTime = _contexts.Input.GetUnique<DeltaTimeComponent>().Value;
                _inputService.Update(deltaTime);

                var isHolding = _inputService.IsHolding();
                _contexts.Input.SetFlag<PointerHoldingComponent>(isHolding);

                var isStartedHolding = _inputService.IsStartedHolding();
                _contexts.Input.SetFlag<PointerStartedHoldingComponent>(isStartedHolding);

                var isReleased = _inputService.IsReleased();
                _contexts.Input.SetFlag<PointerReleasedComponent>(isReleased);
                _contexts.Input.SetUnique<PointerHoldingPositionComponent>(c => GridPosition(c));
                _contexts.Input.SetUnique<PointerHoldingTimeComponent>(c => c.value = _inputService.HoldingTime());

            }
        }

        private GridPosition GridPosition(PointerHoldingPositionComponent c)
        {
            return c.value = _inputService.HoldingPosition();
        }
    }

}
using Entitas;
using Entitas.Generics;

public sealed class UpdateInputSystem : IExecuteSystem
{
    private readonly GenericContexts _contexts;
    private readonly IInputService _inputService;

    public UpdateInputSystem(GenericContexts contexts, Services services)
    {
        _contexts = contexts;
        _inputService = services.InputService;
    }

    public void Execute()
    {
        if (_contexts.GameState.IsFlagged<GameOverComponent>())
        {
            _contexts.Input.SetUniqueFlag<PointerHoldingComponent>(false);
            _contexts.Input.SetUniqueFlag<PointerStartedHoldingComponent>(false);
            _contexts.Input.SetUniqueFlag<PointerReleasedComponent>(true);

            //_contexts.input.isPointerHolding = false;
            //_contexts.input.isPointerStartedHolding = false;
            //_contexts.input.isPointerReleased = true;
        }
        else
        {
            var deltaTime = _contexts.Input.GetUnique<DeltaTimeComponent>().value;
            _inputService.Update(deltaTime);

            var isHolding = _inputService.IsHolding();
            _contexts.Input.SetUniqueFlag<PointerHoldingComponent>(isHolding);

            var isStartedHolding = _inputService.IsStartedHolding();
            _contexts.Input.SetUniqueFlag<PointerStartedHoldingComponent>(isStartedHolding);

            var isReleased = _inputService.IsReleased();
            _contexts.Input.SetUniqueFlag<PointerReleasedComponent>(isReleased);

            _contexts.Input.SetUnique(new PointerHoldingPositionComponent
            {
                value = _inputService.HoldingPosition()
            });

            _contexts.Input.SetUnique(new PointerHoldingTimeComponent
            {
                value = _inputService.HoldingTime()
            });

            //_inputService.Update(_contexts.input.deltaTime.value);
            //_contexts.input.isPointerHolding = _inputService.IsHolding();
            //_contexts.input.isPointerStartedHolding = _inputService.IsStartedHolding();
            //_contexts.input.isPointerReleased = _inputService.IsReleased();
            //_contexts.input.ReplacePointerHoldingPosition(_inputService.HoldingPosition());
            //_contexts.input.ReplacePointerHoldingTime(_inputService.HoldingTime());
        }
    }
}

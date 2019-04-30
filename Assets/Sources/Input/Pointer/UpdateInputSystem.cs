using Entitas;
using EntitasGenerics;

public sealed class UpdateInputSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;
    private readonly IInputService _inputService;

    public UpdateInputSystem(Contexts contexts, GenericContexts genericContexts, Services services)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
        _inputService = services.InputService;
    }

    public void Execute()
    {
        if (_contexts.gameState.isGameOver)
        {
            _genericContexts.Input.Remove<PointerHoldingComponent>();
            _genericContexts.Input.Remove<PointerStartedHoldingComponent>();
            _genericContexts.Input.Set<PointerReleasedComponent>();

            //_contexts.input.isPointerHolding = false;
            //_contexts.input.isPointerStartedHolding = false;
            //_contexts.input.isPointerReleased = true;
        }
        else
        {
            var deltaTime = _genericContexts.Input.Get<DeltaTimeComponent>().value;
            _inputService.Update(deltaTime);

            _genericContexts.Input.Set<PointerHoldingComponent>(_inputService.IsHolding());
            _genericContexts.Input.Set<PointerStartedHoldingComponent>(_inputService.IsStartedHolding());
            _genericContexts.Input.Set<PointerReleasedComponent>(_inputService.IsReleased());

            _genericContexts.Input.Set(new PointerHoldingPositionComponent
            {
                value = _inputService.HoldingPosition()
            });

            _genericContexts.Input.Set(new PointerHoldingTimeComponent
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

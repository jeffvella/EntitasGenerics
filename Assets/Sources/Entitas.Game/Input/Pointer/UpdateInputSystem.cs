using Entitas.Generics;
using System.Diagnostics;

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
                var delta = _contexts.Input.GetUnique<DeltaTimeComponent>().Component.Value;
                _inputService.Update(delta);

                _contexts.Input.SetFlag<PointerHoldingComponent>(_inputService.IsHolding());
                _contexts.Input.SetFlag<PointerStartedHoldingComponent>(_inputService.IsStartedHolding());
                _contexts.Input.SetFlag<PointerReleasedComponent>(_inputService.IsReleased());

                _contexts.Input.GetUnique<PointerHoldingPositionComponent>().Apply(_inputService.HoldingPosition());
                _contexts.Input.GetUnique<PointerHoldingTimeComponent>().Apply(_inputService.HoldingTime());
            }
        }
    }

}
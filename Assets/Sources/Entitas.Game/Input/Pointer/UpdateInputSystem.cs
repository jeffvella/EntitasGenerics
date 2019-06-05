using Entitas.Generics;
using System.Diagnostics;

namespace Entitas.MatchLine
{
    public sealed class UpdateInputSystem : IExecuteSystem
    {
        private readonly Contexts _contexts;
        private readonly InputEntity _inputUnique;
        private readonly IInputService _inputService;

        public UpdateInputSystem(Contexts contexts, IServices services)
        {
            _contexts = contexts;
            _inputUnique = _contexts.Input.Unique;
            _inputService = services.InputService;
        }

        public void Execute()
        {


            if (_contexts.GameState.Unique.IsFlagged<GameOverComponent>())
            {
                _inputUnique.SetFlag<PointerHoldingComponent>(false);
                _inputUnique.SetFlag<PointerStartedHoldingComponent>(false);
                _inputUnique.SetFlag<PointerReleasedComponent>(true);
            }
            else
            {
                var delta = _contexts.Input.Unique.Get<DeltaTimeComponent>().Value;
                _inputService.Update(delta);

                _inputUnique.SetFlag<PointerHoldingComponent>(_inputService.IsHolding());
                _inputUnique.SetFlag<PointerStartedHoldingComponent>(_inputService.IsStartedHolding());
                _inputUnique.SetFlag<PointerReleasedComponent>(_inputService.IsReleased());

                _inputUnique.GetAccessor<PointerHoldingPositionComponent>().Apply(_inputService.HoldingPosition());
                _inputUnique.GetAccessor<PointerHoldingTimeComponent>().Apply(_inputService.HoldingTime());
            }
        }
    }

}
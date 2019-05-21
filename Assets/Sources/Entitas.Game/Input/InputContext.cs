using Entitas.Generics;

namespace Entitas.MatchLine
{
    public class InputContext : GenericContext<InputContext, InputEntity>
    {
        public InputContext() : base(new InputContextDefinition()){ }
    }

    public class InputContextDefinition : ContextDefinition<InputContext, InputEntity>
    {
        public InputContextDefinition()
        {
            Add<PointerHoldingComponent>();
            Add<PointerHoldingPositionComponent>();
            Add<PointerHoldingTimeComponent>();
            Add<PointerReleasedComponent>();
            Add<PointerStartedHoldingComponent>();
            Add<RestartComponent>();
            Add<DeltaTimeComponent>();
            Add<RealtimeSinceStartupComponent>();
            Add<DestroyedComponent>();
        }
    }
}


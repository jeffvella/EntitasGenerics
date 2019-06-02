using Entitas.Generics;
using System;

namespace Entitas.MatchLine
{
    public class InputContext : GenericContext<InputContext, InputEntity>
    {
        public InputContext() : base(new InputContextDefinition()){ }
    }

    public class InputContextDefinition : ContextDefinition<InputContext, InputEntity>
    {
        public override Func<InputEntity> EntityFactory => () => new InputEntity();

        public InputContextDefinition()
        {
            AddComponent<PointerHoldingComponent>();
            AddComponent<PointerHoldingPositionComponent>();
            AddComponent<PointerHoldingTimeComponent>();
            AddComponent<PointerReleasedComponent>();
            AddComponent<PointerStartedHoldingComponent>();
            AddComponent<RestartComponent>();
            AddComponent<DeltaTimeComponent>();
            AddComponent<RealtimeSinceStartupComponent>();
            AddComponent<DestroyedComponent>();
        }
    }
}


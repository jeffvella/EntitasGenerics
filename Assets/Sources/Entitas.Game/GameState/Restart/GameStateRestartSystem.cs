using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class GameStateRestartSystem : GenericReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public GameStateRestartSystem(Contexts contexts) : base(contexts.Input, Trigger)
        {
            _contexts = contexts;
        }

        private static ICollector<InputEntity> Trigger(IGenericContext<InputEntity> context)
        {
            return context.GetTriggerCollector<RestartComponent>();
        }

        protected override void Execute(List<InputEntity> entities)
        {
            _contexts.GameState.ResetState();
        }

    }

}
using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ActionCounterSystem : GenericReactiveSystem<GameEntity>
    {
        private readonly IGenericContext<GameEntity> _game;
        private IGenericContext<GameStateEntity> _gameState;

        public ActionCounterSystem(Contexts contexts) : base(contexts.Game, Trigger)
        {
            _game = contexts.Game;
            _gameState = contexts.GameState;
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            return context.GetTriggerCollector<MatchedComponent>();
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var currentActionCount = _gameState.GetUnique<ActionCountComponent>().Component;
            _gameState.SetUnique<ActionCountComponent>(c => c.value = currentActionCount.value + 1);
        }
    }

}
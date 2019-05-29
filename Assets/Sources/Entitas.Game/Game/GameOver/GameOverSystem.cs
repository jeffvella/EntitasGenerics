using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{

    public sealed class GameOverSystem : GenericReactiveSystem<GameStateEntity>
    {
        private IGenericContext<ConfigEntity> _config;
        private IGenericContext<GameStateEntity> _gameState;

        public GameOverSystem(Contexts contexts) : base(contexts.GameState, Trigger)
        {
            _config = contexts.Config;
            _gameState = contexts.GameState;
        }

        private static ICollector<GameStateEntity> Trigger(IGenericContext<GameStateEntity> context)
        {
            return context.GetTriggerCollector<ActionCountComponent>();
        }

        protected override void Execute(List<GameStateEntity> entities)
        {
            var maxActions = _config.GetUnique<MaxActionCountComponent>().Component.Value;
            var actionCount = _gameState.GetUnique<ActionCountComponent>().Component.value;

            if (actionCount >= maxActions)
            {
                _gameState.SetFlag<GameOverComponent>();
            }

        }
    }
}
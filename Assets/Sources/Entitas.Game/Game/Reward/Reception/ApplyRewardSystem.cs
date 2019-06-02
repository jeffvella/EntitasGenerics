using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ApplyRewardSystem : GenericReactiveSystem<GameEntity>
    {
        private readonly IGenericContext<ConfigEntity> _config;
        private readonly IGenericContext<GameEntity> _game;
        private IGenericContext<GameStateEntity> _gameState;

        public ApplyRewardSystem(Contexts contexts) : base(contexts.Game, Trigger)
        {
            _game = contexts.Game;
            _gameState = contexts.GameState;
            _config = contexts.Config;
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            return context.GetTriggerCollector<RewardComponent>(GroupEvent.Added);
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var score = _gameState.Unique.GetComponent<ScoreComponent>().Value;

            var totalReward = 0;

            foreach (var entity in entities)
            {
                totalReward += entity.GetComponent<RewardComponent>().Value;

                entity.SetFlag<DestroyedComponent>();
            }

            //_gameState.SetUnique<ScoreComponent>(c => c.Value = score + totalReward);

            _gameState.Unique.Get<ScoreComponent>().Apply(score + totalReward);


        }
    }
}
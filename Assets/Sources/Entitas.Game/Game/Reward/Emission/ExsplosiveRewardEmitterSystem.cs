using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ExsplosiveRewardEmitterSystem : GenericReactiveSystem<GameEntity>
    {
        private readonly IGenericContext<ConfigEntity> _config;
        private readonly IGenericContext<GameEntity> _game;

        public ExsplosiveRewardEmitterSystem(Contexts contexts) : base(contexts.Game, Trigger, Filter)
        {
            _game = contexts.Game;
            _config = contexts.Config;
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            return context.GetTriggerCollector<MatchedComponent>(GroupEvent.Added);
        }

        private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
        {
            return context.IsFlagged<MatchedComponent>(entity) && context.IsFlagged<ExplosiveComponent>();
        }


        protected override void Execute(List<GameEntity> entities)
        {
            var table = _config.GetUnique<ExplosiveScoringTableComponent>().value;

            var scoreId = entities.Count;
            scoreId--;

            if (scoreId >= table.Count)
                scoreId = table.Count - 1;

            var reward = table[scoreId];

            var e = _game.CreateEntity();
            _game.Set(e, new RewardComponent { value = reward });
        }
    }
}
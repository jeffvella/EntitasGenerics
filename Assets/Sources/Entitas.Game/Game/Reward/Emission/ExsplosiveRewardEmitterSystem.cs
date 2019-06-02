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
            return entity.IsFlagged<MatchedComponent>() && context.Unique.IsFlagged<ExplosiveComponent>();
        }


        protected override void Execute(List<GameEntity> entities)
        {
            var table = _config.Unique.Get<ExplosiveScoringTableComponent>().Component.Value;

            var scoreId = entities.Count;
            scoreId--;

            if (scoreId >= table.Count)
                scoreId = table.Count - 1;

            var reward = table[scoreId];
  
            _game.CreateEntity().Get<RewardComponent>().Apply(reward);

            //_game.Get<RewardComponent>().
            //_game.Set(e, new RewardComponent { Value = reward });
        }
    }
}
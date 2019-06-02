using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ComboRewardEmitterSystem : GenericReactiveSystem<GameEntity>
    {

        private readonly IGenericContext<ConfigEntity> _config;
        private readonly IGenericContext<GameEntity> _game;

        public ComboRewardEmitterSystem(Contexts contexts) : base(contexts.Game, Trigger, Filter)
        {
            _game = contexts.Game;
            _config = contexts.Config;
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            return context.GetTriggerCollector<ComboComponent>(GroupEvent.Added);
        }

        private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
        {
            return entity.HasComponent<ComboComponent>();
        }


        protected override void Execute(List<GameEntity> entities)
        {
            var definitions = _config.Unique.Get<ComboDefinitionsComponent>().Component.Value;

            foreach (var entity in entities)
            {
                var combo = entity.GetComponent<ComboComponent>().Value;
                var definition = definitions.Definitions[combo];
                var e = _game.CreateEntity();
                e.Get<RewardComponent>().Apply(definition.Reward);
            }
        }
    }
}
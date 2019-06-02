using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class RemoveMatchedSystem : GenericReactiveSystem<GameEntity>
    {
        private readonly IGenericContext<GameEntity> _game;

        public RemoveMatchedSystem(Contexts contexts) : base(contexts.Game, Trigger, Filter)
        {
            _game = contexts.Game;
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            return context.GetTriggerCollector<MatchedComponent>();
        }

        private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
        {
            return !entity.IsFlagged<DestroyedComponent>();
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.SetFlag<DestroyedComponent>();
            }
        }
    }

}
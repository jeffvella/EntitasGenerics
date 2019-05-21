using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class GameRestartSystem : GenericReactiveSystem<InputEntity>
    {
        private readonly IGroup<GameEntity> _elementGroup;
        private readonly List<GameEntity> _buffer;
        private readonly IGenericContext<GameEntity> _game;

        public GameRestartSystem(Contexts contexts) : base(contexts.Input, Trigger)
        {
            _elementGroup = contexts.Game.GetGroup<ElementComponent>();
            _game = contexts.Game;
            _buffer = new List<GameEntity>();
        }

        private static ICollector<InputEntity> Trigger(IGenericContext<InputEntity> context)
        {
            return context.GetTriggerCollector<RestartComponent>();
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var entity in _elementGroup.GetEntities(_buffer))
            {
                _game.SetFlag<DestroyedComponent>(entity, true);
            }

            var e = _game.CreateEntity();
            _game.SetFlag<RestartHappenedComponent>(e, true);
        }
    }
}
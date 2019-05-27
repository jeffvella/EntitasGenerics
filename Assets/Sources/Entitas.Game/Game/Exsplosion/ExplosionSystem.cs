using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ExplosionSystem : GenericReactiveSystem<GameEntity>
    {
        private readonly List<GameEntity> _buffer;
        private readonly List<GridPosition> _positionBuffer;
        private readonly IGroup<GameEntity> _explosiveGroup;
        private readonly IGenericContext<GameEntity> _game;

        public ExplosionSystem(Contexts contexts) : base(contexts.Game, Trigger)
        {
            _explosiveGroup = contexts.Game.GetGroup<ExplosiveComponent>();
            _game = contexts.Game;
            _buffer = new List<GameEntity>();
            _positionBuffer = new List<GridPosition>();
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            return context.GetTriggerCollector<MatchedComponent>(GroupEvent.Added);
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in _explosiveGroup.GetEntities(_buffer))
            {
                bool found = false;

                GridPosition position = _game.Get<PositionComponent>(entity).Value;

                var neighbourPositions = position.GetNeighbours(_positionBuffer);

                foreach (var neighbourPosition in neighbourPositions)
                {
                    foreach (var matchedEntity in entities)
                    {
                        if (_game.TryGet(matchedEntity, out PositionComponent component))
                        {
                            if (neighbourPosition.Equals(component.Value))
                            {
                                found = true;
                                _game.SetFlag<MatchedComponent>(entity, true);
                                break;
                            }
                        }
                    }

                    if (found)
                        break;
                }
            }
        }
    }
}
using System.Collections.Generic;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;

public sealed class ExplosionSystem : GenericReactiveSystem<GameEntity>
{
    private readonly List<GameEntity> _buffer;
    private readonly List<GridPosition> _positionBuffer;
    private readonly IGroup<GameEntity> _explosiveGroup;
    private readonly IGenericContext<GameEntity> _game;

    public ExplosionSystem(Contexts contexts) : base(contexts.Game, Trigger)
    {
        //_exsplosives = contexts.game.GetGroup(GameMatcher.Exsplosive);

        _explosiveGroup = contexts.Game.GetGroup<ExplosiveComponent>();
        _game = contexts.Game;
        _buffer = new List<GameEntity>();
        _positionBuffer = new List<GridPosition>();
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetTriggerCollector<MatchedComponent>(GroupEvent.Added);
    }

    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.Matched.Added());
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in _explosiveGroup.GetEntities(_buffer))
        {
            bool found = false;

            //var positions = exsplosiveEntity.position.value.GetNeighbours(_positionBuffer);

            GridPosition position = _game.Get<PositionComponent>(entity).value;

            var neighbourPositions = position.GetNeighbours(_positionBuffer);

            foreach (var neighbourPosition in neighbourPositions)
            {
                foreach (var matchedEntity in entities)
                {
                    if (_game.TryGet(matchedEntity, out PositionComponent component))
                    {                    
                        if (neighbourPosition.Equals(component.value))
                        {
                            found = true;
                            _game.SetFlag<MatchedComponent>(entity, true);        
                            break;
                        }
                    }

                    //if (matchedEntity.hasPosition)
                    //{
                    //    if (neighbourPosition.Equals(matchedEntity.position.value))
                    //    {
                    //        found = true;
                    //        entity.isMatched = true;
                    //        break;
                    //    }
                    //}
                }

                if (found)
                    break;
            }
        }
    }
}
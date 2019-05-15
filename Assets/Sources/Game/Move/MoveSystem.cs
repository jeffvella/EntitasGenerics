using Assets.Sources.Config;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;
using UnityEngine.UIElements;
using ConfigContext = Assets.Sources.Config.ConfigContext;
using GameContext = Assets.Sources.Game.GameContext;

public sealed class MoveSystem : IExecuteSystem
{
    private IGenericContext<ConfigEntity> _config;
    private IGenericContext<GameEntity> _game;

    public MoveSystem(Contexts contexts)
    {
        _config = contexts.Config;
        _game = contexts.Game;
    }

    public void Execute()
    {
        int moveCount = 0;

        //var size = _contexts.config.mapSize.value;
        var size = _config.GetUnique<MapSizeComponent>().value;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 1; y < size.y; y++)
            {
                GridPosition position = new GridPosition(x, y);

                if(!_game.TryFindEntity<PositionComponent, GridPosition>(position, out GameEntity element))
                {
                    continue;
                }

                //GameEntity element = _game.FindEntity<PositionComponent, GridPosition>(position);
                
                //if(element == null)
                //    continue;

                //if (!element.isMovable)
                //    continue;
        
                if (!_game.IsFlagged<MovableComponent>(element))
                    continue;

                var targetPosition = new GridPosition(x, y - 1);
                if (!_game.TryFindEntity<PositionComponent, GridPosition>(targetPosition, out GameEntity targetEntity))
                {
                    //_game.Set(element, new PositionComponent { value = targetPosition });
                    _game.Set<PositionComponent>(element, c => c.value = targetPosition);
                    moveCount++;
                }

                //var targetEntity = _game.FindEntity<PositionComponent, GridPosition>(targetPosition);
                //if (targetEntity == null)
                //{
                //  _game.Set(element, new PositionComponent { value = targetPosition });
                //    //element.ReplacePosition(targetPosition);
                //    moveCount++;
                //}
            }
        }

        if (moveCount > 0)
        {
            var e = _game.CreateEntity();
            _game.SetFlag<FieldMovedComponent>(e);
            _game.SetFlag<DestroyedComponent>(e);

            //e.isFieldMoved = true;
            //e.isDestroyed = true;
        }
    }
}

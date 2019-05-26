using Entitas.Generics;
using UnityEngine;

namespace Entitas.MatchLine
{
    /// <summary>
    /// Moves blocks downwards to fill any gaps that have opened up beneath them.
    /// </summary>
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

            var size = _config.GetUnique<MapSizeComponent>().Value;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 1; y < size.y; y++)
                {
                    var sourcePosition = new GridPosition(x, y);

                    if (!_game.TryFindEntity<PositionComponent>(c => c.value = sourcePosition, out GameEntity element))
                    {
                        continue;
                    }

                    //if (!_game.TryFindEntity<PositionComponent, GridPosition>(sourcePosition, out GameEntity element))
                    //{
                    //    //if (_game.TryFindEntity2<PositionComponent>(c => c.value = sourcePosition, out GameEntity element2))
                    //    //{
                    //    //    Debug.Log($"Index found entity erroneously linked to position: {sourcePosition}, Element={element2}");                    
                    //    //}
                    //    continue;
                    //}
                    //else
                    //{
                    //    if (!_game.TryFindEntity2<PositionComponent>(c => c.value = sourcePosition, out GameEntity element2))
                    //    {
                    //        Debug.Log($"Index failed to find element by value, Position={sourcePosition}, Element={element}");
                    //    }
                    //}
                    //if (!_game.TryFindEntity2<PositionComponent>(c => c.value = sourcePosition, out GameEntity element))
                    //{
                    //    continue;
                    //}

                    if (!_game.IsFlagged<MovableComponent>(element))
                        continue;

                    var targetPosition = new GridPosition(x, y - 1);

                    if (!_game.EntityWithComponentValueExists<PositionComponent>(c => c.value = targetPosition))
                    {
                        _game.Set<PositionComponent>(element, c => c.value = targetPosition);
                        moveCount++;
                    }

                    //if (!_game.TryFindEntity<PositionComponent, GridPosition>(targetPosition, out GameEntity targetEntity))
                    //{
                    //    _game.Set<PositionComponent>(element, c => c.value = targetPosition);
                    //    moveCount++;
                    //}
                }
            }

            if (moveCount > 0)
            {
                var e = _game.CreateEntity();
                _game.SetFlag<FieldMovedComponent>(e);
                _game.SetFlag<DestroyedComponent>(e);
            }
        }
    }

}
using System.Diagnostics;
using Entitas.Generics;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
            //UnityEngine.Profiling.Profiler.BeginSample("SomeUniqueName");

            int moveCount = 0;

            var size = _config.Unique.Get<MapSizeComponent>().Component.Value;



            for (int x = 0; x < size.x; x++)
            {
                for (int y = 1; y < size.y; y++)
                {
                    var sourcePosition = new GridPosition(x, y);
                     
                    // If the spot is empty ignore it.

                    if (!_game.TryFindEntity<PositionComponent, GridPosition>(sourcePosition, out var element))
                    {          
                        continue;
                    }

                    if (!element.IsFlagged<MovableComponent>())
                    {
                        continue;
                    }

                    var targetPosition = new GridPosition(x, y - 1);

                    // Check if it can be moved to the target position 1 below its current position.

                    if (!_game.TryFindEntity<PositionComponent, GridPosition>(targetPosition, out var result))
                    {

                        //element.Get<PositionComponent>().Value = targetPosition;

                        //element.Get2<PositionComponent>().Set(targetPosition);

                        //element.Get<PositionComponent>(_game).Apply(targetPosition);

                        //Debug.Log($"Moved {sourcePosition} => {targetPosition}");

                        element.Get<PositionComponent>().Apply(targetPosition);


                        //var accessor = element.Find<PositionComponent>();
                        //accessor.Component.Value = targetPosition;
                        //accessor.Apply();


                        //test.Update(targetPosition);


                        //var test = element.With<PositionComponent>(); //.Update(targetPosition);                        

                        //_game.Set<PositionComponent>(element, c => c.Value = targetPosition);

                        moveCount++;
                    }

                    //if (!_game.TryFindEntity<PositionComponent>(c => c.Value = targetPosition, out var result))
                    //{
                    //    _game.Set<PositionComponent>(element, c => c.Value = targetPosition);
                    //    moveCount++;
                    //}

                    //if (!_game.TryFindEntity<PositionComponent, GridPosition>(targetPosition, out GameEntity targetEntity))
                    //{
                    //    _game.Set<PositionComponent>(element, c => c.value = targetPosition);
                    //    moveCount++;
                    //}
                }
            }


            _game.Unique.SetFlag<FieldMovedComponent>(moveCount > 0);

            //UnityEngine.Profiling.Profiler.EndSample();

        }

        //private void Method(ref PositionComponent item)
        //{            
        //    item.Value = new GridPosition();
        //}
    }

}
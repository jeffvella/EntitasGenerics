﻿using System.Diagnostics;
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
            UnityEngine.Profiling.Profiler.BeginSample("SomeUniqueName");

            int moveCount = 0;

            var size = _config.GetUnique<MapSizeComponent>().Value;

            

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 1; y < size.y; y++)
                {
                    var sourcePosition = new GridPosition(x, y);
                   
                    //var sw = new Stopwatch();
                    //sw.Start();

                    if (!_game.TryFindEntity2<PositionComponent, GridPosition>(sourcePosition, out var element))
                    {
                        //sw.Stop();
                        //Debug.Log($"Index Time: {sw.Elapsed.TotalMilliseconds:N6}");               
                        continue;
                    }



                    if (!_game.IsFlagged<MovableComponent>(element))
                    {
                        continue;
                    }

                    var targetPosition = new GridPosition(x, y - 1);

                    if (!_game.TryFindEntity2<PositionComponent, GridPosition>(targetPosition, out var result))
                    {

                        //element.Get<PositionComponent>().Value = targetPosition;

                        element.Get<PositionComponent>().Update(targetPosition);

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


            _game.SetFlag<FieldMovedComponent>(moveCount > 0);

            UnityEngine.Profiling.Profiler.EndSample();

        }

        //private void Method(ref PositionComponent item)
        //{            
        //    item.Value = new GridPosition();
        //}
    }

}
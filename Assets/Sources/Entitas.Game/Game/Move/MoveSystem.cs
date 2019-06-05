using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Entitas.Generics;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using Vella.Common;
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

        private NativePrimaryEntityIndex<GameEntity, PositionComponent> _index;

        public MoveSystem(Contexts contexts)
        {
            _config = contexts.Config;
            _game = contexts.Game;
            _index = _game.GetEntityIndex<PositionComponent>();
        }

        [BurstCompile]
        public struct GetMovableSpaces : IBurstAction<NativeFasterDictionary<PositionComponent, GCHandle>, GridSize, NativeList<GetMovableSpaces.MovablePositionResult>>
        {  
            public static (GameEntity Entity,GridPosition TargetPosition)[] Invoke(NativeFasterDictionary<PositionComponent, GCHandle> index, GridSize size)
            {                
                var buffer = new NativeList<MovablePositionResult>(index.Length, Allocator.TempJob);
                BurstAction<GetMovableSpaces, NativeFasterDictionary<PositionComponent, GCHandle>, GridSize, NativeList<MovablePositionResult>>.Run(Instance, index, size, buffer);

                var results = new (GameEntity, GridPosition)[buffer.Length];
                for (int i = 0; i < results.Length; i++)
                {
                    var result = buffer[i];
                    results[i] = (result.Handle.Target as GameEntity, result.TargetPosition);
                }
                buffer.Dispose();
                return results;
            }

            public struct MovablePositionResult
            {
                public GCHandle Handle;
                public GridPosition TargetPosition;
            }

            public void Execute(NativeFasterDictionary<PositionComponent, GCHandle> index, GridSize size, NativeList<MovablePositionResult> results)
            {
                var sourcePosition = new PositionComponent { Value = new GridPosition() };
                var targetPosition = new PositionComponent { Value = new GridPosition() };

                for (int x = 0; x < size.x; x++)
                {
                    for (int y = 1; y < size.y; y++)
                    {
                        sourcePosition.Value = new GridPosition(x,y);        
                        if (!index.TryFindIndex(sourcePosition, out int sourceValueIndex))
                        {
                            continue;
                        }

                        targetPosition.Value = new GridPosition(x, y - 1);
                        if (!index.TryFindIndex(targetPosition, out int targetValueIndex))
                        {
                            GCHandle handle = index.GetValue(sourceValueIndex);
                            results.Add(new MovablePositionResult
                            {
                                Handle = handle,
                                TargetPosition = targetPosition.Value
                            });
                        }
                    }
                }
            }

            //public void Execute(ref NativeList<IntPtr> arg1, NativeFasterDictionary<PositionComponent, GCHandle> arg2, GridSize arg3)
            //{
            //    throw new NotImplementedException();
            //}

            public static GetMovableSpaces Instance { get; } = new GetMovableSpaces();
        }

        public void Execute()
        {
            //UnityEngine.Profiling.Profiler.BeginSample("SomeUniqueName");

            int moveCount = 0;
            var size = _config.Unique.Get<MapSizeComponent>().Component.Value;

            var movables = GetMovableSpaces.Invoke(_index._index, size);
            for (int i = 0; i < movables.Length; i++)
            {
                var movable = movables[i];   
                if (movable.Entity != null && movable.Entity.IsFlagged<MovableComponent>())
                {
                    movable.Entity.Set(new PositionComponent { Value = movable.TargetPosition });
                    moveCount++;
                }
            }

            //for (int x = 0; x < size.x; x++)
            //{
            //    for (int y = 1; y < size.y; y++)
            //    {
            //        var sourcePosition = new GridPosition(x, y);

            //        // If the spot is empty ignore it.


            //        if(!PositionComponent.TryFindEntity(PositionComponent, _index, new PositionComponent { Value = sourcePosition }, out var element))
            //        {
            //            continue;
            //        }

            //        //return FindEntityJob<PositionComponent, GameEntity>.TryFindEntity(_game.GetEntityIndex(, key, out var entity);

            //        //if (!_game.TryFindEntity3(key, out var element))
            //        //{          
            //        //    continue;
            //        //}

            //        if (!element.IsFlagged<MovableComponent>())
            //        {
            //            continue;
            //        }

            //        var targetPosition = new GridPosition(x, y - 1);

            //        // Check if it can be moved to the target position 1 below its current position.

            //        if (!PositionComponent.TryFindEntity(PositionComponent, _index, new PositionComponent { Value = targetPosition }, out var result))
            //        {
            //            element.Set(new PositionComponent { Value = targetPosition });
            //            moveCount++;
            //        }

            //        //if (!_game.TryFindEntity3(new PositionComponent { Value = targetPosition }, out var result))
            //        //{
            //        //    element.Set(new PositionComponent { Value = targetPosition });
            //        //    moveCount++;
            //        //}

            //    }
            //}


            _game.Unique.SetFlag<FieldMovedComponent>(moveCount > 0);

            //UnityEngine.Profiling.Profiler.EndSample();

        }

        //private void Method(ref PositionComponent item)
        //{            
        //    item.Value = new GridPosition();
        //}
    }

}
using System.Collections.Generic;
using Entitas.Generics;
using UnityEngine;

namespace Entitas.MatchLine
{
    /// <summary>
    /// When a position on the board becomes empty, this system creates a new element to fill that spot.
    /// </summary>
    public sealed class AddElementsSystem : GenericReactiveSystem<GameEntity>
    {
        private readonly IElementService _elementService;
        private readonly IGenericContext<GameEntity> _game;
        private readonly IGenericContext<ConfigEntity> _config;

        public AddElementsSystem(Contexts contexts, IServices services) : base(contexts.Game, Trigger)
        {
            _elementService = services.ElementService;
            _game = contexts.Game;
            _config = contexts.Config;
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            var t1 = context.GetTrigger<ElementComponent>(GroupEvent.Removed);
            var t2 = context.GetTrigger<PositionComponent>(GroupEvent.AddedOrRemoved);
            //var t2 = context.GetTrigger<PositionComponent>(GroupEvent.Removed);
            return context.CreateCollector(t1, t2);
        }

        protected override void Execute(List<GameEntity> entities)
        {
            GridSize size = _config.Unique.Get<MapSizeComponent>().Component.Value;

            //foreach (var entity in entities)
            //{
            //    if (entity.HasComponent<PositionComponent>())
            //    {
            //        Debug.Log($"AddElementSystem Collected: Position={entity.Get<PositionComponent>().value}");
            //    }
            //}

            // _game.CreateIndex<PositionComponent>((e,c) => c.Value);

            for (int x = 0; x < size.x; x++)
            {
                var position = new GridPosition(x, size.y - 1);

                //if (!_game.TryFindEntity<PositionComponent>(p => p.Value = position, out var candidate))
                //{
                //    //Debug.Log($"AddElementSystem Index Not Found for position {position}, Adding");

                //    _elementService.CreateRandomElement(position);
                //}

                //var entity = _game.GetSearchIndex<PositionComponent>().FindEntity(position);

                //query.Add

                //_game.GetEntityIndex<PositionComponent>().WithValue();

                if (!_game.TryFindEntity<PositionComponent, GridPosition>(position, out var candidate))
                {
                    Debug.Log($"AddElementSystem Index Not Found for position {position} - Adding");

                    _elementService.CreateRandomElement(position);
                }

                //if (!_game.TryFindEntity2<PositionComponent>(p => p.value = position, out var candidate))
                //if (!_game.TryFindEntity<PositionComponent, GridPosition>(position, out var candidate))
                //{
                //    Debug.Log($"AddElementSystem Index Not Found for position {position}, Adding");

                //    _elementService.CreateRandomElement(position);
                //}
                //else
                //{
                //    Debug.Log($"AddElementSystem Index Found: Position={position}");
                //}
            }
        }
    }
}

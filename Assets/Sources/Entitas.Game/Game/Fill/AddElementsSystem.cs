using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
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
            return context.CreateCollector(t1, t2);
        }


        protected override void Execute(List<GameEntity> entities)
        {
            GridSize size = _config.GetUnique<MapSizeComponent>().Value;

            for (int x = 0; x < size.x; x++)
            {
                var position = new GridPosition(x, size.y - 1);
                if (!_game.TryFindEntity<PositionComponent, GridPosition>(position, out var candidate))
                {
                    _elementService.CreateRandomElement(position);
                }
            }
        }
    }
}

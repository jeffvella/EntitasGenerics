using System.Collections.Generic;
using Entitas;
using Entitas.Generics;
using GameContext = Assets.Sources.Game.GameContext;

public sealed class AddElementsSystem : GenericReactiveSystem<GameEntity>
{
    private readonly ElementService _elementService;
    private readonly IGenericContext<GameEntity> _game;
    private readonly IGenericContext<ConfigEntity> _config;

    public AddElementsSystem(GenericContexts contexts, Services services) : base(contexts.Game, Trigger)
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

    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.Element.Removed(), GameMatcher.Position.AddedOrRemoved());
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        //var size = _contexts.config.mapSize.value;
        //GridSize size = _genericContexts.Config.Get<MapSizeComponent,GridSize>();
        GridSize size = _config.GetUnique<MapSizeComponent>().value;

        for (int x = 0; x < size.x; x++)
        {
            var position = new GridPosition(x, size.y - 1);
            //var candidate = _contexts.game.GetEntityWithPosition(position);

            //var candidate = _game.FindEntity<PositionComponent, GridPosition>(position);
            //if (candidate == null)
            //{
            //    _elementService.CreateRandomElement(position);
            //}
            if(!_game.TryFindEntity<PositionComponent, GridPosition>(position, out var candidate))
            {
                _elementService.CreateRandomElement(position);
            }
        }
    }
}
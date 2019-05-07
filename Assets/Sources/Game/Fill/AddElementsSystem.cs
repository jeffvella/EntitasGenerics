using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class AddElementsSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly ElementService _elementService;
    private GenericContexts _genericContexts;

    public AddElementsSystem(Contexts contexts, GenericContexts genericContexts, Services services) : base(contexts.game)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
        _elementService = services.ElementService;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Element.Removed(), GameMatcher.Position.AddedOrRemoved());
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        //var size = _contexts.config.mapSize.value;
        //GridSize size = _genericContexts.Config.Get<MapSizeComponent,GridSize>();
        GridSize size = _genericContexts.Config.GetUnique<MapSizeComponent>().value;

        for (int x = 0; x < size.x; x++)
        {
            var position = new GridPosition(x, size.y - 1);
            var candidate = _contexts.game.GetEntityWithPosition(position);

            if (candidate == null)
            {
                _elementService.CreateRandomElement(position);
            }
        }
    }
}
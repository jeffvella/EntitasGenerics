using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class ViewSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;
    private readonly IViewService _viewService;

    public ViewSystem(Contexts contexts, GenericContexts genericContexts, Services services) : base(contexts.game)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
        _viewService = services.ViewService;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Asset.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAsset && !entity.isAssetLoaded;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            _viewService.LoadAsset(_contexts, _genericContexts, entity, entity.asset.value);
            entity.isAssetLoaded = true;
        }
    }
}
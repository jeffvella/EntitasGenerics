using System.Collections.Generic;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;
using UnityEditor.VersionControl;

public sealed class ViewSystem : GenericReactiveSystem<GameEntity>
{
    private readonly GenericContexts _contexts;
    private readonly IViewService _viewService;

    public ViewSystem(GenericContexts contexts, Services services) : base(contexts.Game, Trigger, Filter)
    {
        _contexts = contexts;
        _viewService = services.ViewService;
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetTriggerCollector<AssetComponent>();
    }

    private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
    {
        return context.HasComponent<AssetComponent>(entity) && !context.IsFlagged<AssetLoadedComponent>();
    }

    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.Asset.Added());
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return entity.hasAsset && !entity.isAssetLoaded;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            //_viewService.LoadAsset(_contexts, _genericContexts, entity, entity.asset.value);

            var assetName = _contexts.Game.Get<AssetComponent>(entity).value;

            _viewService.LoadAsset(_contexts, entity, assetName);

            _contexts.Game.SetFlag<AssetLoadedComponent>(entity, true);

            //entity.isAssetLoaded = true;
        }
    }
}

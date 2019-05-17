using System.Collections.Generic;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;
using UnityEditor.VersionControl;

public sealed class ViewSystem : GenericReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly IViewService _viewService;

    public ViewSystem(Contexts contexts, Services services) : base(contexts.Game, Trigger, Filter)
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
        return context.Has<AssetComponent>(entity) && !context.IsFlagged<AssetLoadedComponent>();
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var assetName = _contexts.Game.Get<AssetComponent>(entity).value;     
            _viewService.LoadAsset<GameEntity>(_contexts, entity, assetName);
            _contexts.Game.SetFlag<AssetLoadedComponent>(entity, true);
        }
    }
}

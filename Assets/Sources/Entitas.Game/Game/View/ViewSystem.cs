using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class ViewSystem : GenericReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
        private readonly IViewService _viewService;

        public ViewSystem(Contexts contexts, IServices services) : base(contexts.Game, Trigger, Filter)
        {
            _contexts = contexts;
            _viewService = services.ViewService;
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            return context.GetTriggerCollector<AssetComponent>();
        }

        private static bool Filter(IGenericContext<GameEntity> gameContext, GameEntity entity)
        {
            return entity.Has<AssetComponent>() && !gameContext.Unique.IsFlagged<AssetLoadedComponent>();
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                var assetComponent = entity.Get<AssetComponent>();
                var assetName = assetComponent.Value;
                var assetId = assetComponent.id;
                _viewService.LoadAsset(_contexts, entity, assetName, assetId);
            }

            foreach (var entity in entities)
            {
                entity.SetFlag<AssetLoadedComponent>(true);
            }
        }
    }

}
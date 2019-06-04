using Entitas.Generics;
using System;

namespace Entitas.MatchLine
{
    public class GameContext : GenericContext<GameContext, GameEntity>
    {
        public GameContext() : base(new GameContextDefinition())
        {
            AddIndex<IdComponent>();
            AddIndexStruct<PositionComponent>();
            AddIndex<SelectionIdComponent>();
        }
    }

    public class GameContextDefinition : ContextDefinition<GameContext, GameEntity>
    {
        public override Func<GameEntity> EntityFactory => () => new GameEntity();

        public GameContextDefinition()
        {
            AddComponent<AssetComponent>();
            AddComponent<AssetLoadedComponent>();
            AddComponent<BlockComponent>();
            AddComponent<ColorComponent>();
            AddComponent<ComboComponent>();
            AddComponent<DestroyedComponent>();
            AddComponent<ElementComponent>();
            AddComponent<ElementTypeComponent>();
            AddComponent<ExplosiveComponent>();
            AddComponent<FieldMovedComponent>();
            AddComponent<IdComponent>();
            AddComponent<InComboComponent>();
            AddComponent<MatchedComponent>();
            AddComponent<MovableComponent>();
            AddComponentStruct<PositionComponent>();
            AddComponent<RestartHappenedComponent>();
            AddComponent<RewardComponent>();
            AddComponent<SelectedComponent>();
            AddComponent<SelectionIdComponent>();
        }


    } 
}

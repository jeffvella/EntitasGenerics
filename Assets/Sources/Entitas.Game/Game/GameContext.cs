using Entitas.Generics;

namespace Entitas.MatchLine
{
    public class GameContext : GenericContext<GameContext, GameEntity>
    {
        public GameContext() : base(new GameContextDefinition()) { }
    }

    public class GameContextDefinition : ContextDefinition<GameContext, GameEntity>
    {
        public GameContextDefinition()
        {
            Add<AssetComponent>();
            Add<AssetLoadedComponent>();
            Add<BlockComponent>();
            Add<ColorComponent>();
            Add<ComboComponent>();
            Add<DestroyedComponent>();
            Add<ElementComponent>();
            Add<ElementTypeComponent>();
            Add<ExplosiveComponent>();
            Add<FieldMovedComponent>();
            Add<IdComponent>();
            Add<InComboComponent>();
            Add<MatchedComponent>();
            Add<MovableComponent>();
            Add<PositionComponent>();
            Add<RestartHappenedComponent>();
            Add<RewardComponent>();
            Add<SelectedComponent>();
            Add<SelectionIdComponent>();
        }
    } 
}

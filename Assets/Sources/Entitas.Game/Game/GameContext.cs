using Entitas.Generics;

namespace Entitas.MatchLine
{
    public class GameContext : GenericContext<GameContext, GameEntity>
    {
        public GameContext() : base(new GameContextDefinition())
        {
            AddIndex<IdComponent>();
            AddIndex<PositionComponent>();
            AddIndex<SelectionIdComponent>();
        }
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
            //AddIndexed<IdComponent>();
            Add<InComboComponent>();
            Add<MatchedComponent>();
            Add<MovableComponent>();
            Add<PositionComponent>();
            //AddIndexed<PositionComponent>();
            Add<RestartHappenedComponent>();
            Add<RewardComponent>();
            Add<SelectedComponent>();
            AddIndexed<SelectionIdComponent>();
            //AddIndexed<SelectionIdComponent>();
        }
    } 
}

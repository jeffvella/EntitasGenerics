using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas.Generics;

namespace Assets.Sources.Game
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
            Add<ColorListenerComponent>();
            Add<ComboComponent>();
            Add<DestroyedComponent>();
            Add<ElementComponent>();
            Add<ElementTypeComponent>();
            Add<ExplosiveComponent>();
            Add<FieldMovedComponent>();
            Add<GameDestroyedListenerComponent>();
            Add<IdComponent>();
            Add<InComboComponent>();
            Add<MatchedComponent>();
            Add<MovableComponent>();
            Add<PositionComponent>();
            Add<PositionListenerComponent>();
            Add<RestartHappenedComponent>();
            Add<RewardComponent>();
            Add<SelectedComponent>();
            Add<SelectedListenerComponent>();
            Add<SelectedRemovedListenerComponent>();
            Add<SelectionIdComponent>();
        }
    }
}


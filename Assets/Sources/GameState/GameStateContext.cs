using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas.Generics;

namespace Assets.Sources.GameState
{
    public class GameStateContext : GenericContext<GameStateContext, GameStateEntity>
    {
        public GameStateContext() : base(new GameStateContextDefinition()) { }
    }

    public class GameStateContextDefinition : ContextDefinition<GameStateContext, GameStateEntity>
    {
        public GameStateContextDefinition()
        {
            Add<ActionCountComponent>();
            Add<GameOverComponent>();
            Add<LastSelectedComponent>();
            Add<MaxSelectedElementComponent>();
            Add<ScoreComponent>();
        }
    }
}



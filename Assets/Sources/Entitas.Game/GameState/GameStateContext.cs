using System;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public class GameStateContext : GenericContext<GameStateContext, GameStateEntity>
    {
        public GameStateContext() : base(new GameStateContextDefinition())
        {
            AddIndex<ScoreComponent>();
        }
    }

    public class GameStateContextDefinition : ContextDefinition<GameStateContext, GameStateEntity>
    {
        public override Func<GameStateEntity> EntityFactory => () => new GameStateEntity();

        public GameStateContextDefinition()
        {
            AddComponent<ActionCountComponent>();
            AddComponent<GameOverComponent>();
            AddComponent<LastSelectedComponent>();
            AddComponent<MaxSelectedElementComponent>();
            AddComponent<ScoreComponent>();
        }
    }
}



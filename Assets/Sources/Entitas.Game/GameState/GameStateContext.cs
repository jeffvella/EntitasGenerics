using Entitas.Generics;

namespace Entitas.MatchLine
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
            AddIndexed<ScoreComponent>();
        }
    }
}



using Entitas.Generics;

namespace Entitas.MatchLine
{
    public class Contexts : IContexts
    {
        public static Contexts Instance => _instance ?? (_instance = new Contexts());
        private static Contexts _instance;

        public IContext[] allContexts { get; }

        private Contexts()
        {
            allContexts = new IContext[]
            {
                Config, Input, GameState, Game
            };
        }

        public readonly IGenericContext<ConfigEntity> Config = new ConfigContext();

        public readonly IGenericContext<InputEntity> Input = new InputContext();

        public readonly IGenericContext<GameStateEntity> GameState = new GameStateContext();

        public readonly IGenericContext<GameEntity> Game = new GameContext();


    }
}
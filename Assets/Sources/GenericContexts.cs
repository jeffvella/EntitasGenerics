using System.Runtime.CompilerServices;
using Assets.Sources.Config;
using Assets.Sources.Game;
using Assets.Sources.GameState;
using GameStateContext = Assets.Sources.GameState.GameStateContext;

namespace Entitas.Generics
{
    public class GenericContexts
    {
        private GenericContexts() { }

        public static GenericContexts Instance => _instance ?? (_instance = new GenericContexts());
        private static GenericContexts _instance;

        public LoggingContext Logging = new LoggingContext();

        public IGenericContext<ConfigEntity> Config = new ConfigContext();

        public IGenericContext<InputEntity> Input = new InputContext();

        public IGenericContext<GameStateEntity> GameState = new GameStateContext();

        public IGenericContext<GameEntity> Game = new GameContext();

    }
}

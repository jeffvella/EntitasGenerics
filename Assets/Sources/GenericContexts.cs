using System.Runtime.CompilerServices;
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

        public IGenericContext<ConfigEntity> Config = new Assets.Sources.Config.ConfigContext();

        public IGenericContext<InputEntity> Input = new Assets.Sources.Config.InputContext();

        public IGenericContext<GameStateEntity> GameState = new Assets.Sources.GameState.GameStateContext();

        public IGenericContext<GameEntity> Game = new Assets.Sources.Game.GameContext();

    }
}

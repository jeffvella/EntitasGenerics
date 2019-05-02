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

        public Assets.Sources.Config.ConfigContext Config = new Assets.Sources.Config.ConfigContext();

        public Assets.Sources.Config.InputContext Input = new Assets.Sources.Config.InputContext();

        public Assets.Sources.GameState.GameStateContext GameState = new Assets.Sources.GameState.GameStateContext();

        public Assets.Sources.Game.GameContext Game = new Assets.Sources.Game.GameContext();

    }
}

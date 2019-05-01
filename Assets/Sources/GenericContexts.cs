using System.Runtime.CompilerServices;

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
        
    }
}
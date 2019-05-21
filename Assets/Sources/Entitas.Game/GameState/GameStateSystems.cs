namespace Entitas.MatchLine
{
    public class GameStateSystems : Feature
    {
        public GameStateSystems(Contexts contexts, IServices services)
        {
            Add(new InitStateSystem(contexts));
            Add(new GameStateRestartSystem(contexts));
        }
    }
}
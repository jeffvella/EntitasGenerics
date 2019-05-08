using Entitas.Generics;

public class GameStateSystems : Feature
{
    public GameStateSystems(GenericContexts contexts, Services services)
    {
        Add(new InitStateSystem(contexts));
        Add(new GameStateRestartSystem(contexts));
    }
}
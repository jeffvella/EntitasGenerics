using EntitasGenerics;

public class GameStateSystems : Feature
{
    public GameStateSystems(Contexts contexts, GenericContexts genericContexts, Services services)
    {
        Add(new InitStateSystem(contexts));
        Add(new GameStateRestartSystem(contexts));
    }
}
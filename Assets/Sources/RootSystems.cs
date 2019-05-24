using Entitas.Generics;
using Entitas.MatchLine;

public class RootSystems : Feature
{
    public RootSystems(Contexts contexts, IServices services)
    {
        Add(new InputSystems(contexts, services));
        Add(new GameStateSystems(contexts, services));
        Add(new GameStateEventSystems(contexts));
        Add(new GameSystems(contexts, services));
        Add(new GameEventSystems(contexts));
    }
}
 
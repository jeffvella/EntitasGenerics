using EntitasGenerics;
using UnityEngine;

public class RootSystems : Feature
{
    public RootSystems(Contexts contexts, GenericContexts genericContexts, Services services)
    {
        Add(new InputSystems(contexts, services));

        Add(new GameStateSystems(contexts, services));
        Add(new GameStateEventSystems(contexts));

        Add(new GameSystems(contexts, genericContexts, services));
        Add(new GameEventSystems(contexts));

        Add(new DebugMessageSystem(genericContexts, services));
    }
}


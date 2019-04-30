using EntitasGenerics;
using UnityEngine;

public class RootSystems : Feature
{
    public RootSystems(Contexts contexts, GenericContexts genericContexts, Services services)
    {
        Add(new InputSystems(contexts, genericContexts, services));

        Add(new GameStateSystems(contexts, genericContexts, services));
        Add(new GameStateEventSystems(contexts, genericContexts));

        Add(new GameSystems(contexts, genericContexts, services));
        Add(new GameEventSystems(contexts, genericContexts));

        Add(new DebugMessageSystem(genericContexts, services));
    }
}


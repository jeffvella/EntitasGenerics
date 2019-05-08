using Entitas;
using Entitas.Generics;

public sealed class InitStateSystem : IInitializeSystem
{
    private readonly GenericContexts _contexts;

    public InitStateSystem(GenericContexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        _contexts.GameState.ResetState();
    }
}
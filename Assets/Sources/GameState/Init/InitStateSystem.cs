using Entitas;

public sealed class InitStateSystem : IInitializeSystem
{
    private readonly Contexts _contexts;

    public InitStateSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        _contexts.GenericTemp.GameState.ResetState();

        _contexts.gameState.ResetState();
    }
}
using Entitas;
using Entitas.Generics;

public sealed class UpdateTimeSystem : IExecuteSystem, IInitializeSystem
{
    private readonly Contexts _contexts;
    private readonly ITimeService _timeService;

    public UpdateTimeSystem(Contexts contexts, Services services)
    {
        _contexts = contexts;
        _timeService = services.TimeService;
    }

    public void Initialize()
    {
        //Make it bulletproof
        Execute();
    }

    public void Execute()
    {
        //_contexts.input.ReplaceDeltaTime(_timeService.DeltaTime());
        //_contexts.input.ReplaceRealtimeSinceStartup(_timeService.RealtimeSinceStartup());

        var deltaTime = _timeService.DeltaTime();
        _contexts.Input.SetUnique<DeltaTimeComponent>(c => c.value = deltaTime);

        var timeSinceStartup = _timeService.RealtimeSinceStartup();
        _contexts.Input.SetUnique<RealtimeSinceStartupComponent>(c => c.value = timeSinceStartup);

    }
}
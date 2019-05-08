using Entitas;
using Entitas.Generics;

public sealed class UpdateTimeSystem : IExecuteSystem, IInitializeSystem
{
    private readonly GenericContexts _contexts;
    private readonly ITimeService _timeService;

    public UpdateTimeSystem(GenericContexts contexts, Services services)
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
        _contexts.Input.SetUnique(new DeltaTimeComponent
        {
            value = deltaTime
        });

        var timeSinceStartup = _timeService.RealtimeSinceStartup();
        _contexts.Input.SetUnique(new RealtimeSinceStartupComponent
        {
            value = timeSinceStartup
        });
    }
}
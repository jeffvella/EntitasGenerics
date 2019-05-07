using Entitas;
using Entitas.Generics;

public sealed class UpdateTimeSystem : IExecuteSystem, IInitializeSystem
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;
    private readonly ITimeService _timeService;

    public UpdateTimeSystem(Contexts contexts, GenericContexts genericContexts, Services services)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
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
        _genericContexts.Input.SetUnique(new DeltaTimeComponent
        {
            value = deltaTime
        });

        var timeSinceStartup = _timeService.RealtimeSinceStartup();
        _genericContexts.Input.SetUnique(new RealtimeSinceStartupComponent
        {
            value = timeSinceStartup
        });
    }
}
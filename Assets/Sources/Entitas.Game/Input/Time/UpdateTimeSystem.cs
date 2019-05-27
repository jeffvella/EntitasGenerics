namespace Entitas.MatchLine
{
    public sealed class UpdateTimeSystem : IExecuteSystem, IInitializeSystem
    {
        private readonly Contexts _contexts;
        private readonly ITimeService _timeService;

        public UpdateTimeSystem(Contexts contexts, IServices services)
        {
            _contexts = contexts;
            _timeService = services.TimeService;
        }

        public void Initialize()
        {
            Execute();
        }

        public void Execute()
        {
            //var deltaTime = ;
            _contexts.Input.GetUnique<DeltaTimeComponent>().Value = _timeService.DeltaTime();

            //_contexts.Input.SetUnique<DeltaTimeComponent>(c => c.value = deltaTime);
            //var timeSinceStartup = ;
            //_contexts.Input.SetUnique<RealtimeSinceStartupComponent>(c => c.value = timeSinceStartup);

            _contexts.Input.GetUnique<RealtimeSinceStartupComponent>().Value = _timeService.RealtimeSinceStartup();

        }
    }
}
using Entitas.Generics;

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

            //_realTimeAccessor = _contexts.Input.Unique.Get<RealtimeSinceStartupComponent>().ToPersistant();
            //_detaTimeAccessor = _contexts.Input.Unique.Get<DeltaTimeComponent>().ToPersistant();
        }

        public void Initialize()
        {
            Execute();
        }

        public void Execute()
        {
            //var deltaTime = ;
            //_contexts.Input.Unique.Get<DeltaTimeComponent>().Component.Value = _timeService.DeltaTime();

            //_contexts.Input.SetUnique<DeltaTimeComponent>(c => c.value = deltaTime);
            //var timeSinceStartup = ;
            //_contexts.Input.SetUnique<RealtimeSinceStartupComponent>(c => c.value = timeSinceStartup);

            _contexts.Input.Unique.GetAccessor<RealtimeSinceStartupComponent>().Apply(_timeService.RealtimeSinceStartup());
            _contexts.Input.Unique.GetAccessor<DeltaTimeComponent>().Apply(_timeService.DeltaTime());
        }
    }
}
﻿using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class UpdateTimeSystem : IExecuteSystem, IInitializeSystem
    {
        private readonly Contexts _contexts;
        private readonly ITimeService _timeService;

        private readonly PersistentComponentAccessor<RealtimeSinceStartupComponent> _realTimeAccessor;
        private readonly PersistentComponentAccessor<DeltaTimeComponent> _detaTimeAccessor;

        public UpdateTimeSystem(Contexts contexts, IServices services)
        {
            _contexts = contexts;
            _timeService = services.TimeService;

            _realTimeAccessor = _contexts.Input.GetUnique<RealtimeSinceStartupComponent>().ToPersistant();
            _detaTimeAccessor = _contexts.Input.GetUnique<DeltaTimeComponent>().ToPersistant();
        }

        public void Initialize()
        {
            Execute();
        }

        public void Execute()
        {
            //var deltaTime = ;
            //_contexts.Input.GetUnique<DeltaTimeComponent>().Component.Value = _timeService.DeltaTime();

            //_contexts.Input.SetUnique<DeltaTimeComponent>(c => c.value = deltaTime);
            //var timeSinceStartup = ;
            //_contexts.Input.SetUnique<RealtimeSinceStartupComponent>(c => c.value = timeSinceStartup);

            _realTimeAccessor.Apply(_timeService.RealtimeSinceStartup());
            _detaTimeAccessor.Apply(_timeService.DeltaTime());
        }
    }
}
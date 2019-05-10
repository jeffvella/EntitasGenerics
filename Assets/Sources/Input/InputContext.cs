using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;

namespace Assets.Sources.Config
{
    public class InputContext : GenericContext<InputContext, InputEntity>
    {
        public InputContext() : base(new InputContextDefinition()){ }
    }

    public class InputContextDefinition : ContextDefinition<InputContext, InputEntity>
    {
        public InputContextDefinition()
        {
            Add<PointerHoldingComponent>();
            Add<PointerHoldingPositionComponent>();
            Add<PointerHoldingTimeComponent>();
            Add<PointerReleasedComponent>();
            Add<PointerStartedHoldingComponent>();
            Add<RestartComponent>();
            Add<DeltaTimeComponent>();
            Add<RealtimeSinceStartupComponent>();
            Add<DestroyedComponent>();
        }
    }


    //public class InputContext : Context<InputContext, Entity>
    //{
    //    public InputContext() : base(new InputContextDefinition())
    //    {

    //    }

    //    public class InputContextDefinition : ContextDefinition<InputContext, Entity>
    //    {
    //        public InputContextDefinition()
    //        {
    //            PointerHolding = Add<PointerHoldingComponent>();
    //            PointerHoldingPosition = Add<PointerHoldingPositionComponent>();
    //            PointerHoldingTime = Add<PointerHoldingTimeComponent>();
    //            PointerReleased = Add<PointerReleasedComponent>();
    //            PointerStartedHolding = Add<PointerStartedHoldingComponent>();
    //            Restart = Add<RestartComponent>();
    //            DeltaTime = Add<DeltaTimeComponent>();
    //            RealtimeSinceStartup = Add<RealtimeSinceStartupComponent>();                
    //        }

    //        public IComponentDefinition<RealtimeSinceStartupComponent> RealtimeSinceStartup { get; }

    //        public IComponentDefinition<DeltaTimeComponent> DeltaTime { get; }

    //        public IComponentDefinition<RestartComponent> Restart { get; }

    //        public IComponentDefinition<PointerStartedHoldingComponent> PointerStartedHolding { get; }

    //        public IComponentDefinition<PointerReleasedComponent> PointerReleased { get; }

    //        public IComponentDefinition<PointerHoldingTimeComponent> PointerHoldingTime { get; }

    //        public IComponentDefinition<PointerHoldingPositionComponent> PointerHoldingPosition { get; }

    //        public IComponentDefinition<PointerHoldingComponent> PointerHolding { get; }

    //    }        
    //}
}


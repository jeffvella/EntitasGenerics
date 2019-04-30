using Entitas;

namespace EntitasGenerics
{
    public class LoggingContext : Context<LoggingContext, Entity>
    {
        public LoggingContext() : base(new LoggingContextDefinition())  { }

        public class LoggingContextDefinition : ContextDefinition<LoggingContext, Entity>
        {
            public LoggingContextDefinition()
            {
                DebugMessage = Register<DebugMessageComponent>();
            }

            public IComponentDefinition<DebugMessageComponent> DebugMessage { get; }
        }
    }
}
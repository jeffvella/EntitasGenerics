using Entitas;

namespace Entitas.Generics
{
    public class LoggingContext : GenericContext<LoggingContext, Entity>
    {
        public LoggingContext() : base(new LoggingContextDefinition())  { }

        public class LoggingContextDefinition : ContextDefinition<LoggingContext, Entity>
        {
            public LoggingContextDefinition()
            {
                Add<DebugMessageComponent>();
            }
        }
    }
}
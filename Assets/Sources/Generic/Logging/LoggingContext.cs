using Entitas;

namespace EntitasGeneric
{
    public class LoggingContext : Context<LoggingContext, Entity>
    {
        public LoggingContext() : base(new LoggingContextDefinition())  { }

        /// <summary>
        /// This definition defines the component types that will be in the context,
        /// and produces a contextInfo object for the Context<T> base constructor.
        /// </summary>
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
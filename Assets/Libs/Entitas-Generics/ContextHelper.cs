using System;
using Entitas;

namespace Entitas.Generics {
    public static class ContextHelper<TContext> where TContext : IContext
    {
        public static Type ContextType { get; } = typeof(TContext);

        public static bool IsInitialized { get; private set; }

        public static ContextInfo ContextInfo { get; private set; }


        public static void Initialize(ContextInfo contextInfo)
        {
            ContextInfo = contextInfo;
            IsInitialized = true;
        }
    }
}
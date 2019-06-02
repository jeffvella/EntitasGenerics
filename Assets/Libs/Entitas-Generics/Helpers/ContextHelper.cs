namespace Entitas.Generics
{
    /// <summary>
    /// Provides info about a Context/Get combinations
    /// </summary>
    /// ReSharper disable StaticMemberInGenericType; Intended.
    public static class ContextHelper<TContext> where TContext : IContext
    {
        public static bool IsInitialized { get; private set; }

        public static ContextInfo ContextInfo { get; private set; }

        public static void Initialize(ContextInfo contextInfo)
        {
            ContextInfo = contextInfo;
            IsInitialized = true;
        }
    }
}
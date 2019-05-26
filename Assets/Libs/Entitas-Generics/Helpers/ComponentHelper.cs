using Entitas.CodeGeneration.Attributes;

namespace Entitas.Generics
{
    /// <summary>
    /// Provides info about a Context/Component pair.
    /// </summary>
    /// <typeparam name="TContext">the context that contains the <see cref="TComponent"/></typeparam>
    /// <typeparam name="TComponent">the component to lookup information for</typeparam>
    /// <remarks>
    /// Component Index is unique to a context (the same component can have a different index if its used in different contexts)
    /// It could also be unique to an entity type but using the context allows the use of the same entity type for multiple contexts.
    /// Accessing a static generic type field is roughly twice the speed of a constant (so about the same a property / 1 redirection)
    /// </remarks>
    /// ReSharper disable StaticMemberInGenericType; Intended.
    public static class ComponentHelper<TContext, TComponent> 
        where TContext : IContext 
        where TComponent : IComponent, new()
    {
        //public static ComponentIndex<TContext, TComponent> Index { get; private set; }

        public static int ComponentIndex { get; private set; } = -1;

        public static bool IsUnique { get; private set; }

        public static bool IsEvent { get; private set; }

        public static bool IsFlag { get; private set; }

        public static bool IsIndexed { get; private set; }

        public static TComponent Default { get; private set; }

        public static bool IsInitialized { get; private set; }        

        public static void Initialize(int componentIndex)
        {
            ComponentIndex = componentIndex;
            IsUnique = ComponentHelper.IsUniqueComponent<TComponent>();
            IsEvent = ComponentHelper.IsEventComponent<TComponent>();
            IsFlag = ComponentHelper.IsFlagComponent<TComponent>();  
            IsIndexed = ComponentHelper.IsFlagComponent<TComponent>();
            Default = new TComponent();            
            IsInitialized = true;
        }
    }

    public static class ComponentHelper<TComponent> where TComponent : IComponent, new()
    {
        public static TComponent Default { get; } = new TComponent();

        public static bool IsIndexed { get; } = ComponentHelper.IsIndexedComponent<TComponent>();

        public static bool IsEvent { get; } = ComponentHelper.IsEventComponent<TComponent>();

        public static bool IsUnique { get; } = ComponentHelper.IsUniqueComponent<TComponent>();

        public static bool IsFlag { get; } = ComponentHelper.IsFlagComponent<TComponent>();
    }

    public static class ComponentHelper
    {
        public static bool IsUniqueComponent<T>()
        {
            return typeof(IUniqueComponent).IsAssignableFrom(typeof(T));
        }

        public static bool IsIndexedComponent<T>()
        {
            return typeof(IIndexedComponent<T>).IsAssignableFrom(typeof(T));
        }

        public static bool IsEventComponent<T>()
        {
            return typeof(IEventComponent).IsAssignableFrom(typeof(T));
        }

        public static bool IsFlagComponent<T>()
        {
            return typeof(T).GetMembers().Length == 0 || typeof(IEventComponent).IsAssignableFrom(typeof(T));
        }
    }
}

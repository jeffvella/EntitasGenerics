using System;
using Entitas.CodeGeneration.Attributes;

namespace Entitas.Generics
{
    /// <summary>
    /// Provides info about a components of a context/entity pair.
    /// </summary>
    /// <typeparam name="TContext">the context that contains the <see cref="TComponent"/></typeparam>
    /// <typeparam name="TComponent">the component to lookup information for</typeparam>
    /// ReSharper disable StaticMemberInGenericType; Intended.
    public static class ComponentHelper<TContext, TComponent> 
        where TContext : IContext 
        where TComponent : IComponent, new()
    {
        public static int ComponentIndex { get; private set; } = -1;

        public static bool IsUnique { get; private set; }

        public static bool IsEvent { get; private set; }

        public static bool IsFlag { get; private set; }

        public static TComponent Default { get; private set; }

        public static bool IsInitialized { get; private set; }        

        public static void Initialize(int componentIndex)
        {
            ComponentIndex = componentIndex;
            IsUnique = ComponentHelper.IsUniqueComponent<TComponent>();
            IsEvent = ComponentHelper.IsEventComponent<TComponent>();
            IsFlag = ComponentHelper.IsFlagComponent<TComponent>();  
            Default = new TComponent();            
            IsInitialized = true;
        }
    }

    public static class ComponentHelper<TComponent> where TComponent : IComponent, new()
    {
        public static TComponent Default { get; } = new TComponent();

        public static bool IsEvent { get; } = ComponentHelper.IsEventComponent<TComponent>();

        public static bool IsUnique { get; } = ComponentHelper.IsUniqueComponent<TComponent>();

        public static bool IsFlag { get; } = ComponentHelper.IsFlagComponent<TComponent>();
    }

    public static class ComponentHelper
    {
        public static T Cast<T>(IComponent component)
        {
            if (!(component is T result))
            {
                throw new InvalidCastException();
            }
            return result;
        }

        public static bool IsUniqueComponent<T>()
        {
            return typeof(IUniqueComponent).IsAssignableFrom(typeof(T));
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

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Plugins;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Entitas.Generics
{
    public static class ComponentHelper<TContext, TComponent> 
        where TContext : IContext where TComponent : new()
    {
        public static bool IsInitialized { get; }

        public static int ComponentIndex { get; }

        public static Type ComponentType { get; }

        public static bool IsUnique { get; }

        public static bool IsEvent { get; }

        public static TComponent Default { get; }

        static ComponentHelper()
        {
            if (!ContextHelper<TContext>.IsInitialized)
            {
                throw new InvalidOperationException($"{nameof(ContextHelper<TContext>)} must be initialized before {nameof(ComponentHelper<TContext, TComponent>)} use");
            }

            var contextType = typeof(TContext).Name;
            ComponentType = typeof(TComponent);

            var contextTypes = ContextHelper<TContext>.ContextInfo.componentTypes;
            ComponentIndex = Array.IndexOf(contextTypes, ComponentType);

            if (ComponentIndex < 0)
            {
                // This is the only Type related issue that will not be breaking at compile time.
                // Users must ensure that the components that need to be in a context are in fact defined in that context.
                // todo: see if there's a way to get rid of this without changing the way entitas internally builds contexts.

                throw new ComponentNotInContextException($"Component index for '{ComponentType.Name}' wasn't found for context '{contextType}'. Make sure it was registered in the ContextDefinition");
            }
            
            IsUnique = AttributeHelper.HasAttribute<UniqueAttribute>(ComponentType);
            IsEvent = AttributeHelper.HasAttribute<EventAttribute>(ComponentType);
            Default = new TComponent();
            IsInitialized = true;
        }
    }

    public class ComponentNotInContextException : Exception
    {
        public ComponentNotInContextException(string msg) : base(msg) {}
    }

    public static class AttributeHelper
    {
        public static bool HasAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault() != null;
        }

        public static bool TryGetAttribute<T>(MemberInfo memberInfo, out T customAttribute) where T : Attribute
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            if (attributes == null)
            {
                customAttribute = null;
                return false;
            }
            customAttribute = (T)attributes;
            return true;
        }
    }
}

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Plugins;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace EntitasGenerics
{
    public static class ComponentHelper<TContext, TComponent> 
        where TContext : IContext
    {
        public static bool IsInitialized { get; }

        public static int ComponentIndex { get; }

        public static Type ComponentType { get; }

        public static bool IsUnique { get; }

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
                throw new IndexOutOfRangeException($"Component index for '{ComponentType.Name}' wasn't found for context '{contextType}'. Make sure it was registered in the ContextDefinition");
            }
            
            IsUnique = HasAttribute<UniqueAttribute>(ComponentType);
            IsInitialized = true;
        }

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
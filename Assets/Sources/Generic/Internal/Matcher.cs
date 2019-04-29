using System;
using Entitas;

namespace EntitasGeneric
{
    public class Matcher<TEntityGroup, TEntity, TComponent>
        where TEntity : class, IEntity, new() 
        where TEntityGroup : IContext
        where TComponent : IComponent
    {
        private static IAnyOfMatcher<TEntity> _anyOf;
        private static IAnyOfMatcher<TEntity> _allOf;
        private static int _c1Index;

        static Matcher()
        {
            _c1Index = TypeHelper<TEntityGroup, TComponent>.ComponentIndex;
        }

        public static IMatcher<TEntity> AnyOf
            => _anyOf ?? (_anyOf = Entitas.Matcher<TEntity>.AnyOf(_c1Index));

        public static IMatcher<TEntity> AllOf
            => _allOf ?? (_anyOf = Entitas.Matcher<TEntity>.AllOf(_c1Index));
    }

    public class Matcher<TEntityGroup, TEntity, TComponent1, TComponent2> 
        where TEntity : class, IEntity, new() 
        where TEntityGroup : IContext
        where TComponent1 : IComponent 
        where TComponent2 : IComponent
    {
        private static IAnyOfMatcher<TEntity> _anyOf;
        private static IAnyOfMatcher<TEntity> _allOf;

        private static readonly int _c1Index;
        private static readonly int _c2Index;

        static Matcher()
        {
            _c1Index = TypeHelper<TEntityGroup, TComponent1>.ComponentIndex;
            _c2Index = TypeHelper<TEntityGroup, TComponent2>.ComponentIndex;
        }

        public static IMatcher<TEntity> AnyOf
            => _anyOf ?? (_anyOf = Entitas.Matcher<TEntity>.AnyOf(_c1Index, _c2Index));

        public static IMatcher<TEntity> AllOf
            => _allOf ?? (_anyOf = Entitas.Matcher<TEntity>.AllOf(_c1Index, _c2Index));
    }

    public static class ContextHelper<TContext>
        where TContext : IContext
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

    public static class TypeHelper<TContext, TComponent> 
        where TContext : IContext
    {
        public static bool IsInitialized { get; }

        public static int ComponentIndex { get; }

        public static Type ComponentType { get; }

        static TypeHelper()
        {
            if (!ContextHelper<TContext>.IsInitialized)
                throw new InvalidOperationException("ContextHelper<TContext> must be initialized before use");

            ComponentType = typeof(TComponent);
            ComponentIndex = Array.IndexOf(ContextHelper<TContext>.ContextInfo.componentTypes, ComponentType);
            IsInitialized = true;
        }
    }
}

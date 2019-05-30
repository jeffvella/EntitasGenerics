namespace Entitas.Generics
{
    public class GenericMatcher<TContext, TEntity, TComponent>
        where TContext : IContext
        where TEntity : class, IEntity
        where TComponent : IComponent, new()
    {
        private static IAnyOfMatcher<TEntity> _anyOf;
        private static IAnyOfMatcher<TEntity> _allOf;
        
        // ReSharper disable once StaticMemberInGenericType; Intended.
        private static readonly int Index;

        static GenericMatcher()
        {
            Index = ComponentHelper<TContext, TComponent>.ComponentIndex;
        }

        public static IMatcher<TEntity> AnyOf
            => _anyOf ?? (_anyOf = MatcherFactory.CreateMatcher<TContext, TEntity>(MatcherType.Any, Index));

        public static IMatcher<TEntity> AllOf
            => _allOf ?? (_allOf = MatcherFactory.CreateMatcher<TContext, TEntity>(MatcherType.All, Index));
    }

    public enum MatcherType
    {
        None = 0,
        Any,
        All,
    }

    public class MatcherFactory
    {
        public static IAnyOfMatcher<TEntity> CreateMatcher<TContext, TEntity>(MatcherType type, params int[] indices) 
            where TEntity : class, IEntity
            where TContext : IContext
        {
            Matcher<TEntity> matcher = default;
            switch (type)
            {
                case MatcherType.None:
                case MatcherType.All:
                    matcher = (Entitas.Matcher<TEntity>)Entitas.Matcher<TEntity>.AllOf(indices);
                    matcher.componentNames = ContextHelper<TContext>.ContextInfo.componentNames;
                    break;
                    
                case MatcherType.Any:
                    matcher = (Entitas.Matcher<TEntity>)Entitas.Matcher<TEntity>.AnyOf(indices);
                    matcher.componentNames = ContextHelper<TContext>.ContextInfo.componentNames;
                    break;                 
            }
            return matcher;
        }
    }

}

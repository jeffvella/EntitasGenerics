namespace Entitas.Generics
{
    /// <summary>
    /// Provides matcher instance for a Context/Entity/Get combination.
    /// </summary>
    public class GenericMatcher<TContext, TEntity, TComponent>
        where TContext : IContext
        where TEntity : class, IEntity, new()
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
            => _anyOf ?? (_anyOf = MatcherFactory.CreateAnyOfMatcher<TContext, TEntity>(Index));

        public static IMatcher<TEntity> AllOf
            => _allOf ?? (_allOf = MatcherFactory.CreateAllOfMatcher<TContext, TEntity>(Index));
    }

    public class MatcherFactory
    {
        public static IAnyOfMatcher<TEntity> CreateAnyOfMatcher<TContext, TEntity>(params int[] indices) 
            where TEntity : class, IEntity, new()
            where TContext : IContext
        {
            Matcher<TEntity> matcher = (Entitas.Matcher<TEntity>)Entitas.Matcher<TEntity>.AnyOf(indices);
            matcher.componentNames = ContextHelper<TContext>.ContextInfo.componentNames;
            return matcher;
        }

        public static IAllOfMatcher<TEntity> CreateAllOfMatcher<TContext, TEntity>(params int[] indices)
            where TEntity : class, IEntity, new()
            where TContext : IContext
        {
            Matcher<TEntity> matcher = (Entitas.Matcher<TEntity>)Entitas.Matcher<TEntity>.AllOf(indices);
            matcher.componentNames = ContextHelper<TContext>.ContextInfo.componentNames;
            return matcher;
        }
    }

}

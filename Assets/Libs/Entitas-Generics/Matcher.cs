using Entitas;

namespace Entitas.Generics
{
    //public class EventSystems<TContext, TEntity, TComponent, TListenerComponent>
    //    where TContext : IContext
    //    where TEntity : class, IEntity, new()
    //    where TComponent : IComponent, new()
    //    where TListenerComponent : IListenerComponent<(TEntity Entity, TComponent Component)>, new()
    //{
    //    private static GenericEventSystem<TEntity, TComponent, TListenerComponent> _system;

    //    public static IGenericEventSystem<TComponent> GetOrCreateInstance(IGenericContext<TEntity> context)
    //    {
    //        if (_system == null)
    //        {
    //            _system = new GenericEventSystem<TEntity, TComponent, TListenerComponent>(context);
    //        }
    //        return _system;
    //    }
    //}

    public class Matcher<TContext, TEntity, TComponent>
        where TContext : IContext
        where TEntity : class, IEntity, new()
        where TComponent : IComponent, new()
    {
        private static IAnyOfMatcher<TEntity> _anyOf;
        private static IAnyOfMatcher<TEntity> _allOf;
        private static int _c1Index;

        static Matcher()
        {
            _c1Index = ComponentHelper<TContext, TComponent>.ComponentIndex;
        }

        public static IMatcher<TEntity> AnyOf
            => _anyOf ?? (_anyOf = MatcherFactory.CreateAnyOfMatcher<TContext, TEntity>(_c1Index));

        public static IMatcher<TEntity> AllOf
            => _allOf ?? (_allOf = MatcherFactory.CreateAllOfMatcher<TContext, TEntity>(_c1Index));
    }

    public class Matcher<TContext, TEntity, TComponent1, TComponent2>
        where TContext : IContext
        where TEntity : class, IEntity, new() 
        where TComponent1 : IComponent, new()
        where TComponent2 : IComponent, new()
    {
        private static IAnyOfMatcher<TEntity> _anyOf;
        private static IAnyOfMatcher<TEntity> _allOf;

        private static readonly int _c1Index;
        private static readonly int _c2Index;

        static Matcher()
        {
            _c1Index = ComponentHelper<TContext, TComponent1>.ComponentIndex;
            _c2Index = ComponentHelper<TContext, TComponent2>.ComponentIndex;
        }

        public static IMatcher<TEntity> AnyOf
            => _anyOf ?? (_anyOf = MatcherFactory.CreateAnyOfMatcher<TContext, TEntity>(_c1Index, _c2Index));


        public static IMatcher<TEntity> AllOf
            => _allOf ?? (_allOf = MatcherFactory.CreateAllOfMatcher<TContext,TEntity>(_c1Index, _c2Index));
    }

    public class MatcherFactory
    {
        public static IAnyOfMatcher<TEntity> CreateAnyOfMatcher<TContext, TEntity>(params int[] indices) 
            where TEntity : class, IEntity, new()
            where TContext : IContext
        {
            var matcher = (Entitas.Matcher<TEntity>)Entitas.Matcher<TEntity>.AnyOf(indices);
            matcher.componentNames = ContextHelper<TContext>.ContextInfo.componentNames;
            return matcher;
        }

        public static IAllOfMatcher<TEntity> CreateAllOfMatcher<TContext, TEntity>(params int[] indices)
            where TEntity : class, IEntity, new()
            where TContext : IContext
        {
            var matcher = (Entitas.Matcher<TEntity>)Entitas.Matcher<TEntity>.AllOf(indices);
            matcher.componentNames = ContextHelper<TContext>.ContextInfo.componentNames;
            return matcher;
        }

    }

}

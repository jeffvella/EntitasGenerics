using Entitas;

namespace EntitasGenerics
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
            _c1Index = ComponentHelper<TEntityGroup, TComponent>.ComponentIndex;
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
            _c1Index = ComponentHelper<TEntityGroup, TComponent1>.ComponentIndex;
            _c2Index = ComponentHelper<TEntityGroup, TComponent2>.ComponentIndex;
        }

        public static IMatcher<TEntity> AnyOf
            => _anyOf ?? (_anyOf = Entitas.Matcher<TEntity>.AnyOf(_c1Index, _c2Index));

        public static IMatcher<TEntity> AllOf
            => _allOf ?? (_anyOf = Entitas.Matcher<TEntity>.AllOf(_c1Index, _c2Index));
    }
}

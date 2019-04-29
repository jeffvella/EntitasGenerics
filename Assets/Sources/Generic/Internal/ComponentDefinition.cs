using Entitas;

namespace EntitasGeneric
{
    public interface IComponentDefinition
    {

    }

    public interface IComponentDefinition<T> : IComponentDefinition
    {

    }

    public class ComponentDefinition<TContext, TEntity, TComponent> : IComponentDefinition<TComponent>  
        where TComponent : class, IComponent 
        where TContext : IContext
        where TEntity : class, IEntity, new()
    {
        public static IMatcher<TEntity> GetMatcher() => Matcher<TContext, TEntity, TComponent>.AllOf;
    }
}
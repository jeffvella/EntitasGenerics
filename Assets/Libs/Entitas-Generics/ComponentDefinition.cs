using System;
using System.Linq;
using System.Reflection;
using Entitas;

namespace EntitasGenerics
{
    public interface IComponentDefinition
    {
        bool IsUnique { get; }
    }

    public interface IComponentDefinition<T> : IComponentDefinition
    {

    }

    public class ComponentDefinition<TContext, TEntity, TComponent> : IComponentDefinition<TComponent>  
        where TComponent : class, IComponent 
        where TContext : IContext
        where TEntity : class, IEntity, new()
    {
        public bool IsUnique => ComponentHelper<TContext, TComponent>.IsUnique;

        public static IMatcher<TEntity> Matcher => Matcher<TContext, TEntity, TComponent>.AllOf;
    }
}
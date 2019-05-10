using System;
using System.Linq;
using System.Reflection;
using Entitas;

namespace Entitas.Generics
{
    public interface IComponentDefinition
    {
        bool IsUnique { get; }

        bool IsEvent { get; }

        IMatcher<IEntity> Matcher { get; }
    }

    public interface IComponentDefinition<T> : IComponentDefinition
    {
        int ComponentIndex { get; }        
    }

    public class ComponentDefinition<TContext, TEntity, TComponent> : IComponentDefinition<TComponent>  
        where TComponent : class, IComponent, new()
        where TContext : IContext
        where TEntity : class, IEntity, new()
    {
        public bool IsUnique => ComponentHelper<TContext, TComponent>.IsUnique;

        public bool IsEvent => ComponentHelper<TContext, TComponent>.IsEvent;

        public IMatcher<IEntity> Matcher => (IMatcher<IEntity>)GenericMatcher<TContext, TEntity, TComponent>.AllOf;

        public int ComponentIndex => ComponentHelper<TContext, TComponent>.ComponentIndex;

    }
}
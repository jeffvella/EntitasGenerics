using System;
using System.Linq;
using System.Reflection;
using Entitas;

namespace Entitas.Generics
{
    // This is currently not used; if it were useful to store a version of the Components on
    // ContextDefinition.Add (ie to enumerate them for debugging) then this could be used
    // to cache an IComponentDefinition[] and make it accessible via the created context.
    // But all this information is already accessible from the GenericMatcher/ComponentHelper,
    // (assuming you have TContext/TEntity/TComponent generic type in scope).

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
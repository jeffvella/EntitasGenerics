using System;
using System.Collections.Generic;
using Entitas;

namespace EntitasGeneric
{
    public interface IContextDefinition
    {
        ContextInfo GetContextInfo();

        int ComponentCount { get; }
    }


    public class ContextDefinition<TContext, TEntity> : IContextDefinition where TContext : IContext where TEntity : class, IEntity, new()
    {
        public List<IComponentDefinition> Components { get; } = new List<IComponentDefinition>();

        public List<string> ComponentNames { get; } = new List<string>();

        public List<Type> ComponentTypes { get; } = new List<Type>();

        protected IComponentDefinition<T> Register<T>() where T : class, IComponent
        {
            var def = new ComponentDefinition<TContext, TEntity, T>();
            Components.Add(def);
            ComponentNames.Add(typeof(T).Name);
            ComponentTypes.Add(typeof(T));
            ComponentCount++;
            return def;
        }

        public int ComponentCount { get; private set; }

        public ContextInfo GetContextInfo()
        {
            return new ContextInfo(typeof(TContext).Name, ComponentNames.ToArray(), ComponentTypes.ToArray());
        }
    }
}

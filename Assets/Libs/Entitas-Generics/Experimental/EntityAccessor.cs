using System;

namespace Entitas.Generics
{

    // An alternative to storing the context within each entity would be using a wrapper
    // like this. Tests show that stack-only allocation and a couple of assignments are
    // so fast that it's inconsequential compared to everything else going on;
    // but i'm not yet convinced it feels nice to use.

    public readonly ref struct EntityAccessor<TEntity> where TEntity : class, IEntity
    {
        public readonly IGenericContext<TEntity> Context;
        public readonly TEntity Entity;

        public EntityAccessor(IGenericContext<TEntity> context, TEntity entity)
        {
            Context = context;
            Entity = entity;
        }

        public TComponent Get<TComponent>() where TComponent : IComponent, new()
        {
            var index = Context.GetComponentIndex<TComponent>();
            return (TComponent)Entity.GetComponent(index);
        }

        public void Set<TComponent>(Action<TComponent> componentUpdater) where TComponent : IComponent, new()
        {
            var index = Context.GetComponentIndex<TComponent>();
            var newComponent = Entity.CreateComponent<TComponent>(index);
            componentUpdater(newComponent);
            Entity.ReplaceComponent(index, newComponent);
        }
    }
}
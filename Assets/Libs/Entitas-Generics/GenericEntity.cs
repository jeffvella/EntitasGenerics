using System;
using Entitas;
using Entitas.Generics;

/// <summary>
/// An entity with access to its creating context.
/// </summary>
public interface IContextLinkedEntity : IEntity
{
    IEntityContext Context { get; set; }
}


/// <summary>
/// GenericEntity is an option instead of deriving from Entitas.Entity.
/// It allows access to components directly from the entity.
/// </summary>
public class GenericEntity : Entitas.Entity, IContextLinkedEntity 
{
    // Entities implementing IContextLinkedEntity are on creation given an IEntity version
    // of their context by GenericContext<TEntity>.LinkContextToEntity();

    public IEntityContext Context { get; set; }

    public TComponent Get<TComponent>() where TComponent : IComponent, new()
    {
        return Context.Get<TComponent>(this);
    }

    public bool HasComponent<TComponent>() where TComponent : IComponent, new()
    {
        return Context.Has<TComponent>(this);
    }

    public bool IsFlagged<TComponent>() where TComponent : IFlagComponent, new()
    {
        return Context.Has<TComponent>(this);
    }

    public void SetFlag<TComponent>(bool toggle = true) where TComponent : IFlagComponent, new()
    {
        Context.SetFlag<TComponent>(this, toggle);
    }

    public void Set<TComponent>(Action<TComponent> componentUpdater) where TComponent : class, IComponent, new()
    {
        Context.Set(this, componentUpdater);
    }

    public void RegisterAddedComponentListener<TComponent>(Action<(IEntity Entity, TComponent Component)> action) where TComponent : IEventComponent, new()
    {
        Context.RegisterAddedComponentListener<TComponent>(this, action);
    }

    public void RegisterRemovedComponentListener<TComponent>(Action<IEntity> action) where TComponent : IEventComponent, new()
    {
        Context.RegisterRemovedComponentListener<TComponent>(this, action);
    }
}


using Entitas;
using Entitas.Generics;

public class GenericEntity : Entitas.Entity, IContextLinkedEntity<IEntity>
{
    public IGenericContext<IEntity> Context { get; }

    public void Set<TComponent>(TComponent component = default) where TComponent : IComponent, new()
    {
        Context.Set(this, component);
    }

    public void SetTag<TComponent>(bool toggle) where TComponent : ITagComponent, new()
    {
        Context.SetTag<TComponent>(toggle);
    }
}

public readonly ref struct EntityAccessor<TEntity> where TEntity : class, IEntity
{
    public IGenericContext<TEntity> Context { get; }

    public TEntity Entity { get; }

    public EntityAccessor(IGenericContext<TEntity> context, TEntity entity)
    {
        Context = context;
        Entity = entity;
    }

    public T Get<T>() where T : IComponent, new()
    {
        return Context.Get<T>(Entity);
    }

    public void Set<T>(T component = default) where T : IComponent, new()
    {
        Context.Set(Entity, component);
    }
}

public interface IContextLinkedEntity
{
    void Set<TComponent>(TComponent component = default) where TComponent : IComponent, new();

    void SetTag<TComponent>(bool toggle) where TComponent : ITagComponent, new();
}

public interface IContextLinkedEntity<T> : IContextLinkedEntity where T : class, IEntity
{
    IGenericContext<T> Context { get; }
}

//public static class GenericEntityExtensions
//{
//    public static void Set<TEntity, TComponent>(this TEntity entity, TComponent component = default)
//        where TComponent : IComponent, new()
//        where TEntity : class, IContextLinkedEntity<TEntity>, IEntity, new()
//    {

//        entity.Context.Set(entity, component);
//    }

//    public static void SetTag<TComponent>(this TEntity entity, bool toggle)
//        where TComponent : ITagComponent, new()
//        where TEntity : class, IContextLinkedEntity<TEntity>, IEntity, new()
//    {
//        entity.Context.SetTag<TComponent>(toggle);
//    }
//}

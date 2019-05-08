using Entitas;
using Entitas.Generics;

//public class GenericEntity<TEntity> : Entitas.Entity, IContextLinkedEntity<TEntity> where TEntity : class, IEntity, new()
//{
//    public IGenericContext<TEntity> Context { get; }

//    public void Set<TComponent>(TComponent component = default) where TComponent : IComponent, new()
//    {
//        Context.Set<TComponent>(this, component);
//    }

//    public void SetTag<TComponent>(bool toggle) where TComponent : ITagComponent, new()
//    {
//        Context.SetTag<TComponent>(toggle);
//    }
//}

//public interface IContextLinkedEntity
//{
//    void Set<TComponent>(TComponent component = default) where TComponent : IComponent, new();

//    void SetTag<TComponent>(bool toggle) where TComponent : ITagComponent, new();
//}

//public interface IContextLinkedEntity<T> : IContextLinkedEntity where T : class, IEntity, new()
//{ 
//    IGenericContext<T> Context { get; }
//}

//public static class GenericEntityExtensions
//{
//    public static void Set<TEntity, TComponent>(this TEntity entity, TComponent component = default) 
//        where TComponent : IComponent, new()
//        where TEntity : class, IContextLinkedEntity<TEntity>, IEntity, new() {

//        entity.Context.Set(entity, component);
//    }

//    public static void SetTag<TComponent>(this TEntity entity, bool toggle)
//        where TComponent : ITagComponent, new()
//        where TEntity : class, IContextLinkedEntity<TEntity>, IEntity, new()
//    {
//        entity.Context.SetTag<TComponent>(toggle);
//    }
//}

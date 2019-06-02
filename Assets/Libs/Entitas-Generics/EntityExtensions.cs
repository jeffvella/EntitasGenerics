namespace Entitas.Generics
{
    public static class EntityExtensions
    {
        //public static TComponent GetOrCreateComponent<TComponent>(this IEntity entity, IEntityContext context) where TComponent : IComponent, new()
        //{
        //    var index = context.GetComponentIndex<TComponent>();

        //    TComponent component = !entity.HasComponent(index)
        //        ? entity.CreateComponent<TComponent>(index)
        //        : (TComponent)entity.GetComponent(index);

        //    return component;
        //}

        //public static TComponent GetOrCreateComponent<TEntity, TComponent>(this IContextLinkedEntity<TEntity> entity)
        //    where TEntity : IContextLinkedEntity<TEntity>
        //    where TComponent : IComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.Index;

        //    TComponent component = !entity.HasComponent(index)
        //        ? entity.CreateComponent<TComponent>(index)
        //        : (TComponent)entity.GetComponent(index);

        //    return component;
        //}

        //public static TComponent GetOrCreateComponent<TComponent>(this IContextLinkedEntity entity)
        //    where TComponent : IComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.Index;

        //    TComponent component = !entity.HasComponent(index)
        //        ? entity.CreateComponent<TComponent>(index)
        //        : (TComponent)entity.GetComponent(index);

        //    return component;
        //}

        //public static TComponent GetOrCreateComponent<TComponent>(this IContextLinkedEntity entity)
        //    where TEntity : IContextLinkedEntity
        //    where TComponent : IComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;

        //    TComponent component = !entity.HasComponent(index)
        //        ? entity.CreateComponent<TComponent>(index)
        //        : (TComponent)entity.GetComponent(index);

        //    return component;
        //}

        //public static int GetComponentIndex<TEntity, TComponent>(this IContextLinkedEntity<TEntity> entity)
        //    where TEntity : IContextLinkedEntity<TEntity>
        //    where TComponent : IComponent, new()
        //{
        //    return ComponentHelper<TEntity, TComponent>.Index;
        //}

        //public static TComponent GetOrCreateComponent<TEntity, TComponent>(this IContextLinkedEntity<TEntity> entity)
        //    where TEntity : IContextLinkedEntity<TEntity>
        //    where TComponent : IComponent, new()
        //{
        //    var index = ComponentHelper<TEntity, TComponent>.ComponentIndex;

        //    TComponent component = !entity.HasComponent(index)
        //        ? entity.CreateComponent<TComponent>(index)
        //        : (TComponent)entity.GetComponent(index);

        //    return component;
        //}


    }

}
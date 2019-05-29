namespace Entitas.Generics
{
    public static class EntityExtensions
    {
        public static TComponent GetComponent<TComponent>(this IEntity entity, IEntityContext context) where TComponent : IComponent, new()
        {
            var index = context.GetComponentIndex<TComponent>();

            TComponent component = !entity.HasComponent(index)
                ? entity.CreateComponent<TComponent>(index)
                : (TComponent)entity.GetComponent(index);

            return component;
        }

        public static ComponentAccessor<TComponent> GetAccessor<TComponent>(this IEntity entity, IEntityContext context) where TComponent : IComponent, new()
        {
            return new ComponentAccessor<TComponent>(entity, context);
        }
    }

}
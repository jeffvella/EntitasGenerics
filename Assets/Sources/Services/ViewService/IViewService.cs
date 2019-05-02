using Entitas;
using Entitas.Generics;

public interface IViewService
{
    void LoadAsset(Contexts contexts, GenericContexts genericContexts, IEntity entity, string assetName);
}

public interface IViewService<in TEntity> where TEntity : IEntity
{
    void LoadAsset(Contexts contexts, GenericContexts genericContexts, TEntity entity, string assetName);
}
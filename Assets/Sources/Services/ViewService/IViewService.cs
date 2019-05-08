using Entitas;
using Entitas.Generics;

public interface IViewService
{
    void LoadAsset(GenericContexts contexts, IEntity entity, string assetName);
}

public interface IViewService<in TEntity> where TEntity : IEntity
{
    void LoadAsset(GenericContexts contexts, TEntity entity, string assetName);
}
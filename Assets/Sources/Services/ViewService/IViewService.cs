using Entitas;
using Entitas.Generics;

public interface IViewService
{
    void LoadAsset(Contexts contexts, IEntity entity, string assetName);
}

public interface IViewService<in TEntity> where TEntity : IEntity
{
    void LoadAsset(Contexts contexts, TEntity entity, string assetName);
}
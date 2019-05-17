using Entitas;
using Entitas.Generics;

public interface IViewService
{
    void LoadAsset(Contexts contexts, IEntity entity, string assetName);

    void LoadAsset<TEntity>(Contexts contexts, TEntity entity, string assetName) where TEntity : IEntity;
}

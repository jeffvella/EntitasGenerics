using Entitas;

namespace Entitas.MatchLine
{
    public interface IViewService : IService
    {
        void LoadAsset(Contexts contexts, IEntity entity, string assetName, int assetType = 0);

        void LoadAsset<TEntity>(Contexts contexts, TEntity entity, string assetName, int assetType = 0) where TEntity : IEntity;
    }

}
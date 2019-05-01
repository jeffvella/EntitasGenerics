using Entitas;
using Entitas.Generics;

public interface IViewService
{
    void LoadAsset(Contexts contexts, GenericContexts genericContexts, IEntity entity, string assetName);
}
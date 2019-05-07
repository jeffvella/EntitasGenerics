using Entitas.Generics;
using UnityEngine;

public sealed class ElementService : Service
{
    private int _entityCounter;

    public ElementService(Contexts contexts, GenericContexts genericContexts) : base(contexts, genericContexts)
    {
        _entityCounter = 0;
    }

    public void CreateRandomElement(GridPosition position)
    {
        //var maxType = Mathf.Clamp(_contexts.config.typeCount.Value - 1, 1, 100);

        var typeCount = _genericContexts.Config.GetUnique<TypeCountComponent>().Value;

        var maxType = Mathf.Clamp(typeCount - 1, 1, 100);


        var randomType = Random.Range(0, maxType + 1);
        var normalizedType = Mathf.InverseLerp(0, maxType, randomType);

        var entity = _contexts.game.CreateEntity();
        entity.isElement = true;
        entity.AddId(_entityCounter);
        entity.isMovable = true;
        entity.AddElementType(randomType);
        entity.AddAsset("Element");
        entity.AddColor(new Color(normalizedType, normalizedType, normalizedType));
        entity.AddPosition(position);

        _entityCounter++;
    }

    public void CreateMovableBlock(GridPosition position)
    {
        var entity = _contexts.game.CreateEntity();
        entity.isElement = true;
        entity.AddId(_entityCounter);
        entity.isMovable = true;
        entity.AddAsset("Block");
        entity.AddPosition(position);
        entity.isBlock = true;

        _entityCounter++;
    }

    public void CreateNotMovableBlock(GridPosition position)
    {
        var entity = _contexts.game.CreateEntity();
        entity.isElement = true;
        entity.AddId(_entityCounter);
        entity.AddAsset("NotMovableBlock");
        entity.AddPosition(position);
        entity.isBlock = true;

        _entityCounter++;
    }

    public void CreateExsplosiveBlock(GridPosition position)
    {
        var entity = _contexts.game.CreateEntity();
        entity.isElement = true;
        entity.AddId(_entityCounter);
        entity.AddAsset("ExsplosiveBlock");
        entity.AddPosition(position);
        entity.isExsplosive = true;
        entity.isBlock = true;

        _entityCounter++;
    }

    protected override void DropState()
    {
        _entityCounter = 0;
    }
}
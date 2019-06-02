using Entitas.Generics;
using Entitas.MatchLine;
using UnityEngine;

public sealed class UnityElementService : Service, IElementService
{
    private int _entityCounter;

    private IGenericContext<GameEntity> _game;

    public UnityElementService(Contexts contexts) : base(contexts)
    {
        _entityCounter = 0;
        _game = contexts.Game;
    }

    public void CreateRandomElement(GridPosition position)
    {
        var typeCount = _contexts.Config.Unique.Get<TypeCountComponent>().Component.Value;
        var maxType = Mathf.Clamp(typeCount - 1, 1, 100);
        var randomType = Random.Range(0, maxType + 1);
        var normalizedType = Mathf.InverseLerp(0, maxType, randomType);

        var entity = _game.CreateEntity();
        entity.SetFlag<ElementComponent>();
        entity.SetFlag<MovableComponent>();
        entity.Get<IdComponent>().Apply(_entityCounter);
        entity.Get<ElementTypeComponent>().Apply(randomType);
        entity.Get<AssetComponent>().Apply("Element");
        entity.Get<ColorComponent>().Apply(new Color(normalizedType, normalizedType, normalizedType));
        entity.Get<PositionComponent>().Apply(position);
        _entityCounter++;
    }

    public void CreateMovableBlock(GridPosition position)
    {
        var entity = _game.CreateEntity();
        entity.SetFlag<ElementComponent>();
        entity.SetFlag<MovableComponent>();
        entity.SetFlag<BlockComponent>();
        entity.Get<IdComponent>().Apply(_entityCounter);
        entity.Get<AssetComponent>().Apply("Block");
        entity.Get<PositionComponent>().Apply(position);
        _entityCounter++;
    }

    public void CreateNotMovableBlock(GridPosition position)
    {
        var entity = _game.CreateEntity();
        entity.SetFlag<ElementComponent>();
        entity.SetFlag<BlockComponent>();
        entity.Get<IdComponent>().Apply(_entityCounter);
        entity.Get<AssetComponent>().Apply("NotMovableBlock");
        entity.Get<PositionComponent>().Apply(position);
        _entityCounter++;
    }

    public void CreateExsplosiveBlock(GridPosition position)
    {
        var entity = _game.CreateEntity();
        entity.SetFlag<ElementComponent>();
        entity.SetFlag<ExplosiveComponent>();
        entity.SetFlag<BlockComponent>();
        entity.Get<IdComponent>().Apply(_entityCounter);
        entity.Get<AssetComponent>().Apply("ExsplosiveBlock");
        _entityCounter++;
    }

    public override void DropState()
    {
        _entityCounter = 0;
    }
}
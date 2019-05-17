using Assets.Sources.Game;
using Entitas.Generics;
using UnityEngine;
using GameContext = Assets.Sources.Game.GameContext;

public sealed class ElementService : Service
{
    private int _entityCounter;

    private IGenericContext<GameEntity> _game;

    public ElementService(Contexts contexts) : base(contexts)
    {
        _entityCounter = 0;
        _game = contexts.Game;
    }

    public void CreateRandomElement(GridPosition position)
    {
        //var maxType = Mathf.Clamp(_contexts.config.typeCount.Value - 1, 1, 100);

        var typeCount = _contexts.Config.GetUnique<TypeCountComponent>().Value;
        var maxType = Mathf.Clamp(typeCount - 1, 1, 100);
        var randomType = Random.Range(0, maxType + 1);
        var normalizedType = Mathf.InverseLerp(0, maxType, randomType);

        var entity = _game.CreateEntity();
        _game.SetFlag<ElementComponent>(entity, true);
        _game.SetFlag<MovableComponent>(entity, true);
        _game.Set(entity, new IdComponent { value = _entityCounter });
        _game.Set(entity, new ElementTypeComponent { value = randomType });
        _game.Set(entity, new AssetComponent { value = "Element" });
        _game.Set(entity, new ColorComponent { value = new Color(normalizedType, normalizedType, normalizedType) });
        //_game.Set(entity, new PositionComponent { value = position });
        //_game.Set<PositionComponent>(entity, c => c.value = position);
        entity.Set<PositionComponent>(c => c.value = position);

        //var entity = _contexts.game.CreateEntity();
        //entity.isElement = true;
        //entity.AddId(_entityCounter);
        //entity.isMovable = true;
        //entity.AddElementType(randomType);
        //entity.AddAsset("Element");
        //entity.AddColor(new Color(normalizedType, normalizedType, normalizedType));
        //entity.AddPosition(position);

        _entityCounter++;
    }

    public void CreateMovableBlock(GridPosition position)
    {
        //var entity = _contexts.game.CreateEntity();
        //entity.isElement = true;
        //entity.AddId(_entityCounter);
        //entity.isMovable = true;
        //entity.AddAsset("Block");
        //entity.AddPosition(position);
        //entity.isBlock = true;

        var entity = _game.CreateEntity();
        _game.SetFlag<ElementComponent>(entity, true);
        _game.SetFlag<MovableComponent>(entity, true);
        _game.SetFlag<BlockComponent>(entity, true);
        _game.Set(entity, new IdComponent { value = _entityCounter });       
        _game.Set(entity, new AssetComponent { value = "Block" });
        //_game.Set(entity, new PositionComponent { value = position });
        _game.Set<PositionComponent>(entity, c => c.value = position);

        _entityCounter++;
    }

    public void CreateNotMovableBlock(GridPosition position)
    {
        //var entity = _contexts.game.CreateEntity();
        //entity.isElement = true;
        //entity.AddId(_entityCounter);
        //entity.AddAsset("NotMovableBlock");
        //entity.AddPosition(position);
        //entity.isBlock = true;

        var entity = _game.CreateEntity();
        _game.SetFlag<ElementComponent>(entity, true);
        _game.SetFlag<BlockComponent>(entity, true);
        _game.Set(entity, new IdComponent { value = _entityCounter });
        _game.Set(entity, new AssetComponent { value = "NotMovableBlock" });
        //_game.Set(entity, new PositionComponent { value = position });
        _game.Set<PositionComponent>(entity, c => c.value = position);

        _entityCounter++;
    }

    public void CreateExsplosiveBlock(GridPosition position)
    {
        //var entity = _contexts.game.CreateEntity();
        //entity.isElement = true;
        //entity.AddId(_entityCounter);
        //entity.AddAsset("ExsplosiveBlock");
        //entity.AddPosition(position);
        //entity.isExsplosive = true;
        //entity.isBlock = true;

        var entity = _game.CreateEntity();
        _game.SetFlag<ElementComponent>(entity, true);
        _game.SetFlag<ExplosiveComponent>(entity, true);
        _game.SetFlag<BlockComponent>(entity, true);
        _game.Set(entity, new IdComponent { value = _entityCounter });
        _game.Set(entity, new AssetComponent { value = "ExsplosiveBlock" });
        //_game.Set(entity, new PositionComponent { value = position });
        _game.Set<PositionComponent>(entity, c => c.value = position);

        _entityCounter++;
    }

    protected override void DropState()
    {
        _entityCounter = 0;
    }
}
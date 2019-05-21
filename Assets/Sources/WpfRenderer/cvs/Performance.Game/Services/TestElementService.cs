using Entitas.Generics;
using Entitas.MatchLine;
using Performance.ViewModels;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed partial class TestElementService : Service, IElementService
{
    private int _entityCounter;

    private IGenericContext<GameEntity> _game;

    public TestElementService(Contexts contexts, MainViewModel viewModel) : base(contexts, viewModel)
    {
        _entityCounter = 0;
        _game = contexts.Game;
    }

    private static readonly ThreadLocal<System.Random> Random
        = new ThreadLocal<System.Random>(() => new System.Random());

    public void CreateRandomElement(GridPosition position)
    {
        var typeCount = _contexts.Config.GetUnique<TypeCountComponent>().Value;

        int maxType = typeCount;
        if (typeCount - 1 < 1)
            maxType = 1;
        else if (typeCount >= 100)
            maxType = 100;


        //var maxType = Mathf.Clamp(typeCount - 1, 1, 100);
        //var randomType = Random.Range(0, maxType + 1);
        var randomType = Random.Value.Next(0, maxType + 1);
        var normalizedType = Mathf.InverseLerp(0, maxType, randomType);

        var entity = _game.CreateEntity();
        _game.SetFlag<ElementComponent>(entity, true);
        _game.SetFlag<MovableComponent>(entity, true);
        _game.Set(entity, new IdComponent { value = _entityCounter });
        _game.Set(entity, new ElementTypeComponent { value = randomType });
        _game.Set(entity, new AssetComponent {
            value = "Element",
            id = (int)ActorType.Element
        });
        _game.Set(entity, new ColorComponent { value = new Color(normalizedType, normalizedType, normalizedType) });

        entity.Set<PositionComponent>(c => c.value = position);
        _entityCounter++;
    }

    public void CreateMovableBlock(GridPosition position)
    {
        var entity = _game.CreateEntity();
        _game.SetFlag<ElementComponent>(entity, true);
        _game.SetFlag<MovableComponent>(entity, true);
        _game.SetFlag<BlockComponent>(entity, true);
        _game.Set(entity, new IdComponent { value = _entityCounter });       
        _game.Set(entity, new AssetComponent {
            value = "Block",
            id = (int)ActorType.Block
        });
        _game.Set<PositionComponent>(entity, c => c.value = position);
        _entityCounter++;
    }

    public void CreateNotMovableBlock(GridPosition position)
    {
        var entity = _game.CreateEntity();
        _game.SetFlag<ElementComponent>(entity, true);
        _game.SetFlag<BlockComponent>(entity, true);
        _game.Set(entity, new IdComponent { value = _entityCounter });
        _game.Set(entity, new AssetComponent
        {
            value = "NotMovableBlock",
            id = (int)ActorType.NotMovableBlock
        });
        _game.Set<PositionComponent>(entity, c => c.value = position);
        _entityCounter++;
    }

    public void CreateExsplosiveBlock(GridPosition position)
    {
        var entity = _game.CreateEntity();
        _game.SetFlag<ElementComponent>(entity, true);
        _game.SetFlag<ExplosiveComponent>(entity, true);
        _game.SetFlag<BlockComponent>(entity, true);
        _game.Set(entity, new IdComponent { value = _entityCounter });
        _game.Set(entity, new AssetComponent {
            value = "ExsplosiveBlock",
            id = (int)ActorType.ExplosiveBlock
        });
        _game.Set<PositionComponent>(entity, c => c.value = position);
        _entityCounter++;
    }

    public override void DropState()
    {
        _entityCounter = 0;
    }
}
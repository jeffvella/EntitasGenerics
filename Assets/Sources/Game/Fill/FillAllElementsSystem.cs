using System.Collections.Generic;
using Assets.Sources.Config;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;
using UnityEngine;
using ConfigContext = Assets.Sources.Config.ConfigContext;

public sealed class FillAllElementsSystem : GenericReactiveSystem<GameEntity>, IInitializeSystem
{
    private readonly ElementService _elementService;
    private readonly IGenericContext<ConfigEntity> _config;
    private readonly IGenericContext<GameEntity> _game;

    public FillAllElementsSystem(Contexts contexts, Services services) : base(contexts.Game, Trigger)
    {
        _game = contexts.Game;
        _config = contexts.Config;
        _elementService = services.ElementService;
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetTriggerCollector<RestartHappenedComponent>();
    }

    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.RestartHappened.Added());
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        Fill();

        foreach (var entity in entities)
        {
            _game.SetFlag<DestroyedComponent>(entity, true);
            //entity.isDestroyed = true;
        }
    }

    public void Initialize()
    {
        Fill();
    }

    private void Fill()
    {
        //var size = _contexts.config.mapSize.value;

        var size = _config.GetUnique<MapSizeComponent>().value;

        for (int row = 0; row < size.y; row++)
        {
            for (int column = 0; column < size.x; column++)
            {
                float random = Random.Range(0f, 1f);
                if (random < 0.1f)
                {
                    if (random < 0.05f)
                    {
                        if (random < 0.005f)
                        {
                            _elementService.CreateExsplosiveBlock(new GridPosition(column, row));
                        }
                        else
                        {
                            _elementService.CreateNotMovableBlock(new GridPosition(column, row));
                        }
                    }
                    else
                    {
                        _elementService.CreateMovableBlock(new GridPosition(column, row));
                    }
                }
                else
                {
                    _elementService.CreateRandomElement(new GridPosition(column, row));
                }
            }
        }
    }
}
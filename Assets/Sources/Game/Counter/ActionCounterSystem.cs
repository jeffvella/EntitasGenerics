using System.Collections.Generic;
using Assets.Sources.Game;
using Assets.Sources.GameState;
using Entitas;
using Entitas.Generics;

public sealed class ActionCounterSystem : GenericReactiveSystem<GameEntity>
{
    private readonly IGenericContext<GameEntity> _game;
    private IGenericContext<GameStateEntity> _gameState;

    public ActionCounterSystem(Contexts contexts) : base(contexts.Game, Trigger)
    {
        _game = contexts.Game;
        _gameState = contexts.GameState;
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetTriggerCollector<MatchedComponent>();
    }

    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.Matched);
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        var currentActionCount = _gameState.GetUnique<ActionCountComponent>();

        _gameState.SetUnique<ActionCountComponent>(c => c.value = currentActionCount.value + 1);


        //_contexts.gameState.ReplaceActionCount(_contexts.gameState.actionCount.value + 1);
    }
}

//public static class ValueCoersionExtensions
//{
//    public static void Set2<TComponent, TEntity TValue>(this IGenericContext<TEntity> context, TValue value) 
//        where TComponent : IValueComponent<TValue>, new()
//    {
//        var t = new TComponent { value = value };
//        context
//    }
//}

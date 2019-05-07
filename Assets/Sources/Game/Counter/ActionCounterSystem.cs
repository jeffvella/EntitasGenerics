using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class ActionCounterSystem : GenericReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;

    public ActionCounterSystem(Contexts contexts, GenericContexts genericContexts) : base(genericContexts.Game, Trigger)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
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
        var currentActionCount = Context.GetUnique<ActionCountComponent>();

        Context.SetUnique(new ActionCountComponent
        {
            value = currentActionCount.value + 1
        });

        //Context.SetUnique3(currentActionCount, 5);


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

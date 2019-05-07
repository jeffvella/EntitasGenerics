using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class MarkMatchedSystem : GenericReactiveSystem<InputEntity>
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;
    private readonly IGroup<GameEntity> _selectedGroup;
    private readonly List<GameEntity> _buffer;

    public MarkMatchedSystem(Contexts contexts, GenericContexts genericContexts) 
        : base(genericContexts.Input, TriggerProducer)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
        _selectedGroup = _contexts.game.GetGroup(GameMatcher.Selected);
        _buffer = new List<GameEntity>();
    }

    private static ICollector<InputEntity> TriggerProducer(IGenericContext<InputEntity> context)
    { 
        return context.GetCollector<PointerReleasedComponent>();
    }

    //protected override bool Filter(InputEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<InputEntity> entities)
    {
        //if (_contexts.input.isPointerHolding)
        //    return;

        if (_genericContexts.Input.IsTagged<PointerHoldingComponent>())
            return;

        var selectedEntities = _selectedGroup.GetEntities(_buffer);
        var minMatchCount = _genericContexts.Config.GetUnique<MinMatchCountComponent>().value;

        if (selectedEntities.Count >= minMatchCount)
        {
            foreach (var entity in selectedEntities)
            {
                entity.isMatched = true;

                //_genericContexts.Input.Set<MatchedComponent>(entity);
            }
        }
    }
}

//public sealed class MarkMatchedSystem2 : GenericReactiveSystem<InputEntity>
//{
//    private readonly Contexts _contexts;
//    private readonly GenericContexts _genericContexts;
//    private readonly IGroup<GameEntity> _selectedGroup;
//    private readonly List<GameEntity> _buffer;

//    public MarkMatchedSystem(Contexts contexts, GenericContexts genericContexts)
//        : base(genericContexts.Input, TriggerProducer)
//    {
//        _contexts = contexts;
//        _genericContexts = genericContexts;
//        _selectedGroup = _contexts.game.GetGroup(GameMatcher.Selected);
//        _buffer = new List<GameEntity>();
//    }

//    private static ICollector<InputEntity> TriggerProducer(IGenericContext<InputEntity> context)
//    {
//        return context.GetCollector<PointerReleasedComponent>();
//    }

//    //protected override bool Filter(InputEntity entity)
//    //{
//    //    return true;
//    //}

//    protected override void Execute(List<InputEntity> entities)
//    {
//        //if (_contexts.input.isPointerHolding)
//        //    return;

//        if (_genericContexts.Input.IsTagged<PointerHoldingComponent>())
//            return;

//        var selectedEntities = _selectedGroup.GetEntities(_buffer);
//        var minMatchCount = _genericContexts.Config.GetUnique<MinMatchCountComponent>().value;

//        if (selectedEntities.Count >= minMatchCount)
//        {
//            foreach (var entity in selectedEntities)
//            {
//                entity.isMatched = true;
//            }
//        }
//    }
//}
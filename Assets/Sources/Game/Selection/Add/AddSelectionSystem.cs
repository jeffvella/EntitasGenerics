using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class AddSelectionSystem : GenericReactiveSystem<InputEntity>
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;

    public AddSelectionSystem(Contexts contexts, GenericContexts genericContexts) 
        : base(genericContexts.Input, TriggerProducer)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
    }

    private static ICollector<InputEntity> TriggerProducer(IGenericContext<InputEntity> context)
    {
        return context.GetCollector<PointerHoldingPositionComponent>();
    }

    //protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    //{
    //    return context.CreateCollector(InputMatcher.PointerHoldingPosition);
    //}

    //protected override bool Filter(InputEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<InputEntity> entities)
    {
        //if (!_contexts.input.isPointerHolding)
        //    return;

        if (!_genericContexts.Input.IsTagged<PointerHoldingComponent>())
            return;

        //var position = _contexts.input.pointerHoldingPosition.value.ToGridPosition();

        var position = _genericContexts.Input.Get<PointerHoldingPositionComponent>().value.ToGridPosition();
        var mapSize = _genericContexts.Config.Get<MapSizeComponent>().value;

        var horizontalBounded = position.x >= 0 && position.x < mapSize.x;
        var verticalBounded = position.y >= 0 && position.y < mapSize.y;

        if (horizontalBounded && verticalBounded)
        {
            var entityUnderPointer = _contexts.game.GetEntityWithPosition(position);

            if (entityUnderPointer == null)
                return;

            if (entityUnderPointer.isBlock)
                return;

            if (entityUnderPointer.isSelected)
                return;

            var lastSelectedId = _contexts.gameState.lastSelected.value;
            if (lastSelectedId == -1)
            {
                entityUnderPointer.isSelected = true;
                entityUnderPointer.ReplaceSelectionId(0);

                _contexts.gameState.ReplaceLastSelected(entityUnderPointer.id.value);
                _contexts.gameState.ReplaceMaxSelectedElement(0);
            }
            else
            {
                var lastSelected = _contexts.game.GetEntityWithId(lastSelectedId);
                if (lastSelected.hasElementType && entityUnderPointer.hasElementType)
                {
                    if (lastSelected.elementType.value == entityUnderPointer.elementType.value)
                    {
                        if (GridPosition.Distance(lastSelected.position.value, entityUnderPointer.position.value) <
                            1.25f)
                        {
                            var selectionId = _contexts.gameState.maxSelectedElement.value;
                            selectionId++;

                            entityUnderPointer.isSelected = true;
                            entityUnderPointer.ReplaceSelectionId(selectionId);
                            
                            _contexts.gameState.ReplaceLastSelected(entityUnderPointer.id.value);
                            _contexts.gameState.ReplaceMaxSelectedElement(selectionId);
                        }
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Assets.Sources.Game;
using Assets.Sources.GameState;
using Entitas;
using Entitas.Generics;
using UnityEngine;

public sealed class UnselectionSystem : GenericReactiveSystem<InputEntity>
{
    private IGenericContext<GameStateEntity> _gameState;
    private IGenericContext<InputEntity> _input;
    private IGenericContext<GameEntity> _game;

    public UnselectionSystem(Contexts contexts) : base(contexts.Input, Trigger)
    {
        _gameState = contexts.GameState;
        _input = contexts.Input;
        _game = contexts.Game;
    }

    private static ICollector<InputEntity> Trigger(IGenericContext<InputEntity> context)
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

        if (!_input.IsFlagged<PointerHoldingComponent>())
            return;

        //var targetSelectionId = _contexts.gameState.maxSelectedElement.value - 1;

        var targetSelectionId = _gameState.GetUnique<MaxSelectedElementComponent>().value - 1;
       // Debug.Log($"Id={targetSelectionId}");
        if (targetSelectionId < 0)
            return;

        //var targetEntity = _contexts.game.GetEntityWithSelectionId(targetSelectionId);
        var targetEntity = _game.FindEntity<SelectionIdComponent,int>(targetSelectionId);

        //var target = _game.GetAccessor(targetEntity);

        //var pos = target.Get<PositionComponent>().value;
        //var sw = new System.Diagnostics.Stopwatch();

        //GridPosition targetEntityPosition = default;
        //var component = _game.Get<PositionComponent>(targetEntity);

        //targetEntityPosition = component.value;
        //var pos = targetEntityPosition;

        //sw.Restart();
        //for (int i = 0; i < 100; i++)
        //{
        //    _game.Set<PositionComponent>(targetEntity, c => c.value = pos);
        //    //targetEntityPosition = targetEntity.Get<PositionComponent>().value;
        //}
        //sw.Stop();

        //var acc1 = new ComponentAccessor<PositionComponent, GridPosition>(_game, targetEntity);
        //var cc = new ComponentAccessor<PositionComponent, GridPosition>(_game, targetEntity);

        

        //var sw2 = new System.Diagnostics.Stopwatch();        
        //sw2.Restart();
        //for (int i = 0; i < 100; i++)
        //{
            //cc.Value = pos;

            //acc1.Value = pos;

            //targetEntity.Set<PositionComponent>(c => c.value = pos);

            //var v = _game.ForEntity(targetEntity).Get<PositionComponent>().value;

            //_game.ForEntity(targetEntity).Set<PositionComponent>(c => c.value = pos);

            //var index = _game.GetComponentIndex<PositionComponent>();
            //var c1 = targetEntity.Get<PositionComponent>();
            //c1.value = pos;
            //targetEntity.RaiseComponentReplaced(index, c1, c1);

            //targetEntity.Set<PositionComponent>(c => c.value = pos);

            //.Set<PositionComponent>(c => c.value = pos);
            //targetEntityPosition = _game.Get<PositionComponent>(targetEntity).value;
            //component.value = pos;
            //targetEntity.RaiseComponentReplaced(index, component, component);
            //_game.Set(targetEntity, component);

            //targetEntityPosition = targetEntity.Get<PositionComponent>().value;
        //}
        //sw2.Stop();
        //Debug.Log($"A: {sw.Elapsed.TotalMilliseconds:N6} B: {sw2.Elapsed.TotalMilliseconds:N6} {(sw.Elapsed.TotalMilliseconds/sw2.Elapsed.TotalMilliseconds):N6}");


        //targetEntityPosition = _game.Get<PositionComponent>(targetEntity).value;


        var targetEntityPosition2 = _game.Get<PositionComponent>(targetEntity).value;

        //var position = _contexts.input.pointerHoldingPosition.value.ToGridPosition();
        var pointerHoldingPosition = _input.GetUnique<PointerHoldingPositionComponent>().value.ToGridPosition();

        if (pointerHoldingPosition.Equals(targetEntityPosition2))
        {
            //var lastSelectedEntity = _contexts.game.GetEntityWithId(_contexts.gameState.lastSelected.value);

            DeselectCurrentEntity();

            //lastSelectedEntity.isSelected = false;
            //lastSelectedEntity.RemoveSelectionId();

            SelectEntity(targetEntity, targetSelectionId);

            //_contexts.gameState.ReplaceLastSelected(targetEntity.id.value);
            //_contexts.gameState.ReplaceMaxSelectedElement(targetSelectionId);
        }
    }

    private void SelectEntity(GameEntity targetEntity, int targetSelectionId)
    {
        var targetEntityId = _game.Get<IdComponent>(targetEntity).value;
        _gameState.SetUnique<LastSelectedComponent>(c => c.value = targetEntityId);
        _gameState.SetUnique<MaxSelectedElementComponent>(c => c.value = targetSelectionId);
    }

    private void DeselectCurrentEntity()
    {
        var lastSelectedId = _gameState.GetUnique<LastSelectedComponent>().value;
        var lastSelectedEntity = _game.FindEntity<IdComponent, int>(lastSelectedId);
        _game.SetFlag<SelectedComponent>(lastSelectedEntity, false);
        _game.Remove<SelectionIdComponent>(lastSelectedEntity);
    }
}

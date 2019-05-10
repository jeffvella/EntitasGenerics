using System.Collections.Generic;
using Assets.Sources.Game;
using Assets.Sources.GameState;
using Entitas;
using Entitas.Generics;

public sealed class DropSelectionSystem : GenericReactiveSystem<InputEntity>
{
    private readonly IGroup<GameEntity> _selectedGroup;
    private readonly List<GameEntity> _buffer;
    private readonly IGenericContext<GameEntity> _game;
    private readonly IGenericContext<GameStateEntity> _gameState;
    private readonly IGenericContext<InputEntity> _input;

    public DropSelectionSystem(GenericContexts contexts) : base(contexts.Input, Trigger)
    {
        //_group = _contexts.game.GetGroup(GameMatcher.Selected);
        _selectedGroup = contexts.Game.GetGroup<SelectedComponent>();
        _game = contexts.Game;
        _input = contexts.Input;
        _gameState = contexts.GameState;
        _buffer = new List<GameEntity>();
    }

    private static ICollector<InputEntity> Trigger(IGenericContext<InputEntity> context)
    {
        return context.GetCollector<PointerReleasedComponent>();
    }

    //protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    //{
    //    return context.CreateCollector(InputMatcher.PointerReleased);
    //}

    //protected override bool Filter(InputEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<InputEntity> entities)
    {
        //if (_contexts.input.isPointerHolding)
        //    return;

        if (_input.IsFlagged<PointerHoldingComponent>())
            return;

        foreach (var entity in _selectedGroup.GetEntities(_buffer))
        {
            _game.SetFlag<SelectedComponent>(entity, false);
            _game.Remove<SelectionIdComponent>(entity);

            //entity.isSelected = false;
            //entity.RemoveSelectionId();
        }

        _gameState.SetUnique(new LastSelectedComponent {value = -1});
        _gameState.SetUnique(new MaxSelectedElementComponent { value = 0 });

        //_contexts.gameState.ReplaceLastSelected(-1);
        //_contexts.gameState.ReplaceMaxSelectedElement(0);
    }
}
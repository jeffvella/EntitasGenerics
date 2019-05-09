using System.Collections.Generic;
using System.Linq;
using Entitas;
using Entitas.Generics;
using UnityEngine;
using GameContext = Assets.Sources.Game.GameContext;
using GameStateContext = Assets.Sources.GameState.GameStateContext;

public sealed class DropSelectionOnMoveSystem : GenericReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _selectedGroup;
    private readonly List<GameEntity> _buffer;
    private readonly IGenericContext<GameEntity> _game;
    private readonly IGenericContext<GameStateEntity> _gameState;

    public DropSelectionOnMoveSystem(GenericContexts contexts) : base(contexts.Game, Trigger)
    {
        // todo fix these systems that trigger off one group then do nothing with the data.

        _game = contexts.Game;
        _gameState = contexts.GameState;
        
        //_selectedGroup = _contexts.game.GetGroup(GameMatcher.Selected);
        _selectedGroup = contexts.Game.GetGroup<SelectedComponent>();

        _buffer = new List<GameEntity>();
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetCollector<FieldMovedComponent>();
    }

    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.FieldMoved);
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in _selectedGroup.GetEntities(_buffer))
        {
            Debug.Log($"Drop Selection {_game.Get<SelectionIdComponent>(entity).value}");

            _game.SetTag<SelectedComponent>(entity, false);
            _game.Remove<SelectionIdComponent>(entity);
            
            //entity.isSelected = false;
            //entity.RemoveSelectionId();
        }

        _gameState.SetUnique(new LastSelectedComponent { value = -1 });
        _gameState.SetUnique(new MaxSelectedElementComponent { value = 0 });

        //_contexts.gameState.ReplaceLastSelected(-1);
        //_contexts.gameState.ReplaceMaxSelectedElement(0);
    }
}
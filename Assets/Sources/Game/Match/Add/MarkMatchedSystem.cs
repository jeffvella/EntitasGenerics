﻿using System.Collections.Generic;
using Assets.Sources.Config;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;

public sealed class MarkMatchedSystem : GenericReactiveSystem<InputEntity>
{
    private readonly IGroup<GameEntity> _selectedGroup;
    private readonly List<GameEntity> _buffer;
    private IGenericContext<GameEntity> _game;
    private IGenericContext<ConfigEntity> _config;
    private IGenericContext<InputEntity> _input;

    public MarkMatchedSystem(GenericContexts contexts) : base(contexts.Input, Trigger)
    {
        //_selectedGroup = _contexts.game.GetGroup(GameMatcher.Selected);
        _selectedGroup = contexts.Game.GetGroup<SelectedComponent>();
        _input = contexts.Input;
        _config = contexts.Config;
        _game = contexts.Game;
        _buffer = new List<GameEntity>();
    }

    private static ICollector<InputEntity> Trigger(IGenericContext<InputEntity> context)
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

        if (_input.IsFlagged<PointerHoldingComponent>())
            return;

        var selectedEntities = _selectedGroup.GetEntities(_buffer);
        var minMatchCount = _config.GetUnique<MinMatchCountComponent>().value;

        if (selectedEntities.Count >= minMatchCount)
        {
            foreach (var entity in selectedEntities)
            {               
                _game.SetFlag<MatchedComponent>(entity, true);

                //entity.isMatched = true;

                //_genericContexts.Input.Set<MatchedComponent>(entity);
            }
        }
    }
}

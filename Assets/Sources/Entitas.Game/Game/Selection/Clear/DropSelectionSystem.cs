﻿using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class DropSelectionSystem : GenericReactiveSystem<InputEntity>
    {
        private readonly IGroup<GameEntity> _selectedGroup;
        private readonly List<GameEntity> _buffer;
        private readonly IGenericContext<GameEntity> _game;
        private readonly IGenericContext<GameStateEntity> _gameState;
        private readonly IGenericContext<InputEntity> _input;

        public DropSelectionSystem(Contexts contexts) : base(contexts.Input, Trigger)
        {
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

        protected override void Execute(List<InputEntity> entities)
        {
            if (_input.IsFlagged<PointerHoldingComponent>())
                return;

            foreach (var entity in _selectedGroup.GetEntities(_buffer))
            {
                _game.SetFlag<SelectedComponent>(entity, false);
                _game.Remove<SelectionIdComponent>(entity);
            }

            _gameState.SetUnique<LastSelectedComponent>(c => c.value = -1);
            _gameState.SetUnique<MaxSelectedElementComponent>(c => c.value = 0);
        }
    }
}
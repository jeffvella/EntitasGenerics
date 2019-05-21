using System.Collections.Generic;
using Entitas.Generics;
using UnityEngine;

namespace Entitas.MatchLine
{
    public sealed class DropSelectionOnMoveSystem : GenericReactiveSystem<GameEntity>
    {
        private readonly IGroup<GameEntity> _selectedGroup;
        private readonly List<GameEntity> _buffer;
        private readonly IGenericContext<GameEntity> _game;
        private readonly IGenericContext<GameStateEntity> _gameState;

        public DropSelectionOnMoveSystem(Contexts contexts) : base(contexts.Game, Trigger)
        {
            _game = contexts.Game;
            _gameState = contexts.GameState;
            _selectedGroup = contexts.Game.GetGroup<SelectedComponent>();
            _buffer = new List<GameEntity>();
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            return context.GetCollector<FieldMovedComponent>();
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in _selectedGroup.GetEntities(_buffer))
            {
                Debug.Log($"Drop Selection {_game.Get<SelectionIdComponent>(entity).value}");

                _game.SetFlag<SelectedComponent>(entity, false);
                _game.Remove<SelectionIdComponent>(entity);
            }
            _gameState.SetUnique<LastSelectedComponent>(c => c.value = -1);
            _gameState.SetUnique<MaxSelectedElementComponent>(c => c.value = 0);
        }
    }
}
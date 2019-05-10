using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using Entitas.Generics;

namespace Assets.Sources.GameState
{
    public sealed class GameStateEventSystems : Feature
    {
        public GameStateEventSystems(GenericContexts contexts)
        {
            Add(new GenericEventSystem<GameStateEntity, ScoreComponent>(contexts.GameState));
            Add(new GenericEventSystem<GameStateEntity, GameOverComponent>(contexts.GameState, GroupEvent.Added));
            Add(new GenericEventSystem<GameStateEntity, GameOverComponent>(contexts.GameState, GroupEvent.Removed));
            Add(new GenericEventSystem<GameStateEntity, ActionCountComponent>(contexts.GameState));
        }
    }

}

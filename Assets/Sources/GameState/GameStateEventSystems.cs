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
        public GameStateEventSystems(Contexts contexts)
        {
            Add(new EventSystem<GameStateEntity, ScoreComponent>(contexts.GameState, GroupEvent.Added));
            Add(new EventSystem<GameStateEntity, GameOverComponent>(contexts.GameState, GroupEvent.Added));
            Add(new EventSystem<GameStateEntity, GameOverComponent>(contexts.GameState, GroupEvent.Removed));
            Add(new EventSystem<GameStateEntity, ActionCountComponent>(contexts.GameState, GroupEvent.Added));
        }
    }

}

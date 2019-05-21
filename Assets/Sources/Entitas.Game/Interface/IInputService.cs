using UnityEngine;

namespace Entitas.MatchLine
{
    public interface IInputService : IService
    {
        bool IsHolding();

        GridPosition HoldingPosition();

        bool IsStartedHolding();

        float HoldingTime();

        bool IsReleased();

        void Update(float delta);
    }
}
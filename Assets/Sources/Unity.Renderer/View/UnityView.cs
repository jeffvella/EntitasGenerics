using UnityEngine;
using Entitas.MatchLine;

public class UnityView : MonoBehaviour, IView<GameEntity>
{
    public void InitializeView(Contexts contexts, GameEntity entity)
    {
        contexts.Game.RegisterAddedComponentListener<DestroyedComponent>(entity, OnEntityDestroyed);
    }

    private void OnEntityDestroyed((GameEntity Entity, DestroyedComponent Component) obj)
    {
        Destroy(gameObject);
    }
}
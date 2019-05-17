using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;
using Entitas.Unity;
using UnityEngine;

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
using UnityEngine;
using Entitas.MatchLine;
using Entitas;

public class UnityView : MonoBehaviour, IView<GameEntity>
{
    public void InitializeView(Contexts contexts, GameEntity entity)
    {
        entity.RegisterComponentListener<DestroyedComponent>(OnEntityDestroyed, GroupEvent.Added);
    }

    private void OnEntityDestroyed(GameEntity entity)
    {
        Destroy(gameObject);
    }
}

using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;
using Entitas.Unity;
using UnityEngine;

public class UnityView : MonoBehaviour, IView // , IGameDestroyedListener
{
    private GameEntity _entity;

    public void InitializeView(GenericContexts contexts, IEntity entity)
    {
        _entity = (GameEntity) entity;

        contexts.Game.RegisterAddedComponentListener<DestroyedComponent>(_entity, OnEntityDestroyed);



        //_entity.AddGameDestroyedListener(this);

#if UNITY_EDITOR
        //gameObject.Link(entity, contexts.game);
#endif
    }

    private void OnEntityDestroyed((GameEntity Entity, DestroyedComponent Component) obj)
    {
#if UNITY_EDITOR
        //gameObject.Unlink();
#endif
        Destroy(gameObject);

        //Debug.Log($"Entity was destroyed {obj.Entity}");
    }

    //    public void OnDestroyed(GameEntity entity)
    //    {
    //#if UNITY_EDITOR
    //        //gameObject.Unlink();
    //#endif
    //        Destroy(gameObject);
    //    }
}
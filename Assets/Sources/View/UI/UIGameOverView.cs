using Assets.Sources.GameState;
using Entitas.Generics;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverView : MonoBehaviour//, IGameOverListener, IGameOverRemovedListener, 
    //IEventObserver<GameOverComponent>, IEventObserver<GameStateEntity, GameOverComponent>
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _boolName;

    private int _boolHash;

    private void Start()
    {
        //Contexts.sharedInstance.gameState.CreateEntity().AddGameOverListener(this);
        //Contexts.sharedInstance.gameState.CreateEntity().AddGameOverRemovedListener(this);

        var state = GenericContexts.Instance.GameState;
        state.RegisterAddedTagListener<GameOverComponent>(OnGameOverAdded);
        state.RegisterRemovedTagListener<GameOverComponent>(OnGameOverRemoved);

        _boolHash = Animator.StringToHash(_boolName);

        SetGameOver(state.IsFlagged<GameOverComponent>());
    }

    private void OnGameOverAdded((GameStateEntity Entity, GameOverComponent Component) obj)
    {
        SetGameOver(true);
    }


    //public void OnGameOver(GameStateEntity entity)
    //{
    //    SetGameOver(entity, true);
    //}

    public void OnGameOverRemoved(GameStateEntity entity)
    {
        SetGameOver(false);
    }

    private void SetGameOver(bool value)
    {
        _animator.SetBool(_boolHash, value);
    }

    //public void OnEvent(GameOverComponent gameOverComponent)
    //{
        
    //}

    //public void OnEvent((GameStateEntity Entity, GameOverComponent Component) args)
    //{
        
    //}
}
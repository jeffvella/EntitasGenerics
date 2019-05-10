using Assets.Sources.GameState;
using Entitas.Generics;
using UnityEngine;
using UnityEngine.UI;

public class UIRestartView : MonoBehaviour //, IGameOverListener, IGameOverRemovedListener
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _boolName;

    private int _boolHash;

    private void Start()
    {
        var listenerEntity = GenericContexts.Instance.GameState.CreateEntity();

        var state = GenericContexts.Instance.GameState;
        state.RegisterAddedTagListener<GameOverComponent>(OnGameOverAdded);
        state.RegisterRemovedTagListener<GameOverComponent>(OnGameOverRemoved);

        //Contexts.sharedInstance.gameState.CreateEntity().AddGameOverListener(this);
        //Contexts.sharedInstance.gameState.CreateEntity().AddGameOverRemovedListener(this);

        _boolHash = Animator.StringToHash(_boolName);

        //SetGameOver(Contexts.sharedInstance.gameState.gameOverEntity, Contexts.sharedInstance.gameState.isGameOver);

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

    public void OnPressed()
    {
        var context = GenericContexts.Instance.Input;
        var e = GenericContexts.Instance.Input.CreateEntity();
        context.SetFlag<RestartComponent>(e, true);
        context.SetFlag<DestroyedComponent>(e, true);

        Debug.Log($"UIRestartView OnPressed");
        //var e = Contexts.sharedInstance.input.CreateEntity();
        //e.isRestart = true;
        //e.isDestroyed = true;
    }
}
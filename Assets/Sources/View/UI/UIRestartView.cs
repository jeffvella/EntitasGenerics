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
        //var listenerEntity = Contexts.Instance.GameState.CreateEntity();

        var state = Contexts.Instance.GameState;
        state.RegisterAddedComponentListener<GameOverComponent>(OnGameOverAdded);
        state.RegisterRemovedComponentListener<GameOverComponent>(OnGameOverRemoved);

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
        var context = Contexts.Instance.Input;
        var e = Contexts.Instance.Input.CreateEntity();    
        e.SetFlag<RestartComponent>();
        e.SetFlag<DestroyedComponent>();

        //var e = Contexts.sharedInstance.input.CreateEntity();
        //e.isRestart = true;
        //e.isDestroyed = true;
    }
}
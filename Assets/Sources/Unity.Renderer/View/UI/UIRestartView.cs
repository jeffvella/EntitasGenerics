using UnityEngine;
using Entitas.MatchLine;

public class UIRestartView : MonoBehaviour //, IGameOverListener, IGameOverRemovedListener
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _boolName;

    private int _boolHash;

    private void Start()
    {
        var state = Contexts.Instance.GameState;
        state.RegisterAddedComponentListener<GameOverComponent>(OnGameOverAdded);
        state.RegisterRemovedComponentListener<GameOverComponent>(OnGameOverRemoved);

        _boolHash = Animator.StringToHash(_boolName);

        SetGameOver(state.IsFlagged<GameOverComponent>());
    }

    private void OnGameOverAdded((GameStateEntity Entity, GameOverComponent Component) obj)
    {
        SetGameOver(true);
    }

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
    }
}
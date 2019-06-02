using UnityEngine;
using Entitas;
using Entitas.MatchLine;
using Entitas.Generics;

public class UIRestartView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _boolName;

    private int _boolHash;

    private void Start()
    {
        var state = Contexts.Instance.GameState;
        state.Unique.RegisterComponentListener<GameOverComponent>(OnGameOverAdded, GroupEvent.Added);
        state.Unique.RegisterComponentListener<GameOverComponent>(OnGameOverRemoved, GroupEvent.Added);

        _boolHash = Animator.StringToHash(_boolName);

        SetGameOver(state.Unique.IsFlagged<GameOverComponent>());
    }

    private void OnGameOverAdded(GameStateEntity Entity)
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
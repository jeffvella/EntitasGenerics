using Entitas;
using UnityEngine;
using Entitas.MatchLine;

public class UIGameOverView : MonoBehaviour//, IGameOverListener, IGameOverRemovedListener, 
    //IEventObserver<GameOverComponent>, IEventObserver<GameStateEntity, GameOverComponent>
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _boolName;

    private int _boolHash;

    private void Start()
    {
        var state = Contexts.Instance.GameState;
        state.Unique.RegisterComponentListener<GameOverComponent>(OnGameOverAdded, GroupEvent.Added);
        state.Unique.RegisterComponentListener<GameOverComponent>(OnGameOverRemoved, GroupEvent.Removed);

        _boolHash = Animator.StringToHash(_boolName);
        SetGameOver(state.Unique.IsFlagged<GameOverComponent>());
    }

    private void OnGameOverAdded(GameStateEntity entity)
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

}

using UnityEngine;
using Entitas.MatchLine;
//using UnityEngine.UI;

public class UIGameOverView : MonoBehaviour//, IGameOverListener, IGameOverRemovedListener, 
    //IEventObserver<GameOverComponent>, IEventObserver<GameStateEntity, GameOverComponent>
{
    //[SerializeField] private Animator _animator;
    [SerializeField] private string _boolName;

    private int _boolHash;

    private void Start()
    {
        var state = Contexts.Instance.GameState;
        state.RegisterAddedComponentListener<GameOverComponent>(OnGameOverAdded);
        state.RegisterRemovedComponentListener<GameOverComponent>(OnGameOverRemoved);

        //_boolHash = Animator.StringToHash(_boolName);
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
        //_animator.SetBool(_boolHash, value);
    }

}
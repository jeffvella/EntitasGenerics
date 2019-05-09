using Entitas;
using Entitas.Generics;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreView : MonoBehaviour, IEventObserver<GameStateEntity, ScoreComponent>
{
    [SerializeField] private Text _label;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;

    private int _triggerHash;

    private void Start()
    {
        //Contexts.sharedInstance.gameState.CreateEntity().AddScoreListener(this);

        //GenericContexts.Instance.GameState.RegisterAddedComponentListener<ScoreComponent>(this);

        GenericContexts.Instance.GameState.RegisterAddedComponentListener<ScoreComponent>(this);

        _triggerHash = Animator.StringToHash(_triggerName);
    }

    //public void OnScore(GameStateEntity entity, int value)
    //{
    //    _label.text = value.ToString();
    //    _animator.SetTrigger(_triggerHash);
    //}

    //public void OnEvent((GameStateEntity Entity, ScoreComponent Component) arg)
    //{
    //    OnScore(arg.Entity, arg.Component.value);
    //}

    //public void OnEvent(ScoreComponent component)
    //{
    //    _label.text = component.value.ToString();
    //    _animator.SetTrigger(_triggerHash);
    //}

    public void OnEvent((GameStateEntity Entity, ScoreComponent Component) args)
    {
        _label.text = args.Component.value.ToString();
        _animator.SetTrigger(_triggerHash);
    }

    //public void OnEvent((GameStateContext Context, GameStateEntity Entity, ScoreComponent Component) args)
    //{
    //    _label.text = args.Component.value.ToString();
    //    _animator.SetTrigger(_triggerHash);
    //}

    //public void OnEvent((Assets.Sources.GameState.GameStateContext Context, GameStateEntity Entity, ScoreComponent Component) args)
    //{
    //    _label.text = args.Component.value.ToString();
    //    _animator.SetTrigger(_triggerHash);
    //}



}

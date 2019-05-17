using Assets.Sources.GameState;
using Entitas;
using Entitas.Generics;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreView : MonoBehaviour, IAddedComponentListener<GameStateEntity, ScoreComponent>
{
    [SerializeField] private Text _label;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;

    private int _triggerHash;

    private void Start()
    {
        //Contexts.sharedInstance.gameState.CreateEntity().AddScoreListener(this);

        //Contexts.Instance.GameState.RegisterAddedComponentListener<ScoreComponent>(this);

        Contexts.Instance.GameState.RegisterAddedComponentListener(this);

        _triggerHash = Animator.StringToHash(_triggerName);
    }

    public void OnComponentAdded(GameStateEntity entity, ScoreComponent component)
    {
        _label.text = component.value.ToString();
        _animator.SetTrigger(_triggerHash);
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

    //public void OnEvent((GameStateEntity Entity, ScoreComponent Component) args)
    //{
    //    _label.text = args.Component.value.ToString();
    //    _animator.SetTrigger(_triggerHash);
    //}

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

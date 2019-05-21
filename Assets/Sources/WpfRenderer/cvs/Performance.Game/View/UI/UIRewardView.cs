using UnityEngine;
using Entitas.MatchLine;
//using UnityEngine.UI;

public class UIRewardView : MonoBehaviour //, IScoreListener
{
    //[SerializeField] private Text _label;
    //[SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;

    private int _lastValue = 0;

    private int _triggerHash;

    private void Start()
    {
        Contexts.Instance.GameState.RegisterAddedComponentListener<ScoreComponent>(OnScoreAddedEvent);
        //_triggerHash = Animator.StringToHash(_triggerName);
    }

    private void OnScoreAddedEvent((GameStateEntity Entity, ScoreComponent Component) obj)
    {
        OnScore(obj.Entity, obj.Component.Value);
    }

    public void OnScore(GameStateEntity entity, int value)
    {
        if (value == _lastValue) return;
        
        var difference = value - _lastValue;
                
        //_label.text = difference.ToString();
        //_animator.SetTrigger(_triggerHash);
        
        _lastValue = value;
    }
}
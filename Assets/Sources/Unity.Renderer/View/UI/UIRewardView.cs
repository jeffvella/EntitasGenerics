using UnityEngine;
using UnityEngine.UI;
using Entitas.MatchLine;
using Entitas;
using System;
using Entitas.Generics;

public class UIRewardView : MonoBehaviour
{
    [SerializeField] private Text _label;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;

    private int _lastValue = 0;

    private int _triggerHash;

    private void Start()
    {
        Contexts.Instance.GameState.Unique.RegisterComponentListener<ScoreComponent>(OnScoreAddedEvent, GroupEvent.Added);

        _triggerHash = Animator.StringToHash(_triggerName);
    }

    private void OnScoreAddedEvent(GameStateEntity entity)
    {
        OnScore(entity, entity.Get<ScoreComponent>().Value);
    }

    public void OnScore(GameStateEntity entity, int value)
    {
        if (value == _lastValue) return;
        
        var difference = value - _lastValue;
                
        _label.text = difference.ToString();
        _animator.SetTrigger(_triggerHash);
        
        _lastValue = value;
    }
}
using Entitas.Generics;
using UnityEngine;
using UnityEngine.UI;
using Entitas.MatchLine;

public class UIScoreView : MonoBehaviour, IAddedComponentListener<GameStateEntity, ScoreComponent>
{
    [SerializeField] private Text _label;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;

    private int _triggerHash;

    private void Start()
    {
        Contexts.Instance.GameState.Unique.RegisterComponentListener<ScoreComponent>(this);

        _triggerHash = Animator.StringToHash(_triggerName);
    }

    //public void OnComponentAdded(GameStateEntity entity, ScoreComponent component)
    //{
    //    _label.text = component.Value.ToString();
    //    _animator.SetTrigger(_triggerHash);
    //}

    public void OnComponentAdded(GameStateEntity entity)
    {
        _label.text = entity.Get<ScoreComponent>().Value.ToString();
        _animator.SetTrigger(_triggerHash);
    }
}

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
        Contexts.Instance.GameState.RegisterAddedComponentListener(this);

        _triggerHash = Animator.StringToHash(_triggerName);
    }

    public void OnComponentAdded(GameStateEntity entity, ScoreComponent component)
    {
        _label.text = component.Value.ToString();
        _animator.SetTrigger(_triggerHash);
    }
}

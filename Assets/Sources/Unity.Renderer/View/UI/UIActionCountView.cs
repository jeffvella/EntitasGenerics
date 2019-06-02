using UnityEngine;
using UnityEngine.UI;
using Entitas.MatchLine;
using Entitas;

public class UIActionCountView : MonoBehaviour
{
    [SerializeField] private Text _label;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;

    private int _triggerHash;
    private int _actionCount;
    private int _maxActionCount;

    private void Start()
    {
        Contexts.Instance.GameState.Unique.RegisterComponentListener<ActionCountComponent>(OnActionCountChanged, GroupEvent.Added);
        Contexts.Instance.Config.Unique.RegisterComponentListener<MaxActionCountComponent>(OnMaxActionCountChanged, GroupEvent.Added);

        _triggerHash = Animator.StringToHash(_triggerName);
        _maxActionCount = Contexts.Instance.Config.Unique.Get<MaxActionCountComponent>().Component.Value;    
        
        Apply();
    }

    private void OnActionCountChanged(GameStateEntity entity)
    {
        _actionCount = entity.GetComponent<ActionCountComponent>().Value;
        Apply();
    }


    private void OnMaxActionCountChanged(ConfigEntity entity)
    {
        _maxActionCount = entity.GetComponent<MaxActionCountComponent>().Value;
        Apply();
    }

    private void Apply()
    {
        _label.text = string.Format("{0}/{1}", _actionCount, _maxActionCount);
        _animator.SetTrigger(_triggerHash);
    }

}
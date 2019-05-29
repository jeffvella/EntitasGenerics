using UnityEngine;
using UnityEngine.UI;
using Entitas.MatchLine;

public class UIActionCountView : MonoBehaviour 
    //IEventObserver<ActionCountComponent>, IEventObserver<MaxActionCountComponent>
//, IActionCountListener, IMaxActionCountListener, 
{
    [SerializeField] private Text _label;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;

    private int _triggerHash;
    private int _actionCount;
    private int _maxActionCount;

    private void Start()
    {
        Contexts.Instance.GameState.RegisterAddedComponentListener<ActionCountComponent>(OnActionCountChanged);
        Contexts.Instance.Config.RegisterAddedComponentListener<MaxActionCountComponent>(OnMaxActionCountChanged);

        _triggerHash = Animator.StringToHash(_triggerName);
        _maxActionCount = Contexts.Instance.Config.GetUnique<MaxActionCountComponent>().Component.Value;    
        
        Apply();
    }

    private void OnActionCountChanged((GameStateEntity Entity, ActionCountComponent Component) obj)
    {
        _actionCount = obj.Component.value;
        Apply();
    }


    private void OnMaxActionCountChanged((ConfigEntity Entity, MaxActionCountComponent Component) obj)
    {
        _maxActionCount = obj.Component.Value;
        Apply();
    }

    private void Apply()
    {
        _label.text = string.Format("{0}/{1}", _actionCount, _maxActionCount);
        _animator.SetTrigger(_triggerHash);
    }

}
using Entitas.Generics;
using Events;
using UnityEngine;
using UnityEngine.UI;

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
        //todo fix events
        //Contexts.sharedInstance.gameState.CreateEntity().AddActionCountListener(this);
        //Contexts.sharedInstance.config.CreateEntity().AddMaxActionCountListener(this);

        GenericContexts.Instance.GameState.RegisterAddedComponentListener<ActionCountComponent>(OnActionCountChanged);
        GenericContexts.Instance.Config.RegisterAddedComponentListener<MaxActionCountComponent>(OnMaxActionCountChanged);

        //GenericContexts.Instance.GameState.RegisterAddedComponentListener<ActionCountComponent>(this);
        //GenericContexts.Instance.Config.RegisterAddedComponentListener<MaxActionCountComponent>(this);

        _triggerHash = Animator.StringToHash(_triggerName);

        //Contexts.sharedInstance.config.maxActionCountEntity;

        var pair = GenericContexts.Instance.Config.GetUniqueEntityAndComponent<MaxActionCountComponent>(); 

        OnMaxActionCountChanged(pair);
    }

    private void OnMaxActionCountChanged((ConfigEntity Entity, MaxActionCountComponent Component) obj)
    {
        _maxActionCount = obj.Component.value;
        Apply();
    }

    private void OnActionCountChanged((GameStateEntity Entity, ActionCountComponent Component) arg)
    {
        _actionCount = arg.Component.value;
        Apply();
    }

    //public void OnEvent(ActionCountComponent component)
    //{
    //    _actionCount = component.value;
    //    Apply();
    //}

    //public void OnEvent(MaxActionCountComponent component)
    //{
    //    _maxActionCount = component.value;
    //    Apply();
    //}

    //public void OnActionCount(GameStateEntity entity, int value)
    //{
    //    _actionCount = value;
    //    Apply();
    //}

    //public void OnMaxActionCount(ConfigEntity entity, int value)
    //{
    //    _maxActionCount = value;
    //    Apply();
    //}

    private void Apply()
    {
        _label.text = string.Format("{0}/{1}", _actionCount, _maxActionCount);
        _animator.SetTrigger(_triggerHash);
    }

    //public void OnEvent(ActionCountComponent component)
    //{
    //    _actionCount = value;
    //    Apply();
    //}

    //public void OnEvent(MaxActionCountComponent component)
    //{
    //    throw new System.NotImplementedException();
    //}


}
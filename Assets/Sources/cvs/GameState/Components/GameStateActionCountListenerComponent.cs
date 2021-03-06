//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameStateEntity {

    public ActionCountListenerComponent actionCountListener { get { return (ActionCountListenerComponent)GetComponent(GameStateComponentsLookup.ActionCountListener); } }
    public bool hasActionCountListener { get { return HasComponent(GameStateComponentsLookup.ActionCountListener); } }

    public void AddActionCountListener(System.Collections.Generic.List<IActionCountListener> newValue) {
        var index = GameStateComponentsLookup.ActionCountListener;
        var component = CreateComponent<ActionCountListenerComponent>(index);
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceActionCountListener(System.Collections.Generic.List<IActionCountListener> newValue) {
        var index = GameStateComponentsLookup.ActionCountListener;
        var component = CreateComponent<ActionCountListenerComponent>(index);
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveActionCountListener() {
        RemoveComponent(GameStateComponentsLookup.ActionCountListener);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameStateMatcher {

    static Entitas.IMatcher<GameStateEntity> _matcherActionCountListener;

    public static Entitas.IMatcher<GameStateEntity> ActionCountListener {
        get {
            if (_matcherActionCountListener == null) {
                var matcher = (Entitas.Matcher<GameStateEntity>)Entitas.Matcher<GameStateEntity>.AllOf(GameStateComponentsLookup.ActionCountListener);
                matcher.componentNames = GameStateComponentsLookup.componentNames;
                _matcherActionCountListener = matcher;
            }

            return _matcherActionCountListener;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameStateEntity {

    public void AddActionCountListener(IActionCountListener value) {
        var listeners = hasActionCountListener
            ? actionCountListener.value
            : new System.Collections.Generic.List<IActionCountListener>();
        listeners.Add(value);
        ReplaceActionCountListener(listeners);
    }

    public void RemoveActionCountListener(IActionCountListener value, bool removeComponentWhenEmpty = true) {
        var listeners = actionCountListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveActionCountListener();
        } else {
            ReplaceActionCountListener(listeners);
        }
    }
}

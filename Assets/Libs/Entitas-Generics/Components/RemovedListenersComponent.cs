using Entitas.VisualDebugging.Unity;

namespace Entitas.Generics
{
    /// <summary>
    /// A component that stores event listeners for component 'Removed' events. 
    /// This component type is automatically registered when a component with an event attribute
    /// (or implementing IEventComponent) is added to a <see cref="ContextDefinition{TContext,TEntity}"/>
    /// </summary> 
    /// <typeparam name="TEntity">an implementation of IEntity</typeparam>
    /// <typeparam name="TComponent">the component to monitor for changes</typeparam>
    public class RemovedListenersComponent<TEntity, TComponent> : GameEventBase<TEntity>, ICustomDisplayName
        where TEntity : IEntity where TComponent : IComponent
    {
        public string DisplayName => $"Added Event Listener ({typeof(TComponent).Name})";
    }
}
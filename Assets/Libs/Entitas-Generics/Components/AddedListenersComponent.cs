using Entitas.Generics;

namespace Entitas.Generics
{
    /// <summary>
    /// A component that stores event listeners for component 'Added' events. 
    /// This component type is automatically registered when a component with an event attribute
    /// (or implementing IEventComponent) is added to a <see cref="ContextDefinition{TContext,TEntity}"/>
    /// </summary> 
    /// <typeparam name="TEntity">an implementation of IEntity</typeparam>
    /// <typeparam name="TComponent">the component to monitor for changes</typeparam>
    public class AddedListenersComponent<TEntity, TComponent> : GameEventBase<(TEntity Entity, TComponent Component)>
    {

    }
}

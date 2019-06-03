using System;

namespace Entitas.Generics
{
    /// <summary>
    /// A component that houses an invocation list for events.
    /// </summary>
    public interface IListenerComponent : IComponent
    {
        void ClearListeners();

        string[] GetListenersNames();

        int ListenerCount { get; }
    }

    /// <summary>
    /// A component that houses an invocation list for <typeparamref name="T"/>
    /// <typeparam name="T"></typeparam>
    public interface IListenerComponent<T> : IListenerComponent
    {
        void Register(Action<T> action);

        void Deregister(Action<T> action);

        void Register(IEventListener listener);

        void Deregister(IEventListener listener);        

        void Raise(T arg);
    }
}

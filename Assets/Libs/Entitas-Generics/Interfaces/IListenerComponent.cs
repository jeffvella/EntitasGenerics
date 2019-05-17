using System;

namespace Entitas.Generics
{
    public interface IListenerComponent : IComponent
    {
        void ClearListeners();

        string[] GetListenersNames();

        int ListenerCount { get; }
    }

    public interface IListenerComponent<T> : IListenerComponent
    {
        void Register(Action<T> action);

        void Register(IEventObserver<T> listener);

        void Deregister(IEventObserver<T> listener);

        void Raise(T arg);
    }
}
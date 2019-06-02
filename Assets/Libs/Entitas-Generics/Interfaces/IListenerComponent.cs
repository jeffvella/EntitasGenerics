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

        void Deregister(Action<T> action);

        void Register(IEventListener listener);

        void Deregister(IEventListener listener);        

        void Raise(T arg);
    }
}

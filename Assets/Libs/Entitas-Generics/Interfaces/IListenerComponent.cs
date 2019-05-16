using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Entitas;
using Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.VisualDebugging.Unity;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
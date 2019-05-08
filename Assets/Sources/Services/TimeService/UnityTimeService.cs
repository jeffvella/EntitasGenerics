using Entitas.Generics;
using UnityEngine;

public sealed class UnityTimeService : Service, ITimeService
{
    public UnityTimeService(GenericContexts contexts) : base(contexts)
    {
    }

    public float DeltaTime()
    {
        return Time.deltaTime;
    }

    public float RealtimeSinceStartup()
    {
        return Time.realtimeSinceStartup;
    }
}
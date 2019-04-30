using EntitasGenerics;
using UnityEngine;

public sealed class UnityTimeService : Service, ITimeService
{
    public UnityTimeService(Contexts contexts, GenericContexts genericContexts) : base(contexts, genericContexts)
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
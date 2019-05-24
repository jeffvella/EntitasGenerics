using Entitas.VisualDebugging.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SystemInfo = Entitas.VisualDebugging.Unity.SystemInfo;

namespace Entitas.Generics
{
    public static class VisualDebuggingExtensions
    {
        public static void EnableVisualDebugging(this IContexts contexts)
        {
            for (int i = 0; i < contexts.allContexts.Length; i++)
            {
                SetupVisualDebugging(contexts.allContexts[i]);
            }            
        }

        public static void SetupVisualDebugging(IContext context)
        {
#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
            if (!UnityEngine.Application.isPlaying)
                return;

            var observer = new Entitas.VisualDebugging.Unity.ContextObserver(context);
            UnityEngine.Object.DontDestroyOnLoad(observer.gameObject);

            var behavior = observer.gameObject.GetComponent<ContextObserverBehaviour>();           
#endif
        }

    }
}

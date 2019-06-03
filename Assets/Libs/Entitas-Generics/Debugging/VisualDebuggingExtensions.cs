using Entitas.VisualDebugging.Unity;

namespace Entitas.Generics
{
    public static class VisualDebuggingExtensions
    {
        /// <summary>
        /// Setup contexts to use visual debugging.
        /// </summary>
        public static void EnableVisualDebugging(this IContexts contexts)
        {
            // This moves the registration of visual debugging outside the Context so that games
            // using both Unity projects and external systems at the same time (such as WPF)
            // can optionally exclude debugging support and operate on the same project. 
            // (external projects need to exclude unity specific code because it throws exceptions 
            // whenever it accesses the C++ side of unity engine).

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

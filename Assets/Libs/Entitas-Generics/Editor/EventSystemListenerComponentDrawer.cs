using System;
using Entitas.VisualDebugging.Unity.Editor;
using UnityEditor;

namespace Entitas.Generics
{
    /// <summary>
    /// An inspector drawer for <see cref="AddedListenersComponent{TEntity, TComponent}"/>/
    /// <see cref="RemovedListenersComponent{TEntity, TComponent}"/>; 
    /// Shows information about the event subscribers.
    /// </summary>
    public class EventSystemListenerComponentDrawer : IComponentDrawer
    { 
        public bool HandlesType(Type type)
        {
            return typeof(IListenerComponent).IsAssignableFrom(type);
        }

        public IComponent DrawComponent(IComponent component)
        {
            var listenerComponent = (IListenerComponent)component;
            var listenerNames = listenerComponent.GetListenersNames();

            if(listenerNames.Length == 0)
            {
                EditorGUILayout.LabelField($"No Subscribers");
            }
            else
            {
                EditorGUILayout.LabelField($"{listenerNames.Length} Subscribers");

                for (int i = 0; i < listenerNames.Length; i++)
                {
                    EditorGUILayout.LabelField($"- {listenerNames[i]}");
                }
            }
            return listenerComponent;
        }
    }
}
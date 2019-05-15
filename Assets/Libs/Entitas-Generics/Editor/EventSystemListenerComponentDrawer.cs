using System;
using Entitas.VisualDebugging.Unity.Editor;
using UnityEditor;

namespace Entitas.Generics {
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

            //person.name = EditorGUILayout.TextField("Name", person.name);
            //var gender = (PersonGender)Enum.Parse(typeof(PersonGender), person.gender);
            //gender = (PersonGender)EditorGUILayout.EnumPopup("Gender", gender);
            //person.gender = gender.ToString();
          
            return listenerComponent;
        }
    }
}
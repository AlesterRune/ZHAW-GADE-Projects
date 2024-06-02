using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CUI
{
    public class OnChangedCallAttribute : PropertyAttribute
    {
        public OnChangedCallAttribute(string methodName)
        {
            MethodName = methodName;
        }
        
        public string MethodName { get; }
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(OnChangedCallAttribute))]
    public class OnChangedCallAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);
            if (!EditorGUI.EndChangeCheck()) return;

            var targetObject = property.serializedObject.targetObject;

            var methodName = attribute is OnChangedCallAttribute callAttribute ? callAttribute.MethodName : string.Empty;

            var classType = targetObject.GetType();
            var methodInfo = classType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(info => info.Name == methodName);

            // Update the serialized field
            property.serializedObject.ApplyModifiedProperties();

            // If we found a public function with the given name that takes no parameters, invoke it
            if (methodInfo != null && !methodInfo.GetParameters().Any())
            {
                Debug.Log($"Calling {methodInfo.Name} ({targetObject})");
                methodInfo.Invoke(targetObject, null);
            }
            else
            {
                // TODO: Create proper exception
                Debug.LogError($"OnChangedCall error : No public function taking no argument named {methodName} in class {classType.Name}");
            }
        }
    }
#endif
}
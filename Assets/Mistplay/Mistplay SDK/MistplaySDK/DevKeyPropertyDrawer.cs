using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MistplaySDK 
{
    public class DevKeyAttribute : PropertyAttribute
    {
        public DevKeyAttribute()
        {
        }
    }

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DevKeyAttribute))]
    public class DevKeyProperty : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var content = EditorGUIUtility.IconContent("InspectorLock");
            content.text = property.displayName;

            EditorGUI.PropertyField(position, property, content);
        }
    }
    #endif
}
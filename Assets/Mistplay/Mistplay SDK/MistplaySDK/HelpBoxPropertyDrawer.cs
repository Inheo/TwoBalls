using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MistplaySDK 
{
    public class HelpBoxAttribute : PropertyAttribute
    {
        public string text;
        public HelpBoxAttribute(string text)
        {
            this.text = text;
        }
    }
    
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var content = EditorGUIUtility.IconContent("console.infoicon.sml");
            content.text = property.displayName;

            EditorGUI.PropertyField(position, property, content);
        }
    }
    #endif
}
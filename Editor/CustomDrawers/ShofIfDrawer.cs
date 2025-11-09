using NevermoreStudios.Core;
using UnityEditor;
using UnityEngine;

namespace NevermoreStudios.Editor
{
#if UNITY_EDITOR
    
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;
            SerializedProperty condition = property.serializedObject.FindProperty(showIf.ConditionField);

            bool enabled = condition is { propertyType: SerializedPropertyType.Boolean, boolValue: true };

            if (enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;
            SerializedProperty condition = property.serializedObject.FindProperty(showIf.ConditionField);

            bool enabled = condition is { propertyType: SerializedPropertyType.Boolean, boolValue: true };

            return enabled ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
        }
    }
#endif
}

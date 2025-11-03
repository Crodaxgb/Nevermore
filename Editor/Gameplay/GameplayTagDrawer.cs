using System.Linq;
using NevermoreStudios.Gameplay;
using UnityEditor;
using UnityEngine;
using NevermoreStudios.Core;

namespace NevermoreStudios.Editor
{
    [CustomPropertyDrawer(typeof(GameplayTagEditorAttribute))]
    public class GameplayTagEditorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var tagAttribute = (GameplayTagEditorAttribute)attribute;

            var tagsAsset = GameplayTags.Instance;
            if (tagsAsset == null || tagsAsset.gameplayTags == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            var allTags = tagsAsset.gameplayTags;

            if (!string.IsNullOrEmpty(tagAttribute.FilterPrefix))//Filtering based on the custom editor attribute
            {
                allTags = allTags.Where(tag => tag.StartsWith(tagAttribute.FilterPrefix)).ToList();
            }

            position = EditorGUI.PrefixLabel(position, label);
            
            string currentValue = property.stringValue;
            string buttonLabel = string.IsNullOrEmpty(currentValue) ? "<None>" : currentValue;
            
            if (GUI.Button(position, new GUIContent(buttonLabel, label.tooltip), EditorStyles.popup))
            {
                var dropdown = new GameplayTagDropdown(allTags, (selectedTag) =>
                {
                    property.stringValue = selectedTag;
                    property.serializedObject.ApplyModifiedProperties();
                });

                dropdown.Show(position);
            }
        }
    }
}

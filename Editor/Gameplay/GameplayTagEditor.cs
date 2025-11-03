
namespace NevermoreStudios.Editor
{
#if UNITY_EDITOR
    using NevermoreStudios.Gameplay;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(GameplayTags))]
    public class GameplayTagsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            GameplayTags tags = (GameplayTags)target;

            if (GUILayout.Button("Generate GameplayTag Constants"))
            {
                tags.GenerateConstClass();
            }
        }
    }
#endif
}

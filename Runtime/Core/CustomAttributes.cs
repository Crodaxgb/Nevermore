using UnityEngine;

namespace NevermoreStudios.Core
{
    public class GameplayTagEditorAttribute : PropertyAttribute
    {
        public readonly string FilterPrefix;

        public GameplayTagEditorAttribute(string filterPrefix = null)
        {
            FilterPrefix = filterPrefix;
        }
    }
    
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ConditionField;

        public ShowIfAttribute(string conditionField)
        {
            ConditionField = conditionField;
        }
    }
}

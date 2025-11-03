using UnityEngine;

namespace NevermoreStudios.Core
{
    public static class MBExtensions
    {
        public static bool TryGetComponentInChildren<T>(this Component component, out T result, bool includeInactive = false) where T : Component
        {
            result = component.GetComponentInChildren<T>(includeInactive);
            return result != null;
        }
    }
}

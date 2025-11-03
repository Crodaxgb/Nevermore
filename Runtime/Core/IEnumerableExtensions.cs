using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NevermoreStudios.Core
{
    public static class IEnumerableExtensions
    {
        public static bool IsValidIndex<T>(this IEnumerable<T> collection, int index)
        {
            return index > -1 && collection.Count() > index;
        }
        
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int iterationIndex = list.Count - 1; iterationIndex > 0; iterationIndex--)
            {
                int randomIndex = Random.Range(0, iterationIndex + 1);

                (list[iterationIndex], list[randomIndex]) = (list[randomIndex], list[iterationIndex]);
            }
        }
    }
}

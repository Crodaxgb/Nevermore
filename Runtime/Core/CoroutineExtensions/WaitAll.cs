using System.Collections;
using System.Linq;
using UnityEngine;

namespace NevermoreStudios.Core
{
    public class WaitAll : WaitBase
    {
        public override bool keepWaiting => _wait.Any(t => t);

        public WaitAll(MonoBehaviour monoBehaviour, params IEnumerator[] coroutines) : base(monoBehaviour, coroutines)
        {
            
        }
    }
}
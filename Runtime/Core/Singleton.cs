using UnityEngine;

namespace NevermoreStudios.Core
{
    public abstract class Singleton<T> : Singleton where T : MonoBehaviour
    {
        static T _Instance = null;
        static bool _bInitialising = false;
        static readonly object _InstanceLock = new object();

        public static T Instance
        {
            get
            {
                lock(_InstanceLock)
                {
                    // do nothing if currently quitting
                    if (bIsQuitting)
                        return null;

                    // instance already found?
                    if (_Instance != null)
                        return _Instance;

                    _bInitialising = true;

                    // search for any in-scene instance of T
                    var allInstances = FindObjectsByType<T>(FindObjectsSortMode.None);

                    // found exactly one?
                    if (allInstances.Length == 1)
                    {
                        Debug.Log($"Found exactly 1 {typeof(T)}");
                        _Instance = allInstances[0];
                    } // found none?
                    else if (allInstances.Length == 0)
                    {
                        Debug.Log($"Found exactly no {typeof(T)}");
                        _Instance = new GameObject($"Singleton<{typeof(T)}>").AddComponent<T>();
                    } // multiple found?
                    else
                    {
                        Debug.Log($"Found exactly {allInstances.Length} {typeof(T)}");
                        _Instance = allInstances[0];

                        // destroy the duplicates
                        for (int index = 1; index < allInstances.Length; ++index)
                        {
                            Debug.LogError($"Destroying duplicate {typeof(T)} on {allInstances[0].gameObject.name}");
                            Destroy(allInstances[index].gameObject);
                        }
                    }

                    _bInitialising = false;
                    return _Instance;
                }
            }
        }

        static void ConstructIfNeeded(Singleton<T> inInstance)
        {
            lock(_InstanceLock)
            {
                // only construct if the instance is null and is not being initialised
                if (_Instance == null && !_bInitialising)
                {
                    Debug.Log($"ConstructIfNeeded run for {typeof(T)}");
                    _Instance = inInstance as T;
                }
                else if (_Instance != null && !_bInitialising)
                {
                    Debug.LogError($"Destroying duplicate {typeof(T)} on {inInstance.gameObject.name}");
                    Destroy(inInstance.gameObject);
                }
            }
        }

        protected void Awake()
        {
            ConstructIfNeeded(this);

            OnAwake();
        }

        protected virtual void OnAwake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public abstract class Singleton : MonoBehaviour
    {
        protected static bool bIsQuitting { get; private set; } = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void BeforeSceneLoad()
        {
            Debug.Log("Before Scene Load");
            bIsQuitting = false;
        }

        private void OnApplicationQuit()
        {
            Debug.Log("Quitting in progress");
            bIsQuitting = true;
        }
    }
}

using System;
using System.Collections.Generic;
using NevermoreStudios.Core;
using UnityEngine;

namespace NevermoreStudios.GameState
{
    public class GameState<T> : Singleton<T> where T : MonoBehaviour
    {
        protected readonly Dictionary<Type, IGameStateManager> _managers = new();

        public TState GetState<TState>()
        {
            return ((IGameStateManager<TState>)_managers[typeof(TState)]).State;
        }

        public IGameStateManager<TState> GetManager<TState>()
        {
            if (_managers.TryGetValue(typeof(TState), out var manager))
            {
                if (manager is IGameStateManager<TState> typedManager)
                {
                    return typedManager;
                }

                throw new InvalidCastException(
                    $"Registered manager for {typeof(TState).Name} does not implement IGameStateManager<{typeof(TState).Name}>.");
            }

            throw new KeyNotFoundException($"No manager registered for state type: {typeof(TState).Name}");
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Basics
{
    public class CachedBehaviour : MonoBehaviour
    {
        private readonly Dictionary<Type, object> _cachedComponents = new Dictionary<Type, object>();

        public new Transform transform
        {
            get => GetCachedComponent<Transform>();
        }

        public T GetCachedComponent<T>()
        {
            TryGetCachedComponent<T>(out var cachedComponent);
            return cachedComponent;
        }

        public TryResult TryGetCachedComponent<T>(out T component)
        {
            if(_cachedComponents.TryGetValue(typeof(T), out var cachedComponent))
            {
                component = (T)cachedComponent;
                return true;
            }

            if(TryGetComponent<T>(out component))
            {
                _cachedComponents.Add(typeof(T), component);
                return true;
            }

            component = default(T);
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Basics
{
    public static class NullCast
    {
        public static bool IsNullable<T>()
        {
            if(default(T) == null)
            {
                return true;
            }

            return false;
        }

        public static bool IsStrictNull<T>(T obj)
        {
            if (!IsNullable<T>()) 
            {
                return false;
            }

            if(obj == null)
            {
                return true;
            }

            if (obj is Object unityObject)
            {
                return unityObject == null;
            }

            return false;
        }

        public static NullCast<T> ToNullCast<T>(T obj)
        {
            return new NullCast<T>(obj);
        }
    }

    public struct NullCast<T>
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
            }
        }

        public bool IsNull
        {
            get
            {
                return NullCast.IsStrictNull(Value);
            }
        }

        public NullCast(T value)
        {
            _value = value;
        }

        public static implicit operator bool(NullCast<T> nullCast)
        {
            return !nullCast.IsNull;
        }

        public static implicit operator T(NullCast<T> nullCast)
        {
            return nullCast.Value;
        }
    }
}

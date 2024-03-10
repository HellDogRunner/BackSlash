using System;

namespace RedMoonGames.Basics
{
    public sealed class NullableNull { }

    [Serializable]
    public struct NullableInt
    {
        public int Value;
        public bool HasValue;

        public NullableInt(int value)
        {
            Value = value;
            HasValue = true;
        }

        public static implicit operator NullableInt(int value) => new NullableInt(value);

        public static implicit operator NullableInt(NullableNull value) => new NullableInt();

        public static implicit operator int(NullableInt value) => value.Value;

        public static implicit operator int?(NullableInt value) => value.HasValue ? value.Value : new int?();
    }

    [Serializable]
    public struct NullableFloat
    {
        public float Value;
        public bool HasValue;

        public NullableFloat(float value)
        {
            Value = value;
            HasValue = true;
        }

        public static implicit operator NullableFloat(float value) => new NullableFloat(value);

        public static implicit operator NullableFloat(NullableNull value) => new NullableFloat();

        public static implicit operator float(NullableFloat value) => value.Value;

        public static implicit operator float?(NullableFloat value) => value.HasValue ? value.Value : new float?();
    }
}

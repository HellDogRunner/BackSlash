using System;

namespace RedMoonGames.Basics
{
    public static class ComparableMathf
    {
        public static T Max<T>(T c1, T c2) where T : IComparable<T>
        {
            if (c1.CompareTo(c2) < 0)
            {
                return c2;
            }

            return c1;
        }

        public static T Min<T>(T c1, T c2) where T : IComparable<T>
        {
            if (c1.CompareTo(c2) > 0)
            {
                return c2;
            }

            return c1;
        }
    }
}

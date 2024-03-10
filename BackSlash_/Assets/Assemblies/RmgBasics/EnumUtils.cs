using System;
using System.Collections.Generic;

namespace RedMoonGames.Basics
{
    public static class EnumUtils
    {
		public static List<T> GetValues<T>() where T : Enum
		{
			var list = new List<T>();
			var values = Enum.GetValues(typeof(T));
			foreach (var value in values)
				list.Add((T)value);
			return list;
		}

		public static T ParseEnum<T>(string value) where T : Enum
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}

		public static bool TryParseEnum<T>(string value, out T resultEnum) where T : Enum
		{
			if (!IsValueInEnumRange<T>(value))
			{
				resultEnum = default(T);
				return false;
			}

			resultEnum = ParseEnum<T>(value);
			return true;
		}

		public static bool IsValueInEnumRange<T>(string value) where T : Enum
		{
			var enumValues = GetValues<T>();
			foreach (var enumValue in enumValues)
			{
				var stringEnumValue = Convert.ToString(enumValue);

				if (stringEnumValue == value)
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsValueInEnumRange<T>(int value) where T : Enum
		{
			var enumValues = GetValues<T>();
			foreach (var enumValue in enumValues)
			{
				var intEnumValue = Convert.ToInt32(enumValue);

				if (intEnumValue == value)
				{
					return true;
				}
			}

			return false;
		}

		public static T ConvertToEnum<T>(int value, T defaultValue) where T : Enum
		{
			if (!IsValueInEnumRange<T>(value))
			{
				return defaultValue;
			}

			return (T)(object)value;
		}

		public static bool IsValidEnumValue<T>(T value) where T : Enum
		{
			return Enum.IsDefined(typeof(T), value);
		}
	}
}

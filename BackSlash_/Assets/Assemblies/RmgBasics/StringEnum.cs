using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedMoonGames.Basics
{
    [Serializable]
    public class StringEnum<TEnum> : ISerializationCallbackReceiver where TEnum : Enum, IConvertible
    {
        [SerializeField] private string enumValueName;
        [SerializeField] private bool isValueExist;

        private TEnum _enumValue = default(TEnum);

        public TEnum Value
        {
            get { return _enumValue; }
            set
            {
                _enumValue = value;
                isValueExist = EnumUtils.IsValidEnumValue(_enumValue);
            }
        }

        public bool IsValidValue
        {
            get => isValueExist;
        }

        public StringEnum() { }

        public StringEnum(TEnum enumValue)
        {
            Value = enumValue;
        }

        public void OnAfterDeserialize()
        {
            if (!EnumUtils.TryParseEnum<TEnum>(enumValueName, out var resultEnum))
            {
                _enumValue = default(TEnum);
                isValueExist = false;
                return;
            }

            _enumValue = resultEnum;
            isValueExist = true;
        }

        public void OnBeforeSerialize()
        {
            if (!isValueExist)
            {
                return;
            }

            isValueExist = EnumUtils.IsValidEnumValue(_enumValue);
            enumValueName = _enumValue.ToString();
        }

        public static implicit operator TEnum(StringEnum<TEnum> stringEnum)
        {
            return stringEnum.Value;
        }

        public static implicit operator StringEnum<TEnum>(TEnum enumValue)
        {
            return new StringEnum<TEnum>(enumValue);
        }

        public override int GetHashCode()
        {
            return _enumValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is StringEnum<TEnum> stringEnum)
            {
                return Value.Equals(stringEnum.Value);
            }

            return false;
        }
    }
}

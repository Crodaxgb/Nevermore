using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace NevermoreStudios.Core
{
    public struct EnumContainer<TEnumType> where TEnumType : unmanaged, Enum
    {
        internal UInt16 _enumValue;
        static EnumContainer()
        {
            if (UnsafeUtility.SizeOf<TEnumType>() != sizeof(ushort))
            {
                throw new InvalidOperationException($"{typeof(TEnumType)} must be a ushort-based enum.");
            }
        }
        public EnumContainer(TEnumType enumValue = default)
        {
            this._enumValue = ToUInt16(enumValue);
        }

        private static UInt16 ToUInt16(TEnumType value) => Convert.ToUInt16(value);

        private static TEnumType FromUInt16(UInt16 value) => UnsafeUtility.As<UInt16, TEnumType>(ref value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetWith(TEnumType enumValue = default)
        {
            this._enumValue = ToUInt16(enumValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFlag(TEnumType flag)
        {
            _enumValue |= ToUInt16(flag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveFlag(TEnumType flag)
        {
            _enumValue &= (UInt16)~ToUInt16(flag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleCurrentValue(TEnumType flag)
        {
            _enumValue ^= ToUInt16(flag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(TEnumType flag)
        {
            return (_enumValue & ToUInt16(flag)) != 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(EnumContainer<TEnumType> otherContainer)
        {
            return (_enumValue & otherContainer._enumValue) != 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsAny(in List<TEnumType> flags)
        {
            bool contains = false;
            
            foreach(TEnumType enumType in flags)
            {
                if (Contains(enumType))
                {
                    contains = true;
                    break;
                }
            }

            return contains;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsAll(in List<TEnumType> flags)
        {
            bool contains = true;
            
            foreach(TEnumType enumType in flags)
            {
                if (!Contains(enumType))
                {
                    contains = false;
                    break;
                }
            }

            return contains;
        }

        public TEnumType Value
        {
            get => FromUInt16(_enumValue);
            set => _enumValue = ToUInt16(value);
        }
        
#if !UNITY_BURST
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => Value.ToString();
#endif
    }
}

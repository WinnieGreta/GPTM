using NUnit.Framework;
using UnityEngine;

namespace Gameplay.Monster.Tests
{
    public static class SerializedFieldExtensions
    {
        [SerializeField]
        public static void SetSerializedField<TCarrierType, TPropertyType>(this TCarrierType carrier, string fieldName, TPropertyType value)
        {
            var field = typeof(TCarrierType).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsTrue(field.GetCustomAttributes(typeof(SerializeField), false).Length > 0);
            field.SetValue(carrier, value);
        }
        
        public static void SetSerializedProperty<TCarrierType, TPropertyType>(this TCarrierType carrier, string propertyName, TPropertyType value)
        {
            string backingFieldName = $"<{propertyName}>k__BackingField";
            carrier.SetSerializedField(backingFieldName, value);
        }
    }
}
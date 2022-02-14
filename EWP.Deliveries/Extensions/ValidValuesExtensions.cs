using System;

namespace EWP.Extensions
{
    static class ValidValuesExtensions
    {
        public static void AddRange<T>(this SAPbouiCOM.ValidValues oValidValues, bool FirstElementIsEmpty = false)
        {
            var type = typeof(T);

            if (!type.IsEnum)
                throw new ArgumentException("T must be enum type");

            if (FirstElementIsEmpty)
                oValidValues.Add("", "");

            foreach (var v in Enum.GetValues(type))
            {
                var enumValue = Convert.ChangeType(v, type);
                string value = Convert.ToInt32(enumValue).ToString();
                string description = enumValue.ToString();

                oValidValues.Add(value, description);
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Data;

namespace TestApp
{
    public class EnumToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type enumType = parameter as Type;
            if (value != null && parameter != null && enumType.IsEnum)
            {
                //var tryParseMethod = enumType.GetMethod(nameof(Enum.TryParse), new Type[] { typeof(string), enumType });
                //if (tryParseMethod != null)
                //{
                //    tryParseMethod.Invoke(null, new object[] {  value, null })
                //}

                if (value.GetType().Equals(enumType))
                {
                    return (int) value;
                }

                if (Enum.TryParse(enumType, (string) value, out object result))
                {
                    return (int) result;
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}

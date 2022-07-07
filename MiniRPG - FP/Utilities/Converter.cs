using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MiniRPG.Utilities
{
    static class Converter
    {
        public static string KeyToString(ConsoleKey key)
        {
            var keyLabel = key.ToString();
            return keyLabel.ToLower();

        }

        public static T Convert<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(input);
                }
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }
    }
}

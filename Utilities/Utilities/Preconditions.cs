using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Preconditions
    {
        public static T CheckNotNull<T>(this T obj, string parameterName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName + " can't be null.", parameterName);
            }

            return obj;
        }

        public static string CheckNotNullOrWhitespace(this string str, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException(parameterName + " can't be null or whitespace.", parameterName);
            }

            return str;
        }

        public static void CheckCondition(this bool condition, string message, string parameterName)
        {
            if (!condition)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

    }
}

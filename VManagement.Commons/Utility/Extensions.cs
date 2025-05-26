namespace VManagement.Commons.Utility
{
    public static class Extensions
    {
        public static string BetweenParenthesis(this string text)
        {
            return $"({text})";
        }

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool NotNullOrEmpty(this string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        #region [ Conversions ]
        public static string SafeToString(this object? value)
        {
            try
            {
                #pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
                #pragma warning disable CS8603 // Possível retorno de referência nula.
                return value.ToString();
                #pragma warning restore CS8603 // Possível retorno de referência nula.
                #pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
            }
            catch
            {
                return string.Empty;
            }
        }

        public static long ToInt64(this object? value)
        {
            return Convert.ToInt64(value);
        }

        public static long SafeToInt64(this object? value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch
            {
                return default;
            }
        }

        public static int ToInt32(this object? value)
        {
            return Convert.ToInt32(value);
        }

        public static int SafeToInt32(this object? value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return default;
            }
        }

        public static short ToInt16(this object? value)
        {
            return Convert.ToInt16(value);
        }

        public static short SafeToInt16(this object? value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch
            {
                return default;
            }
        }
        
        public static DateTime ToDateTime(this object? value)
        {
            return Convert.ToDateTime(value);
        }

        public static DateTime SafeToDateTime(this object? value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static decimal ToDecimal(this object? value)
        {
            return Convert.ToDecimal(value);
        }

        public static decimal SafeToDecimal(this object? value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return default;
            }
        }

        public static double ToDouble(this object? value)
        {
            return Convert.ToDouble(value);
        }

        public static double SafeToDouble(this object? value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return default;
            }
        }

        public static float ToSingle(this object? value)
        {
            return Convert.ToSingle(value);
        }

        public static float SafeToSingle(this object? value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch
            {
                return default;
            }
        }
        #endregion
    }
}

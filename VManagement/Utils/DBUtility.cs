using Microsoft.Data.SqlClient;

namespace VManagement.Database.Utils
{
    internal static class DBUtility
    {
        internal static object? GetNullableValue(this SqlDataReader reader, int i)
        {
            object nullableValue = reader.GetValue(i);

            if (nullableValue == DBNull.Value)
                return null;

            return nullableValue;
        }
    }
}

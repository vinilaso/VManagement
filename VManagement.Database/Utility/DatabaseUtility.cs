namespace VManagement.Database.Utility
{
    internal static class DatabaseUtility
    {
        public static string AsParameter(this string field)
        {
            return "@" + field;
        }
    }
}

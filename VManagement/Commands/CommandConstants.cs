namespace VManagement.Database.Commands
{
    internal class CommandConstants
    {
        /// <summary>
        /// <para>{0} = Fields</para>
        /// <para>{1} = Table name</para>
        /// <para>{2} = Restrictions</para>
        /// </summary>
        internal const string SELECT_LAYOUT = @"SELECT {0} FROM {1} {2}";

        /// <summary>
        /// <para>{0} = Table name</para>
        /// <para>{1} = Fields</para>
        /// <para>{2} = Values</para>
        /// </summary>
        internal const string INSERT_LAYOUT = @"INSERT INTO {0} ({1}) OUTPUT INSERTED.ID VALUES ({2})";

        /// <summary>
        /// <para>{0} = Table name</para>
        /// <para>{1} = Values</para>
        /// <para>{2} = Restrictions</para>
        /// </summary>
        internal const string UPDATE_LAYOUT = @"UPDATE {0} SET {1} {2}";

        /// <summary>
        /// <para>{0} = Table name</para>
        /// <para>{1} = Restrictions</para>
        /// </summary>
        internal const string DELETE_LAYOUT = @"DELETE FROM {0} {1}";

        /// <summary>
        /// <para>{0} = Table name</para>
        /// <para>{1} = Restrictions</para>
        /// </summary>
        internal const string EXISTS_LAYOUT = "SELECT CASE WHEN EXISTS (SELECT TOP 1 ID FROM {0} {1}) THEN 1 ELSE 0 END 'EXISTS'";
    }
}

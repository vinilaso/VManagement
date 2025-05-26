namespace VManagement.Commons.Entities
{
    public class EntitySchema
    {
        public string EntityName { get; set; } = string.Empty;
        public string SqlQuery { get; set; } = string.Empty;

        public EntitySchema() { }
        public EntitySchema(string sqlQuery)
        {
            SqlQuery = sqlQuery;
        }
    }
}

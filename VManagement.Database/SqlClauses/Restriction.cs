using Microsoft.Data.SqlClient;
using VManagement.Commons.Utility;

namespace VManagement.Database.SqlClauses
{
    public class Restriction
    {
        public static readonly Restriction Empty;

        static Restriction()
        {
            Empty = new Restriction();
        }

        public ParameterCollection Parameters { get; private set; } = new ParameterCollection();
        public string SortClause { get; set; } = string.Empty;
        public string WhereClause { get; set; } = string.Empty;
        public string GroupClause { get; set; } = string.Empty;

        internal Restriction() { }
        public Restriction(string whereClause)
        {
            this.AddWhere(whereClause);
        }

        public void AddWhere(string whereClause)
        {
            var sb = new DelimitedStringBuilder(" ");

            if (string.IsNullOrEmpty(WhereClause))
                sb.Append("WHERE");
            else
                sb.Append("AND");

            sb.Append(whereClause);
            WhereClause = sb.ToString();
        }

        public override string ToString()
        {
            var sb = new DelimitedStringBuilder(" ");

            if(!string.IsNullOrEmpty(WhereClause))
                sb.Append(WhereClause);

            if (!string.IsNullOrEmpty(GroupClause))
                sb.Append(GroupClause);

            if (!string.IsNullOrEmpty(SortClause))
                sb.Append(SortClause);

            return sb.ToString();
        }

        public void SetParameters(SqlCommand targetCommand)
        {
            foreach (var parameter in Parameters)
                targetCommand.Parameters.Add(parameter.AsSqlParameter());
        }

        public static Restriction FromId(long id)
        {
            var restriction = new Restriction("A.ID = @ID");
            restriction.Parameters.Add("@ID", id);

            return restriction;
        }
    }
}

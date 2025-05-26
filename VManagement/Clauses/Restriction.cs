using System.Text;
using VManagement.Commons.Utility;

namespace VManagement.Database.Clauses
{
    public class Restriction
    {
        public static readonly Restriction Empty = new Restriction();

        private string _whereClause = string.Empty;
        private string _orderClause = string.Empty;
        private string _groupClause = string.Empty;

        public Restriction() { }
        public Restriction(string whereClause)
        {
            AddWhereClause(whereClause);
        }

        public ParameterCollection Parameters { get; set; } = new ParameterCollection();
        public string WhereClause
        {
            get => _whereClause;
            set => _whereClause = value;
        }
        public string OrderClause
        {
            get => _orderClause;
            set => _orderClause = value;
        }
        public string GroupClause
        {
            get => _groupClause;
            set => _groupClause = value;
        }

        public void AddWhereClause(string whereClause)
        {
            if (whereClause.IsNullOrEmpty())
                return;

            if (_whereClause.NotNullOrEmpty())
            {
                _whereClause += " AND ";
            }

            _whereClause += whereClause.BetweenParenthesis();
        }

        public void AddOrderClause(string orderClause)
        {
            if (orderClause.IsNullOrEmpty())
                return;

            if (_orderClause.NotNullOrEmpty())
            {
                _orderClause += ", ";
            }

            _orderClause += orderClause;
        }

        public void AddGroupClause(string groupClause)
        {
            if (groupClause.IsNullOrEmpty())
                return;

            if (_groupClause.NotNullOrEmpty())
            {
                _groupClause += ", ";
            }

            _groupClause += groupClause;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (_whereClause.NotNullOrEmpty())
            {
                sb.Append(" WHERE ");
                sb.Append(_whereClause);
            }

            if (_groupClause.NotNullOrEmpty())
            {
                sb.Append(" GROUP BY ");
                sb.Append(_groupClause);
            }

            if (_orderClause.NotNullOrEmpty())
            {
                sb.Append(" ORDER BY ");
                sb.Append(_orderClause);
            }

            return sb.ToString();
        }

        public static Restriction FromId(long? id, bool withAlias = true)
        {
            Restriction result;

            if (withAlias)
            {
                result = new Restriction("A.ID = @ID");
            }
            else
            {
                result = new Restriction("ID = @ID");
            }

            result.Parameters.Add("@ID", id);
            return result;
        }
    }
}

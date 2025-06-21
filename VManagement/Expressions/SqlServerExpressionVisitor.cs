using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using VManagement.Commons.Entities.Attributes;
using VManagement.Commons.Utility;
using VManagement.Database.Clauses;

namespace VManagement.Database.Expressions
{
    public class SqlServerExpressionVisitor : ExpressionVisitor
    {
        private readonly StringBuilder _sqlBuilder = new();
        private readonly Dictionary<string, object?> _parameters = new();

        public string GetSql() => _sqlBuilder.ToString();
        public IReadOnlyDictionary<string, object?> GetParameters() => _parameters;

        public static (string sql, IReadOnlyDictionary<string, object?> parameters) Parse(Expression expression)
        {
            SqlServerExpressionVisitor visitor = new();
            visitor.Visit(expression);
            return (visitor.GetSql(), visitor.GetParameters());
        }

        public static Restriction ParseToRestriction(Expression expression)
        {
            SqlServerExpressionVisitor visitor = new();
            visitor.Visit(expression);

            Restriction restriction = new(visitor.GetSql());

            foreach (var param in visitor.GetParameters())
                restriction.Parameters.Add(param.Key, param.Value);
            
            return restriction;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType.In(ExpressionType.Modulo, ExpressionType.Add, ExpressionType.Subtract, ExpressionType.Multiply, ExpressionType.Divide))
            {
                Visit(node.Left);

                _sqlBuilder.Append($" {GetSqlOperator(node.NodeType)} ");

                Visit(node.Right);
            }
            else
            {
                _sqlBuilder.Append('(');
                Visit(node.Left);

                if (IsNullCheck(node))
                    _sqlBuilder.Append($" {GetSqlNullCheckOperator(node.NodeType)} ");
                else
                    _sqlBuilder.Append($" {GetSqlOperator(node.NodeType)} ");

                Visit(node.Right);
                _sqlBuilder.Append(')');
            }

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value != null)
            {
                string paramName = $"@p{_parameters.Count}";
                _sqlBuilder.Append(paramName);
                _parameters.Add(paramName, node.Value);
            }
            else
            {
                _sqlBuilder.Append("NULL");
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.MemberType == MemberTypes.Property)
            {
                var columnNameAttr = node.Member.GetCustomAttribute<EntityColumnNameAttribute>();

                if (columnNameAttr != null)
                    _sqlBuilder.Append(columnNameAttr.Name);
                else
                    _sqlBuilder.Append(node.Member.Name);
            }
            else
            {
                throw new NotSupportedException($"Member type {node.Member.MemberType} for member {node.Member.Name} not supported.");
            }

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(string))
            {
                switch (node.Method.Name)
                {
                    case nameof(string.ToUpper):
                        _sqlBuilder.Append("UPPER(");
                        Visit(node.Object);
                        _sqlBuilder.Append(')');
                        break;

                    case nameof(string.ToLower):
                        _sqlBuilder.Append("LOWER(");
                        Visit(node.Object);
                        _sqlBuilder.Append(')');
                        break;

                    case nameof(string.Contains):
                        Visit(node.Object);
                        _sqlBuilder.Append(" LIKE ");
                        AddParameterWithWildcards(node.Arguments.First(), "%", "%");
                        break;

                    case nameof(string.StartsWith):
                        Visit(node.Object);
                        _sqlBuilder.Append(" LIKE ");
                        AddParameterWithWildcards(node.Arguments.First(), string.Empty, "%");
                        break;

                    case nameof(string.EndsWith):
                        Visit(node.Object);
                        _sqlBuilder.Append(" LIKE ");
                        AddParameterWithWildcards(node.Arguments.First(), "%", string.Empty);
                        break;

                    case nameof(string.Equals):
                        Visit(node.Object);
                        _sqlBuilder.Append(" LIKE ");
                        AddParameterWithWildcards(node.Arguments.First(), "", "");
                        break;
                }
            }

            if (node.Method.DeclaringType == typeof(Extensions))
            {
                switch (node.Method.Name)
                {
                    case nameof(Extensions.In):
                        CreateInClause(node);
                        break;
                }
            }


            return node;
        }

        private void CreateInClause(MethodCallExpression node)
        {
            Expression columnExpression = node.Arguments[0];
            Visit(columnExpression);

            _sqlBuilder.Append(" IN (");

            IEnumerable<Expression> valueExpressions = node.Arguments.Skip(1);
            List<string> inParameters = new();

            foreach (var value in valueExpressions)
            {
                object? evaluatedValue = value.EvaluateValue();

                if (evaluatedValue is IEnumerable collection)
                {
                    foreach (var item in collection)
                    {
                        string paramName = $"@p{_parameters.Count}";
                        inParameters.Add(paramName);
                        _parameters.Add(paramName, item);
                    }
                }
            }

            _sqlBuilder.Append(string.Join(", ", inParameters));
            _sqlBuilder.Append(')');
        }

        private static string GetSqlOperator(ExpressionType expressionType)
        {
            return expressionType switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "<>",
                ExpressionType.GreaterThan => ">",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.LessThan => "<",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.Modulo => "%",
                ExpressionType.Add => "+",
                ExpressionType.Subtract => "-",
                ExpressionType.Multiply => "*",
                ExpressionType.Divide => "/",
                ExpressionType.AndAlso => "AND",
                ExpressionType.OrElse => "OR",

                _ => throw new NotSupportedException($"Expression type '{expressionType}' not supported as a SQL operator.")
            };
        }

        private static string GetSqlNullCheckOperator(ExpressionType expressionType)
        {
            return expressionType switch
            {
                ExpressionType.Equal => "IS",
                _ => "IS NOT"
            };
        }

        private void AddParameterWithWildcards(Expression node, string prefix, string suffix)
        {
            object? value = null;

            switch (node)
            {
                case ConstantExpression constant:
                    value = constant.Value;
                    break;

                case MemberExpression member:
                    value = member.EvaluateValue();
                    break;
            }

            string paramName = $"@p{_parameters.Count}";
            _sqlBuilder.Append(paramName);
            _parameters.Add(paramName, $"{prefix}{value}{suffix}");
        }

        private static bool IsNullCheck(BinaryExpression node)
        {
            if (node.Left is ConstantExpression left)
                if (left.Value == null)
                    return true;
            
            if (node.Right is ConstantExpression right)
                if (right.Value == null)
                    return true;

            return false;
        }
    }
}
